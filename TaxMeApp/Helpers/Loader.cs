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

        public ControlViewModel controlVM { get; set; }
        public GraphViewModel graphVM { get; set; }

        public YearsModel yearsModel { get; set; }
        public GraphModel graphModel { get; set; }
        public DataModel dataModel { get; set; }

        public Loader()
        {

            // Make models

            yearsModel = new YearsModel();
            graphModel = new GraphModel();
            dataModel = new DataModel();

            // Load CSVs

            loadYears();


            // make view models

            controlVM = new ControlViewModel(yearsModel, graphModel, dataModel);

            graphVM = new GraphViewModel();
            graphVM.GraphModel = graphModel;

            controlVM.GraphVM = graphVM;

            controlVM.Init();

        }

        public void loadYears()
        {

            // Return an array of all filenames from the directory that end in .csv
            string[] files = Directory.GetFiles("res\\TaxCSV", "*.csv");

            // Iterate through these files
            foreach (string filename in files)
            {

                // Parse the CSV and save it into an IncomeYearModel
                IncomeYearModel year = Parser.ParseCSV(filename);

                // Add that IncomeYearModel into the YearsModel
                yearsModel.Years.Add(year.Year, year);

            }
        }

        public static string[] loadYearsTest()
        {

            // Return an array of all filenames from the directory that end in .csv
            string[] files = Directory.GetFiles("res\\TaxCSV", "*.csv");
            return files;
        }

    }
}
