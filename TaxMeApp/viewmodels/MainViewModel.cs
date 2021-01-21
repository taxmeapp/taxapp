using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxMeApp.models;

namespace TaxMeApp.viewmodels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public GraphModel GraphModel { get; set; }
        public DataModel DataModel { get; set; }
        public YearsModel YearsModel { get; set; }

        public ControlViewModel ControlVM { get; set; }
        public DataViewModel DataVM { get; set; }
        public GraphViewModel GraphVM { get; set; }
        public OutputViewModel OutputVM { get; set; }


        // Inherited from the interface, for notifying the view to update
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
