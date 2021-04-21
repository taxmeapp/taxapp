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
        public List<double> CustomTaxRates { get; set; } = new List<double>();

        public double MaxTaxRate { get; set; } = 0;
        public int MaxBracketCount { get; set; } = 0;
        public int PovertyLineIndex { get; set; } = -1;
        public int FlatTaxRate { get; set; } = 0;
        public bool BalancePovertyWithMax { get; set; } = false;
        public bool BalanceMaxWithPoverty { get; set; } = false;

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