using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxMeApp;
using TaxMeApp.Helpers;
using TaxMeApp.models;
using TaxMeApp.viewmodels;

namespace UnitTests
{
    [TestClass]
    public class HelpersUnitTest
    {
        private ControlViewModel controlVM;
        private string[] filePaths;

        [TestInitialize]
        public void Setup()
        {
            controlVM = new ControlViewModel();
            YearsModel yearsModel = new YearsModel();
            DataModel dataModel = new DataModel();
            controlVM.YearsModel = yearsModel;
            controlVM.DataModel = dataModel;

            filePaths = Directory.GetFiles("res\\TaxCSV", "*.csv");
            for (int i = 0; i < filePaths.Length; i++)
            {
                controlVM.YearsModel.Years.Add(i, Parser.ParseCSV(filePaths[i]));
            }
        }

        //-------------------------------------------------------------------------------------------------
        //Testing Helpers
        //-------------------------------------------------------------------------------------------------

        [TestMethod]
        public void TestFormatter()
        {

            // Testing a number of things here:
            // 1) Proper evaluation of Math.Abs(value), sending to correct logic
            // 2) Proper rounding at some edges, i.e. avoiding "1000.00 billion", isntead should be "1.00 trillion"
            // 3) Rounding of 3rd decimal place, since all word strings are .ToString("F2")
            // 4) Proper formatting of result into readable string with correct numbers + words

            long value;

            // Trillion logic:

            // One trillion + 1 decimal place
            value = 1200000000000;
            Assert.AreEqual("1.20 trillion", Formatter.Format(value));
            // One trillion + 2 decimal places
            value = 1230000000000;
            Assert.AreEqual("1.23 trillion", Formatter.Format(value));
            // One trillion + 3 decimal places, no round
            value = 1234000000000;
            Assert.AreEqual("1.23 trillion", Formatter.Format(value));
            // One trillion + 3 decimal places, with round
            value = 1235000000000;
            Assert.AreEqual("1.24 trillion", Formatter.Format(value));
            // Lowest value for trillion:
            // 999,500,000,000 <- this should round to trillion
            value = 999500000000;
            Assert.AreEqual("1.00 trillion", Formatter.Format(value));

            // Billion logic:

            // Highest value for billion:
            // 999,459,999,999
            value = 999499999999;
            Assert.AreEqual("999.50 billion", Formatter.Format(value));
            // One billion + 1 decimal place
            value = 1200000000;
            Assert.AreEqual("1.20 billion", Formatter.Format(value));
            // One billion + 2 decimal places
            value = 1230000000;
            Assert.AreEqual("1.23 billion", Formatter.Format(value));
            // One billion + 3 decimal places, no round
            value = 1234000000;
            Assert.AreEqual("1.23 billion", Formatter.Format(value));
            // One billion + 3 decimal places, with round
            value = 1235000000;
            Assert.AreEqual("1.24 billion", Formatter.Format(value));
            // Lowest value for billion:
            // 999,500,000 <- this should round to billion
            value = 999500000;
            Assert.AreEqual("1.00 billion", Formatter.Format(value));

            // Million logic:

            // Highest value for million:
            // 999,499,999 
            value = 999499999;
            Assert.AreEqual("999.50 million", Formatter.Format(value));
            // One million + 1 decimal place
            value = 1200000;
            Assert.AreEqual("1.20 million", Formatter.Format(value));
            // One million + 2 decimal places
            value = 1230000;
            Assert.AreEqual("1.23 million", Formatter.Format(value));
            // One million + 3 decimal places, no round
            value = 1234000;
            Assert.AreEqual("1.23 million", Formatter.Format(value));
            // One million + 3 decimal places, with round
            value = 1235000;
            Assert.AreEqual("1.24 million", Formatter.Format(value));
            // Lowest value for million:
            // 1,000,000 (because we catch 999,999 and below explicitly before we evaluate value) 
            value = 1000000;
            Assert.AreEqual("1.00 million", Formatter.Format(value));

            // 999,999 Highest value for just commas
            value = 999999;
            Assert.AreEqual("999,999", Formatter.Format(value));
            // 0 
            value = 0;
            Assert.AreEqual("0", Formatter.Format(value));

            // Negatives in same brackets:

            // -1
            value = -1;
            Assert.AreEqual("-1", Formatter.Format(value));
            // -999,999 Lowest value for just commas
            value = -999999;
            Assert.AreEqual("-999,999", Formatter.Format(value));

            // -1 million Highest for negative million
            value = -1000000;
            Assert.AreEqual("-1.00 million", Formatter.Format(value));
            // -999,499,999 Lowest for negative million
            value = -999499999;
            Assert.AreEqual("-999.50 million", Formatter.Format(value));

            // -999,500,000 Highest for negative billion
            value = -999500000;
            Assert.AreEqual("-1.00 billion", Formatter.Format(value));
            // -999,459,999,999 Lowest for negative billion
            value = -999499999999;
            Assert.AreEqual("-999.50 billion", Formatter.Format(value));


            // -999,500,000,000 Highest for negative trillion
            value = -999500000000;
            Assert.AreEqual("-1.00 trillion", Formatter.Format(value));

        }

        //Testing that a file is parsed correctly, using 2003.csv as an example
        [TestMethod]
        public void TestParser()
        {
            //Make a list of income year models to test
            ObservableCollection<IncomeYearModel> testModels = new ObservableCollection<IncomeYearModel>();
            string[] filePaths = Directory.GetFiles("res\\TaxCSV", "*.csv");

            //Testing 2003.csv
            testModels.Add(Parser.ParseCSV(filePaths[0]));

            ObservableCollection<IncomeYearModel> answers = new ObservableCollection<IncomeYearModel>();
            IncomeYearModel ans1 = new IncomeYearModel();
            ans1.Year = 2003;
            ObservableCollection<BracketModel> brackets1 = new ObservableCollection<BracketModel>();
            BracketModel b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14, b15, b16, b17, b18, b19;
            b1 = new BracketModel();
            b2 = new BracketModel();
            b3 = new BracketModel();
            b4 = new BracketModel();
            b5 = new BracketModel();
            b6 = new BracketModel();
            b7 = new BracketModel();
            b8 = new BracketModel();
            b9 = new BracketModel();
            b10 = new BracketModel();
            b11 = new BracketModel();
            b12 = new BracketModel();
            b13 = new BracketModel();
            b14 = new BracketModel();
            b15 = new BracketModel();
            b16 = new BracketModel();
            b17 = new BracketModel();
            b18 = new BracketModel();
            b19 = new BracketModel();

            //Manually copying the data into the brackets:
            b1.LowerBound = 0;
            b1.UpperBound = 0;
            b1.NumReturns = 4522;
            b1.GrossIncome = -5094991;
            b1.TaxableIncome = 0;
            b1.IncomeTax = 78488;
            b1.PercentOfTaxableIncomePaid = 0;
            b1.PercentOfGrossIncomePaid = 0;
            b1.AverageTotalIncomeTax = 17377;

            b2.LowerBound = 1;
            b2.UpperBound = 4999;
            b2.NumReturns = 835921;
            b2.GrossIncome = 2494290;
            b2.TaxableIncome = 610224;
            b2.IncomeTax = 72959;
            b2.PercentOfTaxableIncomePaid = 11.96;
            b2.PercentOfGrossIncomePaid = 2.9;
            b2.AverageTotalIncomeTax = 87.27977883;

            b3.LowerBound = 5000;
            b3.UpperBound = 9999;
            b3.NumReturns = 4116242;
            b3.GrossIncome = 31995563;
            b3.TaxableIncome = 7956944;
            b3.IncomeTax = 780450;
            b3.PercentOfTaxableIncomePaid = 9.81;
            b3.PercentOfGrossIncomePaid = 2.4;
            b3.AverageTotalIncomeTax = 189.602555;

            b4.LowerBound = 10000;
            b4.UpperBound = 14999;
            b4.NumReturns = 6042925;
            b4.GrossIncome = 75393048;
            b4.TaxableIncome = 28261843;
            b4.IncomeTax = 2750658;
            b4.PercentOfTaxableIncomePaid = 9.73;
            b4.PercentOfGrossIncomePaid = 3.6;
            b4.AverageTotalIncomeTax = 455.1865198;

            b5.LowerBound = 15000;
            b5.UpperBound = 19999;
            b5.NumReturns = 6304104;
            b5.GrossIncome = 110625567;
            b5.TaxableIncome = 50371975;
            b5.IncomeTax = 5404734;
            b5.PercentOfTaxableIncomePaid = 10.73;
            b5.PercentOfGrossIncomePaid = 4.9;
            b5.AverageTotalIncomeTax = 857.3357927;

            b6.LowerBound = 20000;
            b6.UpperBound = 24999;
            b6.NumReturns = 6095228;
            b6.GrossIncome = 137029808;
            b6.TaxableIncome = 72410263;
            b6.IncomeTax = 8274086;
            b6.PercentOfTaxableIncomePaid = 11.4;
            b6.PercentOfGrossIncomePaid = 6;
            b6.AverageTotalIncomeTax = 1357;

            b7.LowerBound = 25000;
            b7.UpperBound = 29999;
            b7.NumReturns = 6092090;
            b7.GrossIncome = 167694124;
            b7.TaxableIncome = 97429358;
            b7.IncomeTax = 11036040;
            b7.PercentOfTaxableIncomePaid = 11.3;
            b7.PercentOfGrossIncomePaid = 6.6;
            b7.AverageTotalIncomeTax = 1812;

            b8.LowerBound = 30000;
            b8.UpperBound = 39999;
            b8.NumReturns = 11856081;
            b8.GrossIncome = 413146253;
            b8.TaxableIncome = 254354428;
            b8.IncomeTax = 29737818;
            b8.PercentOfTaxableIncomePaid = 11.7;
            b8.PercentOfGrossIncomePaid = 7.2;
            b8.AverageTotalIncomeTax = 2508;

            b9.LowerBound = 40000;
            b9.UpperBound = 49999;
            b9.NumReturns = 9668366;
            b9.GrossIncome = 432975517;
            b9.TaxableIncome = 276796514;
            b9.IncomeTax = 34634209;
            b9.PercentOfTaxableIncomePaid = 12.5;
            b9.PercentOfGrossIncomePaid = 8;
            b9.AverageTotalIncomeTax = 3582;

            b10.LowerBound = 50000;
            b10.UpperBound = 74999;
            b10.NumReturns = 17024921;
            b10.GrossIncome = 1045511568;
            b10.TaxableIncome = 702291485;
            b10.IncomeTax = 94256193;
            b10.PercentOfTaxableIncomePaid = 13.4;
            b10.PercentOfGrossIncomePaid = 9;
            b10.AverageTotalIncomeTax = 5536;

            b11.LowerBound = 75000;
            b11.UpperBound = 99999;
            b11.NumReturns = 9486123;
            b11.GrossIncome = 816206695;
            b11.TaxableIncome = 575890228;
            b11.IncomeTax = 84253116;
            b11.PercentOfTaxableIncomePaid = 14.6;
            b11.PercentOfGrossIncomePaid = 10.3;
            b11.AverageTotalIncomeTax = 8882;

            b12.LowerBound = 100000;
            b12.UpperBound = 199999;
            b12.NumReturns = 8861764;
            b12.GrossIncome = 1167988946;
            b12.TaxableIncome = 875512626;
            b12.IncomeTax = 163342405;
            b12.PercentOfTaxableIncomePaid = 18.7;
            b12.PercentOfGrossIncomePaid = 14;
            b12.AverageTotalIncomeTax = 18432;

            b13.LowerBound = 200000;
            b13.UpperBound = 499999;
            b13.NumReturns = 1996787;
            b13.GrossIncome = 575673389;
            b13.TaxableIncome = 482717655;
            b13.IncomeTax = 120710917;
            b13.PercentOfTaxableIncomePaid = 25;
            b13.PercentOfGrossIncomePaid = 21;
            b13.AverageTotalIncomeTax = 60453;

            b14.LowerBound = 500000;
            b14.UpperBound = 999999;
            b14.NumReturns = 355750;
            b14.GrossIncome = 240943755;
            b14.TaxableIncome = 212761707;
            b14.IncomeTax = 60180621;
            b14.PercentOfTaxableIncomePaid = 28.3;
            b14.PercentOfGrossIncomePaid = 25;
            b14.AverageTotalIncomeTax = 169166;

            b15.LowerBound = 1000000;
            b15.UpperBound = 1499999;
            b15.NumReturns = 81588;
            b15.GrossIncome = 98744564;
            b15.TaxableIncome = 88335720;
            b15.IncomeTax = 25550669;
            b15.PercentOfTaxableIncomePaid = 28.9;
            b15.PercentOfGrossIncomePaid = 25.9;
            b15.AverageTotalIncomeTax = 313177;

            b16.LowerBound = 1500000;
            b16.UpperBound = 1999999;
            b16.NumReturns = 33984;
            b16.GrossIncome = 58440100;
            b16.TaxableIncome = 52488902;
            b16.IncomeTax = 15315946;
            b16.PercentOfTaxableIncomePaid = 29.2;
            b16.PercentOfGrossIncomePaid = 26.2;
            b16.AverageTotalIncomeTax = 450683;

            b17.LowerBound = 2000000;
            b17.UpperBound = 4999999;
            b17.NumReturns = 48235;
            b17.GrossIncome = 142091816;
            b17.TaxableIncome = 128190317;
            b17.IncomeTax = 36900818;
            b17.PercentOfTaxableIncomePaid = 28.8;
            b17.PercentOfGrossIncomePaid = 26;
            b17.AverageTotalIncomeTax = 765117;

            b18.LowerBound = 5000000;
            b18.UpperBound = 9999999;
            b18.NumReturns = 11160;
            b18.GrossIncome = 76253821;
            b18.TaxableIncome = 68499870;
            b18.IncomeTax = 19313626;
            b18.PercentOfTaxableIncomePaid = 28.2;
            b18.PercentOfGrossIncomePaid = 25.3;
            b18.AverageTotalIncomeTax = 1730613;

            b19.LowerBound = 10000000;
            b19.UpperBound = 999999999;
            b19.NumReturns = 6114;
            b19.GrossIncome = 158454920;
            b19.TaxableIncome = 140179919;
            b19.IncomeTax = 35416375;
            b19.PercentOfTaxableIncomePaid = 25.3;
            b19.PercentOfGrossIncomePaid = 22.4;
            b19.AverageTotalIncomeTax = 5792690;

            //Add brackets to list
            brackets1.Add(b1);
            brackets1.Add(b2);
            brackets1.Add(b3);
            brackets1.Add(b4);
            brackets1.Add(b5);
            brackets1.Add(b6);
            brackets1.Add(b7);
            brackets1.Add(b8);
            brackets1.Add(b9);
            brackets1.Add(b10);
            brackets1.Add(b11);
            brackets1.Add(b12);
            brackets1.Add(b13);
            brackets1.Add(b14);
            brackets1.Add(b15);
            brackets1.Add(b16);
            brackets1.Add(b17);
            brackets1.Add(b18);
            brackets1.Add(b19);

            //Add brackets to answer
            ans1.Brackets = brackets1;
            answers.Add(ans1);

            //Go through all the tests (Right now there's just 1 for the year 2003)
            for (int i = 0; i < testModels.Count; i++)
            {
                //Check that the years match
                Assert.AreEqual(answers[i].Year, testModels[i].Year);
                //Check that the number of brackets is the same
                Assert.AreEqual(answers[i].Brackets.Count, testModels[i].Brackets.Count);
                //Check that each value is the same
                for (int j = 0; j < testModels[i].Brackets.Count; j++)
                {
                    Assert.AreEqual(answers[i].Brackets[j].LowerBound, testModels[i].Brackets[j].LowerBound);
                    Assert.AreEqual(answers[i].Brackets[j].UpperBound, testModels[i].Brackets[j].UpperBound);
                    Assert.AreEqual(answers[i].Brackets[j].NumReturns, testModels[i].Brackets[j].NumReturns);
                    Assert.AreEqual(answers[i].Brackets[j].GrossIncome, testModels[i].Brackets[j].GrossIncome);
                    Assert.AreEqual(answers[i].Brackets[j].TaxableIncome, testModels[i].Brackets[j].TaxableIncome);
                    Assert.AreEqual(answers[i].Brackets[j].IncomeTax, testModels[i].Brackets[j].IncomeTax);
                    Assert.AreEqual(answers[i].Brackets[j].PercentOfTaxableIncomePaid, testModels[i].Brackets[j].PercentOfTaxableIncomePaid);
                    Assert.AreEqual(answers[i].Brackets[j].PercentOfGrossIncomePaid, testModels[i].Brackets[j].PercentOfGrossIncomePaid);
                    Assert.AreEqual(answers[i].Brackets[j].AverageTotalIncomeTax, testModels[i].Brackets[j].AverageTotalIncomeTax);
                }
            }
        }

    }
}
