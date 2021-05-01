﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxMeApp.models
{
    public class IncomeYearModel
    {
        //Used to store each year's data

        public int Year { get; set; }
        public ObservableCollection<BracketModel> Brackets { get; set; }

    }
}
