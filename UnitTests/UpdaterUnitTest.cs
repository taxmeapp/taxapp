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
        private PrivateObject updaterPrivateObj;

        [TestInitialize]
        public void TestInit()
        {
            updater = new AutoUpdater();
            updaterPrivateObj = new PrivateObject(updater);


            // This probably necessitates rewriting how the updater deals with the updatelog file


            // back up the old updatelog

            
            // Create new update log
            //updaterPrivateObj.Invoke("clearUpdateLog");
            //Assert.IsTrue(File.Exists("UpdateLog.txt"));

            // restore the old update



            // local


            // make a new versioninfo.xml
            //createMockLocalVersionXML();
            //Assert.IsTrue(File.Exists(@".\VersionInfo.xml"));

            // mock remote



        }


        private void backupLocalVersionXML()
        {

            // back up the old versioninfo.xml
            moveFile(@".\VersionInfo.xml", @".\VersionInfo.bak");
            Assert.IsTrue(File.Exists(@".\VersionInfo.bak"));

        }

        private void restoreLocalVersionXML()
        {


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

        private bool moveFile(string filename, string newpath)
        {

            deleteFile(newpath);

            if (File.Exists(filename))
            {
                File.Move(filename, newpath);
                return true;
            }

            return false;

        }

        private bool deleteFile(string filename)
        {

            if (File.Exists(filename))
            {
                File.Delete(filename);
                return true;
            }

            return false;

        }



        [TestMethod]
        public void InitUpdateLog()
        {

            Assert.IsNotNull(updaterPrivateObj);

            // back up UpdateLog and UpdateLogLast if they exist
            moveFile("UpdateLog.txt", "UpdateLog.bak");
            moveFile("UpdateLogLast.txt", "UpdateLogLast.bak");

            // We can now assert that these files don't exist - but the backups may or may not
            Assert.IsFalse(File.Exists("UpdateLog.txt"));
            Assert.IsFalse(File.Exists("UpdateLogLast.txt"));

            // If UpdateLog.txt exists, it moves it to "UpdateLogLast.txt"
            // Create an "UpdateLog.txt" to test this move, and immediately close the filestream
            File.Create("UpdateLog.txt").Close();
            Assert.IsTrue(File.Exists("UpdateLog.txt"));

            updaterPrivateObj.Invoke("initUpdateLog");

            // Now we should have an UpdateLogLast.txt
            Assert.IsTrue(File.Exists("UpdateLogLast.txt"));
            // And if the SW is not null, we should have an UpdateLog.txt
            // If there was exception thrown in the making of this SW, the updater will still work
            StreamWriter sw = (StreamWriter)updaterPrivateObj.GetField("sw");
            if (sw != null)
            {
                Assert.IsTrue(File.Exists("UpdateLog.txt"));
            }

        }


        [TestMethod]
        public void ReadLocalManifest()
        {

            Assert.IsNotNull(updaterPrivateObj);

            // Back up existing VersionInfo.xml
            moveFile("VersionInfo.xml", "VersionInfo.bak");
            // The .xml is sure to not exist, but the .bak may or may not
            Assert.IsFalse(File.Exists("VersionInfo.xml"));

            // Test the exception: no VersionInfo.xml to read, abort should be true
            updaterPrivateObj.Invoke("readLocalManifest");
            Assert.IsTrue((bool)updaterPrivateObj.GetField("abort"));
            // Reset the field to false
            updaterPrivateObj.SetField("abort", false);

            // Test the exception: VersionInfo.xml doesn't have a remoteURL tag
            File.Create("VersionInfo.xml").Close();
            updaterPrivateObj.Invoke("readLocalManifest");
            Assert.IsTrue((bool)updaterPrivateObj.GetField("abort"));
            // Reset the field to false
            updaterPrivateObj.SetField("abort", false);

            // Create our own valid VersionXML to successful read from
            createMockLocalVersionXML();

            // We should definitely have this file now
            Assert.IsTrue(File.Exists("VersionInfo.xml"));
            
            updaterPrivateObj.Invoke("readLocalManifest");

            // Read remoteURL from it
            string fromLocalVersionXML = (string)updaterPrivateObj.GetField("remoteURL");
            // It should match the one we wrote to the XML
            Assert.AreEqual(remoteURLStr, fromLocalVersionXML);
            // And abort should be false
            Assert.IsFalse((bool)updaterPrivateObj.GetField("abort"));

        } 

        [TestCleanup]
        public void Restoration()
        {

            StreamWriter sw = (StreamWriter)updaterPrivateObj.GetField("sw");
            if (sw != null)
            {
                sw.Close();
            }

            // Restore UpdateLog and UpdateLogLast if they were backed up
            moveFile("UpdateLog.bak", "UpdateLog.txt");
            moveFile("UpdateLogLast.bak", "UpdateLogLast.txt");

            // We cannot assert that the .txt files exist (because the backups may not have existed)
            // But we can assert that the backups no longer exist
            Assert.IsFalse(File.Exists("UpdateLog.bak"));
            Assert.IsFalse(File.Exists("UpdateLogLast.bak"));


            // Restore VersionXML
            moveFile("VersionInfo.xml", "VersionInfo.bak");

            // We cannot assert that the .xml exists, but we know for sure the .bak should not
            Assert.IsFalse(File.Exists("VersionInfo.bak"));



        }



    }
}
