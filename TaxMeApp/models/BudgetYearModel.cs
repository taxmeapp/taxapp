using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxMeApp.models
{
    public class BudgetYearModel
    {
        //Used to store GDP, budget, and debt data

        public int Year { get; set; }
        public double GDP { get; set; }
        public double TotalBudget { get; set; }
        public double Deficit { get; set; }
        public double TotalDebt { get; set; }
        public double BudgetPercent { get; set; }
        public double DeficitPercent { get; set; }

        public BudgetYearModel(){
            this.Year = 0;
            this.GDP = 0;
            this.TotalBudget = 0;
            this.Deficit = 0;
            this.TotalDebt = 0;
            this.BudgetPercent = 0;
            this.DeficitPercent = 0;
        }

        public BudgetYearModel(int y, double gdp, double tb, double d, double td, double bp, double dp) {
            this.Year = y;
            this.GDP = gdp;
            this.TotalBudget = tb;
            this.Deficit = d;
            this.TotalDebt = td;
            this.BudgetPercent = bp;
            this.DeficitPercent = dp;
        }

    }
}
