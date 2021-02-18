using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TaxMeApp.Helpers;

namespace TaxMeApp.viewmodels
{
    public class spacerColumn : ColumnDefinition
    {

        public spacerColumn()
        {
            this.Width = new System.Windows.GridLength(30);
        }
    }

    public class OutputViewModel : MainViewModel
    {
        public ListView customProgramListView { get; set; } = new ListView();
        public ICommand AddProgramBtnCommand { get; set; }

        public OutputViewModel() {
            customProgramSource = new DataTable();
            customProgramSource.Columns.Add(new DataColumn("Num"));
            customProgramSource.Columns.Add(new DataColumn("Alpha"));
            customProgramSource.Rows.Add("1", "A");
            customProgramGrid.ItemsSource = customProgramSource.DefaultView;

            AddProgramBtnCommand = new RelayCommand(o => addProgramButtonClick());
        }

        //public ObservableCollection<object> customProgramSource = new ObservableCollection<object>();
        public DataGrid customProgramGrid { get; set; } = new DataGrid();
        public DataTable customProgramSource { get; set; }


        public void addProgramButtonClick() {

            //customProgramListView.Items.Clear();
            Grid g = new Grid();
            ColumnDefinition colTemplate1 = new ColumnDefinition();
            colTemplate1.Width = new System.Windows.GridLength(30);
            ColumnDefinition colTemplate2 = new ColumnDefinition();
            colTemplate2.Width = new System.Windows.GridLength(200);
            ColumnDefinition colTemplate3 = new ColumnDefinition();
            colTemplate3.Width = new System.Windows.GridLength(200);
            ColumnDefinition colTemplate4 = new ColumnDefinition();
            colTemplate4.Width = new System.Windows.GridLength(100);

            g.ColumnDefinitions.Add(colTemplate1);
            g.ColumnDefinitions.Add(new spacerColumn());
            g.ColumnDefinitions.Add(colTemplate2);
            g.ColumnDefinitions.Add(new spacerColumn());
            g.ColumnDefinitions.Add(colTemplate3);
            g.ColumnDefinitions.Add(new spacerColumn());
            g.ColumnDefinitions.Add(colTemplate4);

            g.RowDefinitions.Add(new RowDefinition());
            g.RowDefinitions.Add(new RowDefinition());

            //Row 1
            TextBlock nameLabel = new TextBlock();
            nameLabel.Text = "Program Name:";
            TextBlock costLabel = new TextBlock();
            costLabel.Text = "Cost:";

            //Row 2
            CheckBox c1 = new CheckBox();
            TextBox pName1 = new TextBox();
            TextBox cost1 = new TextBox();
            TextBlock funding1 = new TextBlock();
            funding1.Text = "0% Funded";

            g.Children.Add(nameLabel);
            g.Children.Add(costLabel);
            g.Children.Add(c1);
            g.Children.Add(pName1);
            g.Children.Add(cost1);
            g.Children.Add(funding1);

            Grid.SetRow(nameLabel, 0);
            Grid.SetColumn(nameLabel, 2);
            Grid.SetRow(costLabel, 0);
            Grid.SetColumn(costLabel, 4);

            Grid.SetRow(c1, 1);
            Grid.SetColumn(c1, 0);
            Grid.SetRow(pName1, 1);
            Grid.SetColumn(pName1, 2);
            Grid.SetRow(cost1, 1);
            Grid.SetColumn(cost1, 4);
            Grid.SetRow(funding1, 1);
            Grid.SetColumn(funding1, 6);

            customProgramListView.Items.Add(g);

            OnPropertyChange("customProgramListView");
        }

        public void Update()
        {
            OptionsModel.revenue = DataModel.TotalRevenueNew;

            OnPropertyChange("NumPovertyPopOutput");
            OnPropertyChange("NumMaxPopOutput");
            OnPropertyChange("TotalRevenueOldOutput");
            OnPropertyChange("TotalRevenueNewOutput");
            OnPropertyChange("RevenueDifferenceOutput");
            OnPropertyChange("UBICost");


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
            
            OnPropertyChange("SandersCollegeFunding");
            OnPropertyChange("SandersMedicaidFunding");

            OnPropertyChange("YangUbiFunding");
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

        public string UBICost
        {
            get
            {
                return Formatter.Format(DataModel.TotalUBICost);
            }
        }



        public bool DefenseSpendingChecked {
            get {
                return OptionsModel.DefenseChecked;    
            }
            set {
                OptionsModel.DefenseChecked = value;

                this.Update();
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
                this.Update();
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
                this.Update();
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
                this.Update();
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
                this.Update();
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
                this.Update();
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
                this.Update();
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
                this.Update();
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
                this.Update();
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
                this.Update();
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
                this.Update();
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
                this.Update();
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
                this.Update();
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
                this.Update();
            }
        }
        public string AgricultureFunding
        {
            get
            {
                return OptionsModel.GetFoodAndAgricultureFunding();
            }
        }

        public bool SandersCollegeSpendingChecked
        {
            get
            {
                return OptionsModel.SandersCollegeChecked;
            }
            set
            {
                OptionsModel.SandersCollegeChecked = value;
                this.Update();
            }
        }
        public string SandersCollegeFunding
        {
            get
            {
                return OptionsModel.GetSandersCollegeFunding();
            }
        }

        public bool SandersMedicaidSpendingChecked
        {
            get
            {
                return OptionsModel.SandersMedicaidChecked;
            }
            set
            {
                OptionsModel.SandersMedicaidChecked = value;
                this.Update();
            }
        }
        public string SandersMedicaidFunding
        {
            get
            {
                return OptionsModel.GetSandersMedicaidFunding();
            }
        }

        public bool YangUbiSpendingChecked
        {
            get
            {
                return OptionsModel.YangUbiChecked;
            }
            set
            {
                OptionsModel.YangUbiChecked = value;
                this.Update();
            }
        }
        public string YangUbiFunding
        {
            get
            {
                return OptionsModel.GetYangUbiFunding();
            }
        }

        public bool YangRemoveChecked 
        {
            get 
            {
                return OptionsModel.YangRemoveChecked;            
            }
            set
            {
                OptionsModel.YangRemoveChecked = value;
                DataVM.NewDataRecalcuation();
                this.Update();
            }
        }
    }
}
