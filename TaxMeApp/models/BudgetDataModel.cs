using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxMeApp.models
{
    public class BudgetDataModel
    {

        public ObservableCollection<BudgetYearModel> YearData { get; set; }

        public BudgetDataModel()
        {
            this.YearData = new ObservableCollection<BudgetYearModel>();
        }

    }
}
