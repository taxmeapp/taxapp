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
        public ControlViewModel ControlVM;
        string[] filePaths;

        [SetUp]
        public void Setup()
        {
            ControlVM = new ControlViewModel();
            YearsModel yearsModel = new YearsModel();
            DataModel dataModel = new DataModel();
            ControlVM.YearsModel = yearsModel;
            ControlVM.DataModel = dataModel;

            filePaths = Directory.GetFiles("res\\TaxCSV", "*.csv");
            for (int i = 0; i < filePaths.Length; i++)
            {
                ControlVM.YearsModel.Years.Add(i, Parser.ParseCSV(filePaths[i]));
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
            ControlVM.GraphVM = gvm;
            //cvm.Init();

            Assert.IsNotNull(gvm);
        }
    }
}