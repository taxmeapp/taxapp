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
        public BudgetDataModel BudgetDataModel { get; set; }
        public GraphModel GraphModel { get; set; }
        public DataModel DataModel { get; set; }
        public TaxPlansModel TaxPlansModel { get; set; }
        public OptionsModel OptionsModel { get; set; }

        public Loader()
        {

            // Make models
            YearsModel = new YearsModel();
            BudgetDataModel = new BudgetDataModel();
            GraphModel = new GraphModel();
            DataModel = new DataModel();
            TaxPlansModel = new TaxPlansModel();
            OptionsModel = new OptionsModel();

            // Load CSVs
            // To load new CSVs, place CSV in res folder, under corresponding subfolder
            LoadYears();
            LoadBudget();

            // Make viewmodels
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

            OutputVM.ControlVM = ControlVM;
            OutputVM.DataVM = DataVM;

            SettingsVM.MainVM = MainVM;

            // Connect models to VMs
            DataVM.DataModel = DataModel;
            DataVM.YearsModel = YearsModel;
            DataVM.GraphModel = GraphModel;
            DataVM.TaxPlansModel = TaxPlansModel;
            DataVM.OptionsModel = OptionsModel;

            ControlVM.YearsModel = YearsModel;
            ControlVM.GraphModel = GraphModel;
            ControlVM.DataModel = DataModel;
            ControlVM.TaxPlansModel = TaxPlansModel;
            ControlVM.OptionsModel = OptionsModel;
            ControlVM.BudgetDataModel = BudgetDataModel;

            GraphVM.GraphModel = GraphModel;
            GraphVM.YearsModel = YearsModel;
            GraphVM.DataModel = DataModel;
            GraphVM.TaxPlansModel = TaxPlansModel;
            GraphVM.OptionsModel = OptionsModel;

            OutputVM.DataModel = DataModel;
            OutputVM.OptionsModel = OptionsModel;
            OutputVM.BudgetDataModel = BudgetDataModel;

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

        public void LoadBudget() {
            string[] files = Directory.GetFiles("res\\BudgetData", "*.csv");
            BudgetDataModel = Parser.ParseBudgetData(files[0]);
        }

        public static string[] LoadYearsTest()
        {

            // Return an array of all filenames from the directory that end in .csv
            string[] files = Directory.GetFiles("res\\TaxCSV", "*.csv");
            return files;
        }

    }
}