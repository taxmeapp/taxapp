using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxMeApp.models
{
    public class IndividualTaxPlanModel
    {
        public string Name { get; set; }
        public ObservableCollection<double> TaxRates { get; set; }

        public IndividualTaxPlanModel()
        {
            this.Name = "";
            this.TaxRates = new ObservableCollection<double>();
        }
        public IndividualTaxPlanModel(string n, ObservableCollection<double> rates)
        {
            this.Name = n;
            this.TaxRates = rates;
        }
    }
}