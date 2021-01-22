using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaxMeApp.Helpers;

namespace TaxMeApp.viewmodels
{
    public class SettingsViewModel : MainViewModel
    {

        public ICommand DataViewBtnCommand { get; set; }

        public SettingsViewModel()
        {

            DataViewBtnCommand = new RelayCommand(o => dataViewButtonClick(""));

        }



        private void dataViewButtonClick(object sender)
        {

            MainVM.TabSelected = 0;

        }

    }
}
