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
        public List<double> sTaxRates = new List<double>();
        public List<double> originalTaxVals = new List<double>();
        public List<double> revenueByBracketValsOld = new List<double>();
        public List<double> revenueByBracketValsNew = new List<double>();
        private ObservableCollection<IncomeYearModel> _years;
        private IncomeYearModel CurrentYear;
        private ObservableCollection<TaxPolicyModel> _taxPlans;
        private TaxPolicyModel currentTaxPlan;
        private ObservableCollection<BracketDisplayModel> _brackets = new ObservableCollection<BracketDisplayModel>();
        private BracketDisplayModel currentBracket;
        public double totalRevenueOld;
        public double totalRevenueNew;
        public double maxRate = 0.0;
        public double curRate = 0.0;
        public SeriesCollection seriesCollection = new SeriesCollection();
        public int year { get; set; }
        //Variables used by GUI
        //Old Revenue
        public string tRO {
            get{ return totalRevenueOld.ToString("#,##0"); }
            set {
                OnPropertyChange("tRO");
            }
        }
        //New Revenue
        public string tRN
        {
            get { return totalRevenueNew.ToString("#,##0"); }
            set
            {
                OnPropertyChange("tRN");
            }
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
            set
            {
                OnPropertyChange("rDiff");
            }
        }
        //Pop under poverty (rounded to nearest bracket)
        public string numPovertyPop {
            get 
            {
                return povertyPop.ToString("#,##0");
            }
            set {
                povertyPop = 0;
                for (int i = 0; i < povertyBrackets + 1; i++)
                {
                    povertyPop += Population[i];
                }
                OnPropertyChange("numPovertyPop");
            }
        }
        //Max rate population
        public string numMaxPop
        {
            get
            {
                return maxPop.ToString("#,##0");
            }
            set
            {
                int j = Population.Count - 1;
                maxPop = 0;
                while (maxPop < povertyPop)
                {
                    maxPop += Population[j];
                    j--;
                }
                OnPropertyChange("numMaxPop");
            }
        }
        //Max rate with slant tax
        public string mRate {
            get { return "~" + Math.Round((maxRate * 100), 3) + "%"; }
            set
            {
                OnPropertyChange("mRate");
            }
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
        public double CurrentRate {
            get {
                return curRate;
            }
            set {
                if (curRate != value) {
                    curRate = value;
                    OnPropertyChange("CurrentRate");
                    updateGraph(currentBracket.index, curRate);
                }
                Console.WriteLine("Current Slider Rate = {0}, Bracket Index = {1}", curRate, currentBracket.index);
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
        public void updateGraph(int ind, double rate) {
            List<double> ans = new List<double>();
            List<double> ans2 = new List<double>();
            Console.WriteLine("Changing Bracket {0} to {1}%", ind, rate);

            double hConst = .03;
            double maxY = 0;
            for (int i = maxBrackets; i < CurrentYear.brackets.Count; i++)
            {
                if (CurrentYear.brackets.ElementAt(i).NumReturns > maxY)
                {
                    maxY = CurrentYear.brackets.ElementAt(i).NumReturns;
                }
            }

            
            for (int i = 0; i < sTaxVals.Count; i++) {
                if (i != ind)
                {
                    ans.Add(sTaxVals.ElementAt(i));
                    ans2.Add(sTaxRates.ElementAt(i));
                }
                else {
                    ans.Add(curRate * maxY * hConst);
                    ans2.Add(curRate/100);
                }
            }

            sTaxVals = ans;
            sTaxRates = ans2;
            totalRevenueNew = 0;
            for (int i = 0; i < CurrentYear.brackets.Count; i++) {
                totalRevenueNew += CurrentYear.brackets.ElementAt(i).TaxableIncome * 1000 * sTaxRates.ElementAt(i);
            }
            OnPropertyChange("tRN");
            OnPropertyChange("rDiff");

            SolidColorBrush slantTaxFillBrush = new SolidColorBrush();
            slantTaxFillBrush.Color = Colors.Gold;
            slantTaxFillBrush.Opacity = 0.2;

            SolidColorBrush transparentBrush = new SolidColorBrush();
            transparentBrush.Opacity = 0.0;
            transparentBrush.Color = Colors.White;

            seriesCollection.Clear();

            seriesCollection.Add(
                new ColumnSeries
                {
                    Title = CurrentYear.year.ToString() + " Income",
                    Values = new ChartValues<int>(Population)
                }
            );
            seriesCollection.Add(
                new LineSeries()
                {
                    Title = "Old Tax Revenue By Bracket",
                    Values = new ChartValues<double>(revenueByBracketValsOld),
                    Stroke = Brushes.DarkGreen,
                    Fill = transparentBrush
                }
            );
            seriesCollection.Add(
                new LineSeries()
                {
                    Title = "New Tax Revenue By Bracket",
                    Values = new ChartValues<double>(revenueByBracketValsNew),
                    Stroke = Brushes.LightGreen,
                    Fill = transparentBrush
                }
            );
            seriesCollection.Add(
                new LineSeries()
                {
                    Title = "Old Tax Rates",
                    Values = new ChartValues<double>(originalTaxVals),
                    Stroke = Brushes.DarkGoldenrod,
                    Fill = transparentBrush
                }

            );
            seriesCollection.Add(
                 new LineSeries()
                 {
                     Title = "Slant Tax",
                     Values = new ChartValues<double>(ans),
                     Stroke = Brushes.Yellow,
                     Fill = slantTaxFillBrush
                 }
            );

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
            if (_brackets.Count > 0)
            {
                _brackets.Clear();
            }
            for (int i = 0; i < Labels.Length - 1; i++)
            {
                List<double> rateList = new List<double>();
                rateList.Add(sTaxRates.ElementAt(i));
                _brackets.Add(new BracketDisplayModel(Labels[i], rateList, i));
            }
            SelectedBracket = _brackets.ElementAt(ind);
        }
        public TaxPolicyModel SelectedTaxPlan
        {
            get { return currentTaxPlan; }
            set
            {
                if (currentTaxPlan != value)
                {
                    Console.WriteLine("Tax plan changed from " + currentTaxPlan.name + " to " + value.name);
                    currentTaxPlan = value;
                    OnPropertyChange("SelectedTaxPlan");
                }
            }
        }
        public BracketDisplayModel SelectedBracket
        {
            get { return currentBracket; }
            set
            {
                if (currentBracket != value && value != null)
                {
                    //Console.WriteLine("Tax plan changed from " + currentBracket.label + " to " + value.label);
                    currentBracket = value;
                    double cRate = currentBracket.taxRate.ElementAt(0) * 100;
                    string printRate = "~" + cRate.ToString("##0.###") + "%";
                    Console.WriteLine("Selected Bracket: {0} Slant tax rate is {1}", currentBracket.label, printRate);
                    OnPropertyChange("SelectedBracket");
                }
                curRate = currentBracket.taxRate.ElementAt(0)*100;
                OnPropertyChange("CurrentRate");
            }
        }
        public IncomeYearModel SelectedYear
        {
            get { return CurrentYear; }
            set
            {
                if(CurrentYear.year != value.year)
                {
                    Console.WriteLine("year changed from " + CurrentYear.year + " to " + value.year.ToString());
                    CurrentYear = value;
                    ParseCSV();
                    clearGraph();
                    Graph();
                    numPovertyPop = "0";
                    numMaxPop = "0";
                    tRO = "0";
                    tRN = "0";
                    rDiff = "0";
                    mRate = "0";
                    OnPropertyChange("SelectedYear");
                    OnPropertyChange("Population");
                }
            }
        }
        public void clearGraph() {
            seriesCollection.Clear();
            originalTaxVals.Clear();
            sTaxVals.Clear();
            revenueByBracketValsOld.Clear();
            revenueByBracketValsNew.Clear();
            numPovertyPop = "0";
            numMaxPop = "0";
            tRO = "0";
            tRN = "0";
            rDiff = "0";
            mRate = "0";
            _brackets = new ObservableCollection<BracketDisplayModel>();
        }
        public ObservableCollection<TaxPolicyModel> TaxPlans
        {
            get { return _taxPlans; }
            set {
                if (_taxPlans != value) {
                    _taxPlans = value;
                    OnPropertyChange("TaxPlans");
                }
            }
        }
        public ObservableCollection<BracketDisplayModel> GetBrackets
        {
            get { return _brackets; }
            set
            {
                if (_brackets != value)
                {
                    _brackets = value;
                    OnPropertyChange("GetBrackets");
                }
            }
        }
        public ObservableCollection<IncomeYearModel> IncomeYears
        {
            get { return _years; }
            set
            {
                if(_years != value)
                {
                    _years = value;
                    OnPropertyChange("IncomeYears");
                }
            }
        }

        public ObservableCollection<int> Population
        {
            get
            {
                ObservableCollection<int> pop = new ObservableCollection<int>();
                int i = 0;
                foreach (BracketModel bracket in CurrentYear.brackets)
                {
                    if (i == 11)
                    {
                        pop.Add((bracket.NumReturns * 6) / 10);
                        pop.Add((bracket.NumReturns * 4) / 10);
                    }
                    else
                    {
                        pop.Add(bracket.NumReturns);
                    }

                    i++;
                }
                return pop;
            }
        }
        //End of main variables

        private Test model;
        ObservableCollection<IncomeYearModel> IncomeYear { get; set; }
        public ObservableCollection<BracketModel> Brackets { get; set; }
        public string[] Labels { get; set; }
        public SeriesCollection SeriesCollection { 
            get {
                return seriesCollection;        
            } 
            set {
                seriesCollection = value;
            } 
        }
        public TestViewModel()
        {
            model = new Test
            {
                MinIncome = 16000,
                MaxIncome = 400000
            };
            _years = new ObservableCollection<IncomeYearModel>();
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
            CurrentYear = new IncomeYearModel { 
                year = 2018
            };
            _brackets = new ObservableCollection<BracketDisplayModel>();
            GetYears();
            //IncomeYear = new ObservableCollection<IncomeYearModel>();
            //IncomeYear.ElementAt(0).year = 2018;
            //IncomeYear.ElementAt(0).yearData = Brackets.ElementAt(0);
            // year = 2018;
            GetTaxPlans();
            ParseCSV();
            Graph();
        }

        public void ParseCSV()
        {
            string path = ("res\\" + CurrentYear.year + ".csv");
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                reader.ReadLine();
                var brackets = csv.GetRecords<BracketModel>();
                CurrentYear.brackets = new ObservableCollection<BracketModel>(brackets);
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
            seriesCollection.Add(
                new ColumnSeries
                {
                    Title = CurrentYear.year.ToString() + " Income",
                    Values = new ChartValues<int>(Population)
                }
            );
            seriesCollection.Add(
                new LineSeries()
                {
                    Title = "Old Tax Revenue By Bracket",
                    Values = new ChartValues<double>(revenueByBracketValsOld),
                    Stroke = Brushes.DarkGreen,
                    Fill = transparentBrush
                }
            );
            seriesCollection.Add(
                new LineSeries()
                {
                    Title = "New Tax Revenue By Bracket",
                    Values = new ChartValues<double>(revenueByBracketValsNew),
                    Stroke = Brushes.LightGreen,
                    Fill = transparentBrush
                }
            );
            seriesCollection.Add(
                new LineSeries()
                {
                    Title = "Old Tax Rates",
                    Values = new ChartValues<double>(originalTaxVals),
                    Stroke = Brushes.DarkGoldenrod,
                    Fill = transparentBrush
                }

            );
            seriesCollection.Add(
                 new LineSeries()
                 {
                     Title = "Slant Tax",
                     Values = new ChartValues<double>(sTaxVals),
                     Stroke = Brushes.Yellow,
                     Fill = slantTaxFillBrush
                 }
            );

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
            if (_brackets.Count > 0) {
                _brackets = new ObservableCollection<BracketDisplayModel>();
            }
            for (int i = 0; i < Labels.Length-1; i++)
            {
                List<double> rateList = new List<double>();
                rateList.Add(sTaxRates.ElementAt(i));
                _brackets.Add(new BracketDisplayModel(Labels[i], rateList, i));
            }
            SelectedBracket = _brackets.ElementAt(0);
        }

        private void sTaxGen()
        {
            double hConst;
            double revConst;
            hConst = 8.0; //One graphing solution, just exagerate the height by fixed amount
            revConst = 20000; //Adjust revenue by a constant factor so that it looks nice on the graph
            totalRevenueOld = 0;
            totalRevenueNew = 0;
            Console.WriteLine(CurrentYear.year.ToString());
            for (int i = 0; i < CurrentYear.brackets.Count; i++) {
                //Console.WriteLine("Bracket {0}\nTaxable Income = {1}, revenue = {2}", i,
                //    CurrentYear.brackets.ElementAt(i).TaxableIncome*1000,
                //    CurrentYear.brackets.ElementAt(i).TaxableIncome*1000 * (CurrentYear.brackets.ElementAt(i).PercentOfTaxableIncomePaid/ 100));
                totalRevenueOld += (CurrentYear.brackets.ElementAt(i).TaxableIncome*1000 * (CurrentYear.brackets.ElementAt(i).PercentOfTaxableIncomePaid/100));
                revenueByBracketValsOld.Add(CurrentYear.brackets.ElementAt(i).TaxableIncome * 1000 * (CurrentYear.brackets.ElementAt(i).PercentOfTaxableIncomePaid / 100) / (revConst));
            }
            double maxTotal = 0.0;
            maxRate = 0.0;
            double maxRatio = 0.9;
            double changeAmt = 0.0;
            double maxY = 0;
            for (int i = maxBrackets; i < CurrentYear.brackets.Count; i++) {
                maxTotal += CurrentYear.brackets.ElementAt(i).TaxableIncome*1000;
                if (CurrentYear.brackets.ElementAt(i).NumReturns > maxY) {
                    maxY = CurrentYear.brackets.ElementAt(i).NumReturns;
                }
            }
            for (int i = 0; i < CurrentYear.brackets.Count; i++) {
                originalTaxVals.Add(CurrentYear.brackets.ElementAt(i).PercentOfTaxableIncomePaid / 100 * maxY * hConst);
            }
            maxRate = 0.20; //Start at 20% to save time
            while (totalRevenueNew - totalRevenueOld < 0)
            {
                sTaxRates.Clear();
                maxRate += 0.00001;
                sTaxVals.Clear();
                revenueByBracketValsNew.Clear();
                changeAmt = maxRate / (CurrentYear.brackets.Count - povertyBrackets - (CurrentYear.brackets.Count - maxBrackets) + 1);
                double currentRate = maxRate;
                totalRevenueNew = 0;
                for (int i = CurrentYear.brackets.Count - 1; i >= 0; i--)
                {
                    if (i > maxBrackets)
                    {
                        sTaxVals.Add(currentRate * maxY * hConst);
                        sTaxRates.Add(currentRate);
                        totalRevenueNew += (CurrentYear.brackets.ElementAt(i).TaxableIncome*1000 * currentRate);
                        revenueByBracketValsNew.Add(CurrentYear.brackets.ElementAt(i).TaxableIncome * 1000 * currentRate / (revConst));
                    }
                    else if (i <= maxBrackets && i > povertyBrackets)
                    {
                        currentRate -= changeAmt;
                        sTaxVals.Add(currentRate * maxY * hConst);
                        sTaxRates.Add(currentRate);
                        totalRevenueNew += (CurrentYear.brackets.ElementAt(i).TaxableIncome*1000 * currentRate);
                        revenueByBracketValsNew.Add(CurrentYear.brackets.ElementAt(i).TaxableIncome * 1000 * currentRate / (revConst));
                    }
                    else
                    {
                        currentRate = 0;
                        sTaxVals.Add(0.0);
                        sTaxRates.Add(currentRate);
                        revenueByBracketValsNew.Add(0.0);
                    }
                }
                sTaxVals.Reverse();
                sTaxRates.Reverse();
                revenueByBracketValsNew.Reverse();
                _taxPlans.ElementAt(0).vals = new ObservableCollection<double>(sTaxVals);
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

        public void GetYears()
        {
            string[] files = Directory.GetFiles("res\\");
            foreach(String file in files)
            {
                using (var reader = new StreamReader(file))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    reader.ReadLine();
                    var brackets = csv.GetRecords<BracketModel>();
                    _years.Add(new IncomeYearModel
                    {
                        year = int.Parse(file.Substring(4, 4)),
                        brackets = new ObservableCollection<BracketModel>(brackets)
                    });
                }
            }
            _years = new ObservableCollection<IncomeYearModel>(_years.Reverse());
        }

        public void GetTaxPlans() {
            _taxPlans = new ObservableCollection<TaxPolicyModel>();
            _taxPlans.Add(new TaxPolicyModel
            {
                name = "Slant Tax",
                vals = new ObservableCollection<double>()
            });
            currentTaxPlan = _taxPlans.ElementAt(0);
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
