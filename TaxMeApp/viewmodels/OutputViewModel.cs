using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxMeApp.Helpers;

namespace TaxMeApp.viewmodels
{
    public class OutputViewModel : MainViewModel
    {

        public void Update()
        {

            OnPropertyChange("NumPovertyPopOutput");
            OnPropertyChange("NumMaxPopOutput");
            OnPropertyChange("TotalRevenueOldOutput");
            OnPropertyChange("TotalRevenueNewOutput");
            OnPropertyChange("RevenueDifferenceOutput");
            OnPropertyChange("UBICost");

        }


        /*
                Output Area 
        */

        public string NumPovertyPopOutput
        {
            get
            {
                return Formatter.Format(DataModel.NumPovertyPop);
            }
        }

        public string NumMaxPopOutput
        {
            get
            {
                return Formatter.Format(DataModel.NumMaxPop);
            }
        }

        public string TotalRevenueOldOutput
        {
            get
            {
                return Formatter.Format(DataModel.TotalRevenueOld);
            }
        }

        public string TotalRevenueNewOutput
        {
            get
            {
                return Formatter.Format(DataModel.TotalRevenueNew);
            }
        }

        public string RevenueDifferenceOutput
        {
            get
            {
                return Formatter.Format(DataModel.RevenueDifference);
            }
        }


        public long totalRevenueNewTesting = 0;
        public long TotalRevenueNewTesting
        {
            get
            {
                return totalRevenueNewTesting;
            }
            set
            {
                totalRevenueNewTesting = value;

            }

        }

        public string UBICost
        {
            get
            {
                return Formatter.Format(DataModel.TotalUBICost);
            }
        }



    }
}
