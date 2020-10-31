using System;

namespace TaxMeApp.models
{
    public class BracketModel
    {
        public String Bracket { get; set; }
        public int NumReturns { get; set; }
        public long GrossIncome { get; set; }
        public long TaxableIncome { get; set; }
        public int IncomeTax { get; set; }
        public double PercentOfTaxableIncomePaid { get; set; }
        public double PercentOfGrossIncomePaid { get; set; }
        public int AverageTotalIncomeTax { get; set; }
    }

}
