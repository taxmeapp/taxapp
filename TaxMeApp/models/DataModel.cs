﻿using System;
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

        public List<long> oldRevenueByBracket { get; } = new List<long>();

        public List<long> newRevenueByBracket { get; } = new List<long>();

        public List<double> oldTaxPctByBracket { get; } = new List<double>();

        public List<double> newTaxPctByBracket { get; } = new List<double>();

    }
}
