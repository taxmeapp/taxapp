using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using TaxMeApp.Helpers;
using TaxMeApp.models;
using TaxMeApp.Plans;

namespace TaxMeApp.viewmodels
{
    public class ControlViewModel : MainViewModel
    {


        public ICommand AddTaxPlanBtnCommand { get; set; }
        public ICommand SaveTaxPlanBtnCommand { get; set; }
        public ICommand DeleteTaxPlanBtnCommand { get; set; }
        public ICommand ResetSettingsBtnCommand { get; set; }
        public ICommand ResetTaxRatesBtnCommand { get; set; }
        public ICommand AutoFitSlantTaxBtnCommand { get; set; }
        public ICommand AutoFitFlatTaxBtnCommand { get; set; }
        public ICommand AutoFitBudgetBtnCommand { get; set; }
        public ICommand ResetBudgetBtnCommand { get; set; }


        public ControlViewModel()
        {

            AddTaxPlanBtnCommand = new RelayCommand(o => addTaxPlanButtonClick());
            SaveTaxPlanBtnCommand = new RelayCommand(o => saveTaxPlanButtonClick());
            DeleteTaxPlanBtnCommand = new RelayCommand(o => deleteTaxPlanButtonClick());
            ResetSettingsBtnCommand = new RelayCommand(o => resetSettingsButtonClick());
            ResetTaxRatesBtnCommand = new RelayCommand(o => resetTaxRatesButtonClick());
            AutoFitSlantTaxBtnCommand = new RelayCommand(o => autoFitSlantTaxButtonClick());
            AutoFitFlatTaxBtnCommand = new RelayCommand(o => autoFitFlatTaxButtonClick());
            AutoFitBudgetBtnCommand = new RelayCommand(o => autoFitBudgetButtonClick());
            ResetBudgetBtnCommand = new RelayCommand(o => resetBudgetButtonClick());

        }

        // ControlPanel Init
        public void ControlInit()
        {

            // Automatically select the first item in the list
            SelectedYear = YearsModel.YearList[0];

            //Add Slant Tax to the list and select it when ControlViewModel is Inited
            List<List<double>> slantTaxData = DataVM.CalculateSlantTaxData();
            List<double> slantTaxRates = slantTaxData[0];
            TaxPlansModel.TaxPlans.Add("Slant Tax", new IndividualTaxPlanModel("Slant Tax", new ObservableCollection<double>(slantTaxRates)));
            SelectedTaxPlanName = "Slant Tax";

            TaxPlansModel.TaxPlans.Add("Flat Tax", new IndividualTaxPlanModel("Flat Tax", new ObservableCollection<double>(new double[(int)GraphModel.Labels.Length])));

            PlanLoader.LoadPlans(this);

            //for (int i = 0; i < DataModel.NewTaxPctByBracket.Count; i++) {
            //    Console.WriteLine("Bracket {0}, Value {1}", i, DataModel.NewTaxPctByBracket[i]);
            //}

            //Set selected bracket to first one
            SelectedBracket = GraphModel.Labels[0];
        }

        public Visibility slantVisible
        {
            get
            {
                if (this.SelectedTaxPlanName == "Slant Tax")
                {
                    return Visibility.Visible;
                }
                else {
                    return Visibility.Collapsed;
                }
        }
        }

        /*
                Control Panel interaction: 
                These are all used in dynamic binding and may show "0 references"
        */

        // Get the year list for the dropdown
        public List<int> YearList
        {
            get
            {
                return YearsModel.YearList;
            }
        }

        // When user changes the dropdown selection:
        public int SelectedYear
        {
            get { return YearsModel.SelectedYear; }
            set
            {
                //Trace.WriteLine("Changing selected year to: " + value);

                // Save it in our data model
                YearsModel.SelectedYear = value;

                totalGraphReset();

            }
        }

    

        public List<Tuple<int, string>> GovProgramList
        {
            get
            {
                return OptionsModel.GetGovProgramList();
            }
        }

        public Tuple<int, string> SelectedGovProgram
        {
            get
            {
                return OptionsModel.SelectedGovProgram;
            }
            set
            {
                OptionsModel.SelectedGovProgram = value;
                OnPropertyChange("SelectedGovProgram");
                OnPropertyChange("SelectedTargetFunding");
                OnPropertyChange("TargetFundingSlider");
                OnPropertyChange("SelectedTargetBudget");
            }
        }

        public double SelectedTargetFunding
        {
            get
            {
                double ans = OptionsModel.GetSelectedTargetFunding(SelectedGovProgram.Item1);
                if (OptionsModel.GetGovProgramList().IndexOf(SelectedGovProgram) < 0){
                    SelectedGovProgram = OptionsModel.GetGovProgramList()[GovProgramList.Count-1];
                    OnPropertyChange("SelectedGovProgram");
                    ans = OptionsModel.GetSelectedTargetFunding(GovProgramList.IndexOf(SelectedGovProgram));
                }

                return ans;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > 100)
                {
                    value = 100;
                }
                TargetFundingSlider = value;
            }
        }

        public string SelectedTargetBudget
        {
            get
            {
                return OptionsModel.GetSelectedTargetBudget(SelectedGovProgram.Item1);
            }
        }

        public double TargetFundingSlider {
            get {
                return OptionsModel.GetSelectedTargetFunding(SelectedGovProgram.Item1);
            }
            set {
                //int priority, bool ischecked, string name, double cost, double tFunding)
                int priority = SelectedGovProgram.Item1;
                bool isChecked = OptionsModel.listOfCosts[priority].ischecked;
                string name = SelectedGovProgram.Item2;
                double cost = OptionsModel.listOfCosts[priority].cost;

                OptionsModel.listOfCosts[SelectedGovProgram.Item1] = (priority, isChecked, name, cost, value);
                OnPropertyChange("SelectedTargetFunding");
                OnPropertyChange("SelectedTargetBudget");
                OutputVM.Update();
            }
        }

        public List<string> BracketList
        {
            get
            {
                return new List<string>(GraphModel.Labels);
            }
        }
        public string SelectedBracket
        {
            get
            {
                return GraphModel.SelectedBracket;
            }
            set
            {
                GraphModel.SelectedBracket = value;
                OnPropertyChange("SelectedTaxRate");
                OnPropertyChange("TaxRateSlider");
            }
        }

        public double SelectedTaxRate
        {
            get
            {
                if (SelectedTaxPlanName != null)
                {

                    TaxPlansModel.TaxPlans.TryGetValue(SelectedTaxPlanName, out IndividualTaxPlanModel selectedTaxPlan);
                    return selectedTaxPlan.TaxRates[BracketList.IndexOf(SelectedBracket)];
                }
                return 0;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > 100)
                {
                    value = 100;
                }
                TaxRateSlider = value;
            }
        }

        public double TaxRateSlider
        {
            get
            {
                if (SelectedTaxPlanName != null)
                {
                    TaxPlansModel.TaxPlans.TryGetValue(SelectedTaxPlanName, out IndividualTaxPlanModel selectedTaxPlan);
                    //Console.WriteLine("Selected Bracket = {0}, Tax Rate = {1}", SelectedBracket, selectedTaxPlan.TaxRates[BracketList.IndexOf(SelectedBracket)]);
                    return selectedTaxPlan.TaxRates[BracketList.IndexOf(SelectedBracket)];
                }
                return 0;
            }
            set
            {
                if (SelectedTaxPlanName != null)
                {
                    TaxPlansModel.TaxPlans.TryGetValue(SelectedTaxPlanName, out IndividualTaxPlanModel selectedTaxPlan);
                    double difference = value - selectedTaxPlan.TaxRates[BracketList.IndexOf(SelectedBracket)];

                    if (LockTaxRates)
                    {
                        //Edit rates around the the current bracket by a proportional amount
                        //Midpoint is the selected bracket
                        //Go from midpoint - slider to midpoint + slider (Ex. if the selected bracket = #5, and slider = 2 go from 3 to 7 and edit brackets
                        int midpoint = BracketList.IndexOf(SelectedBracket);
                        for (int i = midpoint - LockNumberSlider; i < midpoint + LockNumberSlider; i++)
                        {
                            if (i >= 0 && i < selectedTaxPlan.TaxRates.Count)
                            {
                                if (selectedTaxPlan.TaxRates[i] + difference > 100)
                                {
                                    selectedTaxPlan.TaxRates[i] = 100;
                                    DataModel.NewTaxPctByBracket[i] = 100;
                                }
                                else if (selectedTaxPlan.TaxRates[i] + difference < 0)
                                {
                                    selectedTaxPlan.TaxRates[i] = 0;
                                    DataModel.NewTaxPctByBracket[i] = 0;
                                }
                                else
                                {
                                    selectedTaxPlan.TaxRates[i] += difference;
                                    DataModel.NewTaxPctByBracket[i] += difference;
                                }
                            }
                        }

                    }
                    else
                    {
                        selectedTaxPlan.TaxRates[BracketList.IndexOf(SelectedBracket)] = value;
                        DataModel.NewTaxPctByBracket[BracketList.IndexOf(SelectedBracket)] = value;
                    }

                    DataModel.NewRevenueByBracket = DataVM.CalculateNewRevenues(selectedTaxPlan.TaxRates);
                    OnPropertyChange("SelectedTaxRate");
                    OutputVM.Update();
                    customGraphReset();
                }
            }
        }

        public bool DontAdjustBracketCount { get; set; } = false;

        public bool BalancePovertyWithMax
        {
            get
            {
                return OptionsModel.BalancePovertyWithMax;
            }
            set
            {
                OptionsModel.BalancePovertyWithMax = value;
                if (value)
                {
                    BalanceMaxWithPoverty = !value;
                    OnPropertyChange("BalanceMaxWithPoverty");
                }

                OnPropertyChange("BalancePovertyWithMax");
            }
        }

        public bool BalanceMaxWithPoverty
        {
            get
            {
                return OptionsModel.BalanceMaxWithPoverty;
            }
            set
            {
                OptionsModel.BalanceMaxWithPoverty = value;

                if (value)
                {
                    BalancePovertyWithMax = !value;
                    OnPropertyChange("BalancePovertyWithMax");
                }

                OnPropertyChange("BalanceMaxWithPoverty");
            }
        }

        private bool locked = false;
        public bool LockTaxRates
        {
            get
            {
                return locked;
            }
            set
            {
                locked = value;
                OnPropertyChange("LockTaxRates");
            }
        }

        private int editingBrackets = 0;
        public int LockNumberSlider
        {
            get
            {
                return editingBrackets;
            }
            set
            {
                editingBrackets = value;
                OnPropertyChange("LockNumberSlider");
            }
        }


        public ObservableCollection<string> TaxPlansList
        {
            get
            {
                return TaxPlansModel.TaxPlanList;
            }
            set
            {
                TaxPlansModel.TaxPlanList = value;
                OnPropertyChange("SelectedTaxRate");
            }
        }

        public bool DeleteTaxPlanBtnEnabled
        {
            get
            {

                if (SelectedTaxPlanName is null || SelectedTaxPlanName.Equals("Slant Tax") || SelectedTaxPlanName.Equals("Flat Tax"))
                {
                    return false;
                }

                return true;
            }
        }

        private int selectedTaxPlanIndex;
        public int SelectedTaxPlanTabIndex
        {
            get
            {
                return selectedTaxPlanIndex;
            }
            set
            {

                selectedTaxPlanIndex = value;
                OnPropertyChange("SelectedTaxPlanTabIndex");
                OnPropertyChange("slantVisible");
            }
        }


        public string SelectedTaxPlanName
        {
            get
            {
                return TaxPlansModel.SelectedTaxPlanName;
            }
            set
            {

                // Null value, do nothing
                if (value is null)
                {
                    return;
                }

                TaxPlansModel.SelectedTaxPlanName = value;
                // Hard-coded plan:
                if (value.Equals("Slant Tax"))
                {

                    MaxTaxRate = (int)OptionsModel.MaxTaxRate;
                    MaxBracketCountSlider = OptionsModel.MaxBracketCount;
                    DataVM.CalculateNewRevenues(TaxPlansModel.SelectedTaxPlan.TaxRates);

                    SelectedTaxPlanTabIndex = (int)TaxPlan.Slant;

                }
                // Hard-coded plan:
                else if (value.Equals("Flat Tax"))
                {
                    SelectedTaxPlanTabIndex = (int)TaxPlan.Flat;
                    MaxTaxRate = 0;
                    MaxBracketCountSlider = 0;
                    FlatTaxSlider = 0;
                }
                else
                {

                    // Else it's a custom plan
                    // If it exists, we want to handle it
                    if (TaxPlansModel.TaxPlans.TryGetValue(value, out IndividualTaxPlanModel selectedTaxPlan))
                    {

                        SelectedTaxPlanTabIndex = (int)TaxPlan.Custom;

                        OptionsModel.MaxTaxRate = MaxTaxRate;
                        OptionsModel.MaxBracketCount = MaxBracketCountSlider;
                        MaxTaxRate = 0;
                        MaxBracketCountSlider = 0;

                        for (int i = 0; i < DataModel.NewTaxPctByBracket.Count; i++)
                        {
                            DataModel.NewTaxPctByBracket[i] = selectedTaxPlan.TaxRates[i];

                        }
                        DataModel.NewRevenueByBracket = DataVM.CalculateNewRevenues(selectedTaxPlan.TaxRates);
                        customGraphReset();
                    }
                    // Otherwise ignore what's going on
                    else
                    {
                        Trace.WriteLine("Name is null or plan doesn't exist");
                    }

                }

                OnPropertyChange("SelectedTaxRate");
                OnPropertyChange("TaxRateSlider");
                OnPropertyChange("MaxBracketCountSlider");
                OnPropertyChange("SelectedTaxPlanName");
                OnPropertyChange("SelectedBracket");
                OnPropertyChange("DeleteTaxPlanBtnEnabled");
                OnPropertyChange("slantVisible");
                OutputVM.Update();

                //if (TaxPlansList.Contains(value))
                //{
                //    customGraphReset();
                //}
            }
        }


        // Slant tax maximum rate, adjustable via slider
        public int MaxTaxRate
        {
            get
            {
                return DataModel.MaxTaxRate;
            }
            set
            {

                if (value < 0)
                {
                    value = 0;
                }
                else if (value > 100)
                {
                    value = 100;
                }

                DataModel.MaxTaxRate = value;

                newDataGraphReset();

                OnPropertyChange("MaxTaxRate");

                List<List<double>> slantTaxData = DataVM.CalculateSlantTaxData();
                List<double> slantTaxRates = slantTaxData[0];
                try
                {
                    TaxPlansModel.TaxPlans.TryGetValue("Slant Tax", out IndividualTaxPlanModel stax);
                    stax.TaxRates = new ObservableCollection<double>(slantTaxRates);
                } catch { }
                OnPropertyChange("TaxRateSlider");
                OnPropertyChange("SelectedTaxRate");
            }
        }

        // Number of brackets at max rate
        public int MaxBracketCount
        {
            get
            {
                return GraphModel.MaxBracketCount;
            }
            set
            {
                OnPropertyChange("MaxBracketCount");
            }
        }

        // Strictly for the slider to interface with - calls for a graph redraw on set
        // Need to rework this logic a bit I think.
        public int MaxBracketCountSlider
        {
            get
            {
                return GraphModel.MaxBracketCount;
            }
            set
            {
                GraphModel.MaxBracketCount = value;

                newDataGraphReset();

                OnPropertyChange("MaxBracketCount");
                OnPropertyChange("MaxBracketCountSlider");

                List<List<double>> slantTaxData = DataVM.CalculateSlantTaxData();
                List<double> slantTaxRates = slantTaxData[0];
                try
                {
                    TaxPlansModel.TaxPlans.TryGetValue("Slant Tax", out IndividualTaxPlanModel stax);
                    stax.TaxRates = new ObservableCollection<double>(slantTaxRates);
                }
                catch { }
                OnPropertyChange("TaxRateSlider");
                OnPropertyChange("SelectedTaxRate");

                if (SlantChangesUBI)
                {
                    MaxUBIBracketCountSlider = MaxBracketCountSlider;
                    MinUBIBracketCountSlider = PovertyLineIndexSlider;
                    OnPropertyChange("MaxUBIBracketCountSlider");
                    OnPropertyChange("MaxUBIBracketCount");
                    OnPropertyChange("MinUBIBracketCountSlider");
                    OnPropertyChange("MinUBIBracketCount");
                }
            }
        }

        // Number of brackets recieving $0 in UBI
        public int MaxUBIBracketCount
        {
            get
            {
                return GraphModel.MaxUBIBracketCount;
            }
            set
            {
                OnPropertyChange("MaxUBIBracketCount");
            }
        }

        //MaxUBI Bracket slider
        public int MaxUBIBracketCountSlider
        {
            get
            {
                return GraphModel.MaxUBIBracketCount;
            }
            set
            {
                GraphModel.MaxUBIBracketCount = value;

                customGraphReset();

                OnPropertyChange("MaxUBIBracketCount");
                OnPropertyChange("MaxUBIBracketCountSlider");


                if (UBIChangesSlant)
                {
                    OnPropertyChange("MaxUBIBracketCount");
                    OnPropertyChange("MaxUBIBracketCountSlider");
                    OnPropertyChange("MinUBIBracketCount");
                    OnPropertyChange("MinUBIBracketCountSlider");

                    MaxBracketCountSlider = MaxUBIBracketCountSlider;
                    PovertyLineIndexSlider = MinUBIBracketCountSlider;
                    
                    OnPropertyChange("MaxBracketCountSlider");
                    OnPropertyChange("MaxBracketCount");
                    OnPropertyChange("PovertyLineIndexSlider");
                    OnPropertyChange("PovertyLineIndex");
                    OnPropertyChange("PovertyLineBrackets");
                }
            }
        }

        // Number of brackets recieving max UBI
        public int MinUBIBracketCount
        {
            get
            {
                return GraphModel.MinUBIBracketCount;
            }
            set
            {
                OnPropertyChange("MinUBIBracketCount");
            }
        }

        //MinUBI Bracket slider
        public int MinUBIBracketCountSlider
        {
            get
            {
                return GraphModel.MinUBIBracketCount;
            }
            set
            {
                GraphModel.MinUBIBracketCount = value;

                customGraphReset();

                OnPropertyChange("MinUBIBracketCount");
                OnPropertyChange("MinUBIBracketCountSlider");

                if (UBIChangesSlant)
                {
                    OnPropertyChange("MaxUBIBracketCount");
                    OnPropertyChange("MaxUBIBracketCountSlider");
                    OnPropertyChange("MinUBIBracketCount");
                    OnPropertyChange("MinUBIBracketCountSlider");

                    PovertyLineIndexSlider = MinUBIBracketCountSlider;
                    MaxBracketCountSlider = MaxUBIBracketCountSlider;

                    OnPropertyChange("MaxBracketCountSlider");
                    OnPropertyChange("MaxBracketCount");
                    OnPropertyChange("PovertyLineIndexSlider");
                    OnPropertyChange("PovertyLineIndex");
                    OnPropertyChange("PovertyLineBrackets");
                }

            }
        }

        // $ Ammount of UBI per month
        public int MaxUBI
        {
            get
            {
                return GraphModel.MaxUBI;
            }
            set
            {
                OnPropertyChange("MaxUBI");
            }
        }

        // UBI ammount slider
        public int MaxUBISlider
        {
            get
            {
                return GraphModel.MaxUBI;
            }
            set
            {
                GraphModel.MaxUBI = value;

                customGraphReset();

                OnPropertyChange("MaxUBI");
                OnPropertyChange("MaxUBISlider");
            }
        }

        // Zero-based (index) number of brackets living at/under poverty line
        public int PovertyLineIndex
        {
            get
            {
                return GraphModel.PovertyLineIndex;
            }
            set
            {
                GraphModel.PovertyLineIndex = value;

                OnPropertyChange("PovertyLineIndex");

            }
        }

        // One-based number of brackets living at/under poverty line (for UI)
        public int PovertyLineBrackets
        {
            get
            {
                return GraphModel.PovertyLineIndex + 1;
            }
        }

        // Updates graph after number of brackets at/under poverty is changed
        public int PovertyLineIndexSlider
        {
            get
            {
                return GraphModel.PovertyLineIndex;
            }
            set
            {
                GraphVM.PovertyLineIndex = value;

                totalGraphReset();

                OnPropertyChange("PovertyLineIndex");

                OnPropertyChange("PovertyLineBrackets");

                OnPropertyChange("PovertyLineIndexSlider");

                OnPropertyChange("MaxBracketCountSlider");


                //UBI sometimes changes when poverty brackets change
                //OnPropertyChange("MaxUBIBracketCount");
                //OnPropertyChange("MaxUBIBracketCountSlider");
                //OnPropertyChange("MaxUBISlider");

                if (SlantChangesUBI) {
                    MaxUBIBracketCountSlider = MaxBracketCountSlider;
                    MinUBIBracketCountSlider = PovertyLineIndexSlider;
                    OnPropertyChange("MaxUBIBracketCountSlider");
                    OnPropertyChange("MaxUBIBracketCount");
                    OnPropertyChange("MinUBIBracketCountSlider");
                    OnPropertyChange("MinUBIBracketCount");
                }
            }
        }

        // Rate that all brackets are taxed at for flat tax plan
        public int FlatTaxRate
        {
            get
            {
                return OptionsModel.FlatTaxRate;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > 100)
                {
                    value = 100;
                }
                FlatTaxSlider = value;
            }
        }

        public int FlatTaxSlider
        {
            get
            {
                return OptionsModel.FlatTaxRate;
            }
            set
            {
                if (SelectedTaxPlanName == "Flat Tax")
                {
                    OptionsModel.FlatTaxRate = value;
                    TaxPlansModel.TaxPlans.TryGetValue("Flat Tax", out IndividualTaxPlanModel selectedTaxPlan);
                    foreach (var bracket in BracketList)
                    {
                        selectedTaxPlan.TaxRates[BracketList.IndexOf(bracket)] = value;
                        DataModel.NewTaxPctByBracket[BracketList.IndexOf(bracket)] = value;
                    }
                    DataModel.NewRevenueByBracket = DataVM.CalculateNewRevenues(selectedTaxPlan.TaxRates);
                    OnPropertyChange("FlatTaxRate");
                    OutputVM.Update();
                    customGraphReset();
                }
            }
        }


        // Checkboxes
        public bool ShowNumberOfReturns
        {
            get
            {
                return GraphModel.ShowNumberOfReturns;
            }
            set
            {
                GraphModel.ShowNumberOfReturns = value;

                displayOnlyGraphReset();

                OnPropertyChange("ShowNumberOfReturns");
            }
        }

        public bool ShowOldRevenue
        {
            get
            {
                return GraphModel.ShowOldRevenue;
            }
            set
            {
                GraphModel.ShowOldRevenue = value;

                displayOnlyGraphReset();

                OnPropertyChange("ShowOldRevenue");
            }
        }

        public bool ShowNewRevenue
        {
            get
            {
                return GraphModel.ShowNewRevenue;
            }
            set
            {
                GraphModel.ShowNewRevenue = value;

                displayOnlyGraphReset();

                OnPropertyChange("ShowNewRevenue");
            }
        }

        public bool ShowOldPercentage
        {
            get
            {
                return GraphModel.ShowOldPercentage;
            }
            set
            {

                GraphModel.ShowOldPercentage = value;

                displayOnlyGraphReset();

                OnPropertyChange("ShowOldPercentage");
            }
        }

        public bool ShowNewPercentage
        {
            get
            {
                return GraphModel.ShowNewPercentage;
            }
            set
            {

                GraphModel.ShowNewPercentage = value;

                displayOnlyGraphReset();

                OnPropertyChange("ShowNewPercentage");
            }
        }

        public bool ShowUBI
        {
            get
            {
                return GraphModel.ShowNewUBI;
            }
            set
            {

                GraphModel.ShowNewUBI = value;

                displayOnlyGraphReset();

                OnPropertyChange("ShowUBI");
            }
        }

        public bool ShowPreTaxMedian
        {
            get
            {
                return GraphModel.ShowPreTaxMedian;
            }
            set
            {
                GraphVM.showPreTaxMedian = value;

                OnPropertyChange("ShowPreTaxMedian");
            }
        }

        public bool ShowPreTaxMean
        {
            get
            {
                return GraphModel.ShowPreTaxMean;
            }
            set
            {
                GraphVM.showPreTaxMean = value;

                OnPropertyChange("ShowPreTaxMean");
            }
        }

        public bool ShowPostTaxMedian
        {
            get
            {
                return GraphModel.ShowPostTaxMedian;
            }
            set
            {
                GraphVM.showPostTaxMedian = value;

                OnPropertyChange("ShowPostTaxMedian");
            }
        }

        public bool ShowPostTaxMean
        {
            get
            {
                return GraphModel.ShowPostTaxMean;
            }
            set
            {
                GraphVM.showPostTaxMean = value;

                OnPropertyChange("ShowPostTaxMean");
            }
        }

        public bool ShowPostTaxMedianUBI
        {
            get
            {
                return GraphModel.ShowPostTaxMedianUBI;
            }
            set
            {
                GraphVM.showPostTaxMedianUBI = value;

                OnPropertyChange("ShowPostTaxMedianUBI");
            }
        }

        public bool ShowPostTaxMeanUBI
        {
            get
            {
                return GraphModel.ShowPostTaxMeanUBI;
            }
            set
            {
                GraphVM.showPostTaxMeanUBI = value;

                OnPropertyChange("ShowPostTaxMeanUBI");
            }
        }

        public bool SlantChangesUBI
        {
            get
            {
                return DataVM.SlantChangesUBI;
            }
            set
            {
                DataVM.SlantChangesUBI = value;

                OnPropertyChange("SlantChangesUBI");

                if (value) {
                    if (UBIChangesSlant) {
                        UBIChangesSlant = false;
                        OnPropertyChange("UBIChangesSlant");
                    }
                    MaxUBIBracketCountSlider = MaxBracketCountSlider;
                    MinUBIBracketCountSlider = PovertyLineIndexSlider;
                    OnPropertyChange("MaxUBIBracketCountSlider");
                    OnPropertyChange("MaxUBIBracketCount");
                    OnPropertyChange("MinUBIBracketCountSlider");
                    OnPropertyChange("MinUBIBracketCount");
                }
            }
        }

        public bool UBIChangesSlant
        {
            get
            {
                return DataVM.UBIChangesSlant;
            }
            set
            {
                DataVM.UBIChangesSlant = value;

                OnPropertyChange("UBIChangesSlant");

                if (value)
                {
                    if (SlantChangesUBI)
                    {
                        SlantChangesUBI = false;
                        OnPropertyChange("SlantChangesUBI");
                    }
                    MaxBracketCountSlider = MaxUBIBracketCountSlider;
                    PovertyLineIndexSlider = MinUBIBracketCountSlider;
                    OnPropertyChange("MaxBracketCountSlider");
                    OnPropertyChange("MaxBracketCount");
                    OnPropertyChange("PovertyLineIndexSlider");
                    OnPropertyChange("PovertyLineIndex");
                    OnPropertyChange("PovertyBracketsCount");
                }
            }
        }

        /*
         
                Button Logic
        */


        //Add Tax Plan opens up a popup box to type in the name of the new plan
        private void addTaxPlanButtonClick()
        {
            //Create a popup box that is 200px x 200px and 100px x 100px from the top left corner of the screen
            Popup createNewTaxPlan = new Popup();
            createNewTaxPlan.Height = 200.0;
            createNewTaxPlan.Width = 200.0;
            createNewTaxPlan.HorizontalOffset = 100;
            createNewTaxPlan.VerticalOffset = 100;

            //Create a stack panel to hold all of the components
            StackPanel content = new StackPanel();
            content.Background = new SolidColorBrush(Colors.White);

            //Create label with instructions
            TextBlock textBlock = new TextBlock();
            textBlock.Text = "Enter Name of New Tax Plan:";
            textBlock.Width = 180;
            content.Children.Add(textBlock);

            System.Windows.Thickness defaultMargin = new System.Windows.Thickness(0, 10, 0, 10);

            //Add Textbox and buttons
            TextBox textInput = new TextBox();
            textInput.Width = 180;
            textInput.Margin = defaultMargin;
            content.Children.Add(textInput);

            Button addButton = new Button();
            addButton.Content = "Add";
            addButton.Click += (sender, EventArgs) => { addButton_Click(sender, EventArgs, createNewTaxPlan, textInput); };
            addButton.Width = 180;
            addButton.Margin = defaultMargin;
            content.Children.Add(addButton);

            Button cancelButton = new Button();
            cancelButton.Content = "Cancel";
            cancelButton.Click += (sender, EventArgs) => { cancelButton_Click(sender, EventArgs, createNewTaxPlan); };
            cancelButton.Width = 180;
            cancelButton.Margin = defaultMargin;
            content.Children.Add(cancelButton);

            createNewTaxPlan.Child = content;
            createNewTaxPlan.IsOpen = true;
        }

        private void addButton_Click(object sender, EventArgs e, Popup p, TextBox tb)
        {
            //Check if the input text is already used
            //If not then add it to the list of tax plans and close the window
            if (!(TaxPlansModel.TaxPlans.ContainsKey(tb.Text)))
            {
                Console.WriteLine("Adding Tax Plan, Name = {0}", tb.Text);

                //Set all of the default values to 0% tax
                ObservableCollection<double> defaultValues = new ObservableCollection<double>(new double[(int)(GraphModel.Labels.Length)]);

                CreateTaxPlan(tb.Text, defaultValues);

                SelectedTaxPlanName = TaxPlansList[0];
                SelectedTaxPlanName = tb.Text;
                SelectedBracket = BracketList[0];

                tb.Text = "";
                p.IsOpen = false;

            }
            //If the name is already being used then clear the text entry and print something out
            else
            {
                Console.WriteLine("\nERROR: The Name: {0} Is Already Taken\n", tb.Text);
                tb.Text = "";
            }
        }

        public void CreateTaxPlan(string name, ObservableCollection<double> taxValues)
        {

            if (TaxPlansModel.TaxPlans.ContainsKey(name))
            {
                return;
            }

            TaxPlansModel.TaxPlans.Add(name, new IndividualTaxPlanModel(name, taxValues));
            TaxPlansList.Add(name);

        }

        //The cancel button just closes the popup window
        private void cancelButton_Click(object sender, EventArgs e, Popup p)
        {
            p.IsOpen = false;
        }

        private void saveTaxPlanButtonClick()
        {

            // print EVERYTHING

            string taxPlanName = SelectedTaxPlanName;

            if (SelectedTaxPlanName != null && SelectedTaxPlanName.Equals("Slant Tax"))
            {
                taxPlanName += " (modified)";
            }

            //Trace.WriteLine(taxPlanName);

            Dictionary<string, object> values = new Dictionary<string, object>();

            values.Add("MaxTaxRate", MaxTaxRate);
            values.Add("MaxBracketCount", MaxBracketCount);
            values.Add("PovertyLineBrackets", PovertyLineBrackets);

            if (TaxPlansModel.TaxPlans.TryGetValue(SelectedTaxPlanName, out IndividualTaxPlanModel selectedTaxPlan))
            { 
                values.Add("TaxRates", selectedTaxPlan.TaxRates);
            }
            else
            {
                //Set all of the default values to 0% tax
                values.Add("TaxRates", new ObservableCollection<double>(new double[(int)(GraphModel.Labels.Length)]));
            }

            /*

            values.Add("ShowNumberOfReturns", ShowNumberOfReturns);
            values.Add("ShowOldRevenue", ShowOldRevenue);
            values.Add("ShowNewRevenue", ShowNewRevenue);
            values.Add("ShowOldPercentage", ShowOldPercentage);
            values.Add("ShowNewPercentage", ShowNewPercentage);
            values.Add("ShowUBI", ShowUBI);
            values.Add("MaxUBIBracketCount", MaxUBIBracketCount);
            values.Add("MinUBIBracketCount", MinUBIBracketCount);
            values.Add("MaxUBI", MaxUBI);

            values.Add("DefenseChecked", OptionsModel.DefenseChecked);
            values.Add("MedicaidChecked", OptionsModel.MedicaidChecked);
            values.Add("WelfareChecked", OptionsModel.WelfareChecked);
            values.Add("VeteransChecked", OptionsModel.VeteransChecked);
            values.Add("FoodStampsChecked", OptionsModel.FoodStampsChecked);
            values.Add("EducationChecked", OptionsModel.EducationChecked);
            values.Add("PublicHousingChecked", OptionsModel.PublicHousingChecked);
            values.Add("HealthChecked", OptionsModel.HealthChecked);
            values.Add("ScienceChecked", OptionsModel.ScienceChecked);
            values.Add("TransportationChecked", OptionsModel.TransportationChecked);
            values.Add("InternationalChecked", OptionsModel.InternationalChecked);
            values.Add("EnergyAndEnvironmentChecked", OptionsModel.EnergyAndEnvironmentChecked);
            values.Add("UnemploymentChecked", OptionsModel.UnemploymentChecked);
            values.Add("FoodAndAgricultureChecked", OptionsModel.FoodAndAgricultureChecked);
            values.Add("SandersCollegeChecked", OptionsModel.SandersCollegeChecked);
            values.Add("SandersMedicaidChecked", OptionsModel.SandersMedicaidChecked);
            values.Add("YangUbiChecked", OptionsModel.YangUbiChecked);
            values.Add("YangRemoveChecked", OptionsModel.YangRemoveChecked);

            */


            PlanSaver.SavePlan(taxPlanName, values);

        }


        //The Delete Tax Plan button deletes the selected tax plan but it can't delete the default slant tax plan
        private void deleteTaxPlanButtonClick()
        {
            if (SelectedTaxPlanName != "Slant Tax")
            {

                PlanSaver.DeletePlan(SelectedTaxPlanName);

                TaxPlansModel.TaxPlans.Remove(SelectedTaxPlanName);
                SelectedTaxPlanName = TaxPlansList[0];

            }
        }

        /*
            Calls to recalculate and regraph based on user input
        */
         
        // Collection of calls to update data, clear graph, regraph
        private void totalGraphReset()
        {

        
            // Update all data

            if (DataVM != null)
            {
                DataVM.TotalRecalculation();
            }
            
            if (GraphVM != null)
            {
                GraphVM.ClearSeries();

                GraphVM.GraphAllChecked();
            }            

        }

        //Graph reset that doesn't recalculate Slant Tax Data
        //Used for manually changing tax rates
        private void customGraphReset()
        {
            if (DataVM != null)
            {
                // Calculate UBI and mean/median only
                DataVM.CustomDataRecalcuation();
            }

            if (GraphVM != null)
            {
                GraphVM.ClearSeries();

                GraphVM.GraphAllChecked();
            }
        }

        private void customGraphReset(List<double> customRates) 
        {
            if (DataVM != null)
            {
                // Calculate UBI and mean/median only
                DataVM.CustomDataRecalcuation();
            }

            if (GraphVM != null)
            {
                GraphVM.ClearSeries();

                GraphVM.GraphAllChecked(customRates);
            }
        }

        // Collections of calls to update only the new data (e.g. max % or # of max brackets changed)
        private void newDataGraphReset()
        {

            if (DataVM != null)
            {
                // Recalculate all the *new* data only
                DataVM.NewDataRecalcuation();
            }
            
            if (GraphVM != null)
            {
                // Clear the graph
                GraphVM.ClearSeries();

                // Graph everything that is checked
                GraphVM.GraphAllChecked();
            }          

        }

        // Colections of calls to update only what is shown on the graph (e.g. checkboxes ticked/unticked)
        private void displayOnlyGraphReset()
        {

            if (GraphVM != null)
            {
                // Clear the graph
                GraphVM.ClearSeries();

                // Graph everything that is checked
                GraphVM.GraphAllChecked();
            }

        }

        //Currently Hardcoded
        //Resets to default options
        private void resetSettingsButtonClick(){
            //Console.WriteLine("Reset Settings Button Clicked");
            ShowNumberOfReturns = true;
            ShowOldRevenue = false;
            ShowNewRevenue = false;
            ShowOldPercentage = false;
            ShowNewPercentage = false;
            ShowUBI = false;
            ShowPreTaxMean = false;
            ShowPreTaxMedian = false;
            ShowPostTaxMean = false;
            ShowPostTaxMedian = false;
            ShowPostTaxMeanUBI = false;
            ShowPostTaxMedianUBI = false;
            MaxBracketCountSlider = 0;
            PovertyLineIndexSlider = -1;
            MaxTaxRate = 0;
            FlatTaxSlider = 0;

            this.resetBudgetButtonClick();

            OutputVM.DefenseSpendingChecked = true;
            OutputVM.MedicaidSpendingChecked = true;
            OutputVM.WelfareSpendingChecked = true;
            OutputVM.VeteransSpendingChecked = true;
            OutputVM.FoodStampsSpendingChecked = true;
            OutputVM.EducationSpendingChecked = true;
            OutputVM.PublicHousingSpendingChecked = true;
            OutputVM.HealthSpendingChecked = true;
            OutputVM.ScienceSpendingChecked = true;
            OutputVM.TransportationSpendingChecked = true;
            OutputVM.InternationalSpendingChecked = true;
            OutputVM.EESpendingChecked = true;
            OutputVM.UnemploymentSpendingChecked = true;
            OutputVM.AgricultureSpendingChecked = true;

            OutputVM.SandersCollegeSpendingChecked = false;
            OutputVM.SandersMedicaidSpendingChecked = false;
            OutputVM.YangUbiSpendingChecked = false;
            OutputVM.YangRemoveChecked = false;
            OutputVM.UBIChecked = false;
            OutputVM.DebtReductionChecked = false;

            OutputVM.propChange("DefenseSpendingChecked");
            OutputVM.propChange("MedicaidSpendingChecked");
            OutputVM.propChange("WelfareSpendingChecked");
            OutputVM.propChange("VeteransSpendingChecked");
            OutputVM.propChange("FoodStampsSpendingChecked");
            OutputVM.propChange("EducationSpendingChecked");
            OutputVM.propChange("PublicHousingSpendingChecked");
            OutputVM.propChange("HealthSpendingChecked");
            OutputVM.propChange("ScienceSpendingChecked");
            OutputVM.propChange("TransportationSpendingChecked");
            OutputVM.propChange("InternationalSpendingChecked");
            OutputVM.propChange("EESpendingChecked");
            OutputVM.propChange("UnemploymentSpendingChecked");
            OutputVM.propChange("AgricultureSpendingChecked");
            OutputVM.propChange("SandersCollegeSpendingChecked");
            OutputVM.propChange("SandersMedicaidSpendingChecked");
            OutputVM.propChange("YangUbiSpendingChecked");
            OutputVM.propChange("YangRemoveChecked");
            OutputVM.propChange("UBIChecked");
            OutputVM.propChange("DebtReductionChecked");

            OutputVM.UncheckCustomPrograms();

            OutputVM.OptionsModel.UncheckCustomPrograms();
            OutputVM.propChange("customProgramListView");

            this.MaxUBI = 1500;
            this.MaxUBIBracketCountSlider = 3;
            this.MinUBIBracketCountSlider = 0;
            OnPropertyChange("MaxUBI");
            OnPropertyChange("MaxUBISlider");
            OnPropertyChange("MaxUBIBracketCountSlider");
            OnPropertyChange("MaxUBIBracketCount");
            OnPropertyChange("MinUBIBracketCountSlider");
            OnPropertyChange("MinUBIBracketCount");


            OutputVM.OptionsModel.TargetDebtPercent = 10;
            OutputVM.OptionsModel.DebtYears = 10;
            OutputVM.OptionsModel.YearlyGDPGrowth = 2.3;
            OutputVM.OptionsModel.AnnualDebtInterest = 3;

            OutputVM.propChange("TargetDebtPercent");
            OutputVM.propChange("DebtYears");
            OutputVM.propChange("YearlyGDPGrowth");
            OutputVM.propChange("AnnualDebtInterest");

            OutputVM.Update();
        }

        private void resetTaxRatesButtonClick() {
            if (SelectedTaxPlanName == "Slant Tax")
            {
                newDataGraphReset();
                TaxPlansModel.TaxPlans.TryGetValue(SelectedTaxPlanName, out IndividualTaxPlanModel selectedTaxPlan);
                for (int i = 0; i < selectedTaxPlan.TaxRates.Count; i++)
                {
                    selectedTaxPlan.TaxRates[i] = DataModel.NewTaxPctByBracket[i];
                }
                OnPropertyChange("TaxRateSlider");
                OnPropertyChange("SelectedTaxRate");
            }
            else if (SelectedTaxPlanName == "Flat Tax")
            {
                FlatTaxSlider = 0;
                OnPropertyChange("TaxRateSlider");
                OnPropertyChange("SelectedTaxRate");
            }
            else {
                TaxPlansModel.TaxPlans.TryGetValue(SelectedTaxPlanName, out IndividualTaxPlanModel selectedTaxPlan);
                for (int i = 0; i < selectedTaxPlan.TaxRates.Count; i++){
                    selectedTaxPlan.TaxRates[i] = 0;
                }
                OnPropertyChange("TaxRateSlider");
                OnPropertyChange("SelectedTaxRate");
                customGraphReset(new List<double>(selectedTaxPlan.TaxRates));
            }
        }

        public void update()
        {
            OnPropertyChange("SelectedTaxRate");
            OnPropertyChange("TaxRateSlider");
            OnPropertyChange("MaxBracketCountSlider");
            OnPropertyChange("SelectedTaxPlanName");
            OnPropertyChange("SelectedBracket");
            OnPropertyChange("TargetFundingSlider");
            OnPropertyChange("SelectedTargetBudget");
            OnPropertyChange("FlatTaxSlider");
            OnPropertyChange("FlatTaxRate");
        }

        public void autoFitSlantTaxButtonClick() {
            double budget = OptionsModel.GetTotalBudget();
            double revenue = 0;

            if (!DontAdjustBracketCount)
            {
                this.MaxBracketCountSlider = 0;
            }
            this.MaxTaxRate = 0;
            revenue = DataModel.TotalRevenueNew;

            while (revenue < budget && MaxTaxRate < 100) {
                if (!DontAdjustBracketCount)
                {
                    for (int i = 0; i < 11; i++)
                    {
                        MaxBracketCountSlider = i;
                        revenue = DataModel.TotalRevenueNew;
                        //Console.WriteLine("TaxRate = {0}, Brackets = {1}, Revenue = {2}, Budget = {3}", MaxTaxRate, MaxBracketCountSlider, revenue, budget);
                        if (revenue >= budget)
                        {
                            break;
                        }
                    }
                }
                else {
                    revenue = DataModel.TotalRevenueNew;
                }

                if (revenue < budget) {
                    this.MaxTaxRate += 1;
                    //revenue = DataModel.TotalRevenueNew;
                }
                if (this.MaxTaxRate >= 100) {
                    break;
                }
            }
            if (this.MaxTaxRate > 100) {
                this.MaxTaxRate = 100;
            }
        }

        public void autoFitFlatTaxButtonClick(){
            double budget = OptionsModel.GetTotalBudget();
            double revenue = 0;

            this.MaxBracketCountSlider = 0;
            this.MaxTaxRate = 0;
            revenue = DataModel.TotalRevenueNew;

            long flatRate = 0;
            ObservableCollection<double> flatRates = new ObservableCollection<double>();

            while (revenue < budget && flatRate < 100) {
                flatRate++;
                flatRates = new ObservableCollection<double>();

                for (int i = 0; i < DataModel.NewTaxPctByBracket.Count; i++) {
                    flatRates.Add(flatRate);
                }

                DataVM.CalculateNewRevenues(flatRates);
                revenue = DataModel.TotalRevenueNew;

            }

            Console.WriteLine("Flat Rate = {0}", flatRate);
            FlatTaxSlider = (int)flatRate;
            this.update();
        }

        public void autoFitBudgetButtonClick(){
            double revenue = DataModel.TotalRevenueNew;
            double flatTFunding = 100.0;

            OptionsModel.setFlatTFunding(flatTFunding);

            double budget = OptionsModel.GetTotalBudget();

            while (revenue < budget && flatTFunding >= 0) {
                OptionsModel.setFlatTFunding(flatTFunding);
                flatTFunding -= 1;
                budget = OptionsModel.GetTotalBudget();
            }
            OutputVM.Update();
            OnPropertyChange("SelectedTargetFunding");
            OnPropertyChange("SelectedTargetBudget");
            OnPropertyChange("TargetFundingSlider");

        }

        public void resetBudgetButtonClick()
        {
            OptionsModel.setAllTFunding(100.0);
            OutputVM.Update();
            OnPropertyChange("SelectedTargetFunding");
            OnPropertyChange("SelectedTargetBudget");
            OnPropertyChange("TargetFundingSlider");
        }
    }

    public enum TaxPlan
    {
        Slant,
        Flat,
        Custom
    }

}