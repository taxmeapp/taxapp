using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using TaxMeApp.Helpers;
using TaxMeApp.models;

namespace TaxMeApp.viewmodels
{
    public class ControlViewModel : MainViewModel
    {
        public ControlViewModel()
        {
            this.YearsModel = new YearsModel();
            this.GraphModel = new GraphModel();
            this.DataModel = new DataModel();
        }
        public ControlViewModel(YearsModel yearsModel, GraphModel graphModel, DataModel dataModel) {
            this.YearsModel = yearsModel;
            this.GraphModel = graphModel;
            this.DataModel = dataModel;
        }

        // TestViewModel init
        public void Init()
        {

            // Initialize graph attributes
            graphInit();

            // Automatically select the first item in the list
            SelectedYear = YearsModel.YearList[0];

        }

        // View Model references
        public GraphViewModel GraphVM { get; set; }


        /*

            Interactions with models

        */

        private SeriesCollection series
        {
            get
            {
                return GraphModel.Series;
            }
        }

        private ObservableCollection<BracketModel> selectedBrackets
        {
            get
            {
                return YearsModel.SelectedIncomeYearModel.Brackets;
            }
        }

        // Population array
        public ObservableCollection<int> population
        {
            get
            {
                return DataModel.Population;
            }
        }

        private int povertyBrackets
        {
            get
            {
                return GraphModel.PovertyLineIndex;
            }
        }

        private int totalBrackets
        {
            get
            {
                return YearsModel.SelectedIncomeYearModel.Brackets.Count;
            }
        }

        private List<long> oldRevenueByBracket
        {
            get
            {
                return DataModel.OldRevenueByBracket;
            }
        }

        private List<long> newRevenueByBracket
        {
            get
            {
                return DataModel.NewRevenueByBracket;
            }
        }

        private List<double> oldTaxPctByBracket
        {
            get
            {
                return DataModel.OldTaxPctByBracket;
            }
        }

        private List<double> newTaxPctByBracket
        {
            get
            {
                return DataModel.NewTaxPctByBracket;
            }
        }

        /*
                Control Panel interaction: 
        */

        // Get the year list for the dropdown
        public List<int> YearList
        {
            get
            {
                return YearsModel.YearList;
            }
        }

        // When user changes the dropdown selection:
        public int SelectedYear
        {
            get { return YearsModel.SelectedYear; }
            set
            {
                //Trace.WriteLine("Changing selected year to: " + value);

                // Save it in our data model
                YearsModel.SelectedYear = value;

                totalGraphReset();

            }
        }

        // Slant tax maximum rate, adjustable via slider
        public int MaxTaxRate
        {
            get
            {
                return DataModel.MaxTaxRate;
            }
            set
            {
                DataModel.MaxTaxRate = value;

                newDataGraphReset();

                OnPropertyChange("MaxTaxRate");
            }
        }

        // Number of brackets at max rate
        public int MaxBracketCount
        {
            get
            {
                return GraphModel.MaxBracketCount;
            }
            set
            {
                GraphModel.MaxBracketCount = value;

                OnPropertyChange("MaxBracketCount");
            }
        }

        // Strictly for the slider to interface with - calls for a graph redraw on set
        public int MaxBracketCountSlider
        {
            get
            {
                return GraphModel.MaxBracketCount;
            }
            set
            {
                GraphModel.MaxBracketCount = value;

                newDataGraphReset();

                OnPropertyChange("MaxBracketCount");

            }
        }


        // Checkboxes
        public bool ShowNumberOfReturns
        {
            get
            {
                return GraphModel.ShowNumberOfReturns;
            }
            set
            {
                GraphModel.ShowNumberOfReturns = value;

                displayOnlyGraphReset();

            }
        }
               
        public bool ShowOldRevenue
        {
            get
            {
                return GraphModel.ShowOldRevenue;
            }
            set
            {
                GraphModel.ShowOldRevenue = value;

                displayOnlyGraphReset();

            }
        }

        public bool ShowNewRevenue
        {
            get
            {
                return GraphModel.ShowNewRevenue;
            }
            set
            {
                GraphModel.ShowNewRevenue = value;

                displayOnlyGraphReset();

            }
        }

        public bool ShowOldPercentage
        {
            get
            {
                return GraphModel.ShowOldPercentage;
            }
            set
            {

                GraphModel.ShowOldPercentage = value;

                displayOnlyGraphReset();

            }
        }

        public bool ShowNewPercentage
        {
            get
            {
                return GraphModel.ShowNewPercentage;
            }
            set
            {

                GraphModel.ShowNewPercentage = value;

                displayOnlyGraphReset();

            }
        }


        /*
                Output Area 
        */

        // Each of the values represented in the output area
        // The private member interacts with the model and this viewmodel
        // The public member formats it for the view to pull

        public int NumPovertyPop
        {
            get
            {
                return DataModel.NumPovertyPop;
            }
            set
            {
                DataModel.NumPovertyPop = value;
                OnPropertyChange("NumPovertyPopOutput");
            }
        }

        public string NumPovertyPopOutput
        {
            get
            {
                return Formatter.Format(NumPovertyPop);
            }
        }

        private int NumMaxPop
        {
            get
            {
                return DataModel.NumMaxPop;
            }
            set
            {
                DataModel.NumMaxPop = value;
                OnPropertyChange("NumMaxPopOutput");
            }
        }

        public string NumMaxPopOutput
        {
            get
            {
                return Formatter.Format(NumMaxPop);
            }
        }

        private long TotalRevenueOld
        {
            get
            {
                return DataModel.TotalRevenueOld;
            }
            set
            {
                DataModel.TotalRevenueOld = value;
                OnPropertyChange("TotalRevenueOldOutput");
                OnPropertyChange("RevenueDifferenceOutput");
            }
        }

        public string TotalRevenueOldOutput
        {
            get
            {
                return Formatter.Format(TotalRevenueOld);
            }
        }

        private long TotalRevenueNew
        {
            get
            {
                return DataModel.TotalRevenueNew;
            }
            set
            {
                DataModel.TotalRevenueNew = value;
                OnPropertyChange("TotalRevenueNewOutput");
                OnPropertyChange("RevenueDifferenceOutput");
            }
        }

        public long totalRevenueNewTesting = 0;
        public long TotalRevenueNewTesting
        {
            get
            {
                return totalRevenueNewTesting;
            }
            set
            {
                totalRevenueNewTesting = value;

            }
        
        }

        public string TotalRevenueNewOutput
        {
            get
            {
                return Formatter.Format(TotalRevenueNew);
            }
        }

        public string RevenueDifferenceOutput
        {
            get
            {
                return Formatter.Format(DataModel.RevenueDifference);
            }
        }


        /*
            Collective functions 
        */

        // Collection of calls to update data, clear graph, regraph
        private void totalGraphReset()
        {

            // Update our population counts
            calculatePopulation();

            // Count how many people are under poverty line
            countUnderPoverty();

            // Determine our baseline number of brackets at max rate
            determineBaselineMaxBracketCount();

            // Count how many people are at max tax rate
            countPopulationAtMaxRate();

            // Calculate how much tax revenue was generated under current plan
            calculateOldTaxData();

            // Calculate how much tax revenue was generated under new plan
            calculateNewTaxData();

            // Clear the graph
            series.Clear();

            // Graph everything that is checked
            graphAllChecked();

        }

        // Collections of calls to update only the new data (e.g. max % or # of max brackets changed)
        private void newDataGraphReset()
        {

            // Count how many people are at max tax rate
            countPopulationAtMaxRate();

            // Calculate how much tax revenue was generated under new plan
            calculateNewTaxData();

            // Clear the graph
            series.Clear();

            // Graph everything that is checked
            graphAllChecked();

        }

        // Colections of calls to update only what is shown on the graph (e.g. checkboxes ticked/unticked)
        private void displayOnlyGraphReset()
        {

            // Clear the graph
            series.Clear();

            // Graph everything that is checked
            graphAllChecked();

        }

      
        /*
            Calculation functions 
        */

        // Recalculate population collection
        public void calculatePopulation()
        {

            // Clear our current count
            population.Clear();

            // Iterating through all brackets:
            for (int i = 0; i < selectedBrackets.Count; i++)
            {

                population.Add(selectedBrackets[i].NumReturns);

            }

        }

        // Count the population that is included in the poverty brackets
        public void countUnderPoverty()
        {
            int povertyPop = 0;

            // Count the number of returns up to and including the designated poverty bracket
            //for (int i = 0; i <= povertyBrackets; i++)
            for (int i = 0; i <= 3; i++)
            {
                povertyPop += population[i];
            }

            NumPovertyPop = povertyPop;

            //Trace.WriteLine("There are " + NumPovertyPop + " returns under poverty line.");

        }

        private void determineBaselineMaxBracketCount()
        {

            // Initialize population and bracket counters
            int maxPopCount = 0;
            int maxBracketCount = 0;

            int brackets = population.Count - 1;

            // Starting at the end of array, work backwards
            // as long as our max population count is below our poverty population count
            for (int i = brackets; maxPopCount < NumPovertyPop && i >= 0; i--)
            {

                // Add to our counters
                maxPopCount += population[i];
                maxBracketCount++;

            }

            //Trace.WriteLine("There are " + maxBracketCount + " brackets at max rate.");

            // Save our value 
            MaxBracketCount = maxBracketCount;

        }

        // Count the population that is at max tax rate
        private void countPopulationAtMaxRate()
        {

            // Initialize population and bracket counters
            int maxPopCount = 0;

            int brackets = population.Count - 1;

            // Starting at the end of array, work backwards
            // as long as our max population count is below our poverty population count
            for (int i = brackets; i > brackets - MaxBracketCount; i--)
            {

                // Add to our counters
                maxPopCount += population[i];

            }

            //Trace.WriteLine("There are " + maxPopCount + " returns at max rate.");

            // Save our value
            NumMaxPop = maxPopCount;

        }

        public int countPopulationWithMaxBrackets(int maxBrackets)
        {

            // Initialize population and bracket counters
            int maxPopCount = 0;

            int brackets = population.Count - 1;

            // Starting at the end of array, work backwards
            // as long as our max population count is below our poverty population count
            for (int i = brackets; i > brackets - maxBrackets; i--)
            {

                // Add to our counters
                maxPopCount += population[i];

            }

            //Trace.WriteLine("There are " + maxPopCount + " returns at max rate.");

            return maxPopCount;

        }

        // Accumulate the data we need from original tax data
        private void calculateOldTaxData()
        {

            // Clear our old bracket data
            oldRevenueByBracket.Clear();
            oldTaxPctByBracket.Clear();

            long totalRevenueOld = 0;

            for (int i = 0; i < selectedBrackets.Count; i++)
            {

                // Get the actual value of income tax paid (values stored as 1000)
                long bracketIncomeTax = selectedBrackets[i].IncomeTax * 1000;

                // Add it to our count
                totalRevenueOld += bracketIncomeTax;

                // Add to our revenue bracket list
                oldRevenueByBracket.Add(bracketIncomeTax);

                // Get the percent of income gross and add to our Pct bracket list
                oldTaxPctByBracket.Add(selectedBrackets[i].PercentOfGrossIncomePaid);

            }

            // Save our value for display in output region
            TotalRevenueOld = totalRevenueOld;

        }

        // Accumulate the data we need using new tax calculations
        // Specifically hardcoded for slant tax currently
        private void calculateNewTaxData()
        {

            // Clear our bracket lists so we can add to fresh lists
            newRevenueByBracket.Clear();
            newTaxPctByBracket.Clear();

            // Initialize our variables
            int middleCount = selectedBrackets.Count - MaxBracketCount;
            long totalRevenueNew = 0;
            double rate = 0;

            // Initialize our index counter that is shared for all three loops
            int i = 0;

            // Handle poverty brackets
            for (; i <= povertyBrackets; i++)
            {

                // Revenue is 0
                newRevenueByBracket.Add(0);
                // Rate is 0
                newTaxPctByBracket.Add(0);

            }

            // Determine how many divisons we want to spread our increment over
            int divisions = middleCount - i + 1;

            // Determine the rate at how much to increment, and round to 1 decimal place
            double increment = Math.Round(MaxTaxRate / (double)divisions, 1);

            // Incremental brackets
            for (; i < middleCount; i++)
            {

                rate = rate + increment;

                // Revenue is Taxable Income * Tax Rate
                long bracketRevenue = (long)(selectedBrackets[i].TaxableIncome * 1000 * rate / 100);
                totalRevenueNew += bracketRevenue;

                newRevenueByBracket.Add(bracketRevenue);

                // Rate is incremental
                newTaxPctByBracket.Add(rate);

            }

            // Max rate:
            for (; i < selectedBrackets.Count; i++)
            {

                // Revenue is Taxable Income * Max Rate
                long bracketRevenue = selectedBrackets[i].TaxableIncome * 1000 * MaxTaxRate / 100;
                totalRevenueNew += bracketRevenue;

                newRevenueByBracket.Add(bracketRevenue);

                // Rate is max
                newTaxPctByBracket.Add(MaxTaxRate);

            }

            // Save our total revenue calculation
            TotalRevenueNew = totalRevenueNew;

        }

        public long calculateNewTaxDataFromBracks(int povBracks, int maxBracks)
        {

            // Clear our bracket lists so we can add to fresh lists
            List<long> nRevByBrack = new List<long>();
            List<double> nTaxPctByBrack = new List<double>();

            // Initialize our variables
            int middleCount = selectedBrackets.Count - maxBracks;
            long totalRevenueNew = 0;
            double rate = 0;

            // Initialize our index counter that is shared for all three loops
            int i = 0;

            // Handle poverty brackets
            for (; i <= povBracks; i++)
            {

                // Revenue is 0
                nRevByBrack.Add(0);
                // Rate is 0
                nTaxPctByBrack.Add(0);

            }

            // Determine how many divisons we want to spread our increment over
            int divisions = middleCount - i + 1;

            // Determine the rate at how much to increment, and round to 1 decimal place
            double increment = Math.Round(25 / (double)divisions, 1);

            // Incremental brackets
            for (; i < middleCount; i++)
            {

                rate = rate + increment;

                // Revenue is Taxable Income * Tax Rate
                long bracketRevenue = (long)(selectedBrackets[i].TaxableIncome * 1000 * rate / 100);
                totalRevenueNew += bracketRevenue;

                nRevByBrack.Add(bracketRevenue);

                // Rate is incremental
                nTaxPctByBrack.Add(rate);

            }

            // Max rate:
            for (; i < selectedBrackets.Count; i++)
            {

                // Revenue is Taxable Income * Max Rate
                long bracketRevenue = selectedBrackets[i].TaxableIncome * 1000 * MaxTaxRate / 100;
                totalRevenueNew += bracketRevenue;

                nRevByBrack.Add(bracketRevenue);

                // Rate is max
                nTaxPctByBrack.Add(MaxTaxRate);

            }

            // Save our total revenue calculation
            //TotalRevenueNewTesting = totalRevenueNew;
            return totalRevenueNew;
        }

        /*
            Graph functions 
        */

        // Graph initialization
        public void graphInit()
        {
            //Brackets including and under poverty line will be one color, normal brackets will be another, 
            //and max will be another color
            Brush povertyColor = Brushes.Red;
            Brush normalColor = Brushes.Blue;
            Brush maxColor = Brushes.Lime;
            CartesianMapper<int> povertyMapper;
            if (this.GraphModel != null)
            {
                povertyMapper = new CartesianMapper<int>()
                    .X((value, index) => index)
                    .Y((value) => value)
                    .Fill((value, index) =>
                    {
                        if (index <= povertyBrackets)
                        {
                            return povertyColor;
                        }
                        else if (index > povertyBrackets && index < totalBrackets - MaxBracketCount)
                        {
                            return normalColor;
                        }
                        else
                        {
                            return maxColor;
                        }

                    });
            }
            else {
                povertyMapper = new CartesianMapper<int>()
                .X((value, index) => index)
                .Y((value) => value)
                .Fill((value, index) =>
                {
                    if (index <= 3)
                    {
                        return povertyColor;
                    }
                    else if (index > 3 && index < totalBrackets - 7)
                    {
                        return normalColor;
                    }
                    else
                    {
                        return maxColor;
                    }

                });
            }
            Charting.For<int>(povertyMapper, SeriesOrientation.Horizontal);

        }

        // Graph things based on whether or not they are checked in the control panel
        private void graphAllChecked()
        {

            if (ShowNumberOfReturns)
            {
                graphTaxReturns();
            }

            if (ShowOldRevenue)
            {
                graphOldTaxRevenue();
            }

            if (ShowNewRevenue)
            {
                graphNewTaxRevenue();
            }

            if (ShowOldPercentage)
            {
                graphOldTaxPercentage();
            }

            if (ShowNewPercentage)
            {
                graphNewTaxPercentage();
            }

        }

        // Bar graph for Total number of tax returns by bracket
        private void graphTaxReturns()
        {

            // Add the tax return count for the population
            series.Add(
                new ColumnSeries
                {
                    Title = SelectedYear + " Income",
                    Values = new ChartValues<int>(population),
                    ScalesYAt = 0
                }
            );          

        }

        // Line chart for Revenue by Bracket under old system
        private void graphOldTaxRevenue()
        {

            series.Add(
                new LineSeries()
                {
                    Title = "Old Tax Revenue By Bracket",
                    Values = new ChartValues<long>(oldRevenueByBracket),
                    Stroke = Brushes.DarkGreen,
                    Fill = Brushes.Transparent,
                    ScalesYAt = 1
                }
            );

        }

        // Line chart for Revenue by Bracket under new system
        private void graphNewTaxRevenue()
        {

            series.Add(
                new LineSeries()
                {
                    Title = "New Tax Revenue By Bracket",
                    Values = new ChartValues<long>(newRevenueByBracket),
                    Stroke = Brushes.LightGreen,
                    Fill = Brushes.Transparent,
                    ScalesYAt = 1
                }
            );

        }

        // Line chart for % of income paid in tax under old system
        private void graphOldTaxPercentage()
        {

            series.Add(
                new LineSeries()
                {
                    Title = "Old Tax Percentage",
                    Values = new ChartValues<double>(oldTaxPctByBracket),
                    Stroke = Brushes.DarkGoldenrod,
                    Fill = Brushes.Transparent,
                    ScalesYAt = 2
                }
            );

        }

        // Line chart for % of income paid in tax under new system
        private void graphNewTaxPercentage()
        {

            series.Add(
                new LineSeries()
                {
                    Title = "New Tax Percentage",
                    Values = new ChartValues<double>(newTaxPctByBracket),
                    Stroke = Brushes.Maroon,
                    Fill = Brushes.Transparent,
                    ScalesYAt = 2
                }
            );;

        }


    }



}
