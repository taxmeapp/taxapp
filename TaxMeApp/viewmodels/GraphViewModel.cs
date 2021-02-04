using LiveCharts;
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
            //Brackets including and under poverty line will be one color, normal brackets will be another, 
            //and max will be another color
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

        public string[] Labels
        {
            get
            {
                return GraphModel.Labels;
            }
            set
            {
                GraphModel.Labels = value;

                OnPropertyChange("Labels");
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
                OnPropertyChange("PovertyLineIndex");
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

        // Population array
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



        /*
        
            Modifying graph contents

        */ 

        // Safe method to clear series
        public void ClearSeries()
        {

            if (Series != null)
            {
                Series.Clear();
            }

        }

        // Safe method to add ColumnSeries to Series
        private void addColumnSeries(ColumnSeries columnSeries)
        {

            if (Series != null)
            {

                Series.Add(columnSeries);

            }

        }

        // Safe method to add LineSeries to Series
        private void addLineSeries(LineSeries lineSeries)
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

        }

        // Private methods to actually draw each

        // Bar graph for Total number of tax returns by bracket
        private void graphTaxReturns()
        {

            // Add the tax return count for the population
            ColumnSeries columnSeries = new ColumnSeries
            {
                Title = selectedYear + " Income",
                Values = new ChartValues<int>(population),
                ScalesYAt = 0
            };

            addColumnSeries(columnSeries);

        }

        // Line chart for Revenue by Bracket under old system
        private void graphOldTaxRevenue()
        {

            LineSeries lineSeries = new LineSeries()
            {
                Title = "Old Tax Revenue By Bracket",
                Values = new ChartValues<long>(oldRevenueByBracket),
                Stroke = Brushes.DarkGreen,
                Fill = Brushes.Transparent,
                ScalesYAt = 1
            };

            addLineSeries(lineSeries);

        }

        // Line chart for Revenue by Bracket under new system
        private void graphNewTaxRevenue()
        {

            LineSeries lineSeries = new LineSeries()
            {
                Title = "New Tax Revenue By Bracket",
                Values = new ChartValues<long>(newRevenueByBracket),
                Stroke = Brushes.LightGreen,
                Fill = Brushes.Transparent,
                ScalesYAt = 1
            };

            addLineSeries(lineSeries);

        }

        // Line chart for % of income paid in tax under old system
        private void graphOldTaxPercentage()
        {

            LineSeries lineSeries = new LineSeries()
            {
                Title = "Old Tax Percentage",
                Values = new ChartValues<double>(oldTaxPctByBracket),
                Stroke = Brushes.DarkGoldenrod,
                Fill = Brushes.Transparent,
                ScalesYAt = 2
            };

            addLineSeries(lineSeries);

        }

        // Line chart for % of income paid in tax under new system
        private void graphNewTaxPercentage()
        {

            LineSeries lineSeries = new LineSeries()
            {
                Title = "New Tax Percentage",
                Values = new ChartValues<double>(newTaxPctByBracket),
                Stroke = Brushes.Maroon,
                Fill = Brushes.Transparent,
                ScalesYAt = 2
            };

            addLineSeries(lineSeries);

        }

        // Updates labels upon graph total reset (e.g. when user changes year)
        public void AddLabels()
        {
            List<string> labels = new List<string>();

            foreach (var bracket in YearsModel.SelectedIncomeYearModel.Brackets)
            {
                labels.Add(bracket.Range);
            }

            Labels = labels.ToArray();
        }
    }
}
