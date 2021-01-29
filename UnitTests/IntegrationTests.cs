using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using LiveCharts.Configurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        private YearsModel yearsModel;
        private DataModel dataModel;
        private GraphModel graphModel;

        [TestInitialize]
        public void Setup()
        {
            // Create VMs
            mainVM = new MainViewModel();
            dataVM = new DataViewModel();
            controlVM = new ControlViewModel();
            outputVM = new OutputViewModel();
            graphVM = new GraphViewModel();


            // Link VMs to VMs
            dataVM.ControlVM = controlVM;
            dataVM.GraphVM = graphVM;
            //controlVM.DataVM = dataVM;
            controlVM.MainVM = mainVM;

            // Create models
            yearsModel = new YearsModel();
            dataModel = new DataModel();
            graphModel = new GraphModel();

            // Link models to VMs

            dataVM.YearsModel = yearsModel;
            dataVM.DataModel = dataModel;
            dataVM.GraphModel = graphModel;

            controlVM.YearsModel = yearsModel;
            controlVM.DataModel = dataModel;
            controlVM.GraphModel = graphModel;

            outputVM.DataModel = dataModel;

            graphVM.YearsModel = yearsModel;
            graphVM.GraphModel = graphModel;
            graphVM.DataModel = dataModel;

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

        [TestMethod]
        public void TestControlVMSettingsClick()
        {

            // Testing the click button logic
            // These steps work for private methods
            PrivateObject obj = new PrivateObject(controlVM);
            object sender = "";
            obj.Invoke("settingsButtonClick", sender);
            Assert.AreEqual(1, mainVM.TabSelected);

        }


        /*
                TODO: GraphViewModel 
        */

        [TestMethod]
        public void TestGraphVMInit()
        {

            // Certain global variables of the graph are initialized here
            graphVM.GraphInit();

            // Not yet sure how or what to test here, but it probably needs something.

        }

        [TestMethod]
        public void TestGraphVMClear()
        {


        }

        [TestMethod]
        public void TestGraphVMAddColumn()
        {


        }

        [TestMethod]
        public void TestGraphVMAddLine()
        {


        }

        [TestMethod]
        public void TestGraphVMGraphAllChecked()
        {



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
        public void TestDataVMTotalRecalculation()
        {

            // Test which compares hand calculations to computer's logic

            // DataVM takes data from models -> runs calculations -> stores in models
            // We can manipulate data in models -> run calculations -> check resulting data in models against hand calculations
            // Select year -> Run Calculations -> Compare Results -> Select another year -> Run Calculations again -> Compare results

            /*
                    Select 2018 @ 20% max tax rate, poverty line @ 3
            */                      

            yearsModel.SelectedYear = 2018;
            dataModel.MaxTaxRate = 20;
            // Currently there is no setter to change this in the model, but if it becomes dynamic later, we can set it to test calculated values
            //graphModel.PovertyLineIndex = 3;
            Assert.IsNotNull(yearsModel.SelectedIncomeYearModel);

            // On new year selection:
            // TotalRecalculation()

            dataVM.TotalRecalculation();

            // calculatePopulation()
            // clear DataModel.Population
            // Iterate through YearsModel.SelectedIncomeYearModel.Brackets
            // Add to DataModel.Population

            // Check a few population values in bracket
            Assert.AreEqual(dataModel.Population[0], 3135);
            Assert.AreEqual(dataModel.Population[1], 136176);
            Assert.AreEqual(dataModel.Population[2], 177974);

            // countUnderPoverty()
            // Count the number of returns up to and including the designated poverty bracket
            Assert.AreEqual(dataModel.NumPovertyPop, 2876697);

            // determineBaselineMaxBracketCount()
            // Starting at the end of array, work backwards
            // as long as our max population count is below our poverty population count
            Assert.AreEqual(graphModel.MaxBracketCount, 7);

            // countPopulationAtMaxRate()
            // Starting at the end of array, work backwards and count population for brackets at max rate
            Assert.AreEqual(dataModel.NumMaxPop, 8538562);

            // calculateOldTaxData()
            // Get the actual value of income tax paid (values stored as 1000 in CSV column G, multiplied for data storage), saved per bracket
            // Test a few brackets:
            Assert.AreEqual(dataModel.OldRevenueByBracket[1], 156305000L);
            Assert.AreEqual(dataModel.OldRevenueByBracket[2], 42202000L);
            Assert.AreEqual(dataModel.OldRevenueByBracket[3], 391728000L);
            // Get the percent of income gross, saved per bracket (pulled straight from column H in CSV)
            // Test a few brackets:
            Assert.AreEqual(dataModel.OldTaxPctByBracket[1], 4.3);
            Assert.AreEqual(dataModel.OldTaxPctByBracket[2], 3.2);
            Assert.AreEqual(dataModel.OldTaxPctByBracket[3], 1.1);
            // Get the total amount of revenue paid (sum of column F in CSV)
            Assert.AreEqual(dataModel.TotalRevenueOld, 1509751196000L);


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
            Assert.AreEqual(dataModel.NewRevenueByBracket[1], 0);
            Assert.AreEqual(dataModel.NewRevenueByBracket[4], 568496698);
            Assert.AreEqual(dataModel.NewRevenueByBracket[11], 407188046495);
            // dataModel.NewTaxPctByBracket
            Assert.AreEqual(dataModel.NewTaxPctByBracket[1], 0);
            Assert.AreEqual(dataModel.NewTaxPctByBracket[4], 2.2);
            // This was failing without the rounds, even though they were equal. Rounds pass
            Assert.AreEqual(Math.Round(dataModel.NewTaxPctByBracket[11], 1), Math.Round(17.6, 1));

            // dataModel.TotalRevenueNew
            Assert.AreEqual(dataModel.TotalRevenueNew, 1504645232320);


            /*
                Select 2016 @ 30% max tax rate, poverty line @ 3
            */

            yearsModel.SelectedYear = 2016;
            dataModel.MaxTaxRate = 30;
            // Currently there is no setter to change this in the model, but if it becomes dynamic later, we can set it to test calculated values
            //graphModel.PovertyLineIndex = 3;
            Assert.IsNotNull(yearsModel.SelectedIncomeYearModel);

            dataVM.TotalRecalculation();

            // calculatePopulation()
            // clear DataModel.Population
            // Iterate through YearsModel.SelectedIncomeYearModel.Brackets
            // Add to DataModel.Population

            // Check a few population values in bracket
            Assert.AreEqual(dataModel.Population[0], 6163);
            Assert.AreEqual(dataModel.Population[1], 166102);
            Assert.AreEqual(dataModel.Population[2], 1815136);

            // countUnderPoverty()
            // Count the number of returns up to and including the designated poverty bracket
            Assert.AreEqual(dataModel.NumPovertyPop, 6313075);

            // determineBaselineMaxBracketCount()
            // Starting at the end of array, work backwards
            // as long as our max population count is below our poverty population count
            Assert.AreEqual(graphModel.MaxBracketCount, 7);

            // countPopulationAtMaxRate()
            // Starting at the end of array, work backwards and count population for brackets at max rate
            Assert.AreEqual(dataModel.NumMaxPop, 6888588);

            // calculateOldTaxData()
            // Get the actual value of income tax paid (values stored as 1000 in CSV column G, multiplied for data storage), saved per bracket
            // Test a few brackets:
            Assert.AreEqual(dataModel.OldRevenueByBracket[1], 28462000L);
            Assert.AreEqual(dataModel.OldRevenueByBracket[2], 349076000L);
            Assert.AreEqual(dataModel.OldRevenueByBracket[3], 1395130000L);
            // Get the percent of income gross, saved per bracket (pulled straight from column H in CSV)
            // Test a few brackets:
            Assert.AreEqual(dataModel.OldTaxPctByBracket[1], 5.4);
            Assert.AreEqual(dataModel.OldTaxPctByBracket[2], 2.4);
            Assert.AreEqual(dataModel.OldTaxPctByBracket[3], 2.5);
            // Get the total amount of revenue paid (sum of column F in CSV)
            Assert.AreEqual(dataModel.TotalRevenueOld, 1426595311000L);


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
            Assert.AreEqual(dataModel.NewRevenueByBracket[1], 0);
            Assert.AreEqual(dataModel.NewRevenueByBracket[4], 1199077110);
            Assert.AreEqual(dataModel.NewRevenueByBracket[11], 505941636024);
            // dataModel.NewTaxPctByBracket
            Assert.AreEqual(dataModel.NewTaxPctByBracket[1], 0);
            Assert.AreEqual(dataModel.NewTaxPctByBracket[4], 3.3);
            // This was failing without the rounds, even though they were equal. Rounds pass
            Assert.AreEqual(Math.Round(dataModel.NewTaxPctByBracket[11], 1), Math.Round(26.4, 1));

            // dataModel.TotalRevenueNew
            Assert.AreEqual(dataModel.TotalRevenueNew, 1831780103651);


        }

        [TestMethod]
        public void TestDataVMNewDataRecalculation()
        {

            // DataVM takes data from models -> runs calculations -> stores in models
            // We can manipulate data in models -> run calculations -> check resulting data in models against hand calculations
            // Select year -> Run Calculations -> Change rate -> Compare results -> Change bracket count -> Run Calculations again -> Compare results

            yearsModel.SelectedYear = 2017;
            dataModel.MaxTaxRate = 20;
            // Currently there is no setter to change this in the model, but if it becomes dynamic later, we can set it to test calculated values
            //graphModel.PovertyLineIndex = 3;
            Assert.IsNotNull(yearsModel.SelectedIncomeYearModel);
            dataVM.TotalRecalculation();

            
            /*
                    Change rate to 30%
            */ 

            dataModel.MaxTaxRate = 30;
            dataVM.NewDataRecalcuation();

            // Count how many people are at max tax rate
            // countPopulationAtMaxRate();
            Assert.AreEqual(dataModel.NumMaxPop, 7706856);

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
            Assert.AreEqual(dataModel.NewRevenueByBracket[1], 0);
            Assert.AreEqual(dataModel.NewRevenueByBracket[4], 1207095054);
            Assert.AreEqual(dataModel.NewRevenueByBracket[11], 536492423544);
            // dataModel.NewTaxPctByBracket
            Assert.AreEqual(dataModel.NewTaxPctByBracket[1], 0);
            Assert.AreEqual(dataModel.NewTaxPctByBracket[4], 3.3);
            // This was failing without the rounds, even though they were equal. Rounds pass
            Assert.AreEqual(Math.Round(dataModel.NewTaxPctByBracket[11], 1), Math.Round(26.4, 1));

            // dataModel.TotalRevenueNew
            Assert.AreEqual(dataModel.TotalRevenueNew, 2022975533276);

            /*
                    Change max brackets to 8
            */

            graphModel.MaxBracketCount = 8;
            dataVM.NewDataRecalcuation();

            // Count how many people are at max tax rate
            // countPopulationAtMaxRate();
            Assert.AreEqual(dataModel.NumMaxPop, 27493446);

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
            Assert.AreEqual(dataModel.NewRevenueByBracket[1], 0);
            Assert.AreEqual(dataModel.NewRevenueByBracket[4], 1389988244);
            Assert.AreEqual(dataModel.NewRevenueByBracket[11], 609650481300);
            // dataModel.NewTaxPctByBracket
            Assert.AreEqual(dataModel.NewTaxPctByBracket[1], 0);
            Assert.AreEqual(dataModel.NewTaxPctByBracket[4], 3.8);
            Assert.AreEqual(dataModel.NewTaxPctByBracket[11], 30);

            // dataModel.TotalRevenueNew
            Assert.AreEqual(dataModel.TotalRevenueNew, 2162582174037);

        }

        [TestMethod]
        public void TestDataVMInit()
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

    }
}
