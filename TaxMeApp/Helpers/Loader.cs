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

        public TestViewModel testVM { get; private set; }
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

            //yearsModel.SelectedYear = yearsModel.YearList[0];


            // make view models
            testVM = new TestViewModel();
            testVM.setYearsModel(yearsModel);
            testVM.setGraphModel(graphModel);
            testVM.setDataModel(dataModel);

            graphVM = new GraphViewModel();
            graphVM.setGraphModel(graphModel);


            testVM.GraphVM = graphVM;


            testVM.Init();

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
