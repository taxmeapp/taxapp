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

        [Test]
        public void TestControlVMInit()
        {

            // Testing the getter logic for ControlVM's initial year in the dropdown box

            // When ControlInit is called, the SelectedYear should be set to the first item of the generated List of keys
            controlVM.ControlInit();
            
            // Pull a copy from ControlVM's stored key value, as well as from the list of keys directly
            IncomeYearModel testOutputFromInit;
            IncomeYearModel testOutputFromList;
            yearsModel.Years.TryGetValue(controlVM.SelectedYear, out testOutputFromInit);
            yearsModel.Years.TryGetValue(yearsModel.YearList[0], out testOutputFromList);

            // Neither of the objects should be null
            Assert.IsNotNull(testOutputFromInit);
            Assert.IsNotNull(testOutputFromList);

            // They should also be the same object
            Assert.AreSame(testOutputFromInit, testOutputFromList);

        }

        [Test]
        public void TestVMtoModelInteraction()
        {

            // Testing the getters/setters and their interactions with models

            // YearList (only getter)
            // The getter in the model is currently set up to generate a new list on each Get
            // Therefore "AreSame" won't work here - need to compare contents
            List<int> yearListFromControlVM = controlVM.YearList;
            List<int> yearListFromModel = yearsModel.YearList;
            // Should be same size
            Assert.AreEqual(yearListFromControlVM.Count, yearListFromModel.Count);
            for (int i = 0; i < yearListFromModel.Count; i++)
            {
                Assert.AreEqual(yearListFromModel[i], yearListFromControlVM[i]);
            }

            // SelectedYear
            // Getter
            Assert.AreEqual(yearsModel.SelectedYear, controlVM.SelectedYear);
            // Setter
            controlVM.SelectedYear = yearsModel.YearList[1];
            Assert.AreEqual(yearsModel.SelectedYear, controlVM.SelectedYear);


            // MaxTaxRate
            // Getter
            Assert.AreEqual(dataModel.MaxTaxRate, controlVM.MaxTaxRate);
            // Setter
            controlVM.MaxTaxRate = 20;
            Assert.AreEqual(dataModel.MaxTaxRate, controlVM.MaxTaxRate);


            // MaxBracketCount - Just for the displayed value above slider
            // Getter
            Assert.AreEqual(graphModel.MaxBracketCount, controlVM.MaxBracketCount);
            // Setter is only used to update displayed value, no other interaction

            // MaxBracketCountSlider - The actual manipulation done via slider movement
            // Getter - Setting slider position when calculations change the underlying value
            Assert.AreEqual(graphModel.MaxBracketCount, controlVM.MaxBracketCountSlider);
            // Setter - On slider adjustment
            controlVM.MaxBracketCountSlider = 3;
            Assert.AreEqual(graphModel.MaxBracketCount, controlVM.MaxBracketCountSlider);


            // ShowNumberOfReturns

            // ShowOldRevenue

            // ShowNewRevenue

            // ShowOldPercentage

            // ShowNewPercentage



        }


    }
}