using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using TaxMeApp;
using TaxMeApp.Helpers;
using TaxMeApp.models;
using TaxMeApp.viewmodels;

namespace UnitTests
{
    [TestClass]
    public class ViewModelUnitTest
    {
        public IncomeYearModel testData2003;

        public void setUpData() {
            List<long> grossIncomes = new List<long>();
            List<long> taxableIncomes = new List<long>();
            List<long> incomeTaxes = new List<long>();
            List<int> numReturns = new List<int>();
            List<double> avgTax = new List<double>();
            List<double> pctGross = new List<double>();
            List<double> pctTaxable = new List<double>();
            List<int> lowBounds = new List<int>();
            List<int> upBounds = new List<int>();


            grossIncomes.Add(-5094991);
            grossIncomes.Add(2494290);
            grossIncomes.Add(31995563);
            grossIncomes.Add(75393048);
            grossIncomes.Add(110625567);
            grossIncomes.Add(137029808);
            grossIncomes.Add(167694124);
            grossIncomes.Add(413146253);
            grossIncomes.Add(432975517);
            grossIncomes.Add(1045511568);
            grossIncomes.Add(816206695);
            grossIncomes.Add(1167988946);
            grossIncomes.Add(575673389);
            grossIncomes.Add(240943755);
            grossIncomes.Add(98744564);
            grossIncomes.Add(58440100);
            grossIncomes.Add(142091816);
            grossIncomes.Add(76253821);
            grossIncomes.Add(158454920);

            taxableIncomes.Add(0);
            taxableIncomes.Add(610224);
            taxableIncomes.Add(7956944);
            taxableIncomes.Add(28261843);
            taxableIncomes.Add(50371975);
            taxableIncomes.Add(72410263);
            taxableIncomes.Add(97429358);
            taxableIncomes.Add(254354428);
            taxableIncomes.Add(276796514);
            taxableIncomes.Add(702291485);
            taxableIncomes.Add(575890228);
            taxableIncomes.Add(875512626);
            taxableIncomes.Add(482717655);
            taxableIncomes.Add(212761707);
            taxableIncomes.Add(88335720);
            taxableIncomes.Add(52488902);
            taxableIncomes.Add(128190317);
            taxableIncomes.Add(68499870);
            taxableIncomes.Add(140179919);

            incomeTaxes.Add(78488);
            incomeTaxes.Add(72959);
            incomeTaxes.Add(780450);
            incomeTaxes.Add(2750658);
            incomeTaxes.Add(5404734);
            incomeTaxes.Add(8274086);
            incomeTaxes.Add(11036040);
            incomeTaxes.Add(29737818);
            incomeTaxes.Add(34634209);
            incomeTaxes.Add(94256193);
            incomeTaxes.Add(84253116);
            incomeTaxes.Add(163342405);
            incomeTaxes.Add(120710917);
            incomeTaxes.Add(60180621);
            incomeTaxes.Add(25550669);
            incomeTaxes.Add(15315946);
            incomeTaxes.Add(36900818);
            incomeTaxes.Add(19313626);
            incomeTaxes.Add(35416375);

            numReturns.Add(4522);
            numReturns.Add(835921);
            numReturns.Add(4116242);
            numReturns.Add(6042925);
            numReturns.Add(6304104);
            numReturns.Add(6095228);
            numReturns.Add(6092090);
            numReturns.Add(11856081);
            numReturns.Add(9668366);
            numReturns.Add(17024921);
            numReturns.Add(9486123);
            numReturns.Add(8861764);
            numReturns.Add(1996787);
            numReturns.Add(355750);
            numReturns.Add(81588);
            numReturns.Add(33984);
            numReturns.Add(48235);
            numReturns.Add(11160);
            numReturns.Add(6114);

            avgTax.Add(17377);
            avgTax.Add(87);
            avgTax.Add(190);
            avgTax.Add(455);
            avgTax.Add(857);
            avgTax.Add(1357);
            avgTax.Add(1812);
            avgTax.Add(2508);
            avgTax.Add(3582);
            avgTax.Add(5536);
            avgTax.Add(8882);
            avgTax.Add(18432);
            avgTax.Add(60453);
            avgTax.Add(169166);
            avgTax.Add(313177);
            avgTax.Add(450683);
            avgTax.Add(765117);
            avgTax.Add(1730613);
            avgTax.Add(5792690);

            pctGross.Add(0);
            pctGross.Add(2.9);
            pctGross.Add(2.4);
            pctGross.Add(3.6);
            pctGross.Add(4.9);
            pctGross.Add(6);
            pctGross.Add(6.6);
            pctGross.Add(7.2);
            pctGross.Add(8);
            pctGross.Add(9);
            pctGross.Add(10.3);
            pctGross.Add(14);
            pctGross.Add(21);
            pctGross.Add(25);
            pctGross.Add(25.9);
            pctGross.Add(26.2);
            pctGross.Add(26);
            pctGross.Add(25.3);
            pctGross.Add(22.4);

            pctTaxable.Add(0);
            pctTaxable.Add(12);
            pctTaxable.Add(9.8);
            pctTaxable.Add(9.7);
            pctTaxable.Add(10.7);
            pctTaxable.Add(11.4);
            pctTaxable.Add(11.3);
            pctTaxable.Add(11.7);
            pctTaxable.Add(12.5);
            pctTaxable.Add(13.4);
            pctTaxable.Add(14.6);
            pctTaxable.Add(18.7);
            pctTaxable.Add(25);
            pctTaxable.Add(28.3);
            pctTaxable.Add(28.9);
            pctTaxable.Add(29.2);
            pctTaxable.Add(28.8);
            pctTaxable.Add(28.2);
            pctTaxable.Add(25.3);

            lowBounds.Add(0);
            lowBounds.Add(1);
            lowBounds.Add(5000);
            lowBounds.Add(10000);
            lowBounds.Add(15000);
            lowBounds.Add(20000);
            lowBounds.Add(25000);
            lowBounds.Add(30000);
            lowBounds.Add(40000);
            lowBounds.Add(50000);
            lowBounds.Add(75000);
            lowBounds.Add(100000);
            lowBounds.Add(200000);
            lowBounds.Add(500000);
            lowBounds.Add(1000000);
            lowBounds.Add(1500000);
            lowBounds.Add(2000000);
            lowBounds.Add(5000000);
            lowBounds.Add(10000000);

            upBounds.Add(0);
            upBounds.Add(4999);
            upBounds.Add(9999);
            upBounds.Add(14999);
            upBounds.Add(19999);
            upBounds.Add(24999);
            upBounds.Add(29999);
            upBounds.Add(39999);
            upBounds.Add(49999);
            upBounds.Add(74999);
            upBounds.Add(99999);
            upBounds.Add(199999);
            upBounds.Add(499999);
            upBounds.Add(999999);
            upBounds.Add(1499999);
            upBounds.Add(1999999);
            upBounds.Add(4999999);
            upBounds.Add(9999999);
            upBounds.Add(15000000);

            testData2003 = new IncomeYearModel();
            testData2003.Year = 2003;
            testData2003.Brackets = new ObservableCollection<BracketModel>();
            for(int i = 0; i < upBounds.Count; i++)
            {
                BracketModel br = new BracketModel();
                br.AverageTotalIncomeTax = avgTax[i];
                br.GrossIncome = grossIncomes[i];
                br.NumReturns = numReturns[i];
                br.PercentOfGrossIncomePaid = pctGross[i];
                br.PercentOfTaxableIncomePaid = pctTaxable[i];
                br.TaxableIncome = taxableIncomes[i];
                br.IncomeTax = incomeTaxes[i];

                testData2003.Brackets.Add(br);
            }
        }

        [TestMethod]
        public void TC040_LoaderFindsAllYears()
        {
            //Test that the loader finds all years from 2003-2018

            //Create a new loader and load years
            Loader ld = new Loader();
            
            //Check that each year was loaded
            Assert.AreEqual(2018, ld.YearsModel.YearList[0]);
            Assert.AreEqual(2017, ld.YearsModel.YearList[1]);
            Assert.AreEqual(2016, ld.YearsModel.YearList[2]);
            Assert.AreEqual(2015, ld.YearsModel.YearList[3]);
            Assert.AreEqual(2014, ld.YearsModel.YearList[4]);
            Assert.AreEqual(2013, ld.YearsModel.YearList[5]);
            Assert.AreEqual(2012, ld.YearsModel.YearList[6]);
            Assert.AreEqual(2011, ld.YearsModel.YearList[7]);
            Assert.AreEqual(2010, ld.YearsModel.YearList[8]);
            Assert.AreEqual(2009, ld.YearsModel.YearList[9]);
            Assert.AreEqual(2008, ld.YearsModel.YearList[10]);
            Assert.AreEqual(2007, ld.YearsModel.YearList[11]);
            Assert.AreEqual(2006, ld.YearsModel.YearList[12]);
            Assert.AreEqual(2005, ld.YearsModel.YearList[13]);
            Assert.AreEqual(2004, ld.YearsModel.YearList[14]);
            Assert.AreEqual(2003, ld.YearsModel.YearList[15]);

            //Check that there aren't any extra years loaded (though there could be if the user were to add year data)
            Assert.AreEqual(16, ld.YearsModel.YearList.Count);
        }

        [TestMethod]
        public void TC041_ParserCreatesFiles() 
        {
            //Test that the parser creates file from data with correct values

            string[] fileNames = Loader.LoadYearsTest();
            List<IncomeYearModel> iyms = new List<IncomeYearModel>();

            foreach(string file in fileNames)
            {
                iyms.Add(Parser.ParseCSV(file));
            }


            //Check that the 1st element has the correct year to look at
            Assert.AreEqual(2003, iyms[0].Year);


            //Manually set up values to check the parser against
            setUpData();

            //Check the values of each bracket
            for (int i = 0; i < iyms[0].Brackets.Count; i++)
            {
                Assert.AreEqual(testData2003.Brackets[i].GrossIncome, iyms[0].Brackets[i].GrossIncome);
                Assert.AreEqual(testData2003.Brackets[i].TaxableIncome, iyms[0].Brackets[i].TaxableIncome);
                Assert.AreEqual(testData2003.Brackets[i].IncomeTax, iyms[0].Brackets[i].IncomeTax);
                Assert.AreEqual(testData2003.Brackets[i].NumReturns, iyms[0].Brackets[i].NumReturns);
                Assert.AreEqual(testData2003.Brackets[i].AverageTotalIncomeTax, iyms[0].Brackets[i].AverageTotalIncomeTax);
                Assert.AreEqual(testData2003.Brackets[i].PercentOfGrossIncomePaid, iyms[0].Brackets[i].PercentOfGrossIncomePaid);
                Assert.AreEqual(testData2003.Brackets[i].PercentOfTaxableIncomePaid, iyms[0].Brackets[i].PercentOfTaxableIncomePaid);
            }
        }

        [TestMethod]
        public void TC042_GraphUpdate() 
        {
            //Test that the graph updates when a new year is selected
            
            //Start the program and load data
            Loader ld = new Loader();

            //Test that the year starts at 2018
            Assert.AreEqual(2018, ld.ControlVM.SelectedYear);

            ld.ControlVM.SelectedYear = 2003;

            //Check that the year was changed successfully
            Assert.AreEqual(2003, ld.ControlVM.SelectedYear);

            //Check that the control vm data updated correctly
            //Use manually entered data to check this
            setUpData();

            for(int i = 0; i < testData2003.Brackets.Count; i++)
            {
                Assert.AreEqual(testData2003.Brackets[i].GrossIncome, ld.ControlVM.YearsModel.Years[2003].Brackets[i].GrossIncome);
                Assert.AreEqual(testData2003.Brackets[i].TaxableIncome, ld.ControlVM.YearsModel.Years[2003].Brackets[i].TaxableIncome);
                Assert.AreEqual(testData2003.Brackets[i].IncomeTax, ld.ControlVM.YearsModel.Years[2003].Brackets[i].IncomeTax);
                Assert.AreEqual(testData2003.Brackets[i].NumReturns, ld.ControlVM.YearsModel.Years[2003].Brackets[i].NumReturns);
                Assert.AreEqual(testData2003.Brackets[i].AverageTotalIncomeTax, ld.ControlVM.YearsModel.Years[2003].Brackets[i].AverageTotalIncomeTax);
                Assert.AreEqual(testData2003.Brackets[i].PercentOfGrossIncomePaid, ld.ControlVM.YearsModel.Years[2003].Brackets[i].PercentOfGrossIncomePaid);
                Assert.AreEqual(testData2003.Brackets[i].PercentOfTaxableIncomePaid, ld.ControlVM.YearsModel.Years[2003].Brackets[i].PercentOfTaxableIncomePaid);
            }

        }

        [TestMethod]
        public void TC043_PovertyPop() 
        {
            //Test that poverty population is calculated correctly

            //Start the program
            Loader ld = new Loader();

            //Check a poverty line value
            //Set the poverty line index
            ld.ControlVM.PovertyLineIndexSlider = 2;

            //Check that the poverty population equals a manually calculated value
            Assert.AreEqual("317,285", ld.OutputVM.NumPovertyPopOutput);

            //Check another poverty line value
            //Set the poverty line index
            ld.ControlVM.PovertyLineIndexSlider = 4;

            //Check that the poverty population equals a manually calculated value
            Assert.AreEqual("7.86 million", ld.OutputVM.NumPovertyPopOutput);
        }

        [TestMethod]
        public void TC044_MaxPop() {
            //Test that maximum population is calculated correctly

            //Start the program
            Loader ld = new Loader();

            //Check a poverty line value
            //Set the poverty line index
            ld.ControlVM.MaxBracketCountSlider = 2;

            //Check that the poverty population equals a manually calculated value
            Assert.AreEqual("56,839", ld.OutputVM.NumMaxPopOutput);

            //Check another poverty line value
            //Set the poverty line index
            ld.ControlVM.MaxBracketCountSlider = 4;

            //Check that the poverty population equals a manually calculated value
            Assert.AreEqual("297,173", ld.OutputVM.NumMaxPopOutput);
        }

        [TestMethod]
        public void TC045_RevenueCalc() {
            //Test that revenue is calculated correctly

            //Start the program
            Loader ld = new Loader();

            //Set some example values
            ld.ControlVM.PovertyLineIndexSlider = 4;
            ld.ControlVM.MaxBracketCountSlider = 4;
            ld.ControlVM.MaxTaxRate = 50;

            Assert.AreEqual("2.91 trillion", ld.OutputVM.TotalRevenueNewOutput);
        }

        [TestMethod]
        public void TC046_GVM() {
            //Check that graph view models can be created correctly

            //Start the program
            Loader ld = new Loader();

            //Check that the GVM is created
            Assert.IsNotNull(ld.GraphVM);

        }

        [TestMethod]
        public void TC047_MVM() {
            //Check that main view model is created correctly

            //Start the program
            Loader ld = new Loader();

            //Check that the MVM is created
            Assert.IsNotNull(ld.MainVM);
        }

        [TestMethod]
        public void TC049_UBI() {
            //Test that UBI works correctly

            //Start the program
            Loader ld = new Loader();

            //Set some example values
            ld.ControlVM.MaxUBIBracketCountSlider = 4;
            ld.ControlVM.MinUBIBracketCountSlider = 4;
            ld.ControlVM.MaxUBISlider = 2500;

            Assert.AreEqual("UBI ($135.81 billion)", ld.OutputVM.UBIText);
        }

        [TestMethod]
        public void TC050_PovertySlider() {
            //Check that poverty slider works properly

            //Test that poverty population is calculated correctly

            //Start the program
            Loader ld = new Loader();

            //Set the poverty line index
            ld.ControlVM.PovertyLineIndexSlider = 1;

            //Save the population
            string pop1 = ld.OutputVM.NumPovertyPopOutput;

            //Change the slider
            ld.ControlVM.PovertyLineIndexSlider = 6;

            //Save the population again
            string pop2 = ld.OutputVM.NumPovertyPopOutput;

            //Check that the two populations are different and that the poverty line slider made changes
            Assert.AreNotEqual(pop1, pop2);
        }

        [TestMethod]
        public void TC051_MaxBracketSlider()
        {
            //Check that max bracket slider works properly

            //Test that max population is calculated correctly

            //Start the program
            Loader ld = new Loader();

            //Set the poverty line index
            ld.ControlVM.MaxBracketCountSlider = 1;

            //Save the population
            string pop1 = ld.OutputVM.NumMaxPopOutput;

            //Change the slider
            ld.ControlVM.MaxBracketCountSlider = 6;

            //Save the population again
            string pop2 = ld.OutputVM.NumMaxPopOutput;

            //Check that the two populations are different and that the poverty line slider made changes
            Assert.AreNotEqual(pop1, pop2);
        }

        [TestMethod]
        public void TC052_MaxRateSlider()
        {
            //Check that max rate slider works properly

            //Test that revenue is calculated correctly

            //Start the program
            Loader ld = new Loader();

            //Set the poverty line index
            ld.ControlVM.MaxTaxRate = 20;

            //Save the population
            string rev1 = ld.OutputVM.TotalRevenueNewOutput;

            //Change the slider
            ld.ControlVM.MaxTaxRate = 60;

            //Save the population again
            string rev2 = ld.OutputVM.TotalRevenueNewOutput;

            //Check that the two populations are different and that the poverty line slider made changes
            Assert.AreNotEqual(rev1, rev2);
        }

        [TestMethod]
        public void TC053_CustomTaxRates() 
        {
            //Check that custom tax rates works correctly

            //Start the program
            Loader ld = new Loader();

            //Set up slant tax
            ld.ControlVM.MaxBracketCountSlider = 4;
            ld.ControlVM.PovertyLineIndexSlider = 4;
            ld.ControlVM.MaxTaxRate = 50;

            //Save revenue
            string rev1 = ld.OutputVM.TotalRevenueNewOutput;

            //Adjust a bracket manually
            ld.ControlVM.SelectedBracket = ld.ControlVM.BracketList[5];
            ld.ControlVM.TaxRateSlider = 90.0;

            //Save revenue
            string rev2 = ld.OutputVM.TotalRevenueNewOutput;

            //Test that revenue changed
            Assert.AreNotEqual(rev1, rev2);

            //Test that the rate changed correctly
            Assert.AreEqual(90.0, ld.ControlVM.TaxPlansModel.SelectedTaxPlan.TaxRates[5]);
        }

        [TestMethod]
        public void TC054_DefaultGovPrograms()
        {
            //Check that existing government budget items/programs are displayed correctly

            //Start the program
            Loader ld = new Loader();

            //Check each program's name and cost
            Assert.AreEqual("Defense", ld.OptionsModel.listOfCosts[0].name);
            Assert.AreEqual(678000000000.0, ld.OptionsModel.listOfCosts[0].cost);

            Assert.AreEqual("Medicaid", ld.OptionsModel.listOfCosts[1].name);
            Assert.AreEqual(613000000000.0, ld.OptionsModel.listOfCosts[1].cost);

            Assert.AreEqual("Welfare", ld.OptionsModel.listOfCosts[2].name);
            Assert.AreEqual(361000000000.0, ld.OptionsModel.listOfCosts[2].cost);

            Assert.AreEqual("Veterans", ld.OptionsModel.listOfCosts[3].name);
            Assert.AreEqual(79000000000.0, ld.OptionsModel.listOfCosts[3].cost);

            Assert.AreEqual("Food Stamps", ld.OptionsModel.listOfCosts[4].name);
            Assert.AreEqual(68000000000.0, ld.OptionsModel.listOfCosts[4].cost);

            Assert.AreEqual("Education", ld.OptionsModel.listOfCosts[5].name);
            Assert.AreEqual(61000000000.0, ld.OptionsModel.listOfCosts[5].cost);

            Assert.AreEqual("Public Housing", ld.OptionsModel.listOfCosts[6].name);
            Assert.AreEqual(56000000000.0, ld.OptionsModel.listOfCosts[6].cost);

            Assert.AreEqual("Health", ld.OptionsModel.listOfCosts[7].name);
            Assert.AreEqual(53000000000.0, ld.OptionsModel.listOfCosts[7].cost);

            Assert.AreEqual("Science", ld.OptionsModel.listOfCosts[8].name);
            Assert.AreEqual(30000000000.0, ld.OptionsModel.listOfCosts[8].cost);

            Assert.AreEqual("Transportation", ld.OptionsModel.listOfCosts[9].name);
            Assert.AreEqual(29000000000.0, ld.OptionsModel.listOfCosts[9].cost);

            Assert.AreEqual("International", ld.OptionsModel.listOfCosts[10].name);
            Assert.AreEqual(29000000000.0, ld.OptionsModel.listOfCosts[10].cost);

            Assert.AreEqual("Energy and Environment", ld.OptionsModel.listOfCosts[11].name);
            Assert.AreEqual(28000000000.0, ld.OptionsModel.listOfCosts[11].cost);

            Assert.AreEqual("Unemployment", ld.OptionsModel.listOfCosts[12].name);
            Assert.AreEqual(24000000000.0, ld.OptionsModel.listOfCosts[12].cost);

            Assert.AreEqual("Food and Agriculture", ld.OptionsModel.listOfCosts[13].name);
            Assert.AreEqual(11000000000.0, ld.OptionsModel.listOfCosts[13].cost);
        }

        [TestMethod]
        public void TC055_AddGovProgram() 
        {
            //Test that you can add custom government programs
            
            //Start the program
            Loader ld = new Loader();

            //Check that there are only 19 budget items
            Assert.AreEqual(19, ld.OptionsModel.listOfCosts.Count);

            //Add a program
            ld.OutputVM.addProgramButtonClick();

            //Check that the program was added
            Assert.AreEqual("", ld.OptionsModel.listOfCosts[19].name);

            //Give the custom program values
            ld.OptionsModel.listOfCosts[19] = (19, false, "My prog", 999999, 100);

            //Check that the name and cost were set properly
            Assert.AreEqual("My prog", ld.OptionsModel.listOfCosts[19].name);
            Assert.AreEqual(999999, ld.OptionsModel.listOfCosts[19].cost);

        }

        [TestMethod]
        public void TC056_InvalidCost() {
            //Test that you can add custom government programs

            //Start the program
            Loader ld = new Loader();

            //Add a program
            ld.OutputVM.addProgramButtonClick();

            //Check that the program was added
            Assert.AreEqual("", ld.OptionsModel.listOfCosts[19].name);

            var enteredVal = "adsakjda";
            try
            {
                ld.OptionsModel.listOfCosts[19] = (19, false, "my prog", Double.Parse(enteredVal), 100);
            }
            catch (Exception e) {
                ld.OptionsModel.listOfCosts[19] = (19, false, "my prog", 0, 100);
            }

            Assert.AreEqual(ld.OptionsModel.listOfCosts[19], (19, false, "my prog", 0, 100));
        }

        [TestMethod]
        public void TC057_TestFunding() {
            //Test that funding is calculated properly

            //Start the program
            Loader ld = new Loader();

            //Setup the slant tax
            ld.ControlVM.MaxBracketCountSlider = 4;
            ld.ControlVM.PovertyLineIndexSlider = 4;
            ld.ControlVM.MaxTaxRate = 26;

            //Use default government programs and check their funding against manually calculated values
            Assert.AreEqual("100", ld.OptionsModel.fundingArray[0].ToString());
            Assert.AreEqual("100", ld.OptionsModel.fundingArray[1].ToString());
            Assert.AreEqual("70.4067890030471", ld.OptionsModel.fundingArray[2].ToString());
            Assert.AreEqual("0", ld.OptionsModel.fundingArray[3].ToString());
            Assert.AreEqual("0", ld.OptionsModel.fundingArray[4].ToString());
            Assert.AreEqual("0", ld.OptionsModel.fundingArray[5].ToString());
            Assert.AreEqual("0", ld.OptionsModel.fundingArray[6].ToString());
            Assert.AreEqual("0", ld.OptionsModel.fundingArray[7].ToString());
            Assert.AreEqual("0", ld.OptionsModel.fundingArray[8].ToString());
            Assert.AreEqual("0", ld.OptionsModel.fundingArray[9].ToString());
            Assert.AreEqual("0", ld.OptionsModel.fundingArray[10].ToString());
            Assert.AreEqual("0", ld.OptionsModel.fundingArray[11].ToString());
            Assert.AreEqual("0", ld.OptionsModel.fundingArray[12].ToString());
            Assert.AreEqual("0", ld.OptionsModel.fundingArray[13].ToString());
            Assert.AreEqual("0", ld.OptionsModel.fundingArray[14].ToString());
            Assert.AreEqual("0", ld.OptionsModel.fundingArray[15].ToString());
            Assert.AreEqual("0", ld.OptionsModel.fundingArray[16].ToString());
            Assert.AreEqual("0", ld.OptionsModel.fundingArray[17].ToString());
            Assert.AreEqual("0", ld.OptionsModel.fundingArray[18].ToString());

        }

        [TestMethod]
        public void TC058_TestBracketLock() {
            //Test that bracket lock option works

            //Start the program
            Loader ld = new Loader();

            //Select Middle bracket
            int center = ld.ControlVM.BracketList.Count / 2;
            ld.ControlVM.SelectedBracket = ld.ControlVM.BracketList[center];

            //Lock brakcets
            ld.ControlVM.LockTaxRates = true;

            //Select 5 brackets
            ld.ControlVM.LockNumberSlider = 5;

            //Increase tax rate
            ld.ControlVM.TaxRateSlider = 50;

            //Check that the tax rates were adjusted correctly
            for (int i = 0; i < ld.ControlVM.BracketList.Count; i++) { 
                if(i >= (center - 5) && i <= (center + 4))
                {
                    Assert.AreEqual(50, ld.ControlVM.TaxPlansModel.TaxPlans[ld.ControlVM.SelectedTaxPlanName].TaxRates[i]);
                }
                else{
                    Assert.AreEqual(0, ld.ControlVM.TaxPlansModel.TaxPlans[ld.ControlVM.SelectedTaxPlanName].TaxRates[i]);
                }
            }
        }

        [TestMethod]
        public void TC059_TestUBILock() {
            //Test that slant brackets change UBI lock works
            
            //Start the program
            Loader ld = new Loader();

            //Set UBI brackets to 0
            ld.ControlVM.MaxUBIBracketCountSlider = 0;
            ld.ControlVM.MinUBIBracketCountSlider = 0;

            //Check the lock option
            ld.ControlVM.SlantChangesUBI = true;

            //Adjust the slant tax
            ld.ControlVM.MaxBracketCountSlider = 5;
            ld.ControlVM.PovertyLineIndexSlider = 5;

            //Check that UBI brackets were changed
            Assert.AreEqual(5, ld.ControlVM.MaxUBIBracketCountSlider);
            Assert.AreEqual(6, ld.ControlVM.MinUBIBracketCountSlider);
        }

        [TestMethod]
        public void TC060_TestRestSettingsButton() {
            //Test that the reset settings button works

            //Start the program
            Loader ld = new Loader();

            //Change the default options
            ld.ControlVM.ShowUBI = true;
            ld.ControlVM.ShowOldPercentage = true;
            ld.ControlVM.ShowOldRevenue = true;
            ld.ControlVM.ShowNewRevenue = true;
            ld.ControlVM.ShowNewPercentage = true;

            ld.OutputVM.DefenseSpendingChecked = false;
            ld.OutputVM.MedicaidSpendingChecked = false;

            //Press the reset button
            ld.ControlVM.resetSettingsButtonClick();

            //Check that the options were reset
            Assert.AreEqual(false, ld.ControlVM.ShowUBI);
            Assert.AreEqual(false, ld.ControlVM.ShowOldPercentage);
            Assert.AreEqual(false, ld.ControlVM.ShowOldRevenue);
            Assert.AreEqual(false, ld.ControlVM.ShowNewRevenue);
            Assert.AreEqual(false, ld.ControlVM.ShowNewPercentage);

            Assert.AreEqual(true, ld.OutputVM.DefenseSpendingChecked);
            Assert.AreEqual(true, ld.OutputVM.MedicaidSpendingChecked);
        }

        [TestMethod]
        public void TC061_TestResetTaxRatesButton() {
            //Test that the reset tax rates button works properly

            //Start the program
            Loader ld = new Loader();

            //Set up the slant tax
            ld.ControlVM.MaxBracketCountSlider = 4;
            ld.ControlVM.PovertyLineIndexSlider = 4;
            ld.ControlVM.MaxTaxRate = 30;

            //Save the value of a middle bracket
            int bracket = 5;
            double oldRate = ld.ControlVM.TaxPlansModel.TaxPlans[ld.ControlVM.SelectedTaxPlanName].TaxRates[bracket];

            //Change the value manually
            ld.ControlVM.SelectedBracket = ld.ControlVM.BracketList[bracket];
            ld.ControlVM.TaxRateSlider = 80;

            //Check that the rate was changed
            Assert.AreEqual(80, ld.ControlVM.TaxPlansModel.TaxPlans[ld.ControlVM.SelectedTaxPlanName].TaxRates[bracket]);

            //Press the reset button
            ld.ControlVM.resetTaxRatesButtonClick();

            //Check that the rate was changed back;
            Assert.AreEqual(oldRate, ld.ControlVM.TaxPlansModel.TaxPlans[ld.ControlVM.SelectedTaxPlanName].TaxRates[bracket]);
        }

        [TestMethod]
        public void TC062_TestAutoFitTaxes() {
            //Test that the auto-fit tax rates button works properly

            //Start the program
            Loader ld = new Loader();

            //Check that the revenue is less than the budget
            Assert.IsTrue(ld.DataModel.TotalRevenueNew < ld.OptionsModel.GetTotalBudget());

            //Press the autofit button
            ld.ControlVM.autoFitTaxButtonClick();

            //Check that the max rate changed
            Assert.AreNotEqual(0, ld.ControlVM.MaxTaxRate);

            //Check that the revenue is greater than or equal to the budget
            Assert.IsTrue(ld.DataModel.TotalRevenueNew >= ld.OptionsModel.GetTotalBudget());
        }

        [TestMethod]
        public void TC063_TestTargetFunding() {
            //Test that the target funding of budget items works properly

            //Start the program
            Loader ld = new Loader();

            //Set up the slant tax
            ld.ControlVM.MaxBracketCountSlider = 4;
            ld.ControlVM.PovertyLineIndexSlider = 4;
            ld.ControlVM.MaxTaxRate = 30;

            //Check that the initial cost of Medicaid is correct
            Assert.AreEqual("Medicaid Spending ($613.00 billion)", ld.OutputVM.MedicaidText);
            Assert.AreEqual(613000000000, ld.OptionsModel.listOfCosts[1].cost * (ld.OptionsModel.listOfCosts[1].tFunding / 100));

            //Change the target funding
            ld.ControlVM.SelectedGovProgram = ld.ControlVM.GovProgramList[1];
            ld.ControlVM.TargetFundingSlider = 50;

            //Check that the new cost of Medicaid is correct
            Assert.AreEqual("Medicaid Spending ($306.50 billion)", ld.OutputVM.MedicaidText);
            Assert.AreEqual(306500000000, ld.OptionsModel.listOfCosts[1].cost * (ld.OptionsModel.listOfCosts[1].tFunding / 100));

        }

        [TestMethod]
        public void TC064_TestIncomeCalc() {
            //Test that the income tax calculator works properly

            //Start the program
            Loader ld = new Loader();

            //Set up the slant tax
            ld.ControlVM.MaxBracketCountSlider = 4;
            ld.ControlVM.PovertyLineIndexSlider = 4;
            ld.ControlVM.MaxTaxRate = 30;

            //Enter data into income tax calc
            ld.OutputVM.YearlyIncome = "50,000";

            Assert.AreEqual("7.6 %", ld.OutputVM.OldTaxRate);
            Assert.AreEqual("$ 46,200", ld.OutputVM.OldAfterTaxIncome);
            Assert.AreEqual("13.5 %", ld.OutputVM.NewTaxRate);
            Assert.AreEqual("$ 43,250", ld.OutputVM.NewAfterTaxIncome);

            //Try another annual income
            ld.OutputVM.YearlyIncome = "30,000";

            Assert.AreEqual("5.9 %", ld.OutputVM.OldTaxRate);
            Assert.AreEqual("$ 28,230", ld.OutputVM.OldAfterTaxIncome);
            Assert.AreEqual("8.1 %", ld.OutputVM.NewTaxRate);
            Assert.AreEqual("$ 27,570", ld.OutputVM.NewAfterTaxIncome);
        }

        [TestMethod]
        public void TC065_TestMedianandMean() {
            //Test that median and mean are calculated properly

            //Start the program
            Loader ld = new Loader();

            //Set up the slant tax
            ld.ControlVM.MaxBracketCountSlider = 4;
            ld.ControlVM.PovertyLineIndexSlider = 4;
            ld.ControlVM.MaxTaxRate = 30;

            //Check the median and mean incomes

            //Pretax
            //Mean
            Assert.AreEqual("$113,855", ld.OutputVM.MeanMedian.Rows[0].ItemArray[1]);
            //Median
            Assert.AreEqual("$65,139", ld.OutputVM.MeanMedian.Rows[0].ItemArray[2]);
            //Difference
            Assert.AreEqual("$48,716", ld.OutputVM.MeanMedian.Rows[0].ItemArray[3]);

            //Postax
            //Mean
            Assert.AreEqual("$91,980", ld.OutputVM.MeanMedian.Rows[1].ItemArray[1]);
            //Median
            Assert.AreEqual("$56,345", ld.OutputVM.MeanMedian.Rows[1].ItemArray[2]);
            //Difference
            Assert.AreEqual("$35,634", ld.OutputVM.MeanMedian.Rows[1].ItemArray[3]);

            //Postax w/ UBI
            //Mean
            Assert.AreEqual("$92,874", ld.OutputVM.MeanMedian.Rows[2].ItemArray[1]);
            //Median
            Assert.AreEqual("$57,215", ld.OutputVM.MeanMedian.Rows[2].ItemArray[2]);
            //Difference
            Assert.AreEqual("$35,658", ld.OutputVM.MeanMedian.Rows[2].ItemArray[3]);
        }

        [TestMethod]
        public void TC066_EditingModes() {
            //Test that the different editing modes work properly

            //Start the program
            Loader ld = new Loader();

            //Check that the UBI options are not visible
            Assert.IsFalse(ld.ControlVM.ShowUBI);
            Assert.IsFalse(ld.ControlVM.ToggleEditingModeBtnEnabled);
            //Check that the current editing mode is tax editing
            Assert.AreEqual(0, ld.ControlVM.SelectedEditingModeIndex);

            //Select the UBI in display options
            ld.ControlVM.ShowUBI = true;

            //Check that the mode can be toggled
            Assert.IsTrue(ld.ControlVM.ShowUBI);
            Assert.IsTrue(ld.ControlVM.ToggleEditingModeBtnEnabled);

            //Toggle the mode
            ld.ControlVM.toggleEditModeButtonClick();
            //Check that the current editing mode is UBI editing
            Assert.AreEqual(1, ld.ControlVM.SelectedEditingModeIndex);
        }

        [TestMethod]
        public void TC067_TypeVals() {
            //Test that typing values into sliders works properly

            //Start the program
            Loader ld = new Loader();

            //Set up the slant tax
            ld.ControlVM.MaxBracketCountSlider = 4;
            ld.ControlVM.PovertyLineIndexSlider = 4;
            ld.ControlVM.MaxTaxRate = 30;

            //Type a value into the max tax rate
            ld.ControlVM.MaxTaxRate = Int32.Parse("50");

            //Check that it changed the max tax rate
            Assert.AreEqual(50, ld.ControlVM.MaxTaxRate);
        }

        [TestMethod]
        public void TC068_UBICustomProgs() {
            //Test that the UBI that is graphed is included in the budget (and can be turned on or off)

            //Start the program
            Loader ld = new Loader();

            //Display the UBI
            ld.ControlVM.ShowUBI = true;

            //Check that the UBI budget item has the correct cost
            Assert.AreEqual("UBI ($89.75 billion)", ld.OptionsModel.GetUBIText());
            //Select it and use auto-fit
            ld.OutputVM.UBIChecked = true;
            ld.ControlVM.autoFitTaxButtonClick();

            //Check that the max rate is correct
            int maxRate1 = ld.ControlVM.MaxTaxRate;
            Assert.AreEqual(14, maxRate1);

            //Edit its values
            ld.ControlVM.toggleEditModeButtonClick();
            ld.ControlVM.MaxUBI = 5000;

            //Use auto-fit again and check that the rate changed
            ld.ControlVM.autoFitTaxButtonClick();
            int maxRate2 = ld.ControlVM.MaxTaxRate;
            Assert.AreNotEqual(16, maxRate2);
        }

        [TestMethod]
        public void TC070_AutoFitBudget() {
            //Test that auto-fit budget works properly

            //Start the program
            Loader ld = new Loader();

            //Set up the slant tax
            ld.ControlVM.MaxBracketCountSlider = 4;
            ld.ControlVM.PovertyLineIndexSlider = 4;
            ld.ControlVM.MaxTaxRate = 10;

            //Press Auto-fit budget
            ld.ControlVM.autoFitBudgetButtonClick();

            //Check that the target funding has changed and that all items were given equal priority
            Assert.AreEqual(27, ld.OptionsModel.listOfCosts[0].tFunding);
            Assert.IsTrue(ld.OptionsModel.listOfCosts[0].tFunding != 100);

            double newTFunding = ld.OptionsModel.listOfCosts[0].tFunding;

            for(int i = 0; i < ld.OptionsModel.listOfCosts.Count; i++)
            {
                if (ld.OptionsModel.listOfCosts[i].ischecked)
                {
                    Assert.AreEqual(newTFunding, ld.OptionsModel.listOfCosts[i].tFunding);
                }
            }

        }

        [TestMethod]
        public void TC071_DeficitCalc() {
            //Test that the deficit or surplus is calculated properly

            //Start the program
            Loader ld = new Loader();

            //Check that the deficit starts at (0 - budget)
            Assert.AreEqual("$" + Formatter.Format(ld.DataModel.TotalRevenueNew - ld.OptionsModel.GetTotalBudget()) + " deficit" , ld.OutputVM.LeftOverBudget.ToLower());

            //Use the auto-fit button
            ld.ControlVM.autoFitTaxButtonClick();

            //Then recheck deficit
            Assert.AreEqual("$8.039 billion surplus", ld.OutputVM.LeftOverBudget.ToLower());

        }

        [TestMethod]
        public void TC072_SaveFile() {
            //Test that files are saved properly

            //Start the program
            Loader ld = new Loader();

            //Adjust the slant tax
            ld.ControlVM.MaxBracketCountSlider = 4;
            ld.ControlVM.PovertyLineIndexSlider = 4;
            ld.ControlVM.MaxTaxRate = 30;

            //Save the tax plan
            ld.ControlVM.saveTaxPlan();

            //Check that the file exists

            string userPlanPath = @".\userPlans\";

            //Check that the directory exists
            Assert.IsTrue(Directory.Exists(userPlanPath));
           
            string[] plans = Directory.GetFiles(userPlanPath, "*.tax");

            //Look for the plan
            bool found = false;
            foreach (string plan in plans) {
                if (plan.Contains("Slant Tax (modified)")) {
                    found = true;
                }
            }

            //Check if the plan is found
            Assert.IsTrue(found);
        }

        [TestMethod]
        public void TC073_CheckBudget() {
            //Test that the budget is calculated correctly

            //Start the program
            Loader ld = new Loader();

            //Check the starting budget against a manually calculated value
            Assert.AreEqual(2120000000000, ld.OptionsModel.GetTotalBudget());

            //Add items to the budget
            ld.OutputVM.SandersCollegeSpendingChecked = true;
            ld.OutputVM.SandersMedicaidSpendingChecked = true;

            //Recheck the budget
            Assert.AreEqual(2690000000000, ld.OptionsModel.GetTotalBudget());
        }

        [TestMethod]
        public void TC074_SineGraph() {
            //Check that the 1st slant tax variation is displayed properly

            //Start the program
            Loader ld = new Loader();
        }
        [TestMethod]
        public void TC075_EllipseGraph()
        {
            //Check that the 2nd slant tax variation is displayed properly

            //Start the program
            Loader ld = new Loader();
        }

        [TestMethod]
        public void TC076_FlatPlan()
        {
            //Check that the flat tax plan is displayed properly

            //Start the program
            Loader ld = new Loader();


            //Select the flat tax
            ld.ControlVM.SelectedTaxPlanName = "Flat Tax";

            //Adjust the flat tax slider
            ld.ControlVM.FlatTaxSlider = 50;

            //Check that the tax rates were set properly
            for (int i = 0; i < ld.DataModel.NewTaxPctByBracket.Count; i++) {
                Assert.AreEqual(50, ld.DataModel.NewTaxPctByBracket[i]);
            }
        }

        [TestMethod]
        public void TC077_FlatOptions()
        {
            //Check that the flat tax plan options change properly

            //Start the program
            Loader ld = new Loader();

            //Check that the flat tax plan options don't work on the slant tax
            //Adjust the flat tax slider
            ld.ControlVM.FlatTaxSlider = 50;

            //Check that the tax rates were set properly
            for (int i = 0; i < ld.DataModel.NewTaxPctByBracket.Count; i++)
            {
                Assert.AreNotEqual(50, ld.DataModel.NewTaxPctByBracket[i]);
            }


            //Try using the flat tax options on the flat tax plan and check that they work
            //Select the flat tax
            ld.ControlVM.SelectedTaxPlanName = "Flat Tax";

            //Adjust the flat tax slider
            ld.ControlVM.FlatTaxSlider = 50;

            //Check that the tax rates were set properly
            for (int i = 0; i < ld.DataModel.NewTaxPctByBracket.Count; i++)
            {
                Assert.AreEqual(50, ld.DataModel.NewTaxPctByBracket[i]);
            }

            //Try using the slant options on the flat tax and make sure they don't work
            ld.ControlVM.MaxTaxRate = 40;
            ld.ControlVM.MaxBracketCountSlider = 7;
            ld.ControlVM.PovertyLineIndexSlider = 3;

            for (int i = 0; i < ld.DataModel.NewTaxPctByBracket.Count; i++)
            {
                Assert.AreEqual(50, ld.DataModel.NewTaxPctByBracket[i]);
            }
        }

        [TestMethod]
        public void TC078_PovertyLineChangesMax() {
            //Test that the poverty changes max option works

            //Start the program
            Loader ld = new Loader();

            //Select the poverty changes max option
            ld.ControlVM.BalanceMaxWithPoverty = true;

            //Adjust the poverty line
            ld.ControlVM.PovertyLineIndexSlider = 5;

            //Check that the max brackets adjusted
            Assert.IsTrue(ld.DataModel.NumMaxPop >= ld.DataModel.NumPovertyPop);

            //Adjust the poverty line again
            ld.ControlVM.PovertyLineIndexSlider = 2;

            //ReCheck the max brackets
            Assert.IsTrue(ld.DataModel.NumMaxPop >= ld.DataModel.NumPovertyPop);
        }
        [TestMethod]
        public void TC079_MaxChangesPoverty() {
            //Test that the max changes poverty option works

            //Start the program
            Loader ld = new Loader();

            //Select the max changes poverty option
            ld.ControlVM.BalancePovertyWithMax = true;

            int povb1 = ld.ControlVM.PovertyLineIndex;

            //Adjust the max brackets
            ld.ControlVM.MaxBracketCountSlider = 5;

            //Check that the poverty brackets adjusted
            int povb2 = ld.ControlVM.PovertyLineIndex;
            Assert.IsTrue(povb1 != povb2);

            //Adjust the max brackets again
            ld.ControlVM.MaxBracketCountSlider = 2;

            //ReCheck the poverty brackets
            int povb3 = ld.ControlVM.PovertyLineIndex;
            Assert.IsTrue(povb2 != povb3);
        }

        [TestMethod]
        public void TC080_AutoFitOption() {
            //Test that auto-fit doesn't change max brackets if an option is selected

            //Start the program
            Loader ld = new Loader();

            //Select the option
            ld.ControlVM.DontAdjustBracketCount = true;

            //Set the max brackets
            ld.ControlVM.MaxBracketCountSlider = 1;

            //Press the auto-fit button
            ld.ControlVM.autoFitTaxButtonClick();

            //Check that the max brackets weren't changed
            Assert.AreEqual(1, ld.ControlVM.MaxBracketCountSlider);
        }

        [TestMethod]
        public void TC081_AutoFitFlat() {
            //Test that the auto-fit button works on the flat tax plan

            //Start the program
            Loader ld = new Loader();

            //Select the flat tax plan
            ld.ControlVM.SelectedTaxPlanName = "Flat Tax";

            //Press the auto-fit button
            ld.ControlVM.autoFitTaxButtonClick();

            //Check that the button worked
            Assert.IsTrue(ld.DataModel.TotalRevenueNew >= ld.OptionsModel.GetTotalBudget());
        }

        [TestMethod]
        public void TC082_AutoFitCustomPlan() {
            //Test that the auto-fit button doesn't work on custom tax plans
            //Start the program
            Loader ld = new Loader();

            //Select a custom tax plan
            ld.ControlVM.newTaxPlanTest("aaa");
            ld.ControlVM.SelectedTaxPlanName = "aaa";

            //Press the auto-fit button
            ld.ControlVM.autoFitTaxButtonClick();

            //Check that the button did not work
            Assert.IsFalse(ld.DataModel.TotalRevenueNew >= ld.OptionsModel.GetTotalBudget());
        }

        [TestMethod]
        public void TC083_AddCustomPlan() {
            //Test that users can add custom tax plans
            //Start the program
            Loader ld = new Loader();

            //Add a custom plan (newTaxPlanTest() is just like the method used to add tax plans
            //except you pass it a string rather than getting a string from a dialog box)
            ld.ControlVM.newTaxPlanTest("My Custom Tax Plan");

            //Check that the plan is now in the list of tax plans
            Assert.IsTrue(ld.ControlVM.TaxPlansList.Contains("My Custom Tax Plan"));

            //Select the new plan
            ld.ControlVM.SelectedTaxPlanName = "My Custom Tax Plan";

            //Check that it was selected
            Assert.IsTrue(ld.ControlVM.SelectedTaxPlanName == "My Custom Tax Plan");
        }

        [TestMethod]
        public void TC084_DeletePlans() {
            //Test that custom plans can be deleted but not default plans
            //Start the program
            Loader ld = new Loader();

            //Add a custom plan
            ld.ControlVM.newTaxPlanTest("My Custom Tax Plan");

            //Check that the plan is now in the list of tax plans
            Assert.IsTrue(ld.ControlVM.TaxPlansList.Contains("My Custom Tax Plan"));

            //Select the new plan
            ld.ControlVM.SelectedTaxPlanName = "My Custom Tax Plan";

            //Delete the tax plan
            ld.ControlVM.deleteTaxPlanButtonClick();

            //Check that the plan was deleted
            Assert.IsFalse(ld.ControlVM.TaxPlansList.Contains("My Custom Tax Plan"));
            Assert.IsFalse(ld.ControlVM.SelectedTaxPlanName == "My Custom Tax Plan");

            //Select the flat tax
            ld.ControlVM.SelectedTaxPlanName = "Flat Tax";

            //Try to delete it and check that it can't be deleted
            ld.ControlVM.deleteTaxPlanButtonClick();
            Assert.IsTrue(ld.ControlVM.TaxPlansList.Contains("Flat Tax"));
            Assert.IsTrue(ld.ControlVM.SelectedTaxPlanName == "Flat Tax");
        }

        [TestMethod]
        public void TC085_SaveDefaultPlan() {
            //Test that users can save default plans properly

            //Start the program
            Loader ld = new Loader();

            //Select a default plan
            ld.ControlVM.SelectedTaxPlanName = "Flat Tax";

            //Save the plan
            ld.ControlVM.saveTaxPlan();

            //Check that the modifed version was created
            Assert.IsTrue(ld.ControlVM.TaxPlansList.Contains("Flat Tax (modified)"));
        }

        [TestMethod]
        public void TC086_UseDefaultOptionsOnModified() {
            //Test that users can use default options on saved copies of the default plans

            //Start the program
            Loader ld = new Loader();


            //Flat tax
            ld.ControlVM.SelectedTaxPlanName = "Flat Tax";

            //Save the plan
            ld.ControlVM.saveTaxPlan();

            //Check that the modifed version was created
            Assert.IsTrue(ld.ControlVM.TaxPlansList.Contains("Flat Tax (modified)"));

            //Select the new plan
            ld.ControlVM.SelectedTaxPlanName = "Flat Tax (modified)";

            //Set all rates to 0
            ld.ControlVM.resetTaxRatesButtonClick();

            //Try using the flat tax options
            ld.ControlVM.FlatTaxRate = 50;

            //Check all of the rates
            for (int i = 0; i < ld.DataModel.NewTaxPctByBracket.Count; i++) {
                Assert.AreEqual(50, ld.DataModel.NewTaxPctByBracket[i]);
            }



            //Slant tax
            ld.ControlVM.SelectedTaxPlanName = "Slant Tax";

            //Save the plan
            ld.ControlVM.saveTaxPlan();

            //Check that the modifed version was created
            Assert.IsTrue(ld.ControlVM.TaxPlansList.Contains("Slant Tax (modified)"));

            //Select the new plan
            ld.ControlVM.SelectedTaxPlanName = "Slant Tax (modified)";

            //Set all rates to 0
            ld.ControlVM.resetSettingsButtonClick();

            //Try using the slant tax options
            ld.ControlVM.MaxBracketCountSlider = 5;
            ld.ControlVM.PovertyLineIndexSlider = 5;
            ld.ControlVM.MaxTaxRate = 50;

            //Check that the options were changed
            Assert.AreEqual(5, ld.ControlVM.MaxBracketCountSlider);
            Assert.AreEqual(5, ld.ControlVM.PovertyLineIndexSlider);
            Assert.AreEqual(50, ld.ControlVM.MaxTaxRate);
        }

        [TestMethod]
        public void TC087_LoadGDPData() { 
        
        }
    }
}
