using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TaxMeApp.models
{
    public class GraphModel
    {

        public GraphModel()
        {

        }

        public SeriesCollection Series { get; set; } = new SeriesCollection();

        public AxesCollection Axes { get; set; } = new AxesCollection
        {
            // number of returns
            new Axis { 
                Position = AxisPosition.RightTop, 
                ShowLabels = false 
            }, 
            // revenue
            new Axis { 
                Position = AxisPosition.RightTop, 
                ShowLabels = false 
            }, 
            // tax percentage
            new Axis { 
                Position = AxisPosition.RightTop, 
                ShowLabels = false, 
                MinValue = 0, 
                MaxValue = 100, 
                Title = "Tax Rate (%)",
                Separator = { IsEnabled = false }
            }, 
            // UBI
            new Axis { 
                ShowLabels = false, 
                MinValue = 0, 
                Title = "UBI ($)",
                Foreground = Brushes.DarkViolet,
                Separator = { IsEnabled = false }
            } 
        };

        public string[] Labels { get; } =
        {
                "0",
                "Below 5k",
                "5k - 10k",
                "10k - 15k",
                "15k - 20k",
                "20k - 25k",
                "25k - 35k",
                "30k - 40k",
                "40k - 50k",
                "50k - 75k",
                "75k - 100k",
                "100k - 200k",
                "200k - 500k",
                "500k - 1m",
                "1m - 1.5m",
                "1.5m - 2m",
                "2m - 5m",
                "5m - 10m",
                "Over 10m"
        };

        public int PovertyLineIndex { get; set; } = -1;

        public int MaxBracketCount { get; set; }

        //Added for Bracket combo box
        public string SelectedBracket { get; set; }


        // Set the defaults for what are displayed on the graph
        public bool ShowNumberOfReturns { get; set; } = true;
        public bool ShowOldRevenue { get; set; } = false;
        public bool ShowNewRevenue { get; set; } = false;
        public bool ShowOldPercentage { get; set; } = false;
        public bool ShowNewPercentage { get; set; } = false;
        public bool ShowNewUBI { get; set; } = false;
        public bool ShowPreTaxMedian { get; set; } = false;
        public bool ShowPreTaxMean { get; set; } = false;
        public bool ShowPostTaxMedian { get; set; } = false;
        public bool ShowPostTaxMean { get; set; } = false;
        public bool ShowPostTaxMedianUBI { get; set; } = false;
        public bool ShowPostTaxMeanUBI { get; set; } = false;
        public int MaxUBIBracketCount { get; set; }
        public int MinUBIBracketCount { get; set; }
        public int MaxUBI { get; set; }
        public int PreTaxMeanLine { get; set; }
        public int PreTaxMedianLine { get; set; }
        public int PostTaxMeanLine { get; set; }
        public int PostTaxMedianLine { get; set; }
        public int PostTaxMeanUBILine { get; set; }
        public int PostTaxMedianUBILine { get; set; }
    }
}