using System;

namespace TaxMeApp.models
{
    public class BracketModel
    {
        public int LowerBound { get; set; }
        public int UpperBound { get; set; }
        public int NumReturns { get; set; }
        public long GrossIncome { get; set; }
        public long TaxableIncome { get; set; }
        public long IncomeTax { get; set; }
        public double PercentOfTaxableIncomePaid { get; set; }
        public double PercentOfGrossIncomePaid { get; set; }
        public double AverageTotalIncomeTax { get; set; }
    }

}
