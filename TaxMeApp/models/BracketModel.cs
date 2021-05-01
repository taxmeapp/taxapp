using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TaxMeApp.models
{
    public class BracketModel
    {
        //Used to store data on each tax bracket in a year of data

        public string Range { get; set; }
        public int NumReturns { get; set; }
        public long GrossIncome { get; set; }
        public long TaxableIncome { get; set; }
        public long IncomeTax { get; set; }
        public double PercentOfTaxableIncomePaid { get; set; }
        public double PercentOfGrossIncomePaid { get; set; }
        public double AverageTotalIncomeTax { get; set; }
        public int LowerBound { get; private set; }
        public int UpperBound { get; private set; }

        public void SetBounds()
        {
            // Retrieve numbers in bracket label
            List<string> bounds = Regex.Matches(this.Range, @"(,*[\d]+,*[\d]*)+").Cast<Match>().Select(match => match.Value).ToList();
            
            // If no numbers in bracket label, bracket is for $0 ("No adjusted gross income")
            // Set both bounds to $0
            if (bounds.Count == 0)
            {
                this.LowerBound = 0;
                this.UpperBound = 0;
            }
            // If only one number in bracket label, bracket is for $10,000,000 or greater ("$10,000,000 or more")
            // Set lower bound to $10,000,000 and upper bound to $15,000,000 (mimic difference in range from preceding bracket)
            else if (bounds.Count == 1)
            {
                this.LowerBound = int.Parse(bounds[0], NumberStyles.AllowThousands);
                this.UpperBound = 15000000;
            }
            // Otherwise, there are two numbers ("$X under $Y")
            // Set these as the lower and upper bounds
            else
            {
                this.LowerBound = int.Parse(bounds[0], NumberStyles.AllowThousands);
                this.UpperBound = int.Parse(bounds[1], NumberStyles.AllowThousands) - 1;
            }
        }
    }

}
