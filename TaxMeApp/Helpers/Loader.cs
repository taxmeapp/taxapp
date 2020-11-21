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

namespace TaxMeApp
{
    class Loader
    {

        public ControlViewModel controlVM { get; private set; }
        public GraphViewModel graphVM { get; private set; }

        private YearsModel yearsModel;
        private GraphModel graphModel;
        private DataModel dataModel;

        public Loader()
        {

            // Make models

            yearsModel = new YearsModel();
            graphModel = new GraphModel();
            dataModel = new DataModel();


            // Load CSVs

            loadYears();


            // make view models

            controlVM = new ControlViewModel();
            controlVM.YearsModel = yearsModel;
            controlVM.GraphModel = graphModel;
            controlVM.DataModel = dataModel;

            graphVM = new GraphViewModel();
            graphVM.GraphModel = graphModel;

            controlVM.GraphVM = graphVM;

            controlVM.Init();

        }

        private void loadYears()
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


    }
}
