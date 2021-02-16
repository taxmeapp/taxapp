using System;
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
        private bool updated = false;
        private bool stagingValid = true;
        private bool abort = false;

        private Dictionary<string, string> fileHashDictionary = new Dictionary<string, string>();

        private WebClient client = new WebClient();

        private StreamWriter sw;

        // re-enabled graphVM series.clear

        public void Update()
        {

            clearUpdateLog();

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
                    sw.WriteLine("Staging valid.");
                    RestartRequired = true;
                    clearStaging(true);
                }
                else
                {
                    sw.WriteLine("Staging invalid.");
                    //clearStaging(false);
                }

            }
            else
            {
                clearStaging(true);
                sw.WriteLine("No update required.");
            }

            sw.Close();

        }

        // Updating process:

        private void clearUpdateLog()
        {

            sw = new StreamWriter("UpdateLog.txt");

            DateTime dateTime = DateTime.Now;

            sw.WriteLine("Update initiated at " + dateTime.ToString());

        }

        private void readLocalManifest()
        {

            try
            {

                sw.WriteLine("Reading local manifest...");

                XmlDocument local = new XmlDocument();
                local.Load(@".\VersionInfo.xml");

                remoteURL = local.GetElementsByTagName("RemoteURL")[0].InnerText;

                sw.WriteLine("\tRemoteURL from Local Manifest: " + remoteURL);

            }
            catch (Exception e)
            {
                sw.WriteLine("Error reading local manifest.");
                abort = true;
                return;
            }

        }

        private void getRemoteManifest()
        {

            sw.WriteLine("Getting remote manifest...");

            downloadFileWeb(remoteURL, @"staging\VersionInfo.xml");

            XmlDocument remote = new XmlDocument();

            try
            {
                remote.Load(@"staging\VersionInfo.xml");
            }
            catch (Exception e)
            {
                sw.WriteLine("\tError in loading remote manifest.");
                abort = true;
                return;
            }

            try
            {
                // Update base URL
                baseRemoteURL = remote.GetElementsByTagName("BaseRemoteURL")[0].InnerText;
                sw.WriteLine("\tBase Remote URL: " + baseRemoteURL);
            }
            catch (Exception e)
            {
                sw.WriteLine("\tError in reading Base Remote URL");
                abort = true;
                return;
            }

            // Build filename : hashmap dictionary
            generateFileHashDict(remote);

        }

        private void downloadUpdates()
        {

            sw.WriteLine("Files requiring updates:");

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

            sw.WriteLine("Validating staged files:");

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

                    sw.WriteLine("\t" + file + " has failed staging checks.");
                    stagingValid = false;
                    return;
                }

                sw.WriteLine("\t" + fileName);

                if (fileName.EndsWith(".exe"))
                {
                    backupEXE(fileName);
                }

            }

        }

        private void backupEXE(string path)
        {

            if (!File.Exists(path))
            {
                return;
            }

            string newFileName = path.Replace(".exe", ".bak");

            deleteFile(newFileName);

            try
            {
                File.Move(path, newFileName);
                sw.WriteLine("\tMoving " + path + " to " + newFileName);
            }
            catch (Exception e)
            {
                sw.WriteLine("Error moving " + path);
                abort = true;
                stagingValid = false;
            }
            

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

                sw.WriteLine("Moving contents of staging folder...");

                foreach (string file in filesInStaging)
                {

                    string newPath = file.Replace(@"staging\", String.Empty);
                    createDirectory(newPath);
                    deleteFile(newPath);
                    File.Move(file, newPath);
                    sw.WriteLine("\tMoving " + file + " to " + newPath);

                }
            }
            else
            {
                sw.WriteLine("Clearing staging folder...");
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

        private void deleteFile(string path)
        {

            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                    sw.WriteLine("\tDeleting file: " + path);
                }
                catch (Exception e)
                {
                    sw.WriteLine("Error deleting " + path);
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
                sw.WriteLine("\tDownloading file from: " + remotePath);
            }
            catch (WebException e)
            {
                sw.WriteLine("\t Error downloading file from :" + remotePath);
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
                    sw.WriteLine("\tCreating directory: " + localDirs);
                }

            }

        }

        private void flushSW()
        {

            if (sw != null)
            {
                sw.Flush();
            }

        }

        public void Restart()
        {

            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();

        }



    }
}
