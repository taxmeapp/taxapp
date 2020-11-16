using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxMeApp.models
{
    class IncomeYearModel
    {
        public int year { get; set; }
        public BracketModel yearData { get; set; }
    }
}
