using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxMeApp;
using System;
using TaxMeApp.Helpers;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace UnitTests
{
    [TestClass]
    public class UpdaterUnitTest
    {

        private string versionStr = "1.2";
        private string remoteURLStr = AppDomain.CurrentDomain.BaseDirectory + @"\remote\VersionInfo.xml";
        private string baseURLStr = AppDomain.CurrentDomain.BaseDirectory + @"\remote\";

        private string testFile1path = @".\remote\testFile1.txt";
        private string testFile2path = @".\remote\testFile2.txt";
        private string testFile3path = @".\remote\testFile3.txt";

        private Dictionary<string, string> testChecksumDictionary = new Dictionary<string, string>();

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

            Assert.IsTrue(File.Exists(@".\VersionInfo.xml"));
            
        }

        private void createRemoteFiles()
        {

            // Clear remote directory and all files if it exists
            if (Directory.Exists(@".\remote\"))
            {
                Directory.Delete(@".\remote\", true);
            }

            string remotePath = @".\remote\";
            Directory.CreateDirectory(remotePath);

            StreamWriter file = new StreamWriter(testFile1path);
            file.WriteLine("123abc");
            file.Close();

            Assert.IsTrue(File.Exists(testFile1path));

            file = new StreamWriter(testFile2path);
            file.WriteLine("456def");
            file.Close();

            Assert.IsTrue(File.Exists(testFile2path));

            file = new StreamWriter(testFile3path);
            file.WriteLine("789ghi");
            file.Close();

            Assert.IsTrue(File.Exists(testFile3path));

        }

        private void createMockRemoteVersionXML()
        {

            testChecksumDictionary.Clear();

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

            string[] files = Directory.GetFiles(baseURLStr, "*", SearchOption.AllDirectories);
            for(int i = 0; i < files.Length; i++) 
            {
                string filename = files[i].Replace(baseURLStr, String.Empty);

                XmlElement filetag = doc.CreateElement(String.Empty, "File", String.Empty);

                filetag.SetAttribute("path", filename);
                filelistXML.AppendChild(filetag);

                MD5 md5 = MD5.Create();
                var localHash = md5.ComputeHash(File.ReadAllBytes(files[i]));
                string localHashStr = BitConverter.ToString(localHash);

                XmlText checksumNode = doc.CreateTextNode(localHashStr);
                filetag.AppendChild(checksumNode);
                filelistXML.AppendChild(filetag);

                testChecksumDictionary.Add(filename, localHashStr);

            }

            manifest.AppendChild(filelistXML);

            doc.Save(remoteURLStr);

            Assert.IsTrue(File.Exists(remoteURLStr));

        }

        private bool moveFile(string filename, string newpath)
        {

            deleteFile(newpath);

            if (File.Exists(filename))
            {
                File.Move(filename, newpath);
                Assert.IsTrue(File.Exists(newpath));
                return true;
            }

            Assert.IsFalse(File.Exists(filename));

            return false;

        }

        private bool deleteFile(string filename)
        {

            if (File.Exists(filename))
            {
                File.Delete(filename);
                return true;
            }

            Assert.IsFalse(File.Exists(filename));

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

        private void cleanDirectory(string directory)
        {

            // Clean given directory
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
            Assert.IsFalse(Directory.Exists(directory));

        }

        [TestMethod]
        public void GetRemoteManifest()
        {

            Assert.IsNotNull(updaterPrivateObj);

            // Method purpose:
            // Download XML file from remote location (specified in remoteURL), put in staging\VersionInfo.XML
            // Staging folder gets automatically created if it doesn't already exist
            // Read the XML file from staging\VersionInfo.XML, parse the BaseRemoteURL
            // Generate the <filename, checksum> dictionary based on filelist present in XML file

            // Need to create a mock remote environment from which the files will be downloaded
            cleanDirectory(@".\remote\");

            // Set our remoteURL on our updater, ensure it is correct
            updaterPrivateObj.SetField("remoteURL", remoteURLStr);
            Assert.AreEqual(remoteURLStr, updaterPrivateObj.GetField("remoteURL"));

            // load XML from staging\VersionInfo.xml

            // Test the exception: no .xml to download
            updaterPrivateObj.Invoke("getRemoteManifest");
            Assert.IsTrue((bool)updaterPrivateObj.GetField("abort"));
            Assert.IsFalse((bool)updaterPrivateObj.GetField("stagingValid"));   // this is a side effect of failing download check
            updaterPrivateObj.SetField("abort", false);
            updaterPrivateObj.SetField("stagingValid", true);

            // Try to read "baseRemoteURL" tag

            // Test the exception: no "baseRemoteURL" tag in our XML file
            // Create a blank remote VersionInfo.xml
            Directory.CreateDirectory(@".\remote\");
            File.Create(@".\remote\VersionInfo.xml").Close();
            updaterPrivateObj.Invoke("getRemoteManifest");
            // If can't read (i.e. tag doesn't exist in document), abort = true
            Assert.IsTrue((bool)updaterPrivateObj.GetField("abort"));
            updaterPrivateObj.SetField("abort", false);
            deleteFile(@".\remote\VersionInfo.xml");

            // Read a valid remote VersionInfo.XML from remoteURL with proper baseRemoteURL tag

            // Create remote files
            createRemoteFiles();

            // Generate valid remote VersionInfo.XML
            createMockRemoteVersionXML();

            // Read it successfully
            updaterPrivateObj.Invoke("getRemoteManifest");
            Assert.IsFalse((bool)updaterPrivateObj.GetField("abort"));

            // Generate file hash dictionary from remote XML
            // Builds a dictionary comprised of <filename, checksum> pairs
            Dictionary<string, string> updaterChecksumDictionary = (Dictionary<string, string>)updaterPrivateObj.GetField("fileHashDictionary");
            Assert.IsNotNull(updaterChecksumDictionary);
            Assert.AreEqual(testChecksumDictionary.Count, updaterChecksumDictionary.Count);

            // Check for the validity of items in the dictionary against ones we have generated
            foreach(KeyValuePair<string, string> kvPair in testChecksumDictionary)
            {
                Assert.IsTrue(updaterChecksumDictionary.ContainsKey(kvPair.Key));
                Assert.AreEqual(kvPair.Value, updaterChecksumDictionary[kvPair.Key]);
            }

        }

        [TestMethod]
        public void DownloadUpdates()
        {

            Assert.IsNotNull(updaterPrivateObj);

            // Method purpose:
            // Iterate through each KeyValuePair in our built Dictionary of <filename, checksum>
            // Check if the file exists - if it does, check if the checksum of the local file matches given checksum
            // If either of those are false, download the remote file into local staging
            // If any file is downloaded, updated variable turns to true


            // First, need to mock our remote environment, and have already loaded remote XML infomation into the updater

            // Ensure we are creating a fresh remote directory
            cleanDirectory(@".\remote\");
            Directory.CreateDirectory(@".\remote\");
            Assert.IsTrue(Directory.Exists(@".\remote"));

            // Create remote files
            createRemoteFiles();

            // Generate valid remote VersionInfo.XML
            createMockRemoteVersionXML();

            XmlDocument remoteVersionXML = new XmlDocument();
            remoteVersionXML.Load(remoteURLStr);

            // Invoke the updater's method to generate dictionary
            updaterPrivateObj.Invoke("generateFileHashDict", remoteVersionXML);

            // Ensure that our content matches
            Dictionary<string, string> updaterChecksumDictionary = (Dictionary<string, string>)updaterPrivateObj.GetField("fileHashDictionary");
            Assert.IsNotNull(updaterChecksumDictionary);
            Assert.AreEqual(testChecksumDictionary.Count, updaterChecksumDictionary.Count);

            // Check for the validity of items in the dictionary against ones we have generated
            foreach (KeyValuePair<string, string> kvPair in testChecksumDictionary)
            {
                Assert.IsTrue(updaterChecksumDictionary.ContainsKey(kvPair.Key));
                Assert.AreEqual(kvPair.Value, updaterChecksumDictionary[kvPair.Key]);
            }

            // Clean staging
            cleanDirectory(@".\staging\");

            // We want to ensure that we are downloading the three test files. So we can safe-delete them from main directory:
            deleteFile(testFile1path.Replace(@".\remote\", String.Empty));
            deleteFile(testFile2path.Replace(@".\remote\", String.Empty));
            deleteFile(testFile3path.Replace(@".\remote\", String.Empty));

            // Set our base remote URL
            updaterPrivateObj.SetField("baseRemoteURL", baseURLStr);

            // Now, we are ready to test our download ability for files which do not exist
            updaterPrivateObj.Invoke("downloadUpdates");

            // The three test files should now be in the staging folder
            Assert.IsTrue(File.Exists(testFile1path.Replace(@"\remote\", @"\staging\")));
            Assert.IsTrue(File.Exists(testFile2path.Replace(@"\remote\", @"\staging\")));
            Assert.IsTrue(File.Exists(testFile3path.Replace(@"\remote\", @"\staging\")));
            // The 'update' bool should now be true
            Assert.IsTrue((bool)updaterPrivateObj.GetField("updated"));

            // Reset update bool
            updaterPrivateObj.SetField("updated", false);

            // Need to move the staged files into their final location in order to test the other pathways:
            updaterPrivateObj.Invoke("clearStaging", true);

            // Testing the updater's ability to not download files that exist and are up-to-date:
            // Run the download logic again - nothing should download
            updaterPrivateObj.Invoke("downloadUpdates");
            Assert.IsFalse(File.Exists(testFile1path.Replace(@"\remote\", @"\staging\")));
            Assert.IsFalse(File.Exists(testFile2path.Replace(@"\remote\", @"\staging\")));
            Assert.IsFalse(File.Exists(testFile3path.Replace(@"\remote\", @"\staging\")));
            // The 'update' bool should still be false
            Assert.IsFalse((bool)updaterPrivateObj.GetField("updated"));

            // Testing the updater's ability to download only files that are not up-to-date

            // Invalidate our local file (easiest way, otherwise would have to update remoteXML and etc)
            StreamWriter file = new StreamWriter(testFile1path.Replace(@".\remote\", String.Empty));
            file.WriteLine("123abcinvalidateme");
            file.Close();

            // Run the updater
            updaterPrivateObj.Invoke("downloadUpdates");
            // It should download file1 (checksum mismatch), but not file2 or file3 (unchanged)
            Assert.IsTrue(File.Exists(testFile1path.Replace(@"\remote\", @"\staging\")));
            Assert.IsFalse(File.Exists(testFile2path.Replace(@"\remote\", @"\staging\")));
            Assert.IsFalse(File.Exists(testFile3path.Replace(@"\remote\", @"\staging\")));
            // The 'update' bool should now be true
            Assert.IsTrue((bool)updaterPrivateObj.GetField("updated"));

            // Reset update bool
            updaterPrivateObj.SetField("updated", false);

        }

        [TestMethod]
        public void ValidateStaging()
        {

            Assert.IsNotNull(updaterPrivateObj);

            // Method purpose:
            // Get a list of all files that we have in staging
            // If a file isn't contained in our Dictionary (e.g. VersionInfo.xml), skip it
            // Else compare checksum with our Dictionary (that we generated from remote VersionXML)
            // If any are invalid, it turns "stagingValid" bool to false and returns
            // If any files end with .exe, a backup method is initiated (but we will test this in its own TestMethod)

            // First, need to mock our remote environment, and have already loaded remote XML infomation into the updater

            // Ensure we are creating a fresh remote directory
            cleanDirectory(@".\remote\");
            Directory.CreateDirectory(@".\remote\");
            Assert.IsTrue(Directory.Exists(@".\remote\"));

            // Create remote files
            createRemoteFiles();

            // Generate valid remote VersionInfo.XML
            createMockRemoteVersionXML();

            XmlDocument remoteVersionXML = new XmlDocument();
            remoteVersionXML.Load(remoteURLStr);

            // Invoke the updater's method to generate dictionary
            updaterPrivateObj.Invoke("generateFileHashDict", remoteVersionXML);

            // We want to ensure that we are downloading the three test files. So we can safe-delete them from main directory:
            deleteFile(testFile1path.Replace(@".\remote\", String.Empty));
            deleteFile(testFile2path.Replace(@".\remote\", String.Empty));
            deleteFile(testFile3path.Replace(@".\remote\", String.Empty));

            // Set our base remote URL
            updaterPrivateObj.SetField("baseRemoteURL", baseURLStr);

            // Invoke the updater's download method to get the test files into staging
            updaterPrivateObj.Invoke("downloadUpdates");

            // The three test files should now be in the staging folder
            Assert.IsTrue(File.Exists(testFile1path.Replace(@"\remote\", @"\staging\")));
            Assert.IsTrue(File.Exists(testFile2path.Replace(@"\remote\", @"\staging\")));
            Assert.IsTrue(File.Exists(testFile3path.Replace(@"\remote\", @"\staging\")));

            // Invoke the updater's validation method
            // These three files should pass the checksum checks
            updaterPrivateObj.Invoke("validateStaging");
            Assert.IsTrue((bool)updaterPrivateObj.GetField("stagingValid"));

            // Add in a file to staging that isn't in our dictionary - this should not change the staging validity
            File.Create(@".\staging\VersionInfo.xml").Close();
            // Invoke the updater's validation method
            // These three files should pass the checksum checks
            updaterPrivateObj.Invoke("validateStaging");
            Assert.IsTrue((bool)updaterPrivateObj.GetField("stagingValid"));

            // Change the contents of one of the staging files - this should now fail staging validity (because checksum will be different)
            deleteFile(testFile1path.Replace(@"\remote\", @"\staging\"));
            StreamWriter file = new StreamWriter(testFile1path.Replace(@".\remote\", @".\staging\"));
            file.WriteLine("123abcinvalidateme");
            file.Close();

            // Invoke the updater's validation method
            // Should now fail validation
            updaterPrivateObj.Invoke("validateStaging");
            Assert.IsFalse((bool)updaterPrivateObj.GetField("stagingValid"));
            // Reset stagingValid bool
            updaterPrivateObj.SetField("stagingValid", true);


        }

        [TestMethod]
        public void BackupEXE()
        {

            Assert.IsNotNull(updaterPrivateObj);

            // Method purpose:
            // Given a valid filepath with .exe extension, delete previous backup and move current .exe to .bak
            // Errors will only come about via moveFile method (abort = true, stagingValid = false)

            // Ensure we are creating a fresh backupTest directory
            cleanDirectory(@".\backupTest\");
            Directory.CreateDirectory(@".\backupTest\");
            Assert.IsTrue(Directory.Exists(@".\backupTest\"));

            // Pass in nonexistant file - should not nothing
            string path = @".\backupTest\doesnotexist.txt";
            deleteFile(path);
            Assert.IsFalse(File.Exists(path));
            updaterPrivateObj.Invoke("backupEXE", path);
            Assert.IsFalse((bool)updaterPrivateObj.GetField("abort"));
            Assert.IsTrue((bool)updaterPrivateObj.GetField("stagingValid"));

            // Pass in file that exists, but is not an .exe
            path = @".\backupTest\notanexe.txt";
            File.Create(path).Close();
            Assert.IsTrue(File.Exists(path));
            updaterPrivateObj.Invoke("backupEXE", path);
            Assert.IsFalse((bool)updaterPrivateObj.GetField("abort"));
            Assert.IsTrue((bool)updaterPrivateObj.GetField("stagingValid"));
            deleteFile(path);

            // Pass in .exe
            path = @".\backupTest\isanexe.exe";
            File.Create(path).Close();
            Assert.IsTrue(File.Exists(path));
            updaterPrivateObj.Invoke("backupEXE", path);
            Assert.IsFalse((bool)updaterPrivateObj.GetField("abort"));
            Assert.IsTrue((bool)updaterPrivateObj.GetField("stagingValid"));
            // .bak should exist, .exe should not
            Assert.IsTrue(File.Exists(path.Replace(".exe", ".bak")));
            Assert.IsFalse(File.Exists(path));

            // clean our directory
            cleanDirectory(@".\backupTest\");

        }

        [TestMethod]
        public void ClearStaging()
        {

            Assert.IsNotNull(updaterPrivateObj);

            // Method purpose:
            // If argument is true, move all the files from staging to their new home
            // Either way, clear the staging folder (delete all files inside and delete the folder)
            // If staging folder does not exist, do nothing

        }

        [TestMethod]
        public void GenerateFileHashDict()
        {

            Assert.IsNotNull(updaterPrivateObj);

            // Method purpose:
            // Given an XmlDocument as an argument, generate <filename, checksum> KeyValuePairs

            // If a document with no <File> tags is passed, nothing adverse should happen
            XmlDocument emptyDoc = new XmlDocument();
            updaterPrivateObj.Invoke("generateFileHashDict", emptyDoc);

            // Get the remote dictionary, clear it, and add example one item to test that it clears
            Dictionary<string, string> remoteDictionary = (Dictionary<string, string>)updaterPrivateObj.GetField("fileHashDictionary");
            remoteDictionary.Clear();
            remoteDictionary.Add("testKey", "testVal");
            Assert.IsTrue(remoteDictionary.Count == 1);
            Assert.IsTrue(remoteDictionary.ContainsKey("testKey"));

            // Ensure the dictionary outcome is the same as is expected - generate an XmlDoc to use as arg, save values into local dictionary

            Dictionary<string, string> expectedDictionary = new Dictionary<string, string>();

            XmlDocument testXmlDoc = new XmlDocument();

            XmlElement manifest = testXmlDoc.CreateElement(String.Empty, "Manifest", String.Empty);
            testXmlDoc.AppendChild(manifest);

            XmlElement filelistXML = testXmlDoc.CreateElement(String.Empty, "FileList", String.Empty);

            string fileName = "testfile1";
            string checksum = "testfile1checksum";
            expectedDictionary.Add(fileName, checksum);

            XmlElement filetag = testXmlDoc.CreateElement(String.Empty, "File", String.Empty);
            filetag.SetAttribute("path", fileName);

            XmlText checksumNode = testXmlDoc.CreateTextNode(checksum);
            filetag.AppendChild(checksumNode);

            filelistXML.AppendChild(filetag);

            fileName = "testfile2";
            checksum = "testfile2checksum";
            expectedDictionary.Add(fileName, checksum);

            filetag = testXmlDoc.CreateElement(String.Empty, "File", String.Empty);
            filetag.SetAttribute("path", fileName);

            checksumNode = testXmlDoc.CreateTextNode(checksum);
            filetag.AppendChild(checksumNode);

            filelistXML.AppendChild(filetag);
            
            manifest.AppendChild(filelistXML);

            // Invoke the updater's method to generate dictionary
            updaterPrivateObj.Invoke("generateFileHashDict", testXmlDoc);
            // Expected and remote dictionaries are equal in count
            Assert.AreEqual(expectedDictionary.Count, remoteDictionary.Count);
            // Our two keys that should be in there:
            Assert.IsTrue(remoteDictionary.ContainsKey("testfile1"));
            Assert.IsTrue(remoteDictionary.ContainsKey("testfile2"));
            // Our key that should have been cleared:
            Assert.IsFalse(remoteDictionary.ContainsKey("testKey"));

        }

        [TestMethod]
        public void MoveFile()
        {

            Assert.IsNotNull(updaterPrivateObj);

            // Method purpose:
            // Safely moves a file from one path to another
            // Safely deletes destination file (tested in another method - this test will assume newPath is valid)
            // Checks if requested file exists (if not, do nothing)
            // Tries the move the file, which should have no concflicts with the destination location
            // If any errors occur, abort = true and stagingValid = false

            // Ensure we are creating a fresh backupTest directory
            cleanDirectory(@".\moveTest\");
            Directory.CreateDirectory(@".\moveTest\");
            Assert.IsTrue(Directory.Exists(@".\moveTest\"));

            string beforePath = @".\moveTest\testBefore.txt";
            string afterPath = @".\moveTest\testAfter.txt";

            // Given file does not exist
            updaterPrivateObj.Invoke("moveFile", beforePath, afterPath);
            Assert.IsFalse(File.Exists(beforePath));
            Assert.IsFalse(File.Exists(afterPath));
            Assert.IsTrue((bool)updaterPrivateObj.GetField("stagingValid"));
            Assert.IsFalse((bool)updaterPrivateObj.GetField("abort"));

            // Given file exists, but is open (should trigger an error on move)
            FileStream beforeFile = File.Create(beforePath);
            updaterPrivateObj.Invoke("moveFile", beforePath, afterPath);
            beforeFile.Close();
            Assert.IsTrue(File.Exists(beforePath));
            Assert.IsFalse(File.Exists(afterPath));
            Assert.IsFalse((bool)updaterPrivateObj.GetField("stagingValid"));
            Assert.IsTrue((bool)updaterPrivateObj.GetField("abort"));
            deleteFile(beforePath);
            updaterPrivateObj.SetField("stagingValid", true);
            updaterPrivateObj.SetField("abort", false);

            // Given file exists, but newPath still exists (because it was kept open)
            File.Create(beforePath).Close();
            FileStream afterFile = File.Create(afterPath);
            updaterPrivateObj.Invoke("moveFile", beforePath, afterPath);
            afterFile.Close();
            Assert.IsTrue(File.Exists(beforePath));
            Assert.IsTrue(File.Exists(afterPath));
            Assert.IsFalse((bool)updaterPrivateObj.GetField("stagingValid"));
            Assert.IsTrue((bool)updaterPrivateObj.GetField("abort"));
            deleteFile(beforePath);
            deleteFile(afterPath);
            updaterPrivateObj.SetField("stagingValid", true);
            updaterPrivateObj.SetField("abort", false);

            // Given file exists, and is successfully moved
            File.Create(beforePath).Close();
            File.Create(afterPath).Close();
            updaterPrivateObj.Invoke("moveFile", beforePath, afterPath);
            Assert.IsFalse(File.Exists(beforePath));
            Assert.IsTrue(File.Exists(afterPath));
            Assert.IsTrue((bool)updaterPrivateObj.GetField("stagingValid"));
            Assert.IsFalse((bool)updaterPrivateObj.GetField("abort"));
            deleteFile(beforePath);
            deleteFile(afterPath);

            cleanDirectory(@".\moveTest\");

        }

        [TestMethod]
        public void DeleteFile()
        {

            Assert.IsNotNull(updaterPrivateObj);

            // Method purpose:
            // Safely delete given file (checks if it exists)

            // Ensure we are creating a fresh deleteTest directory
            cleanDirectory(@".\deleteTest\");
            Directory.CreateDirectory(@".\deleteTest\");
            Assert.IsTrue(Directory.Exists(@".\deleteTest\"));

            string deletePath = @".\deleteTest\testDelete.txt";

            // If given file does not exist, do nothing
            updaterPrivateObj.Invoke("deleteFile", deletePath);
            Assert.IsTrue((bool)updaterPrivateObj.GetField("stagingValid"));
            Assert.IsFalse((bool)updaterPrivateObj.GetField("abort"));

            // If given file exists and is open, will throw exception (abort = true, stagingValid = false)
            FileStream deleteFileStream = File.Create(deletePath);
            updaterPrivateObj.Invoke("deleteFile", deletePath);
            Assert.IsFalse((bool)updaterPrivateObj.GetField("stagingValid"));
            Assert.IsTrue((bool)updaterPrivateObj.GetField("abort"));
            deleteFileStream.Close();
            deleteFile(deletePath);
            updaterPrivateObj.SetField("stagingValid", true);
            updaterPrivateObj.SetField("abort", false);

            // If given file exists and throws no error, it will be deleted
            File.Create(deletePath).Close();
            updaterPrivateObj.Invoke("deleteFile", deletePath);
            Assert.IsTrue((bool)updaterPrivateObj.GetField("stagingValid"));
            Assert.IsFalse((bool)updaterPrivateObj.GetField("abort"));
            Assert.IsFalse(File.Exists(deletePath));

            cleanDirectory(@".\deleteTest\");

        }

        [TestMethod]
        public void DownloadFileWeb()
        {

            Assert.IsNotNull(updaterPrivateObj);

            // Method purpose:
            // First, call safe createDirectory to ensure destination path exists (not tested here)
            // Then, try to download a file from remote path
            // Can be a web path or file path
            // Exception causes abort = true and stagingValid = false

            // Ensure we are creating a fresh downloadTest directory (location of file to download)
            cleanDirectory(@".\downloadTest\");
            Directory.CreateDirectory(@".\downloadTest\");
            Assert.IsTrue(Directory.Exists(@".\downloadTest\"));
            // Clean where we want the file to end up
            cleanDirectory(@".\destinationTest\");
            Assert.IsFalse(Directory.Exists(@".\destinationTest\"));

            File.Create(@".\downloadTest\TestDownload.txt").Close();
            Assert.IsTrue(File.Exists(@".\downloadTest\TestDownload.txt"));

            updaterPrivateObj.Invoke("downloadFileWeb", @".\downloadTest\TestDownload.txt", @".\destinationTest\TestDownload.txt");
            // This is a download, not a move - both files still exist
            Assert.IsTrue(File.Exists(@".\downloadTest\TestDownload.txt"));
            Assert.IsTrue(File.Exists(@".\destinationTest\TestDownload.txt"));
            Assert.IsTrue((bool)updaterPrivateObj.GetField("stagingValid"));
            Assert.IsFalse((bool)updaterPrivateObj.GetField("abort"));

            // Now to cause the exception

            // Delete both directories
            cleanDirectory(@".\downloadTest\");
            cleanDirectory(@".\destinationTest\");
            // The origin path no longer exists
            updaterPrivateObj.Invoke("downloadFileWeb", @".\downloadTest\TestDownload.txt", @".\destinationTest\TestDownload.txt");
            // Neither file should exist
            Assert.IsFalse(File.Exists(@".\downloadTest\TestDownload.txt"));
            Assert.IsFalse(File.Exists(@".\destinationTest\TestDownload.txt"));
            // Both bools are triggered
            Assert.IsFalse((bool)updaterPrivateObj.GetField("stagingValid"));
            Assert.IsTrue((bool)updaterPrivateObj.GetField("abort"));
            // Reset bools
            updaterPrivateObj.SetField("stagingValid", true);
            updaterPrivateObj.SetField("abort", false);

            cleanDirectory(@".\destinationTest\");

        }


        [TestMethod]
        public void CreateDirectory()
        {

            Assert.IsNotNull(updaterPrivateObj);

            // Method purpose:
            // This method is given a file path, and will create all directories/subdirs needed for it
            // Abstracts the need to remove the fileName from the path - otherwise it will also become a directory

            // Ensure we're working with a clean environment
            cleanDirectory(@".\creationPath\");
            Assert.IsFalse(Directory.Exists(@".\creationPath\"));

            updaterPrivateObj.Invoke("createDirectory", @".\creationPath\subdir1\subdir2\testfile.txt");
            Assert.IsTrue(Directory.Exists(@".\creationPath\"));
            Assert.IsTrue(Directory.Exists(@".\creationPath\subdir1\"));
            Assert.IsTrue(Directory.Exists(@".\creationPath\subdir1\subdir2\"));
            // This doesn't actually make the file - also ensure it doesn't make a directory with the filename!
            Assert.IsFalse(Directory.Exists(@".\creationPath\subdir1\subdir2\testfile.txt"));
            Assert.IsFalse(File.Exists(@".\creationPath\subdir1\subdir2\testfile.txt"));

            cleanDirectory(@".\creationPath\");

            // Passing just a filename (with no directory) should do nothing
            updaterPrivateObj.Invoke("createDirectory", @"testfile.txt");
            Assert.IsFalse(Directory.Exists(@"testfile.txt"));
            Assert.IsFalse(File.Exists(@"testfile.txt"));


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
            moveFile("VersionInfo.bak", "VersionInfo.xml");

            // We cannot assert that the .xml exists, but we know for sure the .bak should not
            Assert.IsFalse(File.Exists("VersionInfo.bak"));

            if (Directory.Exists(@".\remote\"))
            {
                Directory.Delete(@".\remote\", true);
            }

            if (Directory.Exists(@".\staging\"))
            {
                Directory.Delete(@".\staging\", true);
            }

            // Delete our test files
            deleteFile(testFile1path.Replace(@".\remote\", String.Empty));
            deleteFile(testFile2path.Replace(@".\remote\", String.Empty));
            deleteFile(testFile3path.Replace(@".\remote\", String.Empty));

        }



    }
}
