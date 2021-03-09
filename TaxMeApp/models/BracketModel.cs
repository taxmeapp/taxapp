using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TaxMeApp.models
{
    public class BracketModel
    {
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
            List<string> bounds = Regex.Matches(this.Range, @"(,*[\d]+,*[\d]*)+").Cast<Match>().Select(match => match.Value).ToList();
            if (bounds.Count == 0)
            {
                this.LowerBound = 0;
                this.UpperBound = 0;
            }
            else if (bounds.Count == 1)
            {
                this.LowerBound = int.Parse(bounds[0], NumberStyles.AllowThousands);
                this.UpperBound = 15000000;
            }
            else
            {
                this.LowerBound = int.Parse(bounds[0], NumberStyles.AllowThousands);
                this.UpperBound = int.Parse(bounds[1], NumberStyles.AllowThousands)-1;
            }
        }
    }

}
