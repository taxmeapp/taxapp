using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxMeApp.Helpers;
using TaxMeApp.models;
using TaxMeApp.viewmodels;

namespace UnitTests
{
    [TestClass]
    public class IntegrationTests
    {

        private ControlViewModel controlVM;
        private OutputViewModel outputVM;
        private DataViewModel dataVM;

        private YearsModel yearsModel;
        private DataModel dataModel;
        private GraphModel graphModel;

        private string[] filePaths;

        [TestInitialize]
        public void Setup()
        {
            // Create VMs
            controlVM = new ControlViewModel();
            outputVM = new OutputViewModel();
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
            controlVM.GraphModel = graphModel;

            filePaths = Directory.GetFiles("res\\TaxCSV", "*.csv");
            for (int i = 0; i < filePaths.Length; i++)
            {
                controlVM.YearsModel.Years.Add(i, Parser.ParseCSV(filePaths[i]));
            }
        }

        //-------------------------------------------------------------------------------------------------
        //  Integration Testing
        //-------------------------------------------------------------------------------------------------

        /*
                ControlViewModel
        */


        // ControlVM: Testing the getter logic for initial year in the dropdown box
        [TestMethod]
        public void TestControlVMInit()
        {

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

        // ControlVM: Testing the getters/setters and their interactions with models
        [TestMethod]
        public void TestControlVMtoModels()
        {



            // YearList (only getter)
            // The getter in the model is currently set up to generate a new list on each Get
            // Therefore "AreSame" won't work here - need to compare contents
            List<int> yearListFromControlVM = controlVM.YearList;
            List<int> yearListFromModel = yearsModel.YearList;
            // Should be same size
            Assert.AreEqual(yearListFromControlVM.Count, yearListFromModel.Count);
            // Compare each item
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


            // Booleans for checkboxes:

            // ShowNumberOfReturns 
            // Getter
            Assert.AreEqual(graphModel.ShowNumberOfReturns, controlVM.ShowNumberOfReturns);
            // Setter
            controlVM.ShowNumberOfReturns = !controlVM.ShowNumberOfReturns;
            Assert.AreEqual(graphModel.ShowNumberOfReturns, controlVM.ShowNumberOfReturns);

            // ShowOldRevenue
            // Getter
            Assert.AreEqual(graphModel.ShowOldRevenue, controlVM.ShowOldRevenue);
            // Setter
            controlVM.ShowOldRevenue = !controlVM.ShowOldRevenue;
            Assert.AreEqual(graphModel.ShowOldRevenue, controlVM.ShowOldRevenue);

            // ShowNewRevenue
            // Getter
            Assert.AreEqual(graphModel.ShowNewRevenue, controlVM.ShowNewRevenue);
            // Setter
            controlVM.ShowNewRevenue = !controlVM.ShowNewRevenue;
            Assert.AreEqual(graphModel.ShowNewRevenue, controlVM.ShowNewRevenue);

            // ShowOldPercentage
            // Getter
            Assert.AreEqual(graphModel.ShowOldPercentage, controlVM.ShowOldPercentage);
            // Setter
            controlVM.ShowOldPercentage = !controlVM.ShowOldPercentage;
            Assert.AreEqual(graphModel.ShowOldPercentage, controlVM.ShowOldPercentage);

            // ShowNewPercentage
            // Getter
            Assert.AreEqual(graphModel.ShowNewPercentage, controlVM.ShowNewPercentage);
            // Setter
            controlVM.ShowNewPercentage = !controlVM.ShowNewPercentage;
            Assert.AreEqual(graphModel.ShowNewPercentage, controlVM.ShowNewPercentage);


        }


        /*
                OutputViewModel 
        */ 

        // OutputVM: Testing the getters and their interactions with models
        [TestMethod]
        public void TestOutputVMtoModels()
        {

            // All values are passed through the formatter for display

            //NumPovertyPopOutput

            //NumMaxPopOutput

            //TotalRevenueOldOutput

            //TotalRevenueNewOutput

            //RevenueDifferenceOutput

        }

    }
}
