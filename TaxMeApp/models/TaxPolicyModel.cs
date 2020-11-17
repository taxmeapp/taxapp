using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxMeApp.models
{
    public class TaxPolicyModel
    {
        public override string ToString()
        {
            return name;
        }
        public string name { get; set; }
        public ObservableCollection<double> vals { get; set; }
    }
}
