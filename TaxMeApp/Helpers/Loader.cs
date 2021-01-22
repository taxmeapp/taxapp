using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxMeApp.Helpers;
using TaxMeApp.models;
using TaxMeApp.viewmodels;
using LiveCharts;
using LiveCharts.Wpf;

namespace TaxMeApp
{
    public class Loader
    {

        public MainViewModel MainVM { get; set; }
        public DataViewModel DataVM { get; set; }
        public ControlViewModel ControlVM { get; set; }
        public GraphViewModel GraphVM { get; set; }
        public OutputViewModel OutputVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }

        public YearsModel YearsModel { get; set; }
        public GraphModel GraphModel { get; set; }
        public DataModel DataModel { get; set; }

        public Loader()
        {

            // Make models

            YearsModel = new YearsModel();
            GraphModel = new GraphModel();
            DataModel = new DataModel();

            // Load CSVs
            LoadYears();

            // make viewmodels
            MainVM = new MainViewModel();
            DataVM = new DataViewModel();
            ControlVM = new ControlViewModel();
            GraphVM = new GraphViewModel();
            OutputVM = new OutputViewModel();
            SettingsVM = new SettingsViewModel();

            // Link VMs
            MainVM.DataVM = DataVM;
            MainVM.SettingsVM = SettingsVM;

            DataVM.ControlVM = ControlVM;
            DataVM.GraphVM = GraphVM;
            DataVM.OutputVM = OutputVM;

            ControlVM.MainVM = MainVM;
            ControlVM.DataVM = DataVM;
            ControlVM.OutputVM = OutputVM;
            ControlVM.GraphVM = GraphVM;

            SettingsVM.MainVM = MainVM;

            // Connect models to VMs

            DataVM.DataModel = DataModel;
            DataVM.YearsModel = YearsModel;
            DataVM.GraphModel = GraphModel;

            ControlVM.YearsModel = YearsModel;
            ControlVM.GraphModel = GraphModel;
            ControlVM.DataModel = DataModel;

            GraphVM.GraphModel = GraphModel;
            GraphVM.YearsModel = YearsModel;
            GraphVM.DataModel = DataModel;

            OutputVM.DataModel = DataModel;

            DataVM.DataInit();

        }

        public void LoadYears()
        {

            // Return an array of all filenames from the directory that end in .csv
            string[] files = Directory.GetFiles("res\\TaxCSV", "*.csv");

            // Iterate through these files
            foreach (string filename in files)
            {

                // Parse the CSV and save it into an IncomeYearModel
                IncomeYearModel year = Parser.ParseCSV(filename);

                // Add that IncomeYearModel into the YearsModel
                YearsModel.Years.Add(year.Year, year);

            }
        }

        public static string[] LoadYearsTest()
        {

            // Return an array of all filenames from the directory that end in .csv
            string[] files = Directory.GetFiles("res\\TaxCSV", "*.csv");
            return files;
        }

    }
}
