using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxMeApp.models;

namespace TaxMeApp.viewmodels
{
    public abstract class MainViewModel : INotifyPropertyChanged
    {

        protected GraphModel graphModel = new GraphModel();
        protected DataModel dataModel = new DataModel();
        protected YearsModel yearsModel = new YearsModel();

        public void setGraphModel(GraphModel graphModel)
        {
            this.graphModel = graphModel;
        }

        public virtual void setDataModel(DataModel dataModel)
        {
            this.dataModel = dataModel;
        }

        public void setYearsModel(YearsModel yearsModel)
        {
            this.yearsModel = yearsModel;
        }


        // Inherited from the interface, for notifying the view to update
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
