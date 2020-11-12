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
        public static int povertyBrackets = 3;
        public static int maxBrackets = 0;
        public static int povertyPop = 0;
        public static int maxPop = 0;
        public List<double> sTaxVals = new List<double>();
        public double totalRevenueOld;
        public double totalRevenueNew;
        public static double newPolicyRate = 0.0;
        public double maxRate = 0.0;
        public string tRO {
            get{ return totalRevenueOld.ToString("#,##0"); }
        }
        public string tRN
        {
            get { return totalRevenueNew.ToString("#,##0"); }
        }
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
        public string numPovertyPop {
            get 
            {
                return povertyPop.ToString("#,##0");
            }
        }
        public string numMaxPop
        {
            get
            {
                return maxPop.ToString("#,##0");
            }
        }
        public string mRate {
            get { return "~" + Math.Round((maxRate * 100), 2) + "%"; }
        }

        private Test model;
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
            ParseCSV();
            Graph();
        }

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

        public double DeltaIncome
        {
            get { return model.MaxIncome - model.MinIncome; }
        }

        public int PovertyLineIndex
        {
            get { return 3; }
        }

        public int NumUnderPoverty
        {
            get { 
                //int total = 0;
                //for(int i = 0; i<=PovertyLineIndex; i++)
                //{
                //    total += Brackets[i].NumReturns;
                //}
                //return total;
                return povertyPop;
            }
        }
        public int NumMax {
            get {
                return maxPop;
            }
        }

        public void ParseCSV()
        {
            string path = "res\\2018Tax.csv";
     
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
                    //int value = bracket.NumReturns / 2;
                    Population.Add((bracket.NumReturns * 6)/10);
                    Population.Add((bracket.NumReturns * 4)/10);
                    //Population.Add(value);

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
            sTaxGen();

            SolidColorBrush lineFillBrush = new SolidColorBrush();
            lineFillBrush.Color = Colors.Gold;
            lineFillBrush.Opacity = 0.2;

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "2018",
                    Values = new ChartValues<int>(Population)
                },

                new LineSeries()
                {
                    Title = "Slant Tax",
                    Values = new ChartValues<double>(sTaxVals),
                    Stroke = Brushes.Yellow,
                    Fill = lineFillBrush
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
            totalRevenueOld = 0;
            totalRevenueNew = 0;
            for (int i = 0; i < Brackets.Count; i++) {
                Console.WriteLine("Bracket {0}\nTaxable Income = {1}, revenue = {2}", i,
                    Brackets.ElementAt(i).TaxableIncome*1000,
                    Brackets.ElementAt(i).TaxableIncome*1000 * (Brackets.ElementAt(i).PercentOfTaxableIncomePaid/ 100));
                totalRevenueOld += (Brackets.ElementAt(i).TaxableIncome*1000 * (Brackets.ElementAt(i).PercentOfTaxableIncomePaid/100));
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

            //maxRate = (totalRevenueOld * maxRatio) / maxTotal;
            maxRate = 0.0;
            double hConst;
            hConst = 3.0; //One graphing solution, just exagerate the height by fixed amount
            //while (totalRevenueNew - totalRevenueOld < 0)
            while(totalRevenueNew - (1932 * Math.Pow(10, 9)) < 0)
            {
                maxRate += 0.01;
                //hConst = 0.9 / maxRate; //One solution to graphing, make graph always look the same no matter the rate
                //maxRate = 0.9;
                sTaxVals.Clear();
                changeAmt = maxRate / (Brackets.Count - povertyBrackets - (Brackets.Count - maxBrackets) + 1);
                double currentRate = maxRate;
                totalRevenueNew = 0;
                for (int i = Brackets.Count - 1; i >= 0; i--)
                {
                    if (i > maxBrackets)
                    {
                        //sTaxVals.Add((currentRate*Brackets.ElementAt(i-1).NumReturns));
                        sTaxVals.Add(currentRate * maxY * hConst);
                        totalRevenueNew += (Brackets.ElementAt(i).TaxableIncome*1000 * currentRate);
                    }
                    else if (i <= maxBrackets && i > povertyBrackets)
                    {
                        currentRate -= changeAmt;
                        sTaxVals.Add(currentRate * maxY * hConst);
                        //sTaxVals.Add((currentRate * Brackets.ElementAt(i-1).NumReturns));
                        totalRevenueNew += (Brackets.ElementAt(i).TaxableIncome*1000 * currentRate);
                    }
                    else
                    {
                        currentRate = 0;
                        sTaxVals.Add(0.0);
                    }
                    Console.WriteLine("Bracket {0}\nTaxable Income = {1}, current rate = {2}, revenue = {3}", i,
                        Brackets.ElementAt(i).TaxableIncome*1000,
                        currentRate,
                        Brackets.ElementAt(i).TaxableIncome*1000 * currentRate);
                }
                sTaxVals.Reverse();
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

        public double nPR{
            get { return newPolicyRate; }
        }
    }
}
