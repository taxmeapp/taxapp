﻿using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace TaxMeApp.viewmodels
{
    public class GraphViewModel : MainViewModel
    {


        // Graph initialization
        public void GraphInit()
        {
            // Brackets including and under poverty line will be red
            // Normal brackets will be blue
            // Max brackets will be lime green
            Brush povertyColor = Brushes.Red;
            Brush normalColor = Brushes.Blue;
            Brush maxColor = Brushes.Lime;
            CartesianMapper<int> povertyMapper;
            if (GraphModel != null)
            {
                povertyMapper = new CartesianMapper<int>()
                    .X((value, index) => index)
                    .Y((value) => value)
                    .Fill((value, index) =>
                    {
                        if (index <= PovertyLineIndex)
                        {
                            return povertyColor;
                        }
                        else if (index > PovertyLineIndex && index < totalBrackets - maxBracketCount)
                        {
                            return normalColor;
                        }
                        else
                        {
                            return maxColor;
                        }

                    });
            }
            else
            {
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

        /*
         
            Variables that are dynamically bound:
            May show "0 references"
         
        */

        public SeriesCollection Series
        {
            get
            {
                return GraphModel.Series;
            }
        }

        public AxesCollection Axes
        {
            get
            {
                return GraphModel.Axes;
            }
        }

        public string[] Labels
        {
            get
            {
                return GraphModel.Labels;
            }
        }

        public int PovertyLineIndex
        {
            get
            {
                return GraphModel.PovertyLineIndex;
            }
            set
            {
                GraphModel.PovertyLineIndex = value;
                OnPropertyChange("PovertyLineIndex");
            }
        }

        // For all statistic lines, if corresponding checkbox is not checked, line's position is -1 (moves line off graph)
        public int PreTaxMeanLine
        {
            get
            {
                if (!showPreTaxMean)
                {
                    return -1;
                }
                return GraphModel.PreTaxMeanLine;
            }
            set
            {
                GraphModel.PreTaxMeanLine = value;
                OnPropertyChange("PreTaxMeanLine");
            }
        }

        public int PreTaxMedianLine
        {
            get
            {
                if (!showPreTaxMedian)
                {
                    return -1;
                }
                return GraphModel.PreTaxMedianLine;
            }
            set
            {
                GraphModel.PreTaxMedianLine = value;
                OnPropertyChange("PreTaxMedianLine");
            }
        }

        public int PostTaxMeanLine
        {
            get
            {
                if (!showPostTaxMean)
                {
                    return -1;
                }
                return GraphModel.PostTaxMeanLine;
            }
            set
            {
                GraphModel.PostTaxMeanLine = value;
                OnPropertyChange("PostTaxMeanLine");
            }
        }

        public int PostTaxMedianLine
        {
            get
            {
                if (!showPostTaxMedian)
                {
                    return -1;
                }
                return GraphModel.PostTaxMedianLine;
            }
            set
            {
                GraphModel.PostTaxMedianLine = value;
                OnPropertyChange("PostTaxMedianLine");
            }
        }

        public int PostTaxMeanUBILine
        {
            get
            {
                if (!showPostTaxMeanUBI)
                {
                    return -1;
                }
                return GraphModel.PostTaxMeanUBILine;
            }
            set
            {
                GraphModel.PostTaxMeanUBILine = value;
                OnPropertyChange("PostTaxMeanUBILine");
            }
        }

        public int PostTaxMedianUBILine
        {
            get
            {
                if (!showPostTaxMedianUBI)
                {
                    return -1;
                }
                return GraphModel.PostTaxMedianUBILine;
            }
            set
            {
                GraphModel.PostTaxMedianUBILine = value;
                OnPropertyChange("PostTaxMedianUBILine");
            }
        }

        /*
             Model interaction (no direct binding)
         */

        private int totalBrackets
        {
            get
            {
                return YearsModel.SelectedIncomeYearModel.Brackets.Count;
            }
        }

        // Checkboxes
        private bool showNumberOfReturns
        {
            get
            {
                return GraphModel.ShowNumberOfReturns;
            }
        }

        private bool showOldRevenue
        {
            get
            {
                return GraphModel.ShowOldRevenue;
            }
        }

        private bool showNewRevenue
        {
            get
            {
                return GraphModel.ShowNewRevenue;
            }
        }

        private bool showOldPercentage
        {
            get
            {
                return GraphModel.ShowOldPercentage;
            }
        }

        private bool showNewPercentage
        {
            get
            {
                return GraphModel.ShowNewPercentage;
            }
        }

        private bool showUBI
        {
            get
            {
                return GraphModel.ShowNewUBI;
            }
        }

        public bool showPreTaxMedian
        {
            get
            {
                return GraphModel.ShowPreTaxMedian;
            }
            set
            {
                GraphModel.ShowPreTaxMedian = value;
                OnPropertyChange("PreTaxMedianLine");
            }
        }

        public bool showPreTaxMean
        {
            get
            {
                return GraphModel.ShowPreTaxMean;
            }
            set
            {
                GraphModel.ShowPreTaxMean = value;
                OnPropertyChange("PreTaxMeanLine");
            }
        }

        public bool showPostTaxMedian
        {
            get
            {
                return GraphModel.ShowPostTaxMedian;
            }
            set
            {
                GraphModel.ShowPostTaxMedian = value;
                OnPropertyChange("PostTaxMedianLine");
            }
        }

        public bool showPostTaxMean
        {
            get
            {
                return GraphModel.ShowPostTaxMean;
            }
            set
            {
                GraphModel.ShowPostTaxMean = value;
                OnPropertyChange("PostTaxMeanLine");
            }
        }

        public bool showPostTaxMedianUBI
        {
            get
            {
                return GraphModel.ShowPostTaxMedianUBI;
            }
            set
            {
                GraphModel.ShowPostTaxMedianUBI = value;
                OnPropertyChange("PostTaxMedianUBILine");
            }
        }

        public bool showPostTaxMeanUBI
        {
            get
            {
                return GraphModel.ShowPostTaxMeanUBI;
            }
            set
            {
                GraphModel.ShowPostTaxMeanUBI = value;
                OnPropertyChange("PostTaxMeanUBILine");
            }
        }

        // Number of brackets at max rate
        private int maxBracketCount
        {
            get
            {
                return GraphModel.MaxBracketCount;
            }
        }

        // When user changes the dropdown selection:
        private int selectedYear
        {
            get
            {
                return YearsModel.SelectedYear;
            }
        }

        // Population (number of returns) in each bracket
        private ObservableCollection<int> population
        {
            get
            {
                return DataModel.Population;
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

        private List<double> ubiByBracket
        {
            get
            {
                return DataModel.UBIPayOutByBracket;
            }
        }

        /*
        
            Modifying graph contents
        */

        // Safe method to clear series
        public void ClearSeries()
        {

            if (Series != null)
            {

                try
                {
                    Series.Clear();

                    // hide axes
                    foreach(Axis axis in Axes)
                    {
                        axis.ShowLabels = false;
                        axis.Title = null;
                    }

                }
                // Allow system to keep executing past raised exception
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }

        }

        // Safe method to add ColumnSeries to Series
        public void AddColumnSeries(ColumnSeries columnSeries)
        {

            if (Series != null)
            {

                Series.Add(columnSeries);

            }

        }

        // Safe method to add LineSeries to Series
        public void AddLineSeries(LineSeries lineSeries)
        {

            if (Series != null)
            {

                Series.Add(lineSeries);

            }

        }

        /*
        
            Drawing on graph
        */

        // Public method to draw all relevant items
        public void GraphAllChecked()
        {

            if (showNumberOfReturns)
            {
                graphTaxReturns();
            }

            if (showOldRevenue)
            {
                graphOldTaxRevenue();
            }

            if (showNewRevenue)
            {
                graphNewTaxRevenue();
            }

            if (showOldPercentage)
            {
                graphOldTaxPercentage();
            }

            if (showNewPercentage)
            {
                graphNewTaxPercentage();
            }

            if (showUBI)
            {
                graphUBI();
            }

        }

        // Public method to draw all relevant items
        public void GraphAllChecked(List<double> customRates)
        {

            if (showNumberOfReturns)
            {
                graphTaxReturns();
            }

            if (showOldRevenue)
            {
                graphOldTaxRevenue();
            }

            if (showNewRevenue)
            {
                graphNewTaxRevenue();
            }

            if (showOldPercentage)
            {
                graphOldTaxPercentage();
            }
            if (showUBI)
            {
                graphUBI();
            }       

            if (showNewPercentage)
            {
                graphCustomRates(customRates);
            }

        }
        // Private methods to actually draw each

        // Bar graph for total number of tax returns by bracket
        private void graphTaxReturns()
        {

            // Add the tax return count for the population
            ColumnSeries columnSeries = new ColumnSeries
            {
                Title = selectedYear + " Income",
                Values = new ChartValues<int>(population),
                ScalesYAt = 0
            };

            AddColumnSeries(columnSeries);

        }

        // Line chart for revenue by bracket under old system
        private void graphOldTaxRevenue()
        {

            LineSeries lineSeries = new LineSeries()
            {
                Title = "Old Tax Revenue By Bracket",
                Values = new ChartValues<long>(oldRevenueByBracket),
                Stroke = Brushes.DarkGreen,
                Fill = Brushes.Transparent,
                PointGeometrySize = 6,
                PointForeground = Brushes.DarkGreen,
                ScalesYAt = 1
            };

            AddLineSeries(lineSeries);

        }

        // Line chart for revenue by bracket under new system
        private void graphNewTaxRevenue()
        {

            LineSeries lineSeries = new LineSeries()
            {
                Title = "New Tax Revenue By Bracket",
                Values = new ChartValues<long>(newRevenueByBracket),
                Stroke = Brushes.LightGreen,
                Fill = Brushes.Transparent,
                PointGeometrySize = 6,
                PointForeground = Brushes.LightGreen,
                ScalesYAt = 1
            };

            AddLineSeries(lineSeries);

        }

        // Line chart for % of income paid in tax (tax rate) for each bracket under old system
        private void graphOldTaxPercentage()
        {

            LineSeries lineSeries = new LineSeries()
            {
                Title = "Old Tax Percentage",
                Values = new ChartValues<double>(oldTaxPctByBracket),
                Stroke = Brushes.DarkGoldenrod,
                Fill = Brushes.Transparent,
                PointGeometrySize = 6,
                PointForeground = Brushes.DarkGoldenrod,
                ScalesYAt = 2
            };

            AddLineSeries(lineSeries);

            Axes[2].ShowLabels = true;
            Axes[2].Title = "Tax Rate (%)";
        }

        // Line chart for % of income paid in tax (tax rate) for each bracket under new system
        private void graphNewTaxPercentage()
        {

            LineSeries lineSeries = new LineSeries()
            {
                Title = "New Tax Percentage",
                Values = new ChartValues<double>(newTaxPctByBracket),
                Stroke = Brushes.Maroon,
                Fill = Brushes.Transparent,
                PointGeometrySize = 6,
                PointForeground = Brushes.Maroon,
                LineSmoothness = 0,
                ScalesYAt = 2
            };

            AddLineSeries(lineSeries);

            Axes[2].ShowLabels = true;
            Axes[2].Title = "Tax Rate (%)";
        }

        // Line chart for UBI payout by bracket
        private void graphUBI()
        {

            LineSeries lineSeries = new LineSeries()
            {
                Title = "UBI",
                Values = new ChartValues<double>(ubiByBracket),
                Stroke = Brushes.DarkViolet,
                Fill = Brushes.Transparent,
                PointGeometrySize = 6,
                PointForeground = Brushes.DarkViolet,
                LineSmoothness = 0,
                ScalesYAt = 3
            };

            AddLineSeries(lineSeries);

            Axes[3].ShowLabels = true;
            Axes[3].Title = "UBI ($)";
        }

        private void graphCustomRates(List<double> customVals)
        {

            LineSeries lineSeries = new LineSeries()
            {
                Title = "New Tax Percentage",
                Values = new ChartValues<double>(customVals),
                Stroke = Brushes.Maroon,
                Fill = Brushes.Transparent,
                PointGeometrySize = 6,
                PointForeground = Brushes.Maroon,
                ScalesYAt = 2
            };

            AddLineSeries(lineSeries);

            Axes[2].ShowLabels = true;
            Axes[2].Title = "Tax Rate (%)";
        }
    }
}