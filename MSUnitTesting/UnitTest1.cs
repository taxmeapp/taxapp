using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.IO;
using TaxMeApp;
using TaxMeApp.Helpers;
using TaxMeApp.models;
using TaxMeApp.viewmodels;
using TaxMeApp.views;

namespace MSUnitTesting
{
    [TestClass]
    public class UnitTest1
    {

        //-------------------------------------------------------------------------------------------------
        //Testing Helpers
        //-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ex() {
            Assert.AreEqual(0, 0);
        }
        //Check that numbers are formatted correctly
        [TestMethod]
        public void TestFormatter()
        {
            Assert.AreEqual(0, 0);
        }

        [TestMethod]
        public void TestLoader()
        {
            Assert.AreEqual(0, 0);
        }

        //Testing that a file is parsed correctly
        [TestMethod]
        public void TestParser()
        {
            Assert.AreEqual(0, 0);
        }

        //-------------------------------------------------------------------------------------------------
        //Testing View Models
        //-------------------------------------------------------------------------------------------------

        //ControlViewModel
        [TestMethod]
        public void TestYearChange()
        {
            Assert.AreEqual(0, 0);
        }
        //GraphViewModel
        //MainViewModel
    }
}
