using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxMeApp;
using System;
using TaxMeApp.Helpers;
using System.IO;
using System.Diagnostics;
using System.Xml;

namespace UnitTests
{
    [TestClass]
    public class UpdaterUnitTest
    {

        private string versionStr = "1.2";
        private string remoteURLStr = "remote";
        private string baseURLStr = "base";

        private AutoUpdater updater;
        private PrivateObject obj;

        [TestInitialize]
        public void TestInit()
        {
            updater = new AutoUpdater();
            obj = new PrivateObject(updater);


            // This probably necessitates rewriting how the updater deals with the updatelog file


            // back up the old updatelog

            
            // Create new update log
            obj.Invoke("clearUpdateLog");
            Assert.IsTrue(File.Exists("UpdateLog.txt"));

            // restore the old update



            // local
            // back up the old versioninfo.xml
            moveFile(@".\VersionInfo.xml", @".\VersionInfo.bak");
            Assert.IsTrue(File.Exists(@".\VersionInfo.bak"));

            // make a new versioninfo.xml
            createMockLocalVersionXML();
            Assert.IsTrue(File.Exists(@".\VersionInfo.xml"));

            // mock remote


            // restore the old local versioninfo.xml

            moveFile(@".\VersionInfo.bak", @".\VersionInfo.xml");
            Assert.IsTrue(File.Exists(@".\VersionInfo.xml"));

        }
        
        private void createMockLocalVersionXML()
        {
            XmlDocument doc = new XmlDocument();

            XmlElement manifest = doc.CreateElement(String.Empty, "Manifest", String.Empty);
            manifest.SetAttribute("version", versionStr);
            doc.AppendChild(manifest);

            XmlElement remoteURL = doc.CreateElement(String.Empty, "RemoteURL", String.Empty);
            XmlText remoteURLtxt = doc.CreateTextNode(remoteURLStr);
            remoteURL.AppendChild(remoteURLtxt);
            manifest.AppendChild(remoteURL);

            XmlElement baseRemoteURL = doc.CreateElement(String.Empty, "BaseRemoteURL", String.Empty);
            XmlText baseRemotetxt = doc.CreateTextNode(baseURLStr);
            baseRemoteURL.AppendChild(baseRemotetxt);
            manifest.AppendChild(baseRemoteURL);

            XmlElement filelistXML = doc.CreateElement(String.Empty, "FileList", String.Empty);

            manifest.AppendChild(filelistXML);

            doc.Save(@".\VersionInfo.xml");
            
        }

        private void moveFile(string filename, string newpath)
        {

            deleteFile(newpath);

            if (File.Exists(filename))
            {
                File.Move(filename, newpath);
            }

        }

        private void deleteFile(string filename)
        {

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

        }

        [TestMethod]
        public void ReadLocalManifest()
        {

            Assert.IsNotNull(obj);

            obj.Invoke("readLocalManifest");

            string fromLocalVersionXML = (string)obj.GetField("remoteURL");

            Assert.AreEqual(remoteURLStr, fromLocalVersionXML);

            obj.Invoke("flushSW");

        } 



    }
}
