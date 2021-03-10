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
    //Spacer column is just a column definition with a set width
    public class spacerColumn : ColumnDefinition
    {

        public spacerColumn()
        {
            this.Width = new System.Windows.GridLength(30);
        }
    }

    public class OutputViewModel : MainViewModel
    {
        //Custom gov programs are held in a listview and the items are connected to the ouputview
        public ListView customProgramListView { get; set; } = new ListView();
        public ICommand AddProgramBtnCommand { get; set; }

        public OutputViewModel() {
            AddProgramBtnCommand = new RelayCommand(o => addProgramButtonClick());
        }


        public void addProgramButtonClick() {

            //customProgramListView.Items.Clear();
            
            //Create a grid to store checkbox and textboxes
            Grid g = new Grid();
            //Column definitions are used to define the width and spacing of the elements
            ColumnDefinition colTemplate1 = new ColumnDefinition();
            colTemplate1.Width = new System.Windows.GridLength(30);
            ColumnDefinition colTemplate2 = new ColumnDefinition();
            colTemplate2.Width = new System.Windows.GridLength(200);
            ColumnDefinition colTemplate3 = new ColumnDefinition();
            colTemplate3.Width = new System.Windows.GridLength(200);
            ColumnDefinition colTemplate4 = new ColumnDefinition();
            colTemplate4.Width = new System.Windows.GridLength(100);

            //Add definitons
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
            CheckBox programChecked = new CheckBox();
            TextBox programName = new TextBox();
            TextBox programCost = new TextBox();
            TextBlock programFunding = new TextBlock();
            programFunding.Text = "0.0% Funded";

            g.Children.Add(nameLabel);
            g.Children.Add(costLabel);
            g.Children.Add(programChecked);
            g.Children.Add(programName);
            g.Children.Add(programCost);
            g.Children.Add(programFunding);

            Grid.SetRow(nameLabel, 0);
            Grid.SetColumn(nameLabel, 2);
            Grid.SetRow(costLabel, 0);
            Grid.SetColumn(costLabel, 4);

            Grid.SetRow(programChecked, 1);
            Grid.SetColumn(programChecked, 0);
            Grid.SetRow(programName, 1);
            Grid.SetColumn(programName, 2);
            Grid.SetRow(programCost, 1);
            Grid.SetColumn(programCost, 4);
            Grid.SetRow(programFunding, 1);
            Grid.SetColumn(programFunding, 6);

            //Add program to list of costs (Used to calculate funding)
            OptionsModel.listOfCosts.Add((OptionsModel.listOfCosts.Count, false, "", 0.0));

            //Set event listeners
            programChecked.Click += ProgramChecked_Click;
            programName.TextChanged += ProgramName_TextChanged;
            programCost.TextChanged += ProgramCost_TextChanged;

            customProgramListView.Items.Add(g);

            OnPropertyChange("customProgramListView");

            //Testing:

            //Console.WriteLine("\nPrinting out listview contents:");
            //for (int i = 0; i < customProgramListView.Items.Count; i++) {
            //    for(int j = 0; j < (customProgramListView.Items[i] as Grid).Children.Count; j++)
            //    {
            //        Console.WriteLine("Grid {0} child {1}: {2}", i, j, (customProgramListView.Items[i] as Grid).Children[j].ToString());
            //    }
            //}
            //Console.WriteLine("\n");
        }

        private void ProgramChecked_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //Find the gov program that was edited
            int gridNum = -1;
            for (int i = 0; i < customProgramListView.Items.Count; i++)
            {
                if ((customProgramListView.Items[i] as Grid).Children.Contains(sender as CheckBox))
                {
                    gridNum = i;
                    break;
                }
            }

            //Update List of costs with new text value
            if (gridNum != -1)
            {
                bool data = (bool)(sender as CheckBox).IsChecked;
                OptionsModel.listOfCosts[gridNum + 17] = (gridNum + 17, data, OptionsModel.listOfCosts[gridNum + 17].name, OptionsModel.listOfCosts[gridNum + 17].cost);

                Update();
                //OptionsModel.updateFunding();
                //((customProgramListView.Items[gridNum] as Grid).Children[5] as TextBlock).Text = (OptionsModel.fundingArray[gridNum + 17].ToString("0.0") + "% Funded");
            }
        }

        private void ProgramName_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Find the gov program that was edited
            int gridNum = -1;
            for (int i = 0; i < customProgramListView.Items.Count; i++)
            {
                //Console.WriteLine("Checking {0} for textbox", i);
                if ((customProgramListView.Items[i] as Grid).Children.Contains(sender as TextBox))
                {
                    //Console.WriteLine("Textbox Found", i);
                    gridNum = i;
                    break;
                }
            }

            //Update List of costs with new text value
            if (gridNum != -1)
            {
                string data = (sender as TextBox).Text;
                OptionsModel.listOfCosts[gridNum + 17] = (gridNum + 17, OptionsModel.listOfCosts[gridNum + 17].ischecked, data, OptionsModel.listOfCosts[gridNum + 17].cost);
            }
        }

        private void ProgramCost_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Find the program that was edited
            int gridNum = -1;
            for (int i = 0; i < customProgramListView.Items.Count; i++) {
                //Console.WriteLine("Checking {0} for textbox", i);
                if ((customProgramListView.Items[i] as Grid).Children.Contains(sender as TextBox)) {
                    //Console.WriteLine("Textbox Found", i);
                    gridNum = i;
                    break;
                }
            }

            //Update List of costs with new text value
            if (gridNum != -1)
            {

                //Try parsing the text value, and if it's invalid just set it to 0
                string sdata = (sender as TextBox).Text;
                double data = 0;
                try
                {
                    data = Double.Parse(sdata);
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.StackTrace);
                }

     
                OptionsModel.listOfCosts[gridNum + 17] = (gridNum + 17, OptionsModel.listOfCosts[gridNum + 17].ischecked, OptionsModel.listOfCosts[gridNum + 17].name, data);

                Update();
                //OptionsModel.updateFunding();
                //((customProgramListView.Items[gridNum] as Grid).Children[5] as TextBlock).Text = (OptionsModel.fundingArray[gridNum + 17].ToString("0.0") + "% Funded");
            }


            //Print out list of costs for testing:

            //for (int i = 0; i < OptionsModel.listOfCosts.Count; i++) {
            //    Console.WriteLine("i={0}, name={1}, cost={2}", i, OptionsModel.listOfCosts[i].name, OptionsModel.listOfCosts[i].cost);
            //}
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
            OnPropertyChange("PreTaxMeanMedian");
            OnPropertyChange("PostTaxMeanMedian");

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

            for (int i = 0; i < customProgramListView.Items.Count; i++) { 
                //OptionsModel.updateFunding();
                ((customProgramListView.Items[i] as Grid).Children[5] as TextBlock).Text = (OptionsModel.fundingArray[i + 17].ToString("0.0") + "% Funded");
            }
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
            get 
            {
                return OptionsModel.DefenseChecked;    
            }
            set 
            {
                
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

        public string PreTaxMeanMedian
        {
            get
            {
                return $"Pre-tax: Mean: ${DataModel.PreTaxMean:n0}  " +
                    $"|  Median: ${DataModel.PreTaxMedian:n0}  " +
                    $"|  Difference: ${Math.Abs(DataModel.PreTaxMedian-DataModel.PreTaxMean):n0}";
            }
        }

        public string PostTaxMeanMedian
        {
            get
            {
                return $"Post-tax: Mean: ${DataModel.PostTaxMean:n0}  " +
                    $"|  Median: ${DataModel.PostTaxMedian:n0}  " +
                    $"|  Difference: ${Math.Abs(DataModel.PostTaxMedian - DataModel.PostTaxMean):n0}";
            }
        }
    }
}
