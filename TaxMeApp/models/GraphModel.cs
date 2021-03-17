using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxMeApp.models
{
    public class GraphModel
    {

        public GraphModel()
        {

        }

        public SeriesCollection Series { get; set; } = new SeriesCollection();


        public AxisSection MaxBracketLine { get; set; } = new AxisSection();

        public string[] Labels { get; } =
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
                "$100,000 under $200,000",
                "$200,000 under $500,000",
                "$500,000 under $1,000,000",
                "$1,000,000 under $1,500,000",
                "$1,500,000 under $2,000,000",
                "$2,000,000 under $5,000,000",
                "$5,000,000 under $10,000,000",
                "$10,000,000 or more"
        };

        public int PovertyLineIndex { get; set; } = 3;

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