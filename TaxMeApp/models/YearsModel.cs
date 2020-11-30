using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxMeApp.models
{
    public class YearsModel
    {
        public YearsModel() { 
        
        }
        // Store dictionary references of all years
        public Dictionary<int, IncomeYearModel> Years { get; set; } = new Dictionary<int, IncomeYearModel>();

        // Get a list of all years in the dictionary
        public List<int> YearList
        {
            get
            {
                // Make a new list from keys
                var yearList = new List<int>(Years.Keys);
                // Ascending (later years on top)
                yearList.Sort();
                // Now descending (most recent at top)
                yearList.Reverse();

                return yearList;
            }
        }

        // Year currently selected on the dropdown
        public int SelectedYear { get; set; }

        public IncomeYearModel SelectedIncomeYearModel
        {
            get 
            {
                Years.TryGetValue(SelectedYear, out IncomeYearModel selected);
                return selected; 
            }

        }

    }
}
