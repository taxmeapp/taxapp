using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using TaxMeApp.Helpers;
using TaxMeApp.models;
using TaxMeApp.viewmodels;

namespace TaxMeAppNUnitTesting
{
    class UnitTestViewModels
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
        //Testing View Models
        //-------------------------------------------------------------------------------------------------

        //ControlViewModel
        [Test]
        public void TestControllerViewModelYearChange()
        {

            //Check that year is set correctly
            cvm.YearsModel.SelectedYear = 5;
            Assert.AreEqual(5, cvm.YearsModel.SelectedYear);

            ObservableCollection<BracketModel> answerBracket;
            //Check that the brackets match
            answerBracket = Parser.ParseCSV(filePaths[5]).Brackets;
            for (int i = 0; i < answerBracket.Count; i++)
            {
                Assert.AreEqual(answerBracket.ElementAt(i).LowerBound, cvm.YearsModel.Years[cvm.SelectedYear].Brackets.ElementAt(i).LowerBound);
                Assert.AreEqual(answerBracket.ElementAt(i).UpperBound, cvm.YearsModel.Years[cvm.SelectedYear].Brackets.ElementAt(i).UpperBound);
                Assert.AreEqual(answerBracket.ElementAt(i).NumReturns, cvm.YearsModel.Years[cvm.SelectedYear].Brackets.ElementAt(i).NumReturns);
                Assert.AreEqual(answerBracket.ElementAt(i).GrossIncome, cvm.YearsModel.Years[cvm.SelectedYear].Brackets.ElementAt(i).GrossIncome);
                Assert.AreEqual(answerBracket.ElementAt(i).TaxableIncome, cvm.YearsModel.Years[cvm.SelectedYear].Brackets.ElementAt(i).TaxableIncome);
                Assert.AreEqual(answerBracket.ElementAt(i).IncomeTax, cvm.YearsModel.Years[cvm.SelectedYear].Brackets.ElementAt(i).IncomeTax);
                Assert.AreEqual(answerBracket.ElementAt(i).PercentOfTaxableIncomePaid, cvm.YearsModel.Years[cvm.SelectedYear].Brackets.ElementAt(i).PercentOfTaxableIncomePaid);
                Assert.AreEqual(answerBracket.ElementAt(i).PercentOfGrossIncomePaid, cvm.YearsModel.Years[cvm.SelectedYear].Brackets.ElementAt(i).PercentOfGrossIncomePaid);
                Assert.AreEqual(answerBracket.ElementAt(i).AverageTotalIncomeTax, cvm.YearsModel.Years[cvm.SelectedYear].Brackets.ElementAt(i).AverageTotalIncomeTax);
            }

            //Check that year changed
            cvm.YearsModel.SelectedYear = 6;
            Assert.AreEqual(6, cvm.YearsModel.SelectedYear);

            //Check that the brackets changed
            answerBracket = Parser.ParseCSV(filePaths[6]).Brackets;
            for (int i = 0; i < answerBracket.Count; i++)
            {
                Assert.AreEqual(answerBracket.ElementAt(i).LowerBound, cvm.YearsModel.Years[cvm.SelectedYear].Brackets.ElementAt(i).LowerBound);
                Assert.AreEqual(answerBracket.ElementAt(i).UpperBound, cvm.YearsModel.Years[cvm.SelectedYear].Brackets.ElementAt(i).UpperBound);
                Assert.AreEqual(answerBracket.ElementAt(i).NumReturns, cvm.YearsModel.Years[cvm.SelectedYear].Brackets.ElementAt(i).NumReturns);
                Assert.AreEqual(answerBracket.ElementAt(i).GrossIncome, cvm.YearsModel.Years[cvm.SelectedYear].Brackets.ElementAt(i).GrossIncome);
                Assert.AreEqual(answerBracket.ElementAt(i).TaxableIncome, cvm.YearsModel.Years[cvm.SelectedYear].Brackets.ElementAt(i).TaxableIncome);
                Assert.AreEqual(answerBracket.ElementAt(i).IncomeTax, cvm.YearsModel.Years[cvm.SelectedYear].Brackets.ElementAt(i).IncomeTax);
                Assert.AreEqual(answerBracket.ElementAt(i).PercentOfTaxableIncomePaid, cvm.YearsModel.Years[cvm.SelectedYear].Brackets.ElementAt(i).PercentOfTaxableIncomePaid);
                Assert.AreEqual(answerBracket.ElementAt(i).PercentOfGrossIncomePaid, cvm.YearsModel.Years[cvm.SelectedYear].Brackets.ElementAt(i).PercentOfGrossIncomePaid);
                Assert.AreEqual(answerBracket.ElementAt(i).AverageTotalIncomeTax, cvm.YearsModel.Years[cvm.SelectedYear].Brackets.ElementAt(i).AverageTotalIncomeTax);
            }
        }

        [Test]
        public void TestControllerViewModelCalculatePopulation()
        {
            cvm.calculatePopulation();
            //Test that the population changes
            Assert.AreEqual(4522, cvm.population[0]);
        }

        [Test]
        public void TestControllerViewModelCountUnderPoverty()
        {
            cvm.calculatePopulation();
            cvm.countUnderPoverty();
            Assert.AreEqual(10999610, cvm.NumPovertyPop);
        }

        [Test]
        public void TestControllerViewModelCalcMaxPop()
        {
            cvm.calculatePopulation();
            Assert.AreEqual(2533618, cvm.countPopulationWithMaxBrackets(7));
        }
        [Test]
        public void TestControllerViewModelCalcNewTaxData()
        {
            cvm.calculatePopulation();
            Assert.AreEqual(507862946387, cvm.calculateNewTaxDataFromBracks(3, 7));
        }

        //GraphViewModel
        [Test]
        public void TestGraphViewModel()
        {
            GraphViewModel gvm = new GraphViewModel();
            Assert.IsFalse(gvm.Equals(null));
        }
        //MainViewModel
        [Test]
        public void TestMainViewModel()
        {
            GraphViewModel gvm = new GraphViewModel();
            gvm.YearsModel = new YearsModel();
            gvm.YearsModel.SelectedYear = 2003;
            Assert.IsTrue(gvm.YearsModel.SelectedYear == 2003);
        }
    }
}
