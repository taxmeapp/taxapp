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
        //Stores an individual tax plan and options that were selected with that plan

        public string Name { get; set; }
        public ObservableCollection<double> TaxRates { get; set; }

        //CustomTaxRates are used to manually override the tax rates and are only changed by the 
        //Bracket adjustment slider. 
        
        //If you are using the slant tax, setting the max and poverty bracket sliders will
        //Recalculate the slant tax and overwrite manual changes. Custom tax rates is used to store the
        //manual changes and add them back to the tax rates
        public List<double> CustomTaxRates { get; set; } = new List<double>();

        //Used to store options for each tax plan so they can be restored when you change plans and
        //then change back. 
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