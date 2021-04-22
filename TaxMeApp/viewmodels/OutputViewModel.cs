using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TaxMeApp.Helpers;
using TaxMeApp.models;

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

        public OutputViewModel()
        {
            AddProgramBtnCommand = new RelayCommand(o => addProgramButtonClick());
        }

        public BudgetYearModel bym = new BudgetYearModel();
        public void updateBYM()
        {

            if (BudgetDataModel is null || BudgetDataModel.YearData is null || ControlVM is null)
            {
                return;
            }

            for (int i = 0; i < BudgetDataModel.YearData.Count; i++)
            {
                if (BudgetDataModel.YearData[i].Year == ControlVM.SelectedYear)
                {
                    bym = BudgetDataModel.YearData[i];
                    break;
                }
            }
        }

        public string OldBudget
        {
            get
            {
                string ans = "$";
                for (int i = 0; i < BudgetDataModel.YearData.Count; i++)
                {
                    if (BudgetDataModel.YearData[i].Year == ControlVM.SelectedYear)
                    {
                        ans += BudgetDataModel.YearData[i].TotalBudget + " Trillion";
                        break;
                    }
                }
                return ans;
            }
        }
        public string OldBudgetPercent
        {
            get
            {
                string ans = "";
                for (int i = 0; i < BudgetDataModel.YearData.Count; i++)
                {
                    if (BudgetDataModel.YearData[i].Year == ControlVM.SelectedYear)
                    {
                        ans += BudgetDataModel.YearData[i].BudgetPercent.ToString("#,##0.##") + " %";
                        break;
                    }
                }
                return ans;
            }
        }
        public string OldDeficit
        {
            get
            {
                string ans = "$";
                for (int i = 0; i < BudgetDataModel.YearData.Count; i++)
                {
                    if (BudgetDataModel.YearData[i].Year == ControlVM.SelectedYear)
                    {
                        double deficit = BudgetDataModel.YearData[i].Deficit * 1000;
                        ans += Formatter.Format(deficit * Math.Pow(10, 9));
                        break;
                    }
                }
                return ans;
            }
        }
        public string OldDeficitPercent
        {
            get
            {
                string ans = "";
                for (int i = 0; i < BudgetDataModel.YearData.Count; i++)
                {
                    if (BudgetDataModel.YearData[i].Year == ControlVM.SelectedYear)
                    {
                        ans += BudgetDataModel.YearData[i].DeficitPercent.ToString("#,##0.##") + " %";
                        break;
                    }
                }
                return ans;
            }
        }
        public string NewBudgetPercent
        {
            get
            {
                string ans = "";
                for (int i = 0; i < BudgetDataModel.YearData.Count; i++)
                {
                    if (BudgetDataModel.YearData[i].Year == ControlVM.SelectedYear)
                    {
                        double p = 0;
                        double GDP = BudgetDataModel.YearData[i].GDP * (Math.Pow(10, 12));
                        p = OptionsModel.GetTotalBudget() / GDP * 100;
                        ans += p.ToString("#,##0.##") + " %";
                        break;
                    }
                }
                return ans;
            }
        }
        public string NewDeficitPercent
        {
            get
            {
                string ans = "";
                for (int i = 0; i < BudgetDataModel.YearData.Count; i++)
                {
                    if (BudgetDataModel.YearData[i].Year == ControlVM.SelectedYear)
                    {
                        double p = 0;
                        double GDP = BudgetDataModel.YearData[i].GDP * (Math.Pow(10, 12));
                        double tb = OptionsModel.GetTotalBudget();
                        double difference = DataModel.TotalRevenueNew - tb;
                        p = difference / GDP * 100;
                        ans += p.ToString("#,##0.##") + " %";
                        break;
                    }
                }
                return ans;
            }
        }
        public string TotalDebt
        {
            get
            {
                string ans = "$";
                for (int i = 0; i < BudgetDataModel.YearData.Count; i++)
                {
                    if (BudgetDataModel.YearData[i].Year == ControlVM.SelectedYear)
                    {
                        ans += BudgetDataModel.YearData[i].TotalDebt.ToString("#,##0.##");
                        break;
                    }
                }
                return ans;
            }
        }
        public string OldDebtPercent
        {
            get
            {
                string ans = "";
                for (int i = 0; i < BudgetDataModel.YearData.Count; i++)
                {
                    if (BudgetDataModel.YearData[i].Year == ControlVM.SelectedYear)
                    {
                        double p = 0;
                        double GDP = BudgetDataModel.YearData[i].GDP * (Math.Pow(10, 12));
                        p = BudgetDataModel.YearData[i].TotalDebt / GDP * 100;
                        ans += p.ToString("#,##0.##") + " %";
                        break;
                    }
                }
                return ans;
            }
        }

        public string GDP
        {
            get
            {
                string ans = "$";
                for (int i = 0; i < BudgetDataModel.YearData.Count; i++)
                {
                    if (BudgetDataModel.YearData[i].Year == ControlVM.SelectedYear)
                    {
                        ans += BudgetDataModel.YearData[i].GDP + " Trillion";
                        break;
                    }
                }
                return ans;
            }
        }


        public void addProgramButtonClick()
        {

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
            OptionsModel.listOfCosts.Add((OptionsModel.listOfCosts.Count, false, "", 0.0, 100.0));

            //Set event listeners
            programChecked.Click += ProgramChecked_Click;
            programName.TextChanged += ProgramName_TextChanged;
            programCost.TextChanged += ProgramCost_TextChanged;

            customProgramListView.Items.Add(g);

            OnPropertyChange("customProgramListView");

            OptionsModel.SelectedGovProgram = OptionsModel.GetGovProgramList()[OptionsModel.GetGovProgramList().Count - 1];
            ControlVM.propChange("GovProgramList");
            ControlVM.propChange("SelectedGovProgram");
            ControlVM.propChange("SelectedTargetFunding");
            ControlVM.propChange("SelectedTargetBudget");
            ControlVM.propChange("TargetFundingSlider");

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

        public void UncheckCustomPrograms() {
            for (int i = 0; i < customProgramListView.Items.Count; i++) {
                for (int j = 0; j < (customProgramListView.Items[i] as Grid).Children.Count; j++) {
                    try
                    {
                        ((customProgramListView.Items[i] as Grid).Children[j] as CheckBox).IsChecked = false;
                    }
                    catch (Exception e) { 
                    
                    }
                }
            }
        }

        int customStart = 19;

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
                OptionsModel.listOfCosts[gridNum + customStart] = (gridNum + customStart, data, OptionsModel.listOfCosts[gridNum + customStart].name, OptionsModel.listOfCosts[gridNum + customStart].cost, OptionsModel.listOfCosts[gridNum + customStart].tFunding);

                Update();
                //OptionsModel.updateFunding();
                //((customProgramListView.Items[gridNum] as Grid).Children[5] as TextBlock).Text = (OptionsModel.fundingArray[gridNum + customStart].ToString("0.0") + "% Funded");
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
                OptionsModel.listOfCosts[gridNum + customStart] = (gridNum + customStart, OptionsModel.listOfCosts[gridNum + customStart].ischecked, data, OptionsModel.listOfCosts[gridNum + customStart].cost, OptionsModel.listOfCosts[gridNum + customStart].tFunding);
            }
            ControlVM.propChange("SelectedTargetFunding");
            ControlVM.propChange("GovProgramList");
            ControlVM.propChange("SelectedGovProgram");
        }

        private void ProgramCost_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Find the program that was edited
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

                //Try parsing the text value, and if it's invalid just set it to 0
                string sdata = (sender as TextBox).Text;
                double data = 0;
                try
                {
                    data = Double.Parse(sdata);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }


                OptionsModel.listOfCosts[gridNum + customStart] = (gridNum + customStart, OptionsModel.listOfCosts[gridNum + customStart].ischecked, OptionsModel.listOfCosts[gridNum + customStart].name, data, OptionsModel.listOfCosts[gridNum + customStart].tFunding);

                Update();
                //OptionsModel.updateFunding();
                //((customProgramListView.Items[gridNum] as Grid).Children[5] as TextBlock).Text = (OptionsModel.fundingArray[gridNum + customStart].ToString("0.0") + "% Funded");
            }


            //Print out list of costs for testing:

            //for (int i = 0; i < OptionsModel.listOfCosts.Count; i++) {
            //    Console.WriteLine("i={0}, name={1}, cost={2}", i, OptionsModel.listOfCosts[i].name, OptionsModel.listOfCosts[i].cost);
            //}

            ControlVM.propChange("SelectedTargetBudget");
            ControlVM.propChange("SelectedGovProgram");
        }

        public string DefenseText
        {
            get
            {
                string ans = "Defense Spending ($";
                string cost = Formatter.Format(OptionsModel.listOfCosts[0].cost * (OptionsModel.listOfCosts[0].tFunding / 100));
                ans += cost + ")";
                return ans;
            }
        }
        public string MedicaidText
        {
            get
            {
                string ans = "Medicaid Spending ($";
                string cost = Formatter.Format(OptionsModel.listOfCosts[1].cost * (OptionsModel.listOfCosts[1].tFunding / 100));
                ans += cost + ")";
                return ans;
            }
        }

        public string WelfareText
        {
            get
            {
                string ans = "Welfare Spending ($";
                string cost = Formatter.Format(OptionsModel.listOfCosts[2].cost * (OptionsModel.listOfCosts[2].tFunding / 100));
                ans += cost + ")";
                return ans;
            }
        }

        public string VeteransText
        {
            get
            {
                string ans = "Veterans Spending ($";
                string cost = Formatter.Format(OptionsModel.listOfCosts[3].cost * (OptionsModel.listOfCosts[3].tFunding / 100));
                ans += cost + ")";
                return ans;
            }
        }

        public string FoodStampsText
        {
            get
            {
                string ans = "Food Stamps Spending ($";
                string cost = Formatter.Format(OptionsModel.listOfCosts[4].cost * (OptionsModel.listOfCosts[4].tFunding / 100));
                ans += cost + ")";
                return ans;
            }
        }

        public string EducationText
        {
            get
            {
                string ans = "Education Spending ($";
                string cost = Formatter.Format(OptionsModel.listOfCosts[5].cost * (OptionsModel.listOfCosts[5].tFunding / 100));
                ans += cost + ")";
                return ans;
            }
        }

        public string PublicHousingText
        {
            get
            {
                string ans = "Public Housing Spending ($";
                string cost = Formatter.Format(OptionsModel.listOfCosts[6].cost * (OptionsModel.listOfCosts[6].tFunding / 100));
                ans += cost + ")";
                return ans;
            }
        }

        public string HealthText
        {
            get
            {
                string ans = "Health Spending ($";
                string cost = Formatter.Format(OptionsModel.listOfCosts[7].cost * (OptionsModel.listOfCosts[7].tFunding / 100));
                ans += cost + ")";
                return ans;
            }
        }

        public string ScienceText
        {
            get
            {
                string ans = "Science Spending ($";
                string cost = Formatter.Format(OptionsModel.listOfCosts[8].cost * (OptionsModel.listOfCosts[8].tFunding / 100));
                ans += cost + ")";
                return ans;
            }
        }

        public string TransportationText
        {
            get
            {
                string ans = "Transportation Spending ($";
                string cost = Formatter.Format(OptionsModel.listOfCosts[9].cost * (OptionsModel.listOfCosts[9].tFunding / 100));
                ans += cost + ")";
                return ans;
            }
        }

        public string InternationalAffairsText
        {
            get
            {
                string ans = "International Affairs Spending ($";
                string cost = Formatter.Format(OptionsModel.listOfCosts[10].cost * (OptionsModel.listOfCosts[10].tFunding / 100));
                ans += cost + ")";
                return ans;
            }
        }

        public string EnergyAndEnvironmentText
        {
            get
            {
                string ans = "Energy and Environment Spending ($";
                string cost = Formatter.Format(OptionsModel.listOfCosts[11].cost * (OptionsModel.listOfCosts[11].tFunding / 100));
                ans += cost + ")";
                return ans;
            }
        }

        public string UnemploymentText
        {
            get
            {
                string ans = "Unemployment Spending ($";
                string cost = Formatter.Format(OptionsModel.listOfCosts[12].cost * (OptionsModel.listOfCosts[12].tFunding / 100));
                ans += cost + ")";
                return ans;
            }
        }

        public string FoodAndAgricultureText
        {
            get
            {
                string ans = "Food and Agriculture Spending ($";
                string cost = Formatter.Format(OptionsModel.listOfCosts[13].cost * (OptionsModel.listOfCosts[13].tFunding / 100));
                ans += cost + ")";
                return ans;
            }
        }

        public string SandersCollegeText
        {
            get
            {
                string ans = "College Tuition Plan ($";
                string cost = Formatter.Format(OptionsModel.listOfCosts[14].cost * (OptionsModel.listOfCosts[14].tFunding / 100));
                ans += cost + ")";
                return ans;
            }
        }

        public string SandersMedicaidText
        {
            get
            {
                string ans = "Medicaid For all ($";
                string cost = Formatter.Format(OptionsModel.listOfCosts[15].cost * (OptionsModel.listOfCosts[15].tFunding / 100));
                ans += cost + ")";
                return ans;
            }
        }

        public string YangUbiText
        {
            get
            {
                string ans = "Yang UBI Plan ($";
                string cost = Formatter.Format(OptionsModel.listOfCosts[16].cost * (OptionsModel.listOfCosts[16].tFunding / 100));
                ans += cost + ")";
                return ans;
            }
        }

        public void Update()
        {

            if (OptionsModel is null || DataModel is null || ControlVM is null)
            {
                return;
            }

            updateBYM();
            OptionsModel.UpdateUBI(DataModel.TotalUBICost);
            ControlVM.propChange("SelectedTargetBudget");

            OptionsModel.revenue = DataModel.TotalRevenueNew;

            OnPropertyChange("NumPovertyPopOutput");
            OnPropertyChange("NumMaxPopOutput");
            OnPropertyChange("TotalRevenueOldOutput");
            OnPropertyChange("TotalRevenueNewOutput");
            OnPropertyChange("RevenueDifferenceOutput");
            OnPropertyChange("UBICost");
            OnPropertyChange("MeanMedian");

            OnPropertyChange("DefenseText");
            OnPropertyChange("MedicaidText");
            OnPropertyChange("WelfareText");
            OnPropertyChange("VeteransText");
            OnPropertyChange("FoodStampsText");
            OnPropertyChange("EducationText");
            OnPropertyChange("PublicHousingText");
            OnPropertyChange("HealthText");
            OnPropertyChange("ScienceText");
            OnPropertyChange("TransportationText");
            OnPropertyChange("InternationalAffairsText");
            OnPropertyChange("EnergyAndEnvironmentText");
            OnPropertyChange("UnemploymentText");
            OnPropertyChange("FoodAndAgricultureText");
            OnPropertyChange("SandersCollegeText");
            OnPropertyChange("SandersMedicaidText");
            OnPropertyChange("YangUbiText");

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

            OnPropertyChange("UBIFunding");
            OnPropertyChange("UBIText");


            OnPropertyChange("TotalSelectedBudget");
            OnPropertyChange("LeftOverBudget");

            OnPropertyChange("GDP");
            OnPropertyChange("OldBudget");
            OnPropertyChange("OldDebtPercent");
            OnPropertyChange("OldDeficit");
            OnPropertyChange("OldBudgetPercent");
            OnPropertyChange("NewBudgetPercent");
            OnPropertyChange("OldDeficitPercent");
            OnPropertyChange("NewDeficitPercent");
            OnPropertyChange("TotalDebt");

            OnPropertyChange("PaymentPerYear");
            OnPropertyChange("InterestPerYear");

            OnPropertyChange("DebtReductionFunding");
            OnPropertyChange("DebtReductionText");
            OnPropertyChange("DebtYears");
            OnPropertyChange("TargetDebtPercent");
            OnPropertyChange("YearlyGDPGrowth");

            OnPropertyChange("ProjectedGDP");
            OnPropertyChange("TargetDebt");
            OnPropertyChange("DebtDifference");

            OnPropertyChange("TotalInterestPayments");

            OnPropertyChange("OldTaxRate");
            OnPropertyChange("OldAfterTaxIncome");
            OnPropertyChange("NewTaxRate");
            OnPropertyChange("NewAfterTaxIncome");

            ControlVM.update();

            for (int i = 0; i < customProgramListView.Items.Count; i++)
            {
                //OptionsModel.updateFunding();
                ((customProgramListView.Items[i] as Grid).Children[5] as TextBlock).Text = (OptionsModel.fundingArray[i + customStart].ToString("0.0") + "% Funded");
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




        public string UBICost
        {
            get
            {
                return Formatter.Format(DataModel.TotalUBICost * (OptionsModel.listOfCosts[customStart].tFunding / 100));
            }
        }



        public bool DefenseSpendingChecked
        {
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
        public string DefenseFunding
        {
            get
            {
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

                // If user has checked this box
                if (value)
                {

                    // if regular UBI is also checked
                    if (UBIChecked)
                    {
                        UBIChecked = false;
                        OnPropertyChange("UBIChecked");
                    }

                    // if medicaid is also checked
                    if (MedicaidSpendingChecked)
                    {
                        MedicaidSpendingChecked = false;
                        OnPropertyChange("MedicaidSpendingChecked");
                    }

                    // if welfare is also checked
                    if (WelfareSpendingChecked)
                    {
                        WelfareSpendingChecked = false;
                        OnPropertyChange("WelfareSpendingChecked");
                    }

                    // if foodstamps is also checked
                    if (FoodStampsSpendingChecked)
                    {
                        FoodStampsSpendingChecked = false;
                        OnPropertyChange("FoodStampsSpendingChecked");
                    }

                    // if unemployment is also checked
                    if (UnemploymentSpendingChecked)
                    {
                        UnemploymentSpendingChecked = false;
                        OnPropertyChange("UnemploymentSpendingChecked");
                    }

                }

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

        public bool UBIChecked
        {
            get
            {
                return OptionsModel.UBIChecked;
            }
            set
            {
                OptionsModel.UBIChecked = value;

                // If user has checked this box
                if (value)
                {

                    // if YangUBI is also checked
                    if (YangUbiSpendingChecked)
                    {
                        YangUbiSpendingChecked = false;
                        OnPropertyChange("YangUbiSpendingChecked");
                    }

                    // if medicaid is also checked
                    if (MedicaidSpendingChecked)
                    {
                        MedicaidSpendingChecked = false;
                        OnPropertyChange("MedicaidSpendingChecked");
                    }

                    // if welfare is also checked
                    if (WelfareSpendingChecked)
                    {
                        WelfareSpendingChecked = false;
                        OnPropertyChange("WelfareSpendingChecked");
                    }

                    // if foodstamps is also checked
                    if (FoodStampsSpendingChecked)
                    {
                        FoodStampsSpendingChecked = false;
                        OnPropertyChange("FoodStampsSpendingChecked");
                    }

                    // if unemployment is also checked
                    if (UnemploymentSpendingChecked)
                    {
                        UnemploymentSpendingChecked = false;
                        OnPropertyChange("UnemploymentSpendingChecked");
                    }

                }

                this.Update();
            }
        }
        public string UBIFunding
        {
            get
            {
                return OptionsModel.GetUBIFunding();
            }
        }

        public string UBIText
        {
            get
            {
                return OptionsModel.GetUBIText();
            }
        }

        public bool DebtReductionChecked
        {
            get
            {
                return OptionsModel.DebtReductionChecked;
            }
            set
            {

                OptionsModel.DebtReductionChecked = value;

                this.Update();
            }
        }

        public string TotalSelectedBudget
        {
            get
            {
                string ans = "";
                double tb = OptionsModel.GetTotalBudget();
                if (tb >= 1000000000000)
                {
                    tb = tb / 1000000000000;
                    ans = "$" + tb.ToString("0.###") + " Trillion";
                }
                else if (tb >= 1000000000)
                {
                    tb = tb / 1000000000;
                    ans = "$" + tb.ToString("0.###") + " Billion";
                }
                else if (tb >= 1000000)
                {
                    tb = tb / 1000000;
                    ans = "$" + tb.ToString("0.###") + " Million";
                }
                else if (tb >= 1000)
                {
                    tb = tb / 1000;
                    ans = "$" + tb.ToString("0.###") + " Thousand";
                }
                else
                {
                    ans = "$" + tb.ToString();
                }

                return ans;
            }
        }

        public string LeftOverBudget
        {
            get
            {
                string ans = "";
                double tb = OptionsModel.GetTotalBudget();
                double difference = DataModel.TotalRevenueNew - tb;
                double ab = Math.Abs(difference);

                if (ab >= 1000000000000)
                {
                    difference = difference / 1000000000000;
                    ans = "$" + difference.ToString("0.###") + " Trillion";
                }
                else if (ab >= 1000000000)
                {
                    difference = difference / 1000000000;
                    ans = "$" + difference.ToString("0.###") + " Billion";
                }
                else if (ab >= 1000000)
                {
                    difference = difference / 1000000;
                    ans = "$" + difference.ToString("0.###") + " Million";
                }
                else if (ab >= 1000)
                {
                    difference = difference / 1000;
                    ans = "$" + difference.ToString("0.###") + " Thousand";
                }
                else
                {
                    ans = "$" + difference.ToString("0.###");
                }



                if (difference > 0)
                {
                    ans += " Surplus";
                }
                else if (difference < 0)
                {
                    ans += " Deficit";
                }

                return ans;
            }
        }

        public DataTable MeanMedian
        {
            get
            {
                DataTable table = new DataTable();
                table.Columns.Add("System", typeof(string));
                table.Columns.Add("Mean", typeof(string));
                table.Columns.Add("Median", typeof(string));
                table.Columns.Add("Difference", typeof(string));

                var preTaxRow = table.NewRow();
                preTaxRow["System"] = "Pre-Tax";
                preTaxRow["Mean"] = $"{DataModel.PreTaxMean:c0}";
                preTaxRow["Median"] = $"{DataModel.PreTaxMedian:c0}";
                preTaxRow["Difference"] = $"{Math.Abs(DataModel.PreTaxMedian - DataModel.PreTaxMean):c0}";
                table.Rows.Add(preTaxRow);

                var postTaxRow = table.NewRow();
                postTaxRow["System"] = "Post-Tax";
                postTaxRow["Mean"] = $"{DataModel.PostTaxMean:c0}";
                postTaxRow["Median"] = $"{DataModel.PostTaxMedian:c0}";
                postTaxRow["Difference"] = $"{Math.Abs(DataModel.PostTaxMedian - DataModel.PostTaxMean):c0}";
                table.Rows.Add(postTaxRow);

                var postTaxUBIRow = table.NewRow();
                postTaxUBIRow["System"] = "Post-Tax (with UBI)";
                postTaxUBIRow["Mean"] = $"{DataModel.PostTaxMeanWithUBI:c0}";
                postTaxUBIRow["Median"] = $"{DataModel.PostTaxMedianWithUBI:c0}";
                postTaxUBIRow["Difference"] = $"{Math.Abs(DataModel.PostTaxMedianWithUBI - DataModel.PostTaxMeanWithUBI):c0}";
                table.Rows.Add(postTaxUBIRow);

                return table;
            }
        }

        public string DebtReductionText
        {
            get
            {
                string ans = "Debt Redution ";
                string debtPaymentString = "";
                double debt = 0;
                double gdp = 0;
                
                for (int i = 0; i < BudgetDataModel.YearData.Count; i++)
                {
                    if (BudgetDataModel.YearData[i].Year == ControlVM.SelectedYear)
                    {
                        debt = BudgetDataModel.YearData[i].TotalDebt;
                        gdp = BudgetDataModel.YearData[i].GDP;
                        break;
                    }
                }

                double fundingTarget = OptionsModel.CalculateYearlyDebtPayment(debt, gdp) * (OptionsModel.listOfCosts[18].tFunding / 100);
                debtPaymentString = Formatter.Format(fundingTarget);
                
                ans += "($" + debtPaymentString + ")";

                return ans;
            }
        }

        public string DebtReductionFunding {
            get 
            {
                return OptionsModel.GetDebtReductionFunding();
            }
        }

        public string TargetDebtPercent
        {
            get
            {
                return OptionsModel.TargetDebtPercent.ToString();
            }
            set
            {
                try
                {
                    OptionsModel.TargetDebtPercent = double.Parse(value, CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    OptionsModel.TargetDebtPercent = 0;
                }
                this.Update();
                OnPropertyChange("PaymentPerYear");
                OnPropertyChange("InterestPerYear");

                OnPropertyChange("DebtReductionFunding");
                OnPropertyChange("DebtReductionText");
                OnPropertyChange("DebtYears");
                OnPropertyChange("TargetDebtPercent");
                OnPropertyChange("YearlyGDPGrowth");

                OnPropertyChange("ProjectedGDP");
                OnPropertyChange("TargetDebt");
                OnPropertyChange("DebtDifference");

                OnPropertyChange("TotalInterestPayments");
            }
        }

        public string DebtYears
        {
            get
            {
                return OptionsModel.DebtYears.ToString();
            }
            set
            {
                try
                {
                    OptionsModel.DebtYears = double.Parse(value, CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    OptionsModel.DebtYears = 0;
                }
                this.Update();
            }
        }

        public string YearlyGDPGrowth
        {
            get
            {
                return OptionsModel.YearlyGDPGrowth.ToString();
            }
            set
            {
                try
                {
                    OptionsModel.YearlyGDPGrowth = double.Parse(value, CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    OptionsModel.YearlyGDPGrowth = 0;
                }
                this.Update();
                OnPropertyChange("PaymentPerYear");
                OnPropertyChange("InterestPerYear");

                OnPropertyChange("DebtReductionFunding");
                OnPropertyChange("DebtReductionText");
                OnPropertyChange("DebtYears");
                OnPropertyChange("TargetDebtPercent");
                OnPropertyChange("YearlyGDPGrowth");

                OnPropertyChange("ProjectedGDP");
                OnPropertyChange("TargetDebt");
                OnPropertyChange("DebtDifference");

                OnPropertyChange("TotalInterestPayments");
            }
        }

        public string AnnualDebtInterest
        {
            get
            {
                return OptionsModel.AnnualDebtInterest.ToString();
            }
            set
            {
                try
                {
                    OptionsModel.AnnualDebtInterest = double.Parse(value, CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    OptionsModel.AnnualDebtInterest = 0;
                }
                this.Update();
                OnPropertyChange("PaymentPerYear");
                OnPropertyChange("InterestPerYear");

                OnPropertyChange("DebtReductionFunding");
                OnPropertyChange("DebtReductionText");
                OnPropertyChange("DebtYears");
                OnPropertyChange("TargetDebtPercent");
                OnPropertyChange("YearlyGDPGrowth");

                OnPropertyChange("ProjectedGDP");
                OnPropertyChange("TargetDebt");
                OnPropertyChange("DebtDifference");

                OnPropertyChange("TotalInterestPayments");
            }
        }

        public string ProjectedGDP
        {
            get
            {
                return "$" + Formatter.Format(OptionsModel.PGDP);
            }
        }
        public string FormattedTotalDebt
        {
            get
            {
                string ans = "$";
                for (int i = 0; i < BudgetDataModel.YearData.Count; i++)
                {
                    if (BudgetDataModel.YearData[i].Year == ControlVM.SelectedYear)
                    {
                        ans += Formatter.Format(BudgetDataModel.YearData[i].TotalDebt);
                        break;
                    }
                }
                return ans;
            }
        }
        public string TargetDebt
        {
            get
            {
                return "$" + Formatter.Format(OptionsModel.TargetDebt);
            }
        }
        public string DebtDifference
        {
            get
            {
                return "$" + Formatter.Format(OptionsModel.DebtDifference);
            }
        }

        public string InterestPerYear
        {
            get
            {
                return "$" + Formatter.Format(OptionsModel.InterestPerYear);
            }
        }
        public string TotalInterestPayments
        {
            get
            {
                return "$" + Formatter.Format(OptionsModel.TotalInterestPayments);
            }
        }
        public string PaymentPerYear
        {
            get
            {
                return "$" + Formatter.Format(OptionsModel.PaymentPerYear);
            }
        }

        public double yi = 0;
        public string YearlyIncome {
            get {
                return yi.ToString("#,##0.##");
            } 
            set {
                double val = 0;
                try
                {
                    val = double.Parse(value.ToString());
                }
                catch (Exception e) { 
                
                }

                yi = val;
            
                
                OnPropertyChange("OldTaxRate");
                OnPropertyChange("OldAfterTaxIncome");
                OnPropertyChange("NewTaxRate");
                OnPropertyChange("NewAfterTaxIncome");
            }
        }

        public double otr;
        public string OldTaxRate {
            get {
                string ans = "0 %";

                if (yi == 0)
                {
                    otr = DataModel.OldTaxPctByBracket[0];
                }
                else if (yi > 0 && yi < 5000)
                {
                    otr = DataModel.OldTaxPctByBracket[1];
                }
                else if (yi >= 5000 && yi < 10000)
                {
                    otr = DataModel.OldTaxPctByBracket[2];
                }
                else if (yi >= 10000 && yi < 15000)
                {
                    otr = DataModel.OldTaxPctByBracket[3];
                }
                else if (yi >= 15000 && yi < 20000)
                {
                    otr = DataModel.OldTaxPctByBracket[4];
                }
                else if (yi >= 20000 && yi < 25000)
                {
                    otr = DataModel.OldTaxPctByBracket[5];
                }
                else if (yi >= 25000 && yi < 30000)
                {
                    otr = DataModel.OldTaxPctByBracket[6];
                }
                else if (yi >= 30000 && yi < 40000)
                {
                    otr = DataModel.OldTaxPctByBracket[7];
                }
                else if (yi >= 40000 && yi < 50000)
                {
                    otr = DataModel.OldTaxPctByBracket[8];
                }
                else if (yi >= 50000 && yi < 75000)
                {
                    otr = DataModel.OldTaxPctByBracket[9];
                }
                else if (yi >= 75000 && yi < 100000)
                {
                    otr = DataModel.OldTaxPctByBracket[10];
                }
                else if (yi >= 100000 && yi < 200000)
                {
                    otr = DataModel.OldTaxPctByBracket[11];
                }
                else if (yi >= 200000 && yi < 500000)
                {
                    otr = DataModel.OldTaxPctByBracket[12];
                }
                else if (yi >= 500000 && yi < 1000000)
                {
                    otr = DataModel.OldTaxPctByBracket[13];
                }
                else if (yi >= 1000000 && yi < 1500000)
                {
                    otr = DataModel.OldTaxPctByBracket[14];
                }
                else if (yi >= 1500000 && yi < 2000000)
                {
                    otr = DataModel.OldTaxPctByBracket[15];
                }
                else if (yi >= 2000000 && yi < 5000000)
                {
                    otr = DataModel.OldTaxPctByBracket[16];
                }
                else if (yi >= 5000000 && yi < 10000000)
                {
                    otr = DataModel.OldTaxPctByBracket[17];
                }
                else if (yi >= 10000000)
                {
                    otr = DataModel.OldTaxPctByBracket[18];
                }

                ans = otr + " %";
                return ans;
            }
        }

        public string OldAfterTaxIncome { 
            get {
                return "$ " + (yi * (1 - (otr / 100))).ToString("#,##0");    
            } 
        }

        public double ntr;
        public string NewTaxRate
        {
            get
            {
                string ans = "0 %";

                if (yi == 0)
                {
                    ntr = DataModel.NewTaxPctByBracket[0];
                }
                else if (yi > 0 && yi < 5000)
                {
                    ntr = DataModel.NewTaxPctByBracket[1];
                }
                else if (yi >= 5000 && yi < 10000)
                {
                    ntr = DataModel.NewTaxPctByBracket[2];
                }
                else if (yi >= 10000 && yi < 15000)
                {
                    ntr = DataModel.NewTaxPctByBracket[3];
                }
                else if (yi >= 15000 && yi < 20000)
                {
                    ntr = DataModel.NewTaxPctByBracket[4];
                }
                else if (yi >= 20000 && yi < 25000)
                {
                    ntr = DataModel.NewTaxPctByBracket[5];
                }
                else if (yi >= 25000 && yi < 30000)
                {
                    ntr = DataModel.NewTaxPctByBracket[6];
                }
                else if (yi >= 30000 && yi < 40000)
                {
                    ntr = DataModel.NewTaxPctByBracket[7];
                }
                else if (yi >= 40000 && yi < 50000)
                {
                    ntr = DataModel.NewTaxPctByBracket[8];
                }
                else if (yi >= 50000 && yi < 75000)
                {
                    ntr = DataModel.NewTaxPctByBracket[9];
                }
                else if (yi >= 75000 && yi < 100000)
                {
                    ntr = DataModel.NewTaxPctByBracket[10];
                }
                else if (yi >= 100000 && yi < 200000)
                {
                    ntr = DataModel.NewTaxPctByBracket[11];
                }
                else if (yi >= 200000 && yi < 500000)
                {
                    ntr = DataModel.NewTaxPctByBracket[12];
                }
                else if (yi >= 500000 && yi < 1000000)
                {
                    ntr = DataModel.NewTaxPctByBracket[13];
                }
                else if (yi >= 1000000 && yi < 1500000)
                {
                    ntr = DataModel.NewTaxPctByBracket[14];
                }
                else if (yi >= 1500000 && yi < 2000000)
                {
                    ntr = DataModel.NewTaxPctByBracket[15];
                }
                else if (yi >= 2000000 && yi < 5000000)
                {
                    ntr = DataModel.NewTaxPctByBracket[16];
                }
                else if (yi >= 5000000 && yi < 10000000)
                {
                    ntr = DataModel.NewTaxPctByBracket[17];
                }
                else if (yi >= 10000000)
                {
                    ntr = DataModel.NewTaxPctByBracket[18];
                }

                ans = ntr + " %";
                return ans;
            }
        }

        public string NewAfterTaxIncome
        {
            get
            {
                return "$ " + (yi * (1 - (ntr / 100))).ToString("#,##0");
            }
        }
    }
}
