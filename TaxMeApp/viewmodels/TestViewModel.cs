﻿using CsvHelper;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using TaxMeApp.Helpers;
using TaxMeApp.models;
using TaxMeApp.viewmodels;

namespace TaxMeApp
{
    public class TestViewModel : MainViewModel
    {

        public TestViewModel()
        {
           
        }

        // TestViewModel init
        public void Init()
        {

            // Initialize graph attributes
            graphInit();

            // Automatically select the first item in the list
            SelectedYear = yearsModel.YearList[0];

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
                return graphModel.Series;
            }
        }

        private ObservableCollection<BracketModel> selectedBrackets
        {
            get
            {
                return yearsModel.SelectedIncomeYearModel.Brackets;
            }
        }

        // Population array
        private ObservableCollection<int> population
        {
            get
            {
                return dataModel.Population;
            }
        }

        private int povertyBrackets
        {
            get
            {
                return graphModel.PovertyLineIndex;
            }
        }

        private int totalBrackets
        {
            get
            {
                return yearsModel.SelectedIncomeYearModel.Brackets.Count;
            }
        }

        private List<long> oldRevenueByBracket
        {
            get
            {
                return dataModel.oldRevenueByBracket;
            }
        }

        private List<long> newRevenueByBracket
        {
            get
            {
                return dataModel.newRevenueByBracket;
            }
        }

        private List<double> oldTaxPctByBracket
        {
            get
            {
                return dataModel.oldTaxPctByBracket;
            }
        }

        private List<double> newTaxPctByBracket
        {
            get
            {
                return dataModel.newTaxPctByBracket;
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
                return yearsModel.YearList;
            }
        }

        // When user changes the dropdown selection:
        public int SelectedYear
        {
            get { return yearsModel.SelectedYear; }
            set
            {
                //Trace.WriteLine("Changing selected year to: " + value);

                // Save it in our data model
                yearsModel.SelectedYear = value;

                totalGraphReset();

            }
        }

        // Slant tax maximum rate, adjustable via slider
        public int MaxTaxRate
        {
            get
            {
                return dataModel.MaxTaxRate;
            }
            set
            {
                dataModel.MaxTaxRate = value;

                newDataGraphReset();

                OnPropertyChange("MaxTaxRate");
            }
        }

        // Number of brackets at max rate
        public int MaxBracketCount
        {
            get
            {
                return graphModel.MaxBracketCount;
            }
            set
            {
                graphModel.MaxBracketCount = value;

                OnPropertyChange("MaxBracketCount");
            }
        }

        // Strictly for the slider to interface with - calls for a graph redraw on set
        public int MaxBracketCountSlider
        {
            get
            {
                return graphModel.MaxBracketCount;
            }
            set
            {
                graphModel.MaxBracketCount = value;

                newDataGraphReset();

                OnPropertyChange("MaxBracketCount");

            }
        }


        // Checkboxes
        public bool ShowNumberOfReturns
        {
            get
            {
                return graphModel.showNumberOfReturns;
            }
            set
            {
                graphModel.showNumberOfReturns = value;

                displayOnlyGraphReset();

            }
        }
               
        public bool ShowOldRevenue
        {
            get
            {
                return graphModel.showOldRevenue;
            }
            set
            {
                graphModel.showOldRevenue = value;

                displayOnlyGraphReset();

            }
        }

        public bool ShowNewRevenue
        {
            get
            {
                return graphModel.showNewRevenue;
            }
            set
            {
                graphModel.showNewRevenue = value;

                displayOnlyGraphReset();

            }
        }

        public bool ShowOldPercentage
        {
            get
            {
                return graphModel.showOldPercentage;
            }
            set
            {

                graphModel.showOldPercentage = value;

                displayOnlyGraphReset();

            }
        }

        public bool ShowNewPercentage
        {
            get
            {
                return graphModel.showNewPercentage;
            }
            set
            {

                graphModel.showNewPercentage = value;

                displayOnlyGraphReset();

            }
        }


        /*
                Output Area 
        */

        // Each of the values represented in the output area
        // The private member interacts with the model and this viewmodel
        // The public member formats it for the view to pull

        private int NumPovertyPop
        {
            get
            {
                return dataModel.NumPovertyPop;
            }
            set
            {
                dataModel.NumPovertyPop = value;
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
                return dataModel.NumMaxPop;
            }
            set
            {
                dataModel.NumMaxPop = value;
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
                return dataModel.TotalRevenueOld;
            }
            set
            {
                dataModel.TotalRevenueOld = value;
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
                return dataModel.TotalRevenueNew;
            }
            set
            {
                dataModel.TotalRevenueNew = value;
                OnPropertyChange("TotalRevenueNewOutput");
                OnPropertyChange("RevenueDifferenceOutput");
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
                return Formatter.Format(dataModel.RevenueDifference);
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

        // Collections of calls to update 
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
        private void calculatePopulation()
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
        private void countUnderPoverty()
        {
            int povertyPop = 0;

            // Count the number of returns up to and including the designated poverty bracket
            for (int i = 0; i <= povertyBrackets; i++)
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


        /*
            Graph functions 
        */

        // Graph initialization
        private void graphInit()
        {
            //Brackets including and under poverty line will be one color, normal brackets will be another, 
            //and max will be another color
            Brush povertyColor = Brushes.Red;
            Brush normalColor = Brushes.Blue;
            Brush maxColor = Brushes.Lime;
            CartesianMapper<int> povertyMapper = new CartesianMapper<int>()
                .X((value, index) => index)
                .Y((value) => value)
                .Fill((value, index) => {
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
                    Values = new ChartValues<int>(population)
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
                    Fill = Brushes.Transparent
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
                    Fill = Brushes.Transparent
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
                    Fill = Brushes.Transparent
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
                    Fill = Brushes.Transparent
                }
            );;

        }


    }



}
