using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxMeApp.models
{
    public class BracketDisplayModel
    {
        public string label { get; set; }
        public double taxRate { get; set; }
        public BracketDisplayModel(string l, double t) {
            label = l;
            taxRate = t;
        }
    }
}
