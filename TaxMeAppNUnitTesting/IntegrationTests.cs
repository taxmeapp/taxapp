using TaxMeApp;
using TaxMeApp.Helpers;
using TaxMeApp.models;
using TaxMeApp.viewmodels;
using TaxMeApp.views;
using LiveCharts;
using LiveCharts.Wpf;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.ObjectModel;
using System.IO;
using Moq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace TaxMeAppNUnitTesting
{
    public class IntegrationTests
    {
        public ControlViewModel cvm;
        string[] filePaths;

        [SetUp]
        public void Setup()
        {
            cvm = new ControlViewModel(new YearsModel(), null, new DataModel());

            filePaths = Directory.GetFiles("res\\TaxCSV", "*.csv");
            for (int i = 0; i < filePaths.Length; i++)
            {
                cvm.YearsModel.Years.Add(i, Parser.ParseCSV(filePaths[i]));
            }
        }

        //-------------------------------------------------------------------------------------------------
        //Integration Testing
        //-------------------------------------------------------------------------------------------------
        [Test]
        public void TestViewModelIntegration() {
            //Loader ld = new Loader();
            //MainWindow mw = new MainWindow();
            GraphViewModel gvm = new GraphViewModel();
            cvm.GraphVM = gvm;
            //cvm.Init();

            Assert.IsNotNull(gvm);
        }
    }
}