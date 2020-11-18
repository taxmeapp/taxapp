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
        public List<double> taxRate { get; set; }
        public int index { get; set; }
        public BracketDisplayModel(string l, List<double> t, int i) {
            label = l;
            taxRate = t;
            index = i;
        }
    }
}
