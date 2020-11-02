using CsvHelper;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using TaxMeApp.models;

namespace TaxMeApp
{
    public class TestViewModel : INotifyPropertyChanged
    {
        private Test model;
        public ObservableCollection<BracketModel> Brackets { get; set; }
        public string[] Labels { get; set; }
        public List<int> Population = new List<int>();
        public SeriesCollection SeriesCollection { get; set; }

        public TestViewModel()
        {
            model = new Test
            {
                MinIncome = 16000,
                MaxIncome = 400000
            };

            Brackets = new ObservableCollection<BracketModel>();
            ParseCSV();
            Graph();
        }

        public double MinIncome
        {
            get { return model.MinIncome; }
            set
            {

                if (model.MinIncome != value)
                {
                    model.MinIncome = value;
                    OnPropertyChange("MinIncome");
                    OnPropertyChange("DeltaIncome");
                }


            }
        }

        public double MaxIncome
        {
            get { return model.MaxIncome; }
            set
            {

                if (model.MaxIncome != value)
                {
                    model.MaxIncome = value;
                    OnPropertyChange("MaxIncome");
                    OnPropertyChange("DeltaIncome");
                }

            }
        }

        public double DeltaIncome
        {
            get { return model.MaxIncome - model.MinIncome; }
        }

        public int PovertyLineIndex
        {
            get { return 3; }
        }

        public int NumUnderPoverty
        {
            get { int total = 0;
                for(int i = 0; i<=PovertyLineIndex; i++)
                {
                    total += Brackets[i].NumReturns;
                }
                return total;
            }
        }

        public void ParseCSV()
        {
            string path = "res\\2018Tax.csv";
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                reader.ReadLine();
                var brackets = csv.GetRecords<BracketModel>();
                Brackets = new ObservableCollection<BracketModel>(brackets);
            }

            foreach (BracketModel bracket in Brackets)
            {
                Population.Add(bracket.NumReturns);
            }
        }

        public void Graph()
        {
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "2018",
                    Values = new ChartValues<int>(Population)
                }
            };
            Labels = new[]
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
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
