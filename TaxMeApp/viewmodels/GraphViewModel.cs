using LiveCharts;
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
                return GraphModel.Series;
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
        }

    }
}
