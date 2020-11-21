using CsvHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxMeApp.models;

namespace TaxMeApp.Helpers
{
    public static class Parser
    {

        public static IncomeYearModel ParseCSV(string path)
        {

            var reader = new StreamReader(path);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            reader.ReadLine();

            // Get the year from the filename:
            // Split at the backslash
            string[] pathSplit = path.Split('\\');
            // Keep the last split (xxxx.csv)
            string filename = pathSplit[pathSplit.Length - 1];
            // get rid of extension
            filename = filename.Replace(".csv", "");

            // The two variables we need for an IncomeYearModel:
            int year = Int32.Parse(filename);

            var brackets = new ObservableCollection<BracketModel>(csv.GetRecords<BracketModel>());

            /*
              
                //Verification of parsing:
             
            Trace.WriteLine("Year: " + path);

            foreach (var item in brackets)
            {
                Trace.WriteLine("Income Tax: " + item.IncomeTax);
            }

            Trace.WriteLine("");
            */

            // Create a new Model and return it
            return new IncomeYearModel
            {
                Year = year,
                Brackets = brackets
            };
            

        }

    }
}
