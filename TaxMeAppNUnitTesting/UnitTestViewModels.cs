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
        private ControlViewModel ControlVM;
        private YearsModel yearsModel;
        string[] filePaths;

        [SetUp]
        public void Setup()
        {
            ControlVM = new ControlViewModel();
            yearsModel = new YearsModel();
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
        //Testing View Models
        //-------------------------------------------------------------------------------------------------

        // ControlViewModel



        


        //ControlViewModel
        [Test]
        public void TestControllerViewModelYearChange()
        {

            //Check that year is set correctly
            ControlVM.YearsModel.SelectedYear = 5;
            Assert.AreEqual(5, ControlVM.YearsModel.SelectedYear);

            ObservableCollection<BracketModel> answerBracket;
            //Check that the brackets match
            answerBracket = Parser.ParseCSV(filePaths[5]).Brackets;
            for (int i = 0; i < answerBracket.Count; i++)
            {
                Assert.AreEqual(answerBracket.ElementAt(i).LowerBound, ControlVM.YearsModel.Years[ControlVM.SelectedYear].Brackets.ElementAt(i).LowerBound);
                Assert.AreEqual(answerBracket.ElementAt(i).UpperBound, ControlVM.YearsModel.Years[ControlVM.SelectedYear].Brackets.ElementAt(i).UpperBound);
                Assert.AreEqual(answerBracket.ElementAt(i).NumReturns, ControlVM.YearsModel.Years[ControlVM.SelectedYear].Brackets.ElementAt(i).NumReturns);
                Assert.AreEqual(answerBracket.ElementAt(i).GrossIncome, ControlVM.YearsModel.Years[ControlVM.SelectedYear].Brackets.ElementAt(i).GrossIncome);
                Assert.AreEqual(answerBracket.ElementAt(i).TaxableIncome, ControlVM.YearsModel.Years[ControlVM.SelectedYear].Brackets.ElementAt(i).TaxableIncome);
                Assert.AreEqual(answerBracket.ElementAt(i).IncomeTax, ControlVM.YearsModel.Years[ControlVM.SelectedYear].Brackets.ElementAt(i).IncomeTax);
                Assert.AreEqual(answerBracket.ElementAt(i).PercentOfTaxableIncomePaid, ControlVM.YearsModel.Years[ControlVM.SelectedYear].Brackets.ElementAt(i).PercentOfTaxableIncomePaid);
                Assert.AreEqual(answerBracket.ElementAt(i).PercentOfGrossIncomePaid, ControlVM.YearsModel.Years[ControlVM.SelectedYear].Brackets.ElementAt(i).PercentOfGrossIncomePaid);
                Assert.AreEqual(answerBracket.ElementAt(i).AverageTotalIncomeTax, ControlVM.YearsModel.Years[ControlVM.SelectedYear].Brackets.ElementAt(i).AverageTotalIncomeTax);
            }

            //Check that year changed
            ControlVM.YearsModel.SelectedYear = 6;
            Assert.AreEqual(6, ControlVM.YearsModel.SelectedYear);

            //Check that the brackets changed
            answerBracket = Parser.ParseCSV(filePaths[6]).Brackets;
            for (int i = 0; i < answerBracket.Count; i++)
            {
                Assert.AreEqual(answerBracket.ElementAt(i).LowerBound, ControlVM.YearsModel.Years[ControlVM.SelectedYear].Brackets.ElementAt(i).LowerBound);
                Assert.AreEqual(answerBracket.ElementAt(i).UpperBound, ControlVM.YearsModel.Years[ControlVM.SelectedYear].Brackets.ElementAt(i).UpperBound);
                Assert.AreEqual(answerBracket.ElementAt(i).NumReturns, ControlVM.YearsModel.Years[ControlVM.SelectedYear].Brackets.ElementAt(i).NumReturns);
                Assert.AreEqual(answerBracket.ElementAt(i).GrossIncome, ControlVM.YearsModel.Years[ControlVM.SelectedYear].Brackets.ElementAt(i).GrossIncome);
                Assert.AreEqual(answerBracket.ElementAt(i).TaxableIncome, ControlVM.YearsModel.Years[ControlVM.SelectedYear].Brackets.ElementAt(i).TaxableIncome);
                Assert.AreEqual(answerBracket.ElementAt(i).IncomeTax, ControlVM.YearsModel.Years[ControlVM.SelectedYear].Brackets.ElementAt(i).IncomeTax);
                Assert.AreEqual(answerBracket.ElementAt(i).PercentOfTaxableIncomePaid, ControlVM.YearsModel.Years[ControlVM.SelectedYear].Brackets.ElementAt(i).PercentOfTaxableIncomePaid);
                Assert.AreEqual(answerBracket.ElementAt(i).PercentOfGrossIncomePaid, ControlVM.YearsModel.Years[ControlVM.SelectedYear].Brackets.ElementAt(i).PercentOfGrossIncomePaid);
                Assert.AreEqual(answerBracket.ElementAt(i).AverageTotalIncomeTax, ControlVM.YearsModel.Years[ControlVM.SelectedYear].Brackets.ElementAt(i).AverageTotalIncomeTax);
            }
        }

        [Test]
        public void TestControllerViewModelCalculatePopulation()
        {
            //ControlVM.CalculatePopulation();
            //Test that the population changes
            //Assert.AreEqual(4522, ControlVM.Population[0]);
        }

        [Test]
        public void TestControllerViewModelCountUnderPoverty()
        {
            //ControlVM.CalculatePopulation();
            //ControlVM.CountUnderPoverty();
            //Assert.AreEqual(10999610, ControlVM.NumPovertyPop);
        }

        [Test]
        public void TestControllerViewModelCalcMaxPop()
        {
            //ControlVM.CalculatePopulation();
            //Assert.AreEqual(2533618, ControlVM.CountPopulationWithMaxBrackets(7));
        }
        [Test]
        public void TestControllerViewModelCalcNewTaxData()
        {
            //ControlVM.CalculatePopulation();
            //Assert.AreEqual(507862946387, ControlVM.CalculateNewTaxDataFromBracks(3, 7));
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
