using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxMeApp.viewmodels
{
    class GraphViewModel : MainViewModel
    {

        public SeriesCollection SeriesCollection
        {
            get
            {
                return graphModel.SeriesCollection;
            }
        }

        public string[] Labels
        {
            get
            {
                return graphModel.Labels;
            }
        }

        public int PovertyLineIndex
        {
            get
            {
                return graphModel.PovertyLineIndex;
            }
        }

    }
}
