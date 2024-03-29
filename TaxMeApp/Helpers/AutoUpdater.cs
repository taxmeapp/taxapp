﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace TaxMeApp.Helpers
{
    public class AutoUpdater
    {

        private string remoteURL = "";
        private string baseRemoteURL = "";

        public bool RestartRequired { get; private set; } = false;
        public string Version { get; private set; } = "";

        private bool updated = false;
        private bool stagingValid = true;
        private bool abort = false;

        private Dictionary<string, string> fileHashDictionary = new Dictionary<string, string>();

        private WebClient client = new WebClient();

        private StreamWriter sw;

        // re-enabled graphVM series.clear

        public void Update()
        {

            initUpdateLog();

            clearStaging(false);

            // Read local manifest to get remote URL
            readLocalManifest();

            if (abort)
            {
                sw.Close();
                return;
            }

            getRemoteManifest();

            if (abort)
            {
                clearStaging(false);
                sw.Close();
                return;
            }

            downloadUpdates();

            if (abort)
            {
                clearStaging(false);
                sw.Close();
                return;
            }

            if (updated)
            {

                validateStaging();

                if (stagingValid)
                {
                    writeLine("Staging valid.");
                    RestartRequired = true;
                    clearStaging(true);
                }
                else
                {
                    writeLine("Staging invalid.");
                    //clearStaging(false);
                }

            }
            else
            {
                clearStaging(true);
                writeLine("No update required.");
            }

            sw.Close();

        }

        // Updating process:

        private void initUpdateLog()
        {

            moveFile("UpdateLog.txt", "UpdateLogLast.txt");

            try
            {
                sw = new StreamWriter("UpdateLog.txt");
                sw.AutoFlush = true;

                DateTime dateTime = DateTime.Now;

                writeLine("Update initiated at " + dateTime.ToString());
            }
            catch (Exception e)
            {
                // continue without an update log
            }

        }


        private void readLocalManifest()
        {

            try
            {

                writeLine("Reading local manifest...");

                XmlDocument local = new XmlDocument();
                local.Load(@".\VersionInfo.xml");

                remoteURL = local.GetElementsByTagName("RemoteURL")[0].InnerText;

                Version = local.GetElementsByTagName("Manifest")[0].Attributes[0].Value;

                writeLine("\tLocal version is: " + Version);

                writeLine("\tRemoteURL from Local Manifest: " + remoteURL);

            }
            catch (Exception e)
            {
                writeLine("Error reading local manifest.");
                abort = true;
                return;
            }

        }

        private void getRemoteManifest()
        {

            writeLine("Getting remote manifest...");

            downloadFileWeb(remoteURL, @"staging\VersionInfo.xml");

            XmlDocument remote = new XmlDocument();

            try
            {
                remote.Load(@"staging\VersionInfo.xml");
            }
            catch (Exception e)
            {
                writeLine("\tError in loading remote manifest.");
                abort = true;
                return;
            }

            try
            {
                // Update base URL
                baseRemoteURL = remote.GetElementsByTagName("BaseRemoteURL")[0].InnerText;
                writeLine("\tBase Remote URL: " + baseRemoteURL);
            }
            catch (Exception e)
            {
                writeLine("\tError in reading Base Remote URL");
                abort = true;
                return;
            }

            try
            {
                // Read version
                string version = remote.GetElementsByTagName("Manifest")[0].Attributes[0].Value;
                writeLine("\tRemote version is: " + version);
            }
            catch (Exception e)
            {

                // This isn't critical by any means, so we can just continue. But take note of the error.
                writeLine("\tError in reading Remote version");

            }

            // Build filename : hashmap dictionary
            generateFileHashDict(remote);

        }

        private void downloadUpdates()
        {

            writeLine("Files requiring updates:");

            foreach (KeyValuePair<string, string> kv in fileHashDictionary)
            {

                if (!File.Exists(kv.Key) || !compareChecksum(kv.Key, kv.Value))
                {
                                        
                    downloadFileWeb(baseRemoteURL + kv.Key, @"staging\" + kv.Key);

                    updated = true;

                }

            }

        }

        private void validateStaging()
        {

            string[] filesInStaging = Directory.GetFiles(@"staging\", "*", SearchOption.AllDirectories);

            writeLine("Validating staged files:");

            foreach (string file in filesInStaging)
            {

                string fileName = file.Replace(@"staging\", String.Empty);

                if (!fileHashDictionary.ContainsKey(fileName))
                {
                    continue;
                }

                if (!compareChecksum(file, fileHashDictionary[fileName]))
                {

                    //Trace.WriteLine(fileName + " failed staging checks");

                    writeLine("\t" + file + " has failed staging checks.");
                    stagingValid = false;
                    return;
                }

                writeLine("\t" + fileName);

                if (fileName.EndsWith(".exe"))
                {
                    backupEXE(fileName);
                }

            }

        }

        private void backupEXE(string path)
        {

            if (!File.Exists(path) || !path.EndsWith(".exe"))
            {
                return;
            }

            string newFileName = path.Replace(".exe", ".bak");

            moveFile(path, newFileName);            

        }

        private void clearStaging(bool move)
        {

            if (!Directory.Exists(@"staging\"))
            {
                return;
            }            

            string[] filesInStaging = Directory.GetFiles(@"staging\", "*", SearchOption.AllDirectories);

            if (move)
            {

                writeLine("Moving contents of staging folder...");

                foreach (string file in filesInStaging)
                {

                    string newPath = file.Replace(@"staging\", String.Empty);

                    createDirectory(newPath);
                    moveFile(file, newPath);
                    

                }
            }
            else
            {
                writeLine("Clearing staging folder...");
            }

            Directory.Delete(@"staging\", true);

        }

        private void generateFileHashDict(XmlDocument doc)
        {

            fileHashDictionary.Clear();

            foreach (XmlNode node in doc.GetElementsByTagName("File"))
            {
                //Trace.WriteLine(node.Attributes[0].Value + " | " + node.InnerText);
                fileHashDictionary.Add(node.Attributes[0].Value, node.InnerText);
            }

            //foreach (KeyValuePair<string, string> kvPair in fileHashDictionary)
            //{
            //    Trace.WriteLine(kvPair.Key + " | " + kvPair.Value);
            //}            

        }

        private bool compareChecksum(string localFile, string remoteHash)
        {

            MD5 md5 = MD5.Create();
            var localHash = md5.ComputeHash(File.ReadAllBytes(localFile));
            string localHashStr = BitConverter.ToString(localHash);

            return localHashStr.Equals(remoteHash);

        }

        private void moveFile(string filename, string newpath)
        {

            deleteFile(newpath);

            if (File.Exists(filename))
            {
                try
                {
                    File.Move(filename, newpath);
                    writeLine("\tMoving " + filename + " to " + newpath);
                }
                catch (Exception e)
                {
                    writeLine("\tError moving " + filename + " to " + newpath);
                    abort = true;
                    stagingValid = false;
                }

            }

        }

        private void deleteFile(string filename)
        {

            if (File.Exists(filename))
            {
                try
                {
                    File.Delete(filename);
                    writeLine("\tDeleting file: " + filename);
                }
                catch (Exception e)
                {
                    writeLine("Error deleting " + filename);
                    abort = true;
                    stagingValid = false;
                }

            }

        }

        private void downloadFileWeb(string remotePath, string localPath)
        {

            createDirectory(localPath);

            try
            {
                client.DownloadFile(remotePath, localPath);
                writeLine("\tDownloading file from: " + remotePath);
            }
            catch (WebException e)
            {
                writeLine("\t Error downloading file from :" + remotePath);
                abort = true;
                stagingValid = false;

            }

        }

        private void createDirectory(string path)
        {


            string[] localDirsOnly = path.Split('\\');

            string localDirs = "";

            for (int i = 0; i < localDirsOnly.Length - 1; i++)
            {
                localDirs += localDirsOnly[i] + "\\";
            }

            if (!localDirs.Equals(""))
            {

                if (!Directory.Exists(localDirs))
                {
                    Directory.CreateDirectory(localDirs);
                    writeLine("\tCreating directory: " + localDirs);
                }

            }

        }

        private void writeLine(string line)
        {

            if (sw == null)
            {
                return;
            }

            sw.WriteLine(line);
            sw.Flush();

        }

        public void Restart()
        {

            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();

        }



    }
}
