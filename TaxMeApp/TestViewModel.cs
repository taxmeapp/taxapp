using CsvHelper;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using TaxMeApp.models;

namespace TaxMeApp
{
    public class TestViewModel : INotifyPropertyChanged
    {
        public static int povertyBrackets = 3; //Fixed position of poverty line
        public static int maxBrackets = 0; //Calculated, # of brackets on right side
        public static int povertyPop = 0; //Calculated, currently rounded to nearest bracket
        public static int maxPop = 0; //Calculated, population of maxBrackets
        public List<double> sTaxVals = new List<double>();
        public List<double> originalTaxVals = new List<double>();
        public List<double> revenueByBracketValsOld = new List<double>();
        public List<double> revenueByBracketValsNew = new List<double>();
        public double totalRevenueOld;
        public double totalRevenueNew;
        public double maxRate = 0.0;
        public int year { get; set; }
        //Variables used by GUI
        //Old Revenue
        public string tRO {
            get{ return totalRevenueOld.ToString("#,##0"); }
        }
        //New Revenue
        public string tRN
        {
            get { return totalRevenueNew.ToString("#,##0"); }
        }
        //Difference
        public string rDiff {
            get
            {
                double diff = totalRevenueNew - totalRevenueOld;
                string ans = diff.ToString("#,##0");
                if (diff > 0)
                {
                    ans = "+" + ans + " Surplus";
                }
                else {
                    ans = "-" + ans + " Deficit";
                }
                return ans;
            }
        }
        //Pop under poverty (rounded to nearest bracket)
        public string numPovertyPop {
            get 
            {
                return povertyPop.ToString("#,##0");
            }
        }
        //Max rate population
        public string numMaxPop
        {
            get
            {
                return maxPop.ToString("#,##0");
            }
        }
        //Max rate with slant tax
        public string mRate {
            get { return "~" + Math.Round((maxRate * 100), 3) + "%"; }
        }
        //Used by min income text box
        public double MinIncome
        {
            get { return model.MinIncome; }
            set
            {

                if (model.MinIncome != value)
                {
                    model.MinIncome = value;
                    OnPropertyChange("MinIncome");
                    OnPropertyChange("DeltaIncome");
                }


            }
        }
        //Used by max income text box
        public double MaxIncome
        {
            get { return model.MaxIncome; }
            set
            {

                if (model.MaxIncome != value)
                {
                    model.MaxIncome = value;
                    OnPropertyChange("MaxIncome");
                    OnPropertyChange("DeltaIncome");
                }

            }
        }
        //Used by GUI
        public double DeltaIncome
        {
            get { return model.MaxIncome - model.MinIncome; }
        }
        public int NumUnderPoverty
        {
            get
            {
                return povertyPop;
            }
        }
        public int PovertyLineIndex {
            get {
                return povertyBrackets;
            }
        }
        public int NumMax
        {
            get
            {
                return maxPop;
            }
        }
        //List of years from /res
        public List<string> YearsAvailable {
            get {
                List<string> ans = new List<string>();

                //Get a list of files from the res folder
                string[] files = Directory.GetFiles("res\\");
                string curString = "";
                int extInd = 0; //extInd gets rid of Tax.csv
                //Add all the years to the answer list
                for (int i = 0; i < files.Length; i++) {
                    curString = files[i];
                    curString = curString.Substring(4); //get rid of res/
                    extInd = curString.IndexOf("T");
                    curString = curString.Substring(0, extInd); //get rid of Tax.csv
                    ans.Add(curString);
                }

                return ans;
            }
        }
        public string YearsAvailableFirst {
            get {
                return YearsAvailable.ElementAt(0);
            }
        }
        //End of main variables

        private Test model;
        ObservableCollection<IncomeYearModel> IncomeYear { get; set; }
        public ObservableCollection<BracketModel> Brackets { get; set; }
        public string[] Labels { get; set; }
        public List<int> Population = new List<int>();
        public SeriesCollection SeriesCollection { get; set; }

        public TestViewModel()
        {
            model = new Test
            {
                MinIncome = 16000,
                MaxIncome = 400000
            };

            //Brackets including and under poverty line will be one color, normal brackets will be another, 
            //and max will be another color
            Brush povertyColor = Brushes.Red;
            Brush normalColor = Brushes.Blue;
            Brush maxColor = Brushes.Lime;
            CartesianMapper<int> povertyMapper = new CartesianMapper<int>()
                .X((value, index) => index)
                .Y((value) => value)
                .Fill((value, index) => {
                    if (index <= povertyBrackets) {
                        return povertyColor;
                    }
                    else if (index > povertyBrackets && index <= maxBrackets) {
                        return normalColor;
                    }
                    else {
                        return maxColor;
                    }

                });
            LiveCharts.Charting.For<int>(povertyMapper, SeriesOrientation.Horizontal);

            Brackets = new ObservableCollection<BracketModel>();
            //IncomeYear = new ObservableCollection<IncomeYearModel>();
            //IncomeYear.ElementAt(0).year = 2018;
            //IncomeYear.ElementAt(0).yearData = Brackets.ElementAt(0);
            year = 2018;
            ParseCSV("res\\2018Tax.csv");
            Graph();
        }

        public void ParseCSV(string path)
        {
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                reader.ReadLine();
                var brackets = csv.GetRecords<BracketModel>();
                Brackets = new ObservableCollection<BracketModel>(brackets);
            }

            int i = 0;
            foreach (BracketModel bracket in Brackets)
            {
                if(i == 11)
                {
                    Population.Add((bracket.NumReturns * 6)/10);
                    Population.Add((bracket.NumReturns * 4)/10);
                }
                else
                {
                    Population.Add(bracket.NumReturns);
                }
                
                i++;
            }
        }
        public void Graph()
        {
            ColorGraph(); //Find max brackets
            sTaxGen(); //Get Slant Tax Values

            SolidColorBrush slantTaxFillBrush = new SolidColorBrush();
            slantTaxFillBrush.Color = Colors.Gold;
            slantTaxFillBrush.Opacity = 0.2;

            SolidColorBrush transparentBrush = new SolidColorBrush();
            transparentBrush.Opacity = 0.0;
            transparentBrush.Color = Colors.White;

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = year.ToString(),
                    Values = new ChartValues<int>(Population)
                },

                new LineSeries()
                {
                    Title = "Old Tax Revenue By Bracket",
                    Values = new ChartValues<double>(revenueByBracketValsOld),
                    Stroke = Brushes.DarkGreen,
                    Fill = transparentBrush
                },
                new LineSeries()
                {
                    Title = "New Tax Revenue By Bracket",
                    Values = new ChartValues<double>(revenueByBracketValsNew),
                    Stroke = Brushes.LightGreen,
                    Fill = transparentBrush
                },
                new LineSeries()
                {
                    Title = "Old Tax Rates",
                    Values = new ChartValues<double>(originalTaxVals),
                    Stroke = Brushes.DarkGoldenrod,
                    Fill = transparentBrush
                },
                new LineSeries()
                {
                    Title = "Slant Tax",
                    Values = new ChartValues<double>(sTaxVals),
                    Stroke = Brushes.Yellow,
                    Fill = slantTaxFillBrush
                }
            };


            Labels = new[]
            {
                "$0",
                "$1 under $5,000",
                "$5,000 under $10,000",
                "$10,000 under $15,000",
                "$15,000 under $20,000",
                "$20,000 under $25,000",
                "$25,000 under $30,000",
                "$30,000 under $40,000",
                "$40,000 under $50,000",
                "$50,000 under $75,000",
                "$75,000 under $100,000",
                "$100,000 under $150,000",
                "$150,000 under $200,000",
                "$200,000 under $500,000",
                "$500,000 under $1,000,000",
                "$1,000,000 under $1,500,000",
                "$1,500,000 under $2,000,000",
                "$2,000,000 under $5,000,000",
                "$5,000,000 under $10,000,000",
                "$10,000,000 or more"
            };
        }

        private void sTaxGen()
        {
            double hConst;
            double revConst;
            hConst = 8.0; //One graphing solution, just exagerate the height by fixed amount
            revConst = 20000; //Adjust revenue by a constant factor so that it looks nice on the graph
            totalRevenueOld = 0;
            totalRevenueNew = 0;
            for (int i = 0; i < Brackets.Count; i++) {
                Console.WriteLine("Bracket {0}\nTaxable Income = {1}, revenue = {2}", i,
                    Brackets.ElementAt(i).TaxableIncome*1000,
                    Brackets.ElementAt(i).TaxableIncome*1000 * (Brackets.ElementAt(i).PercentOfTaxableIncomePaid/ 100));
                totalRevenueOld += (Brackets.ElementAt(i).TaxableIncome*1000 * (Brackets.ElementAt(i).PercentOfTaxableIncomePaid/100));
                revenueByBracketValsOld.Add(Brackets.ElementAt(i).TaxableIncome * 1000 * (Brackets.ElementAt(i).PercentOfTaxableIncomePaid / 100) / (revConst));
            }
            double maxTotal = 0.0;
            maxRate = 0.0;
            double maxRatio = 0.9;
            double changeAmt = 0.0;
            double maxY = 0;
            for (int i = maxBrackets; i < Brackets.Count; i++) {
                maxTotal += Brackets.ElementAt(i).TaxableIncome*1000;
                if (Brackets.ElementAt(i).NumReturns > maxY) {
                    maxY = Brackets.ElementAt(i).NumReturns;
                }
            }
            for (int i = 0; i < Brackets.Count; i++) {
                originalTaxVals.Add(Brackets.ElementAt(i).PercentOfTaxableIncomePaid / 100 * maxY * hConst);
            }
            maxRate = 0.20; //Start at 20% to save time
            while (totalRevenueNew - totalRevenueOld < 0)
            {
                maxRate += 0.00001;
                sTaxVals.Clear();
                revenueByBracketValsNew.Clear();
                changeAmt = maxRate / (Brackets.Count - povertyBrackets - (Brackets.Count - maxBrackets) + 1);
                double currentRate = maxRate;
                totalRevenueNew = 0;
                for (int i = Brackets.Count - 1; i >= 0; i--)
                {
                    if (i > maxBrackets)
                    {
                        sTaxVals.Add(currentRate * maxY * hConst);
                        totalRevenueNew += (Brackets.ElementAt(i).TaxableIncome*1000 * currentRate);
                        revenueByBracketValsNew.Add(Brackets.ElementAt(i).TaxableIncome * 1000 * currentRate / (revConst));
                    }
                    else if (i <= maxBrackets && i > povertyBrackets)
                    {
                        currentRate -= changeAmt;
                        sTaxVals.Add(currentRate * maxY * hConst);
                        totalRevenueNew += (Brackets.ElementAt(i).TaxableIncome*1000 * currentRate);
                        revenueByBracketValsNew.Add(Brackets.ElementAt(i).TaxableIncome * 1000 * currentRate / (revConst));
                    }
                    else
                    {
                        currentRate = 0;
                        sTaxVals.Add(0.0);
                        revenueByBracketValsNew.Add(0.0);
                    }
                }
                sTaxVals.Reverse();
                revenueByBracketValsNew.Reverse();
            }
        }

        //Used to find and color the number of tax brackets that pay the maximum rate
        //Poverty line is basically hard-coded because it is a known and set value
        //Maximum rate brackets are generated
        private void ColorGraph()
        {
            //Original Slant Tax Coloring:
            povertyPop = 0;
            for (int i = 0; i < povertyBrackets + 1; i++)
            {
                povertyPop += Population[i];
            }
            int j = Population.Count - 1;
            maxPop = 0;
            //Find max tax population >= poverty population
            while (maxPop < povertyPop)
            {
                //Console.WriteLine("j = {0}, pop = {1}", j, Population[j]);
                maxPop += Population[j];
                j--;
            }
            maxBrackets = j;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
