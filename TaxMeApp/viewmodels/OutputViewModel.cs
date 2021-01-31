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

            OptionsModel.updateFunding();
            OnPropertyChange("DefenseFunding");
            OnPropertyChange("MedicaidFunding");
            OnPropertyChange("WelfareFunding");
            OnPropertyChange("VeteransFunding");
            OnPropertyChange("FoodStampsFunding");
            OnPropertyChange("EducationFunding");
            OnPropertyChange("PublicHousingFunding");
            OnPropertyChange("HealthFunding");
            OnPropertyChange("ScienceFunding");
            OnPropertyChange("TransportationFunding");
            OnPropertyChange("InternationalFunding");
            OnPropertyChange("EEFunding");
            OnPropertyChange("UnemploymentFunding");
            OnPropertyChange("AgricultureFunding");

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
                OptionsModel.revenue = DataModel.TotalRevenueNew;
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



        public bool DefenseSpendingChecked {
            get {
                return OptionsModel.DefenseChecked;    
            }
            set {
                OptionsModel.DefenseChecked = value;
                OptionsModel.updateFunding();
                OnPropertyChange("DefenseFunding");
                OnPropertyChange("MedicaidFunding");
                OnPropertyChange("WelfareFunding");
                OnPropertyChange("VeteransFunding");
                OnPropertyChange("FoodStampsFunding");
                OnPropertyChange("EducationFunding");
                OnPropertyChange("PublicHousingFunding");
                OnPropertyChange("HealthFunding");
                OnPropertyChange("ScienceFunding");
                OnPropertyChange("TransportationFunding");
                OnPropertyChange("InternationalFunding");
                OnPropertyChange("EEFunding");
                OnPropertyChange("UnemploymentFunding");
                OnPropertyChange("AgricultureFunding");
            } 
        }
        public string DefenseFunding {
            get {
                return OptionsModel.GetDefenseFunding();
            }
        }

        public bool MedicaidSpendingChecked
        {
            get
            {
                return OptionsModel.MedicaidChecked;
            }
            set
            {
                OptionsModel.MedicaidChecked = value;
                OptionsModel.updateFunding();
                OnPropertyChange("DefenseFunding");
                OnPropertyChange("MedicaidFunding");
                OnPropertyChange("WelfareFunding");
                OnPropertyChange("VeteransFunding");
                OnPropertyChange("FoodStampsFunding");
                OnPropertyChange("EducationFunding");
                OnPropertyChange("PublicHousingFunding");
                OnPropertyChange("HealthFunding");
                OnPropertyChange("ScienceFunding");
                OnPropertyChange("TransportationFunding");
                OnPropertyChange("InternationalFunding");
                OnPropertyChange("EEFunding");
                OnPropertyChange("UnemploymentFunding");
                OnPropertyChange("AgricultureFunding");
            }
        }
        public string MedicaidFunding
        {
            get
            {
                return OptionsModel.GetMedicaidFunding();
            }
        }

        public bool WelfareSpendingChecked
        {
            get
            {
                return OptionsModel.WelfareChecked;
            }
            set
            {
                OptionsModel.WelfareChecked = value;
                OptionsModel.updateFunding();
                OnPropertyChange("DefenseFunding");
                OnPropertyChange("MedicaidFunding");
                OnPropertyChange("WelfareFunding");
                OnPropertyChange("VeteransFunding");
                OnPropertyChange("FoodStampsFunding");
                OnPropertyChange("EducationFunding");
                OnPropertyChange("PublicHousingFunding");
                OnPropertyChange("HealthFunding");
                OnPropertyChange("ScienceFunding");
                OnPropertyChange("TransportationFunding");
                OnPropertyChange("InternationalFunding");
                OnPropertyChange("EEFunding");
                OnPropertyChange("UnemploymentFunding");
                OnPropertyChange("AgricultureFunding");
            }
        }
        public string WelfareFunding
        {
            get
            {
                return OptionsModel.GetWelfareFunding();
            }
        }

        public bool VeteransSpendingChecked
        {
            get
            {
                return OptionsModel.VeteransChecked;
            }
            set
            {
                OptionsModel.VeteransChecked = value;
                OptionsModel.updateFunding();
                OnPropertyChange("DefenseFunding");
                OnPropertyChange("MedicaidFunding");
                OnPropertyChange("WelfareFunding");
                OnPropertyChange("VeteransFunding");
                OnPropertyChange("FoodStampsFunding");
                OnPropertyChange("EducationFunding");
                OnPropertyChange("PublicHousingFunding");
                OnPropertyChange("HealthFunding");
                OnPropertyChange("ScienceFunding");
                OnPropertyChange("TransportationFunding");
                OnPropertyChange("InternationalFunding");
                OnPropertyChange("EEFunding");
                OnPropertyChange("UnemploymentFunding");
                OnPropertyChange("AgricultureFunding");
            }
        }
        public string VeteransFunding
        {
            get
            {
                return OptionsModel.GetVeteransFunding();
            }
        }

        public bool FoodStampsSpendingChecked
        {
            get
            {
                return OptionsModel.FoodStampsChecked;
            }
            set
            {
                OptionsModel.FoodStampsChecked = value;
                OptionsModel.updateFunding();
                OnPropertyChange("DefenseFunding");
                OnPropertyChange("MedicaidFunding");
                OnPropertyChange("WelfareFunding");
                OnPropertyChange("VeteransFunding");
                OnPropertyChange("FoodStampsFunding");
                OnPropertyChange("EducationFunding");
                OnPropertyChange("PublicHousingFunding");
                OnPropertyChange("HealthFunding");
                OnPropertyChange("ScienceFunding");
                OnPropertyChange("TransportationFunding");
                OnPropertyChange("InternationalFunding");
                OnPropertyChange("EEFunding");
                OnPropertyChange("UnemploymentFunding");
                OnPropertyChange("AgricultureFunding");
            }
        }
        public string FoodStampsFunding
        {
            get
            {
                return OptionsModel.GetFoodStampsFunding();
            }
        }

        public bool EducationSpendingChecked
        {
            get
            {
                return OptionsModel.EducationChecked;
            }
            set
            {
                OptionsModel.EducationChecked = value;
                OptionsModel.updateFunding();
                OnPropertyChange("DefenseFunding");
                OnPropertyChange("MedicaidFunding");
                OnPropertyChange("WelfareFunding");
                OnPropertyChange("VeteransFunding");
                OnPropertyChange("FoodStampsFunding");
                OnPropertyChange("EducationFunding");
                OnPropertyChange("PublicHousingFunding");
                OnPropertyChange("HealthFunding");
                OnPropertyChange("ScienceFunding");
                OnPropertyChange("TransportationFunding");
                OnPropertyChange("InternationalFunding");
                OnPropertyChange("EEFunding");
                OnPropertyChange("UnemploymentFunding");
                OnPropertyChange("AgricultureFunding");
            }
        }
        public string EducationFunding
        {
            get
            {
                return OptionsModel.GetEducationFunding();
            }
        }

        public bool PublicHousingSpendingChecked
        {
            get
            {
                return OptionsModel.PublicHousingChecked;
            }
            set
            {
                OptionsModel.PublicHousingChecked = value;
                OptionsModel.updateFunding();
                OnPropertyChange("DefenseFunding");
                OnPropertyChange("MedicaidFunding");
                OnPropertyChange("WelfareFunding");
                OnPropertyChange("VeteransFunding");
                OnPropertyChange("FoodStampsFunding");
                OnPropertyChange("EducationFunding");
                OnPropertyChange("PublicHousingFunding");
                OnPropertyChange("HealthFunding");
                OnPropertyChange("ScienceFunding");
                OnPropertyChange("TransportationFunding");
                OnPropertyChange("InternationalFunding");
                OnPropertyChange("EEFunding");
                OnPropertyChange("UnemploymentFunding");
                OnPropertyChange("AgricultureFunding");
            }
        }
        public string PublicHousingFunding
        {
            get
            {
                return OptionsModel.GetPublicHousingFunding();
            }
        }

        public bool HealthSpendingChecked
        {
            get
            {
                return OptionsModel.HealthChecked;
            }
            set
            {
                OptionsModel.HealthChecked = value;
                OptionsModel.updateFunding();
                OnPropertyChange("DefenseFunding");
                OnPropertyChange("MedicaidFunding");
                OnPropertyChange("WelfareFunding");
                OnPropertyChange("VeteransFunding");
                OnPropertyChange("FoodStampsFunding");
                OnPropertyChange("EducationFunding");
                OnPropertyChange("PublicHousingFunding");
                OnPropertyChange("HealthFunding");
                OnPropertyChange("ScienceFunding");
                OnPropertyChange("TransportationFunding");
                OnPropertyChange("InternationalFunding");
                OnPropertyChange("EEFunding");
                OnPropertyChange("UnemploymentFunding");
                OnPropertyChange("AgricultureFunding");
            }
        }
        public string HealthFunding
        {
            get
            {
                return OptionsModel.GetHealthFunding();
            }
        }

        public bool ScienceSpendingChecked
        {
            get
            {
                return OptionsModel.ScienceChecked;
            }
            set
            {
                OptionsModel.ScienceChecked = value;
                OptionsModel.updateFunding();
                OnPropertyChange("DefenseFunding");
                OnPropertyChange("MedicaidFunding");
                OnPropertyChange("WelfareFunding");
                OnPropertyChange("VeteransFunding");
                OnPropertyChange("FoodStampsFunding");
                OnPropertyChange("EducationFunding");
                OnPropertyChange("PublicHousingFunding");
                OnPropertyChange("HealthFunding");
                OnPropertyChange("ScienceFunding");
                OnPropertyChange("TransportationFunding");
                OnPropertyChange("InternationalFunding");
                OnPropertyChange("EEFunding");
                OnPropertyChange("UnemploymentFunding");
                OnPropertyChange("AgricultureFunding");
            }
        }
        public string ScienceFunding
        {
            get
            {
                return OptionsModel.GetScienceFunding();
            }
        }

        public bool TransportationSpendingChecked
        {
            get
            {
                return OptionsModel.TransportationChecked;
            }
            set
            {
                OptionsModel.TransportationChecked = value;
                OptionsModel.updateFunding();
                OnPropertyChange("DefenseFunding");
                OnPropertyChange("MedicaidFunding");
                OnPropertyChange("WelfareFunding");
                OnPropertyChange("VeteransFunding");
                OnPropertyChange("FoodStampsFunding");
                OnPropertyChange("EducationFunding");
                OnPropertyChange("PublicHousingFunding");
                OnPropertyChange("HealthFunding");
                OnPropertyChange("ScienceFunding");
                OnPropertyChange("TransportationFunding");
                OnPropertyChange("InternationalFunding");
                OnPropertyChange("EEFunding");
                OnPropertyChange("UnemploymentFunding");
                OnPropertyChange("AgricultureFunding");
            }
        }
        public string TransportationFunding
        {
            get
            {
                return OptionsModel.GetTransportationFunding();
            }
        }

        public bool InternationalSpendingChecked
        {
            get
            {
                return OptionsModel.InternationalChecked;
            }
            set
            {
                OptionsModel.InternationalChecked = value;
                OptionsModel.updateFunding();
                OnPropertyChange("DefenseFunding");
                OnPropertyChange("MedicaidFunding");
                OnPropertyChange("WelfareFunding");
                OnPropertyChange("VeteransFunding");
                OnPropertyChange("FoodStampsFunding");
                OnPropertyChange("EducationFunding");
                OnPropertyChange("PublicHousingFunding");
                OnPropertyChange("HealthFunding");
                OnPropertyChange("ScienceFunding");
                OnPropertyChange("TransportationFunding");
                OnPropertyChange("InternationalFunding");
                OnPropertyChange("EEFunding");
                OnPropertyChange("UnemploymentFunding");
                OnPropertyChange("AgricultureFunding");
            }
        }
        public string InternationalFunding
        {
            get
            {
                return OptionsModel.GetInternationalFunding();
            }
        }

        public bool EESpendingChecked
        {
            get
            {
                return OptionsModel.EnergyAndEnvironmentChecked;
            }
            set
            {
                OptionsModel.EnergyAndEnvironmentChecked = value;
                OptionsModel.updateFunding();
                OnPropertyChange("DefenseFunding");
                OnPropertyChange("MedicaidFunding");
                OnPropertyChange("WelfareFunding");
                OnPropertyChange("VeteransFunding");
                OnPropertyChange("FoodStampsFunding");
                OnPropertyChange("EducationFunding");
                OnPropertyChange("PublicHousingFunding");
                OnPropertyChange("HealthFunding");
                OnPropertyChange("ScienceFunding");
                OnPropertyChange("TransportationFunding");
                OnPropertyChange("InternationalFunding");
                OnPropertyChange("EEFunding");
                OnPropertyChange("UnemploymentFunding");
                OnPropertyChange("AgricultureFunding");
            }
        }
        public string EEFunding
        {
            get
            {
                return OptionsModel.GetEnergyAndEnvironmentFunding();
            }
        }

        public bool UnemploymentSpendingChecked
        {
            get
            {
                return OptionsModel.UnemploymentChecked;
            }
            set
            {
                OptionsModel.UnemploymentChecked = value;
                OptionsModel.updateFunding();
                OnPropertyChange("DefenseFunding");
                OnPropertyChange("MedicaidFunding");
                OnPropertyChange("WelfareFunding");
                OnPropertyChange("VeteransFunding");
                OnPropertyChange("FoodStampsFunding");
                OnPropertyChange("EducationFunding");
                OnPropertyChange("PublicHousingFunding");
                OnPropertyChange("HealthFunding");
                OnPropertyChange("ScienceFunding");
                OnPropertyChange("TransportationFunding");
                OnPropertyChange("InternationalFunding");
                OnPropertyChange("EEFunding");
                OnPropertyChange("UnemploymentFunding");
                OnPropertyChange("AgricultureFunding");
            }
        }
        public string UnemploymentFunding
        {
            get
            {
                return OptionsModel.GetUnemploymentFunding();
            }
        }

        public bool AgricultureSpendingChecked
        {
            get
            {
                return OptionsModel.FoodAndAgricultureChecked;
            }
            set
            {
                OptionsModel.FoodAndAgricultureChecked = value;
                OptionsModel.updateFunding();
                OnPropertyChange("DefenseFunding");
                OnPropertyChange("MedicaidFunding");
                OnPropertyChange("WelfareFunding");
                OnPropertyChange("VeteransFunding");
                OnPropertyChange("FoodStampsFunding");
                OnPropertyChange("EducationFunding");
                OnPropertyChange("PublicHousingFunding");
                OnPropertyChange("HealthFunding");
                OnPropertyChange("ScienceFunding");
                OnPropertyChange("TransportationFunding");
                OnPropertyChange("InternationalFunding");
                OnPropertyChange("EEFunding");
                OnPropertyChange("UnemploymentFunding");
                OnPropertyChange("AgricultureFunding");
            }
        }
        public string AgricultureFunding
        {
            get
            {
                return OptionsModel.GetFoodAndAgricultureFunding();
            }
        }
    }
}
