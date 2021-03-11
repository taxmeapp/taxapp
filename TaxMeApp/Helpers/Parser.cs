using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
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

            // Remove footnote symbol from cells
            File.WriteAllText(path, File.ReadAllText(path).Replace("*", ""));

            // Allow typeconverter to parse cells with commas and spaces
            var NumberOptions = new TypeConverterOptions
            {
                NumberStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite
            };

            string StartLine = "size of adjusted gross income";
            string EndLine = "$10,000,000 or more";

            var reader = new StreamReader(path);

            // Skip initial information | todo: optimize this
            reader.ReadLine();
            reader.ReadLine();
            reader.ReadLine();

            // Skip header rows until start of relevant information
            string currentLine = reader.ReadLine();
            while (!currentLine.StartsWith(StartLine, StringComparison.InvariantCultureIgnoreCase))
            {
                currentLine = reader.ReadLine();
            }

            // Configure CSVReader to skip rows that are empty
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                ShouldSkipRecord = line => line.Record.All(string.IsNullOrWhiteSpace)
            };

            using (var csv = new CsvReader(reader, config))
            {

                // Add converters that check for empty or null cells and replace with 0
                csv.Context.TypeConverterCache.AddConverter<double>(new CustomDoubleConverter());
                csv.Context.TypeConverterCache.AddConverter<long>(new CustomLongConverter());

                // Add number styling to typeconverter configuration
                csv.Context.TypeConverterOptionsCache.AddOptions<long>(NumberOptions);
                csv.Context.TypeConverterOptionsCache.AddOptions<int>(NumberOptions);

                // Add class map to only read in designated properties
                csv.Context.RegisterClassMap<BracketModelMap>();

                // Skip cumulative tax returns row
                csv.Read();

                // Get the year from the filename:
                // Split at the backslash
                string[] pathSplit = path.Split('\\');
                // Keep the last split (xxxx.csv)
                string filename = pathSplit[pathSplit.Length - 1];
                // Get rid of extension
                filename = filename.Replace(".csv", "");

                // The two variables we need for an IncomeYearModel:
                int year = Int32.Parse(filename);
                var brackets = new ObservableCollection<BracketModel>();

                // Create array to check if end of relevant information has been parsed
                var row = Array.Empty<string>();

                // Check if most recent row was the last relevant line of data
                // If not, keep reading into brackets collection
                while (csv.Read() && !Array.Exists(row, field => field.StartsWith(EndLine)))
                {
                    row = csv.Parser.Record;
                    BracketModel bracket = csv.GetRecord<BracketModel>();
                    bracket.SetBounds();
                    brackets.Add(bracket);
                }

                // Create a new Model and return it
                return new IncomeYearModel
                {
                    Year = year,
                    Brackets = brackets
                };
            }

        }

    }

    public class BracketModelMap : ClassMap<BracketModel>
    {
        public BracketModelMap()
        {
            Map(m => m.Range).Index(0);
            Map(m => m.NumReturns).Index(6);
            Map(m => m.GrossIncome).Index(8);
            Map(m => m.TaxableIncome).Index(11);
            Map(m => m.IncomeTax).Index(14);
            Map(m => m.PercentOfTaxableIncomePaid).Index(18);
            Map(m => m.PercentOfGrossIncomePaid).Index(19);
            Map(m => m.AverageTotalIncomeTax).Index(20);
        }
    }

    public class CustomDoubleConverter : DoubleConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text))
            {
                return (double)0;
            }

            else if (text.Contains("["))
            {
                return (double)0;
            }

            return base.ConvertFromString(text, row, memberMapData);
        }
    }

    public class CustomLongConverter : Int64Converter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text))
            {
                return (long)0;
            }

            else if (text.Contains("--"))
            {
                return (long)0;
            }
            return base.ConvertFromString(text, row, memberMapData);

        }
    }
}