using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxMeApp.models
{
    public class TaxPlansModel
    {
        public ObservableCollection<string> tpl = new ObservableCollection<string>();

        public TaxPlansModel()
        {
            this.TaxPlans = new Dictionary<string, IndividualTaxPlanModel>();
            this.SelectedTaxPlanName = "";
        }

        public Dictionary<string, IndividualTaxPlanModel> TaxPlans { get; set; }

        public ObservableCollection<string> TaxPlanList
        {
            get
            {
                tpl.Clear();
                List<string> keys = TaxPlans.Keys.ToList();
                for (int i = 0; i < keys.Count; i++)
                {
                    tpl.Add(keys[i]);
                }

                return (tpl);
            }
            set
            {
                TaxPlanList = value;
            }
        }

        public string SelectedTaxPlanName { get; set; }
        public IndividualTaxPlanModel SelectedTaxPlan
        {
            get
            {
                TaxPlans.TryGetValue(SelectedTaxPlanName, out IndividualTaxPlanModel selected);
                return selected;
            }
        }
    }
}