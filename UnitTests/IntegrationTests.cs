using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using LiveCharts.Configurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxMeApp;
using TaxMeApp.Helpers;
using TaxMeApp.models;
using TaxMeApp.viewmodels;

namespace UnitTests
{
    [TestClass]
    public class IntegrationTests
    {

        private MainViewModel mainVM;
        private ControlViewModel controlVM;
        private OutputViewModel outputVM;
        private DataViewModel dataVM;
        private GraphViewModel graphVM;
        private SettingsViewModel settingsVM;

        private YearsModel yearsModel;
        private DataModel dataModel;
        private GraphModel graphModel;
        private TaxPlansModel taxPlansModel;
        private OptionsModel optionsModel;

        [TestInitialize]
        public void Setup()
        {
            // Create VMs
            mainVM = new MainViewModel();
            dataVM = new DataViewModel();
            controlVM = new ControlViewModel();
            outputVM = new OutputViewModel();
            graphVM = new GraphViewModel();
            settingsVM = new SettingsViewModel();

            // Link VMs to VMs
            mainVM.DataVM = dataVM;
            mainVM.SettingsVM = settingsVM;

            dataVM.ControlVM = controlVM;
            dataVM.GraphVM = graphVM;
            dataVM.OutputVM = outputVM;

            controlVM.DataVM = dataVM;
            controlVM.MainVM = mainVM;
            controlVM.OutputVM = outputVM;
            controlVM.GraphVM = graphVM;

            outputVM.ControlVM = controlVM;
            outputVM.DataVM = dataVM;

            settingsVM.MainVM = mainVM;

            // Create models
            yearsModel = new YearsModel();
            dataModel = new DataModel();
            graphModel = new GraphModel();
            taxPlansModel = new TaxPlansModel();
            optionsModel = new OptionsModel();

            // Link models to VMs

            dataVM.YearsModel = yearsModel;
            dataVM.DataModel = dataModel;
            dataVM.GraphModel = graphModel;
            dataVM.TaxPlansModel = taxPlansModel;
            dataVM.OptionsModel = optionsModel;

            controlVM.YearsModel = yearsModel;
            controlVM.DataModel = dataModel;
            controlVM.GraphModel = graphModel;
            controlVM.TaxPlansModel = taxPlansModel;
            controlVM.OptionsModel = optionsModel;

            outputVM.DataModel = dataModel;
            outputVM.OptionsModel = optionsModel;

            graphVM.YearsModel = yearsModel;
            graphVM.GraphModel = graphModel;
            graphVM.DataModel = dataModel;
            graphVM.TaxPlansModel = taxPlansModel;
            graphVM.OptionsModel = optionsModel;

            string[] filePaths = Directory.GetFiles("res\\TaxCSV", "*.csv");
            for (int i = 0; i < filePaths.Length; i++)
            {

                IncomeYearModel year = Parser.ParseCSV(filePaths[i]);
                yearsModel.Years.Add(year.Year, year);

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
        public void TC105_TestControlVMInit()
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
        public void TC106_TestControlVMtoModels()
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

        [TestMethod]
        public void TC107_TestControlVMSettingsClick()
        {

            // Testing the click button logic
            // These steps work for private methods
            //PrivateObject obj = new PrivateObject(controlVM);
            //object sender = "";
            //obj.Invoke("resetSettingsButtonClick", null, null);

            Loader ld = new Loader();
            ld.ControlVM.resetSettingsButtonClick();

            Assert.AreEqual(0, mainVM.TabSelected);

        }


        /*
                TODO: GraphViewModel 
        */

        [TestMethod]
        public void TC108_TestGraphVMInit()
        {

            // Certain global variables of the graph are initialized here
            graphVM.GraphInit();

            // Not yet sure how or what to test here, but it probably needs something.

        }

        [TestMethod]
        public void TC109_TestGraphVMClear()
        {


        }

        [TestMethod]
        public void TC110_TestGraphVMAddColumn()
        {


        }

        [TestMethod]
        public void TC111_TestGraphVMAddLine()
        {


        }

        [TestMethod]
        public void TC112_TestGraphVMGraphAllChecked()
        {



        }

        /*
                OutputViewModel 
        */

        // OutputVM: Testing the getters and their interactions with models
        [TestMethod]
        public void TC113_TestOutputVMtoModels()
        {

            // All values are passed through the formatter for display

            //NumPovertyPopOutput
            Assert.AreEqual(Formatter.Format(dataModel.NumPovertyPop), outputVM.NumPovertyPopOutput);

            //NumMaxPopOutput
            Assert.AreEqual(Formatter.Format(dataModel.NumMaxPop), outputVM.NumMaxPopOutput);

            //TotalRevenueOldOutput
            Assert.AreEqual(Formatter.Format(dataModel.TotalRevenueOld), outputVM.TotalRevenueOldOutput);

            //TotalRevenueNewOutput
            Assert.AreEqual(Formatter.Format(dataModel.TotalRevenueNew), outputVM.TotalRevenueNewOutput);

            //RevenueDifferenceOutput
            Assert.AreEqual(Formatter.Format(dataModel.RevenueDifference), outputVM.RevenueDifferenceOutput);

        }


        /*
                DataViewModel 
        */

        [TestMethod]
        public void TC114_TestDataVMInit()
        {

            // Make sure our VMs are not null
            Assert.IsNotNull(dataVM);
            Assert.IsNotNull(controlVM);
            Assert.IsNotNull(graphVM);

            // Run our init, which simply calls two other inits:
            dataVM.DataInit();

            /*
                    ControlVM.ControlInit
                    This is tested explicitly above, but it doesn't hurt to test it as its called from DataVM
            */
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

            /*
                    GraphVM.GraphInit()
            */

            // Still TODO


        }

        [TestMethod]
        public void TC115_TestDataVMTotalRecalculation()
        {

            // Test which compares hand calculations to computer's logic

            // DataVM takes data from models -> runs calculations -> stores in models
            // We can manipulate data in models -> run calculations -> check resulting data in models against hand calculations
            // Select year -> Run Calculations -> Compare Results -> Select another year -> Run Calculations again -> Compare results

            /*
                    Select 2018 @ 20% max tax rate, poverty line @ 3
            */

            Loader ld = new Loader();

            //ld.YearsModel.SelectedYear = 2018;
            //ld.DataModel.MaxTaxRate = 20;
            ld.ControlVM.SelectedYear = 2018;
            ld.ControlVM.MaxTaxRate = 20;
            ld.ControlVM.PovertyLineIndexSlider = 3;
            ld.ControlVM.MaxBracketCountSlider = 7;

            // Currently there is no setter to change this in the model, but if it becomes dynamic later, we can set it to test calculated values
            //graphModel.PovertyLineIndex = 3;
            Assert.IsNotNull(ld.YearsModel.SelectedIncomeYearModel);

            // On new year selection:
            // TotalRecalculation()

            ld.DataVM.TotalRecalculation();

            // calculatePopulation()
            // clear DataModel.Population
            // Iterate through YearsModel.SelectedIncomeYearModel.Brackets
            // Add to DataModel.Population

            // Check a few population values in bracket
            Assert.AreEqual(3135, ld.DataModel.Population[0]);
            Assert.AreEqual(136176, ld.DataModel.Population[1]);
            Assert.AreEqual(177974, ld.DataModel.Population[2]);

            // countUnderPoverty()
            // Count the number of returns up to and including the designated poverty bracket
            Assert.AreEqual(2876697, ld.DataModel.NumPovertyPop);

            // determineBaselineMaxBracketCount()
            // Starting at the end of array, work backwards
            // as long as our max population count is below our poverty population count
            Assert.AreEqual(7, ld.GraphModel.MaxBracketCount);

            // countPopulationAtMaxRate()
            // Starting at the end of array, work backwards and count population for brackets at max rate
            Assert.AreEqual(8538562, ld.DataModel.NumMaxPop);

            // calculateOldTaxData()
            // Get the actual value of income tax paid (values stored as 1000 in CSV column G, multiplied for data storage), saved per bracket
            // Test a few brackets:
            Assert.AreEqual(156305000L, ld.DataModel.OldRevenueByBracket[1]);
            Assert.AreEqual(42202000L, ld.DataModel.OldRevenueByBracket[2]);
            Assert.AreEqual(391728000L, ld.DataModel.OldRevenueByBracket[3]);
            // Get the percent of income gross, saved per bracket (pulled straight from column H in CSV)
            // Test a few brackets:
            Assert.AreEqual(4.3, ld.DataModel.OldTaxPctByBracket[1]);
            Assert.AreEqual(3.2, ld.DataModel.OldTaxPctByBracket[2]);
            Assert.AreEqual(1.1, ld.DataModel.OldTaxPctByBracket[3]);
            // Get the total amount of revenue paid (sum of column F in CSV)
            Assert.AreEqual(1509751196000L, ld.DataModel.TotalRevenueOld);


            // calculateNewTaxData();
            // This is currently set up for just slant tax
            // Three things are calculated: NewRevenueByBracket, NewTaxPctByBracket, TotalRevenueNew
            // Poverty and below pay 0% tax
            // Middle brackets increment for each bracket until max-rate-bracket
            // Max-rate-brackets pay max rate % tax
            // Increment = maxTaxRate / (selectedBrackets.Count - maxBracketCount - (povertyBrackets + 1) + 1) , rounded to 1 decimal place
            // Thus, the need to set maxTaxRate for testing as above
            // Increment = 2.2% at this MaxTaxRate (20%) w/ 7 brackets

            // dataModel.NewRevenueByBracket
            // Test some brackets to ensure calculations are correct
            Assert.AreEqual(0, ld.DataModel.NewRevenueByBracket[1]);
            Assert.AreEqual(568496698, ld.DataModel.NewRevenueByBracket[4]);
            Assert.AreEqual(407188046495, ld.DataModel.NewRevenueByBracket[11]);
            // dataModel.NewTaxPctByBracket
            Assert.AreEqual(0, ld.DataModel.NewTaxPctByBracket[1]);
            Assert.AreEqual(2.2, ld.DataModel.NewTaxPctByBracket[4]);
            // This was failing without the rounds, even though they were equal. Rounds pass
            Assert.AreEqual(Math.Round(17.6, 1), Math.Round(ld.DataModel.NewTaxPctByBracket[11], 1));

            // dataModel.TotalRevenueNew
            Assert.AreEqual(1504645232320, ld.DataModel.TotalRevenueNew);


            /*
                Select 2016 @ 30% max tax rate, poverty line @ 3
            */

            //yearsModel.SelectedYear = 2016;
            //ld.DataModel.MaxTaxRate = 30;
            ld.ControlVM.SelectedYear = 2016;
            ld.ControlVM.MaxTaxRate = 30;
            ld.ControlVM.MaxBracketCountSlider = 7;
            ld.ControlVM.PovertyLineIndexSlider = 3;

            // Currently there is no setter to change this in the model, but if it becomes dynamic later, we can set it to test calculated values
            //graphModel.PovertyLineIndex = 3;
            Assert.IsNotNull(ld.ControlVM.YearsModel.SelectedIncomeYearModel);

            ld.DataVM.TotalRecalculation();

            // calculatePopulation()
            // clear DataModel.Population
            // Iterate through YearsModel.SelectedIncomeYearModel.Brackets
            // Add to DataModel.Population

            // Check a few population values in bracket
            Assert.AreEqual(6163, ld.DataModel.Population[0]);
            Assert.AreEqual(166102, ld.DataModel.Population[1]);
            Assert.AreEqual(1815136, ld.DataModel.Population[2]);

            // countUnderPoverty()
            // Count the number of returns up to and including the designated poverty bracket
            Assert.AreEqual(6313075, ld.DataModel.NumPovertyPop);

            // determineBaselineMaxBracketCount()
            // Starting at the end of array, work backwards
            // as long as our max population count is below our poverty population count
            Assert.AreEqual(7, ld.GraphModel.MaxBracketCount);

            // countPopulationAtMaxRate()
            // Starting at the end of array, work backwards and count population for brackets at max rate
            Assert.AreEqual(6888588, ld.DataModel.NumMaxPop);

            // calculateOldTaxData()
            // Get the actual value of income tax paid (values stored as 1000 in CSV column G, multiplied for data storage), saved per bracket
            // Test a few brackets:
            Assert.AreEqual(28462000L, ld.DataModel.OldRevenueByBracket[1]);
            Assert.AreEqual(349076000L, ld.DataModel.OldRevenueByBracket[2]);
            Assert.AreEqual(1395130000L, ld.DataModel.OldRevenueByBracket[3]);
            // Get the percent of income gross, saved per bracket (pulled straight from column H in CSV)
            // Test a few brackets:
            Assert.AreEqual(5.4, ld.DataModel.OldTaxPctByBracket[1]);
            Assert.AreEqual(2.4, ld.DataModel.OldTaxPctByBracket[2]);
            Assert.AreEqual(2.5, ld.DataModel.OldTaxPctByBracket[3]);
            // Get the total amount of revenue paid (sum of column F in CSV)
            Assert.AreEqual(1426595311000L, ld.DataModel.TotalRevenueOld);


            // calculateNewTaxData();
            // This is currently set up for just slant tax
            // Three things are calculated: NewRevenueByBracket, NewTaxPctByBracket, TotalRevenueNew
            // Poverty and below pay 0% tax
            // Middle brackets increment for each bracket until max-rate-bracket
            // Max-rate-brackets pay max rate % tax
            // Increment = maxTaxRate / (selectedBrackets.Count - maxBracketCount - (povertyBrackets + 1) + 1) , rounded to 1 decimal place
            // Thus, the need to set maxTaxRate for testing as above
            // Increment = 3.3% at this MaxTaxRate (30%) w/ 7 brackets

            // dataModel.NewRevenueByBracket
            // Test some brackets to ensure calculations are correct
            Assert.AreEqual(0, ld.DataModel.NewRevenueByBracket[1]);
            Assert.AreEqual(1199077110, ld.DataModel.NewRevenueByBracket[4]);
            Assert.AreEqual(505941636024, ld.DataModel.NewRevenueByBracket[11]);
            // dataModel.NewTaxPctByBracket
            Assert.AreEqual(0, ld.DataModel.NewTaxPctByBracket[1]);
            Assert.AreEqual(3.3, ld.DataModel.NewTaxPctByBracket[4]);
            // This was failing without the rounds, even though they were equal. Rounds pass
            Assert.AreEqual(Math.Round(26.4, 1), Math.Round(ld.DataModel.NewTaxPctByBracket[11], 1));

            // dataModel.TotalRevenueNew
            Assert.AreEqual(1831780103651, ld.DataModel.TotalRevenueNew);


        }

        [TestMethod]
        public void TC116_TestDataVMNewDataRecalculation()
        {

            // DataVM takes data from models -> runs calculations -> stores in models
            // We can manipulate data in models -> run calculations -> check resulting data in models against hand calculations
            // Select year -> Run Calculations -> Change rate -> Compare results -> Change bracket count -> Run Calculations again -> Compare results

            Loader ld = new Loader();

            ld.DataVM.YearsModel.SelectedYear = 2017;
            ld.DataModel.MaxTaxRate = 20;


            // Currently there is no setter to change this in the model, but if it becomes dynamic later, we can set it to test calculated values
            //graphModel.PovertyLineIndex = 3;

            ld.GraphModel.PovertyLineIndex = 3;
            Assert.IsNotNull(ld.YearsModel.SelectedIncomeYearModel);
            //dataVM.TotalRecalculation();
            ld.DataVM.TotalRecalculation(); 


            /*
                    Change rate to 30%
            */

            ld.DataModel.MaxTaxRate = 30;
            ld.DataVM.TotalRecalculation();

            //dataModel.MaxTaxRate = 30;
            //dataVM.NewDataRecalcuation();

            // Count how many people are at max tax rate
            // countPopulationAtMaxRate();

            Assert.AreEqual(0, ld.DataModel.NumMaxPop);
            Assert.AreEqual(6380813, ld.DataModel.NumPovertyPop);

            // Calculate how much tax revenue was generated under new plan
            // calculateNewTaxData();
            // This is currently set up for just slant tax
            // Three things are calculated: NewRevenueByBracket, NewTaxPctByBracket, TotalRevenueNew
            // Poverty and below pay 0% tax
            // Middle brackets increment for each bracket until max-rate-bracket
            // Max-rate-brackets pay max rate % tax
            // Increment = maxTaxRate / (selectedBrackets.Count - maxBracketCount - (povertyBrackets + 1) + 1) , rounded to 1 decimal place
            // Thus, the need to set maxTaxRate for testing as above
            // Increment = 3.3% at this MaxTaxRate (30%) w/ 7 brackets

            // dataModel.NewRevenueByBracket
            // Test some brackets to ensure calculations are correct
            Assert.AreEqual(0, ld.DataModel.NewRevenueByBracket[1]);
            Assert.AreEqual(694994122, ld.DataModel.NewRevenueByBracket[4]);
            Assert.AreEqual(308889577192, ld.DataModel.NewRevenueByBracket[11]);
            // dataModel.NewTaxPctByBracket
            Assert.AreEqual(0, ld.DataModel.NewTaxPctByBracket[1]);
            Assert.AreEqual(1.9, ld.DataModel.NewTaxPctByBracket[4]);
            // This was failing without the rounds, even though they were equal. Rounds pass
            Assert.AreEqual(Math.Round(15.2, 1), Math.Round(ld.DataModel.NewTaxPctByBracket[11], 1));

            // dataModel.TotalRevenueNew
            Assert.AreEqual(1292646674666, ld.DataModel.TotalRevenueNew);

            /*
                    Change max brackets to 8
            */

            ld.GraphModel.MaxBracketCount = 8;
            ld.DataVM.NewDataRecalcuation();

            // Count how many people are at max tax rate
            // countPopulationAtMaxRate();
            Assert.AreEqual(27493446, ld.DataModel.NumMaxPop);

            // Calculate how much tax revenue was generated under new plan
            // calculateNewTaxData();
            // This is currently set up for just slant tax
            // Three things are calculated: NewRevenueByBracket, NewTaxPctByBracket, TotalRevenueNew
            // Poverty and below pay 0% tax
            // Middle brackets increment for each bracket until max-rate-bracket
            // Max-rate-brackets pay max rate % tax
            // Increment = maxTaxRate / (selectedBrackets.Count - maxBracketCount - (povertyBrackets + 1) + 1) , rounded to 1 decimal place
            // Thus, the need to set maxTaxRate for testing as above
            // Increment = 3.8% at this MaxTaxRate (30%) and 8 brackets

            // dataModel.NewRevenueByBracket
            // Test some brackets to ensure calculations are correct
            Assert.AreEqual(0, ld.DataModel.NewRevenueByBracket[1]);
            Assert.AreEqual(1389988244, ld.DataModel.NewRevenueByBracket[4]);
            Assert.AreEqual(609650481300, ld.DataModel.NewRevenueByBracket[11]);
            // dataModel.NewTaxPctByBracket
            Assert.AreEqual(0, ld.DataModel.NewTaxPctByBracket[1]);
            Assert.AreEqual(3.8, ld.DataModel.NewTaxPctByBracket[4]);
            Assert.AreEqual(30, ld.DataModel.NewTaxPctByBracket[11]);

            // dataModel.TotalRevenueNew
            Assert.AreEqual(2162582174037, ld.DataModel.TotalRevenueNew);

        }

        

    }
}
