using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TaxMeApp.viewmodels
{
    public class GraphViewModel : MainViewModel
    {

        public SeriesCollection Series
        {
            get
            {
                return base.GraphModel.Series;
            }
        }

        public string[] Labels
        {
            get
            {
                return base.GraphModel.Labels;
            }
        }

        public int PovertyLineIndex
        {
            get
            {
                return base.GraphModel.PovertyLineIndex;
            }
        }

    }
}
