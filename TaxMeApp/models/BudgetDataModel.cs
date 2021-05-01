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
        //Used to store a list of budget years (which have GDP and other data)

        public ObservableCollection<BudgetYearModel> YearData { get; set; }

        public BudgetDataModel()
        {
            this.YearData = new ObservableCollection<BudgetYearModel>();
        }

    }
}
