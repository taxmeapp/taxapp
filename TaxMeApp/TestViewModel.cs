using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxMeApp
{
    public class TestViewModel : INotifyPropertyChanged
    {

        private Test model;

        public TestViewModel()
        {
            model = new Test
            {
                MinIncome = 16000,
                MaxIncome = 400000
            };
        }

        public double MinIncome
        {
            get { return model.MinIncome; }
            set 
            { 
                
                if (model.MinIncome != value)
                {
                    model.MinIncome = value;
                    OnPropertyChange("MinIncome");
                    OnPropertyChange("DeltaIncome");
                }           

            
            }
        }

        public double MaxIncome
        {
            get { return model.MaxIncome; }
            set
            {

                if (model.MaxIncome != value)
                {
                    model.MaxIncome = value;
                    OnPropertyChange("MaxIncome");
                    OnPropertyChange("DeltaIncome");
                }

            }
        }

        public double DeltaIncome
        {
            get { return model.MaxIncome - model.MinIncome; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
