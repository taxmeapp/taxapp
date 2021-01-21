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

        private int totalBrackets
        {
            get
            {
                return YearsModel.SelectedIncomeYearModel.Brackets.Count;
            }
        }

        // Number of brackets at max rate
        public int MaxBracketCount
        {
            get
            {
                return GraphModel.MaxBracketCount;
            }
        }

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
                        else if (index > PovertyLineIndex && index < totalBrackets - MaxBracketCount)
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

    }
}
