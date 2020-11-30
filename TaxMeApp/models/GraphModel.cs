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
        public SeriesCollection Series { get; } = new SeriesCollection();

       
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

        public int PovertyLineIndex { get; } = 3;

        public int MaxBracketCount { get; set; }


        // Set the defaults for what are displayed on the graph
        public bool ShowNumberOfReturns { get; set; } = true;
        public bool ShowOldRevenue { get; set; } = false;
        public bool ShowNewRevenue { get; set; } = false;
        public bool ShowOldPercentage { get; set; } = false;
        public bool ShowNewPercentage { get; set; } = false;

    }
}
