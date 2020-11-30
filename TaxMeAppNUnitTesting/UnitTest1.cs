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
    public class Tests
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
        //Testing Helpers
        //-------------------------------------------------------------------------------------------------

        //Check that numbers are formatted correctly
        [Test]
        public void TestFormatterThousands() {
            Random r = new Random();
            long t1, t2;

            //Generate Random numbers to test
            //1st Set of Cases, numbers < 1,000 and numbers > 1,000 but < 1,000,000
            t1 = r.Next(1, 999);
            t2 = r.Next(1, 999) * (long)Math.Pow(10, 3);
           
            //Test Each Case
            Assert.AreEqual(t1.ToString(), Formatter.Format(t1));
            Assert.AreEqual(t2.ToString("#,##0"), Formatter.Format(t2));
        }

        [Test]
        public void TestFormatterMillions() {
            Random r = new Random();
            long t3, t4;
            string t3Ans, t4Ans;

            //Generate Random numbers to test
            //2nd Set of Cases, numbers in the millions
            t3 = r.Next(1, 999) * (long)Math.Pow(10, 6);
            t4 = r.Next(1, 999) * (long)Math.Pow(10, 6);

            //For the millions, billions and trillions format the answer manually
            t3Ans = (t3 / (1 * Math.Pow(10, 6))).ToString(".00") + " million";
            t4Ans = (t4 / (1 * Math.Pow(10, 6))).ToString(".00") + " million";

            //Test Each Case
            Assert.AreEqual(t3Ans, Formatter.Format(t3));
            Assert.AreEqual(t4Ans, Formatter.Format(t4));
        }

        [Test]
        public void TestFormatterBillions() {
            Random r = new Random();
            long t5, t6;
            string t5Ans, t6Ans;

            //Generate Random numbers to test
            //3rd Set of Cases, numbers in the billions
            t5 = r.Next(1, 999) * (long)Math.Pow(10, 9);
            t6 = r.Next(1, 999) * (long)Math.Pow(10, 9);

            //For the millions, billions and trillions format the answer manually
            t5Ans = (t5 / (1 * Math.Pow(10, 9))).ToString(".00") + " billion";
            t6Ans = (t6 / (1 * Math.Pow(10, 9))).ToString(".00") + " billion";

            //Test Each Case
            Assert.AreEqual(t5Ans, Formatter.Format(t5));
            Assert.AreEqual(t6Ans, Formatter.Format(t6));
        }

        [Test]
        public void TestFormatterTrillions() {
            Random r = new Random();
            long t7, t8;
            string t7Ans, t8Ans;

            //Generate Random numbers to test
            //4th Set of Cases, numbers in the trillions
            t7 = r.Next(1, 999) * (long)Math.Pow(10, 12);
            t8 = r.Next(1, 999) * (long)Math.Pow(10, 12);

            //For the millions, billions and trillions format the answer manually
            t7Ans = (t7 / (1 * Math.Pow(10, 12))).ToString(".00") + " trillion";
            t8Ans = (t8 / (1 * Math.Pow(10, 12))).ToString(".00") + " trillion";

            //Test Each Case
            Assert.AreEqual(t7Ans, Formatter.Format(t7));
            Assert.AreEqual(t8Ans, Formatter.Format(t8));
        }

        [Test]
        public void TestLoader()
        {
            //Check that all of the years are loaded properly
            //Loader loader = new Loader();
            
            //Create answer of 2003-2018
            string[] fileNames = new string[18 - 2];
            string ans = "";
            int j;
            for (int i = 0; i < fileNames.Length; i++) {
                ans = "res\\TaxCSV\\";
                ans += "20";
                j = i + 3;
                if (j < 10)
                {
                    ans += "0" + j;
                }
                else {
                    ans += j;
                }
                ans += ".csv";
                fileNames[i] = ans;
            }
            //Check that the loader got all of the years
            Assert.AreEqual(fileNames, Loader.loadYearsTest());
        }

        //Testing that a file is parsed correctly, using 2003.csv as an example
        [Test]
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
            for (int i = 0; i < answerBracket.Count; i++) {
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
        public void TestControllerViewModelCalculatePopulation() {            
            cvm.calculatePopulation();
            //Test that the population changes
            Assert.AreEqual(4522, cvm.population[0]);
        }

        [Test]
        public void TestControllerViewModelCountUnderPoverty() {
            cvm.calculatePopulation();
            cvm.countUnderPoverty();
            Assert.AreEqual(10999610, cvm.NumPovertyPop);
        }

        [Test]
        public void TestControllerViewModelCalcMaxPop() {
            cvm.calculatePopulation();
            Assert.AreEqual(2533618, cvm.countPopulationWithMaxBrackets(7));
        }

        //GraphViewModel
        [Test]
        public void TestGraphViewModel() {
            GraphViewModel gvm = new GraphViewModel();
            Assert.IsFalse(gvm.Equals(null));
        }
        //MainViewModel
        [Test]
        public void TestMainViewModel() {
            GraphViewModel gvm = new GraphViewModel();
            gvm.YearsModel = new YearsModel();
            gvm.YearsModel.SelectedYear = 2003;
            Assert.IsTrue(gvm.YearsModel.SelectedYear == 2003);
        }
    }
}