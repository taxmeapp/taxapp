using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxMeApp.models
{
    public class OptionsModel
    {
        public OptionsModel() {
            this.MaxTaxRate = 0;
            this.MaxBracketCount = 0;
        }

        public double MaxTaxRate { get; set; }
        public int MaxBracketCount { get; set; }
    }
}
