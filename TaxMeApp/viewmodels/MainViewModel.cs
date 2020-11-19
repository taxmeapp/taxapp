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

        protected GraphModel graphModel = new GraphModel();

        public void setGraphModel(GraphModel graphModel)
        {
            this.graphModel = graphModel;
        }


        // Inherited from the interface, for notifying the view to update
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
