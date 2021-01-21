using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxMeApp.viewmodels
{
    public class DataViewModel : MainViewModel
    {

        public void DataInit()
        {

            ControlVM.ControlInit();
            GraphVM.GraphInit();

        }

    }
}
