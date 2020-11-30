using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxMeApp.models
{
    public class DataModel
    {
        public DataModel() {
            Population = new ObservableCollection<int>();
            NumPovertyPop = 0;
            NumMaxPop = 0;
            TotalRevenueOld = 0;
            TotalRevenueNew = 0;
            MaxTaxRate = 0;
            OldRevenueByBracket = new List<long>();
            NewRevenueByBracket = new List<long>();
            OldTaxPctByBracket = new List<double>();
            NewTaxPctByBracket = new List<double>();
        }
        public ObservableCollection<int> Population { get; } = new ObservableCollection<int>();
        public int NumPovertyPop { get; set; }
        public int NumMaxPop { get; set; }
        public long TotalRevenueOld { get; set; }
        public long TotalRevenueNew { get; set; }

        public long RevenueDifference
        {
            get
            {
                return TotalRevenueNew - TotalRevenueOld;
            }
        }

        public int MaxTaxRate { get; set; } = 20;

        public List<long> OldRevenueByBracket { get; set; } = new List<long>();

        public List<long> NewRevenueByBracket { get; set; } = new List<long>();

        public List<double> OldTaxPctByBracket { get; set; } = new List<double>();

        public List<double> NewTaxPctByBracket { get; set; } = new List<double>();

    }
}
