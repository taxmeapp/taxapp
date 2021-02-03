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
        private ControlViewModel controlVM;
        private DataViewModel dataVM;

        private YearsModel yearsModel;
        private DataModel dataModel;
        private GraphModel graphModel;

        private string[] filePaths;

        [SetUp]
        public void Setup()
        {
            // Create VMs
            controlVM = new ControlViewModel();
            //dataVM = new DataViewModel();


            // Link VMs to VMs
            //controlVM.DataVM = dataVM;


            // Create models
            yearsModel = new YearsModel();
            dataModel = new DataModel();
            graphModel = new GraphModel();

            // Link models to VMs
            controlVM.YearsModel = yearsModel;
            controlVM.DataModel = dataModel;

            filePaths = Directory.GetFiles("res\\TaxCSV", "*.csv");
            for (int i = 0; i < filePaths.Length; i++)
            {
                controlVM.YearsModel.Years.Add(i, Parser.ParseCSV(filePaths[i]));
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
            controlVM.GraphVM = gvm;
            //cvm.Init();

            Assert.IsNotNull(gvm);
        }

        /*
                ControlViewModel
        */


    }
}