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
        public SeriesCollection Series { get; set; } = new SeriesCollection();

        public AxisSection MaxBracketLine { get; set; } = new AxisSection();

        public string[] Labels { get; set; } = new string[] { };

        public int PovertyLineIndex { get; set; } = 3;

        public int MaxBracketCount { get; set; }


        // Set the defaults for what are displayed on the graph
        public bool ShowNumberOfReturns { get; set; } = true;
        public bool ShowOldRevenue { get; set; } = false;
        public bool ShowNewRevenue { get; set; } = false;
        public bool ShowOldPercentage { get; set; } = false;
        public bool ShowNewPercentage { get; set; } = false;

    }
}
