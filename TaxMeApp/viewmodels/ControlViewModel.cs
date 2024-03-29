﻿using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using TaxMeApp.dialog;
using TaxMeApp.Helpers;
using TaxMeApp.models;
using TaxMeApp.Plans;

namespace TaxMeApp.viewmodels
{
    public class ControlViewModel : MainViewModel
    {

        //Buttons are connected to ICommands which call a method
        public ICommand NewTaxPlanBtnCommand { get; set; }
        public ICommand SaveTaxPlanBtnCommand { get; set; }
        public ICommand DeleteTaxPlanBtnCommand { get; set; }
        public ICommand ResetSettingsBtnCommand { get; set; }
        public ICommand ResetTaxRatesBtnCommand { get; set; }
        public ICommand AutoFitTaxBtnCommand { get; set; }
        public ICommand AutoFitBudgetBtnCommand { get; set; }
        public ICommand ResetBudgetBtnCommand { get; set; }
        public ICommand ToggleEditModeBtnCommand { get; set; }


        public ControlViewModel()
        {
            //Connect ICommands to methods
            NewTaxPlanBtnCommand = new RelayCommand(o => newTaxPlanButtonClick());
            SaveTaxPlanBtnCommand = new RelayCommand(o => saveTaxPlan());
            DeleteTaxPlanBtnCommand = new RelayCommand(o => deleteTaxPlanButtonClick());
            ResetSettingsBtnCommand = new RelayCommand(o => resetSettingsButtonClick());
            ResetTaxRatesBtnCommand = new RelayCommand(o => resetTaxRatesButtonClick());
            AutoFitTaxBtnCommand = new RelayCommand(o => autoFitTaxButtonClick());
            AutoFitBudgetBtnCommand = new RelayCommand(o => autoFitBudgetButtonClick());
            ResetBudgetBtnCommand = new RelayCommand(o => resetBudgetButtonClick());
            ToggleEditModeBtnCommand = new RelayCommand(o => toggleEditModeButtonClick());

        }

        // ControlPanel Init
        public void ControlInit()
        {

            // Automatically select the first item in the list
            SelectedYear = YearsModel.YearList[0];

            // Add Slant Tax plan to the list and select it when ControlViewModel is initialized
            List<List<double>> slantTaxData = DataVM.CalculateSlantTaxData();
            List<double> slantTaxRates = slantTaxData[0];
            TaxPlansModel.TaxPlans.Add("Slant Tax", new IndividualTaxPlanModel("Slant Tax", new ObservableCollection<double>(slantTaxRates)));
            SelectedTaxPlanName = "Slant Tax";

            // Add Slant Tax Mod plan where slope is 1/4 of sin
            List<List<double>> mod1Data = DataVM.CalculateMod1Data();
            List<double> mod1TaxRates = mod1Data[0];
            TaxPlansModel.TaxPlans.Add("Slant Mod 1", new IndividualTaxPlanModel("Slant Mod 1", new ObservableCollection<double>(mod1TaxRates)));

            // Add Slant Tax Mod plan where slope is 1/4 of ellipse
            List<List<double>> mod2Data = DataVM.CalculateMod2Data();
            List<double> mod2TaxRates = mod2Data[0];
            TaxPlansModel.TaxPlans.Add("Slant Mod 2", new IndividualTaxPlanModel("Slant Mod 2", new ObservableCollection<double>(mod2TaxRates)));

            // Add Slant Tax Mod plan where slope is based on 1/x
            //List<List<double>> mod3Data = DataVM.CalculateMod3Data();
            //List<double> mod3TaxRates = mod3Data[0];
            //TaxPlansModel.TaxPlans.Add("Slant / Mod 3", new IndividualTaxPlanModel("Slant / Mod 3", new ObservableCollection<double>(mod3TaxRates)));

            // Add Flat Tax plan
            TaxPlansModel.TaxPlans.Add("Flat Tax", new IndividualTaxPlanModel("Flat Tax", new ObservableCollection<double>(new double[(int)GraphModel.Labels.Length])));

            // Load saved tax plans
            PlanLoader.LoadPlans(this);

            // Set selected bracket to first one
            SelectedBracket = GraphModel.Labels[0];
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

                    TaxPlansModel.TaxPlans[SelectedTaxPlanName].CustomTaxRates.Clear();
                    for(int i = 0; i < DataModel.NewTaxPctByBracket.Count; i++)
                    {
                        TaxPlansModel.TaxPlans[SelectedTaxPlanName].CustomTaxRates.Add(DataModel.NewTaxPctByBracket[i]);
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

                if (SelectedTaxPlanName is null || SelectedTaxPlanName.Equals("Slant Tax") || SelectedTaxPlanName.Equals("Flat Tax") || SelectedTaxPlanName.Equals("Slant Mod 1") || SelectedTaxPlanName.Equals("Slant Mod 2"))
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
                // Save current tax plan when switching
                if (TaxPlansModel.TaxPlans.ContainsKey(this.SelectedTaxPlanName))
                {
                   
                    TaxPlansModel.TaxPlans[SelectedTaxPlanName].CustomTaxRates.Clear();
                    for (int i = 0; i < DataModel.NewTaxPctByBracket.Count; i++)
                    {
                        TaxPlansModel.TaxPlans[SelectedTaxPlanName].CustomTaxRates.Add(DataModel.NewTaxPctByBracket[i]);
                    }



                    for (int i = 0; i < DataModel.NewTaxPctByBracket.Count; i++)
                    {
                        TaxPlansModel.TaxPlans[SelectedTaxPlanName].TaxRates[i] = DataModel.NewTaxPctByBracket[i];
                    }
                    TaxPlansModel.TaxPlans[SelectedTaxPlanName].MaxTaxRate = this.MaxTaxRate;
                    TaxPlansModel.TaxPlans[SelectedTaxPlanName].MaxBracketCount = this.MaxBracketCount;
                    TaxPlansModel.TaxPlans[SelectedTaxPlanName].PovertyLineIndex = this.PovertyLineIndex;
                    TaxPlansModel.TaxPlans[SelectedTaxPlanName].FlatTaxRate = this.FlatTaxRate;
                    TaxPlansModel.TaxPlans[SelectedTaxPlanName].BalanceMaxWithPoverty = this.BalanceMaxWithPoverty;
                    TaxPlansModel.TaxPlans[SelectedTaxPlanName].BalancePovertyWithMax = this.BalancePovertyWithMax;
                }

                // Null value, do nothing
                if (value is null)
                {
                    return;
                }

                TaxPlansModel.SelectedTaxPlanName = value;
                // Hard-coded plan
                if (value.Contains("Slant Tax") || value.Contains("Slant Mod 1") || value.Contains("Slant Mod 2"))
                {
                    if (TaxPlansModel.TaxPlans.ContainsKey(this.SelectedTaxPlanName))
                    {
                        //DataModel.NewTaxPctByBracket = new List<double>(TaxPlansModel.TaxPlans[SelectedTaxPlanName].TaxRates);
                        
                        this.MaxTaxRate = (int) TaxPlansModel.TaxPlans[SelectedTaxPlanName].MaxTaxRate;

                        this.MaxBracketCount = TaxPlansModel.TaxPlans[SelectedTaxPlanName].MaxBracketCount;
                        MaxBracketCountSlider = TaxPlansModel.TaxPlans[SelectedTaxPlanName].MaxBracketCount;

                        //this.PovertyLineIndex = TaxPlansModel.TaxPlans[SelectedTaxPlanName].PovertyLineIndex;
                        this.PovertyLineIndexSlider = TaxPlansModel.TaxPlans[SelectedTaxPlanName].PovertyLineIndex;

                        this.BalanceMaxWithPoverty = TaxPlansModel.TaxPlans[SelectedTaxPlanName].BalanceMaxWithPoverty;
                        this.BalancePovertyWithMax = TaxPlansModel.TaxPlans[SelectedTaxPlanName].BalancePovertyWithMax;

                        DataVM.CalculateNewRevenues(TaxPlansModel.SelectedTaxPlan.TaxRates);

                        SelectedTaxPlanTabIndex = (int)TaxPlan.Slant;
                        BracketAdjustmentsExpanded = false;

                    }
                    else
                    {
                        MaxTaxRate = (int)OptionsModel.MaxTaxRate;
                        MaxBracketCountSlider = OptionsModel.MaxBracketCount;
                        PovertyLineIndexSlider = -1;
                        DataVM.CalculateNewRevenues(TaxPlansModel.SelectedTaxPlan.TaxRates);

                        SelectedTaxPlanTabIndex = (int)TaxPlan.Slant;
                        BracketAdjustmentsExpanded = false;
                    }
                }
                // Hard-coded plan:
                else if (value.Contains("Flat Tax"))
                {

                    if (TaxPlansModel.TaxPlans.ContainsKey(this.SelectedTaxPlanName))
                    {
                        FlatTaxSlider = TaxPlansModel.TaxPlans[SelectedTaxPlanName].FlatTaxRate;
                    }
                    else
                    {
                        FlatTaxSlider = 0;
                    }

                    SelectedTaxPlanTabIndex = (int)TaxPlan.Flat;
                    MaxTaxRate = 0;
                    MaxBracketCountSlider = 0;
                    PovertyLineIndexSlider = -1;
                    BracketAdjustmentsExpanded = false;

                }
                else
                {

                    // Else it's a custom plan
                    // If it exists, we want to handle it
                    if (TaxPlansModel.TaxPlans.TryGetValue(value, out IndividualTaxPlanModel selectedTaxPlan))
                    {

                        SelectedTaxPlanTabIndex = (int)TaxPlan.Custom;

                        BracketAdjustmentsExpanded = true;

                        OptionsModel.MaxTaxRate = MaxTaxRate;
                        OptionsModel.MaxBracketCount = MaxBracketCountSlider;
                        MaxTaxRate = 0;
                        MaxBracketCountSlider = 0;
                        PovertyLineIndexSlider = -1;

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

                if (TaxPlansModel.TaxPlans[SelectedTaxPlanName].CustomTaxRates.Count > 1)
                {
                    for (int i = 0; i < TaxPlansModel.TaxPlans[SelectedTaxPlanName].CustomTaxRates.Count; i++)
                    {
                        TaxPlansModel.TaxPlans[SelectedTaxPlanName].TaxRates[i] = TaxPlansModel.TaxPlans[SelectedTaxPlanName].CustomTaxRates[i];
                        DataModel.NewTaxPctByBracket[i] = TaxPlansModel.TaxPlans[SelectedTaxPlanName].CustomTaxRates[i];
                    }
                    customGraphReset();
                }
                this.update();

                OnPropertyChange("SelectedTaxRate");
                OnPropertyChange("TaxRateSlider");
                OnPropertyChange("MaxBracketCountSlider");
                OnPropertyChange("PovertyLineIndex");
                OnPropertyChange("PovertyLineBrackets");
                OnPropertyChange("PovertyLineIndexSlider");
                OnPropertyChange("SelectedTaxPlanName");
                OnPropertyChange("SelectedBracket");
                OnPropertyChange("DeleteTaxPlanBtnEnabled");
                OnPropertyChange("AutoFitBtnEnabled");
                OnPropertyChange("slantVisible");
                OutputVM.Update();

            }
        }

        private int selectedEditingModeIndex = 0;
        public int SelectedEditingModeIndex
        {
            get
            {
                return selectedEditingModeIndex;
            }
            set
            {
                selectedEditingModeIndex = value;
                OnPropertyChange("SelectedEditingModeIndex");
                OnPropertyChange("SelectedEditingMode");
                OnPropertyChange("AutoFitBtnEnabled");
            }
        }

        public string SelectedEditingMode
        {
            get
            {
                // set toggle button text to edit the opposite of the current mode
                EditingMode mode = (EditingMode)1-SelectedEditingModeIndex;
                return "Edit " + mode.ToString();
            }
        }

        public bool ToggleEditingModeBtnEnabled
        {
            get
            {
                if (ShowUBI)
                {
                    return true;
                }

                // go back to tax editing mode if ubi is not selected
                SelectedEditingModeIndex = 0;
                return false;

            }
        }

        public bool AutoFitBtnEnabled
        {
            get
            {
                if((SelectedTaxPlanName.Contains("Slant Tax") || SelectedTaxPlanName.Contains("Flat Tax") || this.SelectedTaxPlanName.Contains("Slant Mod 1") || this.SelectedTaxPlanName.Contains("Slant Mod 2")) && SelectedEditingModeIndex == 0 )
                {
                    return true;
                }

                return false;
            }
        }

        private bool curveEditorExpanded = true;
        public bool CurveEditorExpanded
        {
            get
            {
                return curveEditorExpanded;
            }
            set
            {
                curveEditorExpanded = value;

                // collapse bracket adjustments when closing the curve editor
                if (!value)
                {
                    BracketAdjustmentsExpanded = false;
                }

                OnPropertyChange("CurveEditorExpanded");
            }
        }

        private bool bracketAdjustmentsExpanded = false;
        public bool BracketAdjustmentsExpanded
        {
            get
            {
                return bracketAdjustmentsExpanded;
            }
            set
            {
                bracketAdjustmentsExpanded = value;
                OnPropertyChange("BracketAdjustmentsExpanded");
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

                if (this.SelectedTaxPlanName.Contains("Slant Tax"))
                {
                    List<List<double>> slantTaxData = DataVM.CalculateSlantTaxData();
                    List<double> slantTaxRates = slantTaxData[0];
                    try
                    {
                        TaxPlansModel.TaxPlans.TryGetValue(this.SelectedTaxPlanName, out IndividualTaxPlanModel stax);
                        stax.TaxRates = new ObservableCollection<double>(slantTaxRates);
                    }
                    catch { }
                }
                else if (this.SelectedTaxPlanName.Contains("Slant Mod 1")){
                    List<List<double>> mod1Data = DataVM.CalculateMod1Data();
                    List<double> mod1Rates = mod1Data[0];
                    try
                    {
                        TaxPlansModel.TaxPlans.TryGetValue(this.SelectedTaxPlanName, out IndividualTaxPlanModel stax);
                        //stax.TaxRates = new ObservableCollection<double>(mod1Rates);
                        foreach (var bracket in BracketList)
                        {
                            // updates bracket adjustment selected tax rate to equal tax rate shown in graph
                            int index = BracketList.IndexOf(bracket);
                            stax.TaxRates[index] = DataModel.NewTaxPctByBracket[index];
                        }
                    }
                    catch { }
                }
                else if (this.SelectedTaxPlanName.Contains("Slant Mod 2"))
                {
                    List<List<double>> mod2Data = DataVM.CalculateMod2Data();
                    List<double> mod2Rates = mod2Data[0];
                    try
                    {
                        TaxPlansModel.TaxPlans.TryGetValue(this.SelectedTaxPlanName, out IndividualTaxPlanModel stax);
                        // stax.TaxRates = new ObservableCollection<double>(mod2Rates);
                        foreach (var bracket in BracketList)
                        {
                            // updates bracket adjustment selected tax rate to equal tax rate shown in graph
                            int index = BracketList.IndexOf(bracket);
                            stax.TaxRates[index] = DataModel.NewTaxPctByBracket[index];
                        }
                    }
                    catch { }
                }
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

                if (BalancePovertyWithMax)
                {
                    // calculate the number of poverty brackets to closest match the max tax population
                    int brackets = DataVM.determineBaselinePovertyBrackets();
                    // subtract 1 from number of poverty brackets to make index-based
                    PovertyLineIndexSlider = brackets - 1;
                }

                if(SelectedTaxPlanName.Contains("Slant Tax")){ 
                    List<List<double>> slantTaxData = DataVM.CalculateSlantTaxData();
                    List<double> slantTaxRates = slantTaxData[0];
                    try
                    {
                        TaxPlansModel.TaxPlans.TryGetValue(this.SelectedTaxPlanName, out IndividualTaxPlanModel stax);
                        stax.TaxRates = new ObservableCollection<double>(slantTaxRates);
                    }
                    catch { }
                }
                else if (this.SelectedTaxPlanName.Contains("Slant Mod 1"))
                {
                    List<List<double>> mod1Data = DataVM.CalculateMod1Data();
                    List<double> mod1Rates = mod1Data[0];
                    try
                    {
                        TaxPlansModel.TaxPlans.TryGetValue(this.SelectedTaxPlanName, out IndividualTaxPlanModel stax);
                        //stax.TaxRates = new ObservableCollection<double>(mod1Rates);
                        foreach (var bracket in BracketList)
                        {
                            // updates bracket adjustment selected tax rate to equal tax rate shown in graph
                            int index = BracketList.IndexOf(bracket);
                            stax.TaxRates[index] = DataModel.NewTaxPctByBracket[index];
                        }
                    }
                    catch { }
                }
                else if (this.SelectedTaxPlanName.Contains("Slant Mod 2"))
                {
                    List<List<double>> mod2Data = DataVM.CalculateMod2Data();
                    List<double> mod2Rates = mod2Data[0];
                    try
                    {
                        TaxPlansModel.TaxPlans.TryGetValue(this.SelectedTaxPlanName, out IndividualTaxPlanModel stax);
                        //stax.TaxRates = new ObservableCollection<double>(mod2Rates);
                        foreach (var bracket in BracketList)
                        {
                            // updates bracket adjustment selected tax rate to equal tax rate shown in graph
                            int index = BracketList.IndexOf(bracket);
                            stax.TaxRates[index] = DataModel.NewTaxPctByBracket[index];
                        }
                    }
                    catch { }
                }

                OnPropertyChange("TaxRateSlider");
                OnPropertyChange("SelectedTaxRate");

                if (SlantChangesUBI)
                {
                    MaxUBIBracketCountSlider = MaxBracketCountSlider;
                    MinUBIBracketCountSlider = PovertyLineBrackets;
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

            }
        }

        // $ Amount of UBI per month
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

                if (this.SelectedTaxPlanName.Contains("Slant Tax"))
                {
                    List<List<double>> slantTaxData = DataVM.CalculateSlantTaxData();
                    List<double> slantTaxRates = slantTaxData[0];
                    try
                    {
                        TaxPlansModel.TaxPlans.TryGetValue(this.SelectedTaxPlanName, out IndividualTaxPlanModel stax);
                        stax.TaxRates = new ObservableCollection<double>(slantTaxRates);
                    }
                    catch { }
                }
                else if (this.SelectedTaxPlanName.Contains("Slant Mod 1"))
                {
                    List<List<double>> mod1Data = DataVM.CalculateMod1Data();
                    List<double> mod1Rates = mod1Data[0];
                    try
                    {
                        TaxPlansModel.TaxPlans.TryGetValue(this.SelectedTaxPlanName, out IndividualTaxPlanModel stax);
                        //stax.TaxRates = new ObservableCollection<double>(mod1Rates);
                        foreach (var bracket in BracketList)
                        {
                            // updates bracket adjustment selected tax rate to equal tax rate shown in graph
                            int index = BracketList.IndexOf(bracket);
                            stax.TaxRates[index] = DataModel.NewTaxPctByBracket[index];
                        }
                    }
                    catch { }
                }
                else if (this.SelectedTaxPlanName.Contains("Slant Mod 2"))
                {
                    List<List<double>> mod2Data = DataVM.CalculateMod2Data();
                    List<double> mod2Rates = mod2Data[0];
                    try
                    {
                        TaxPlansModel.TaxPlans.TryGetValue(this.SelectedTaxPlanName, out IndividualTaxPlanModel stax);
                        //stax.TaxRates = new ObservableCollection<double>(mod2Rates);
                        foreach (var bracket in BracketList)
                        {
                            // updates bracket adjustment selected tax rate to equal tax rate shown in graph
                            int index = BracketList.IndexOf(bracket);
                            stax.TaxRates[index] = DataModel.NewTaxPctByBracket[index];
                        }
                    }
                    catch { }
                }
                OnPropertyChange("TaxRateSlider");
                OnPropertyChange("SelectedTaxRate");

                if (SlantChangesUBI) {
                    MaxUBIBracketCountSlider = MaxBracketCountSlider;
                    MinUBIBracketCountSlider = PovertyLineBrackets;
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
                if (SelectedTaxPlanName.Contains("Flat Tax"))
                {
                    OptionsModel.FlatTaxRate = value;
                    TaxPlansModel.TaxPlans.TryGetValue(this.SelectedTaxPlanName, out IndividualTaxPlanModel selectedTaxPlan);
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

        public Visibility slantVisible
        {
            get
            {
                if (this.SelectedTaxPlanName.Contains("Slant Tax") || this.SelectedTaxPlanName.Contains("Slant Mod 1") || this.SelectedTaxPlanName.Contains("Slant Mod 2"))
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }


        // Display checkboxes
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

                OnPropertyChange("ToggleEditingModeBtnEnabled");
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
                    MaxUBIBracketCountSlider = MaxBracketCountSlider;
                    MinUBIBracketCountSlider = PovertyLineBrackets;
                    OnPropertyChange("MaxUBIBracketCountSlider");
                    OnPropertyChange("MaxUBIBracketCount");
                    OnPropertyChange("MinUBIBracketCountSlider");
                    OnPropertyChange("MinUBIBracketCount");
                }
            }
        }


        /*
         
                Button Logic
        */

        public void CreateTaxPlan(string name, ObservableCollection<double> taxValues)
        {

            if (TaxPlansModel.TaxPlans.ContainsKey(name))
            {
                return;
            }

            TaxPlansModel.TaxPlans.Add(name, new IndividualTaxPlanModel(name, taxValues));
            TaxPlansList.Add(name);

        }

        // Close popup window
        private void cancelButton_Click(object sender, EventArgs e, Popup p)
        {
            p.IsOpen = false;
        }

        private void newTaxPlanButtonClick()
        {

            DialogBox dialog = new DialogBox();
            dialog.ShowDialog();

            if (dialog.Saved)
            {

                string taxPlanName = dialog.Filename;

                if (taxPlanName.Equals("Slant Tax") || taxPlanName.Equals("Flat Tax") || taxPlanName.Equals("Slant Mod 1") || taxPlanName.Equals("Slant Mod 2"))
                {
                    taxPlanName += " (modified)";
                }

                Dictionary<string, object> values = new Dictionary<string, object>();

                values.Add("MaxTaxRate", MaxTaxRate);
                values.Add("MaxBracketCount", MaxBracketCount);
                values.Add("PovertyLineBrackets", PovertyLineBrackets);

                ObservableCollection<double> newTaxPlanRates;

                if (TaxPlansModel.TaxPlans.TryGetValue(SelectedTaxPlanName, out IndividualTaxPlanModel selectedTaxPlan))
                {
                    newTaxPlanRates = selectedTaxPlan.TaxRates;
                }
                else
                {
                    //Set all of the default values to 0% tax
                    newTaxPlanRates = new ObservableCollection<double>(new double[(int)(GraphModel.Labels.Length)]);
                }

                values.Add("TaxRates", newTaxPlanRates);


                if (TaxPlansModel.TaxPlans.ContainsKey(taxPlanName))
                {

                    int increment = 2;

                    string nameIncrement = taxPlanName + " (" + increment + ")";

                    while (TaxPlansModel.TaxPlans.ContainsKey(nameIncrement))
                    {

                        increment++;
                        nameIncrement = taxPlanName + " (" + increment + ")";

                    }

                    taxPlanName = nameIncrement;

                }
                else
                {
                    
                }

                TaxPlansModel.TaxPlans.Add(taxPlanName, new IndividualTaxPlanModel(taxPlanName, newTaxPlanRates));

                PlanSaver.SavePlan(taxPlanName, values);

                OnPropertyChange("TaxPlansList");

                SelectedTaxPlanName = taxPlanName;

            }

            return;

        }

        public void newTaxPlanTest(string fName)
        {

            if (true)
            {

                string taxPlanName = fName;

                if (taxPlanName.Equals("Slant Tax") || taxPlanName.Equals("Flat Tax") || taxPlanName.Equals("Slant Mod 1") || taxPlanName.Equals("Slant Mod 2"))
                {
                    taxPlanName += " (modified)";
                }

                Dictionary<string, object> values = new Dictionary<string, object>();

                values.Add("MaxTaxRate", MaxTaxRate);
                values.Add("MaxBracketCount", MaxBracketCount);
                values.Add("PovertyLineBrackets", PovertyLineBrackets);

                ObservableCollection<double> newTaxPlanRates;

                if (TaxPlansModel.TaxPlans.TryGetValue(SelectedTaxPlanName, out IndividualTaxPlanModel selectedTaxPlan))
                {
                    newTaxPlanRates = selectedTaxPlan.TaxRates;
                }
                else
                {
                    //Set all of the default values to 0% tax
                    newTaxPlanRates = new ObservableCollection<double>(new double[(int)(GraphModel.Labels.Length)]);
                }

                values.Add("TaxRates", newTaxPlanRates);


                if (TaxPlansModel.TaxPlans.ContainsKey(taxPlanName))
                {

                    int increment = 2;

                    string nameIncrement = taxPlanName + " (" + increment + ")";

                    while (TaxPlansModel.TaxPlans.ContainsKey(nameIncrement))
                    {

                        increment++;
                        nameIncrement = taxPlanName + " (" + increment + ")";

                    }

                    taxPlanName = nameIncrement;

                }
                else
                {

                }

                TaxPlansModel.TaxPlans.Add(taxPlanName, new IndividualTaxPlanModel(taxPlanName, newTaxPlanRates));

                PlanSaver.SavePlan(taxPlanName, values);

                OnPropertyChange("TaxPlansList");

                SelectedTaxPlanName = taxPlanName;

            }

            return;
        }

        public void saveTaxPlan()
        {

            string taxPlanName = this.SelectedTaxPlanName;
            string oldName = this.SelectedTaxPlanName;

            bool baseModified = false;

            if (taxPlanName.Equals("Slant Tax") || taxPlanName.Equals("Flat Tax") || taxPlanName.Equals("Slant Mod 1") || taxPlanName.Equals("Slant Mod 2"))
            {
                taxPlanName += " (modified)";
                baseModified = true;
            }

            Dictionary<string, object> values = new Dictionary<string, object>();

            values.Add("MaxTaxRate", MaxTaxRate);
            values.Add("MaxBracketCount", MaxBracketCount);
            values.Add("PovertyLineBrackets", PovertyLineBrackets);

            ObservableCollection<double> newTaxPlanRates;

            if (TaxPlansModel.TaxPlans.TryGetValue(SelectedTaxPlanName, out IndividualTaxPlanModel selectedTaxPlan))
            {
                newTaxPlanRates = selectedTaxPlan.TaxRates;
            }
            else
            {
                //Set all of the default values to 0% tax
                newTaxPlanRates = new ObservableCollection<double>(new double[(int)(GraphModel.Labels.Length)]);
            }

            if (baseModified && TaxPlansModel.TaxPlans.ContainsKey(taxPlanName))
            {

                int increment = 2;

                string nameIncrement = taxPlanName + " (" + increment + ")";

                while (TaxPlansModel.TaxPlans.ContainsKey(nameIncrement))
                {

                    increment++;
                    nameIncrement = taxPlanName + " (" + increment + ")";

                }

                taxPlanName = nameIncrement;

            }

            if (baseModified)
            {
                TaxPlansModel.TaxPlans.Add(taxPlanName, new IndividualTaxPlanModel(taxPlanName, newTaxPlanRates));
                
                TaxPlansModel.TaxPlans[taxPlanName].CustomTaxRates = TaxPlansModel.TaxPlans[oldName].CustomTaxRates;
                TaxPlansModel.TaxPlans[taxPlanName].MaxTaxRate = TaxPlansModel.TaxPlans[oldName].MaxTaxRate;
                TaxPlansModel.TaxPlans[taxPlanName].MaxBracketCount = TaxPlansModel.TaxPlans[oldName].MaxBracketCount;
                TaxPlansModel.TaxPlans[taxPlanName].PovertyLineIndex = TaxPlansModel.TaxPlans[oldName].PovertyLineIndex;
                TaxPlansModel.TaxPlans[taxPlanName].FlatTaxRate = TaxPlansModel.TaxPlans[oldName].FlatTaxRate;
                TaxPlansModel.TaxPlans[taxPlanName].BalanceMaxWithPoverty = TaxPlansModel.TaxPlans[oldName].BalanceMaxWithPoverty;
                TaxPlansModel.TaxPlans[taxPlanName].BalancePovertyWithMax = TaxPlansModel.TaxPlans[oldName].BalancePovertyWithMax;

                TaxPlansModel.TaxPlans[taxPlanName].CustomTaxRates.Clear();
                TaxPlansModel.TaxPlans[oldName].CustomTaxRates.Clear();
                for (int i = 0; i < TaxPlansModel.TaxPlans[oldName].TaxRates.Count; i++)
                {
                    TaxPlansModel.TaxPlans[taxPlanName].TaxRates[i] = TaxPlansModel.TaxPlans[oldName].TaxRates[i];
                    TaxPlansModel.TaxPlans[taxPlanName].CustomTaxRates.Add(TaxPlansModel.TaxPlans[oldName].TaxRates[i]);
                }
                for (int i = 0; i < TaxPlansModel.TaxPlans[oldName].TaxRates.Count; i++)
                {
                    TaxPlansModel.TaxPlans[oldName].TaxRates[i] = 0;
                    TaxPlansModel.TaxPlans[oldName].CustomTaxRates.Add(0);
                    TaxPlansModel.TaxPlans[taxPlanName].TaxRates[i] = TaxPlansModel.TaxPlans[taxPlanName].CustomTaxRates[i];
                }

                TaxPlansModel.TaxPlans[oldName].MaxTaxRate = 0;
                TaxPlansModel.TaxPlans[oldName].MaxBracketCount = 0;
                TaxPlansModel.TaxPlans[oldName].PovertyLineIndex = -1;
                TaxPlansModel.TaxPlans[oldName].FlatTaxRate = 0;
                TaxPlansModel.TaxPlans[oldName].BalanceMaxWithPoverty = false;
                TaxPlansModel.TaxPlans[oldName].BalancePovertyWithMax = false;

                OnPropertyChange("TaxPlansList");

                SelectedTaxPlanName = taxPlanName;

                this.MaxTaxRate = (int) TaxPlansModel.TaxPlans[taxPlanName].MaxTaxRate;
                this.MaxBracketCount = TaxPlansModel.TaxPlans[taxPlanName].MaxBracketCount;
                this.MaxBracketCountSlider = TaxPlansModel.TaxPlans[taxPlanName].MaxBracketCount;
                this.PovertyLineIndexSlider = TaxPlansModel.TaxPlans[taxPlanName].PovertyLineIndex;
                this.FlatTaxSlider = TaxPlansModel.TaxPlans[taxPlanName].FlatTaxRate;
                this.BalanceMaxWithPoverty = TaxPlansModel.TaxPlans[taxPlanName].BalanceMaxWithPoverty;
                this.BalancePovertyWithMax = TaxPlansModel.TaxPlans[taxPlanName].BalancePovertyWithMax;
                for (int i = 0; i < TaxPlansModel.TaxPlans[taxPlanName].CustomTaxRates.Count; i++)
                {
                    TaxPlansModel.TaxPlans[SelectedTaxPlanName].TaxRates[i] = TaxPlansModel.TaxPlans[taxPlanName].CustomTaxRates[i];
                    DataModel.NewTaxPctByBracket[i] = TaxPlansModel.TaxPlans[taxPlanName].CustomTaxRates[i];
                }

                DataVM.CalculateNewRevenues(TaxPlansModel.SelectedTaxPlan.TaxRates);

                customGraphReset();

            }
            this.update();
            OutputVM.Update();
            values.Add("TaxRates", TaxPlansModel.TaxPlans[taxPlanName].TaxRates);
            PlanSaver.SavePlan(taxPlanName, values);

        }


        // Delete selected custom/non-default tax plan
        public void deleteTaxPlanButtonClick()
        {
            if (SelectedTaxPlanName != "Slant Tax" && SelectedTaxPlanName != "Flat Tax" && SelectedTaxPlanName != "Slant Mod 1" && SelectedTaxPlanName != "Slant Mod 2")
            {

                PlanSaver.DeletePlan(SelectedTaxPlanName);

                TaxPlansModel.TaxPlans.Remove(SelectedTaxPlanName);
                SelectedTaxPlanName = TaxPlansList[0];

            }
        }

        // Switch the editing mode between Tax and UBI
        public void toggleEditModeButtonClick()
        {
            SelectedEditingModeIndex = 1 - SelectedEditingModeIndex;
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

        // Graph reset that doesn't recalculate Slant Tax Data
        // Used for manually changing tax rates
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

        // Collections of calls to update only what is shown on the graph (e.g. checkboxes ticked/unticked)
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

        // Resets to default options
        public void resetSettingsButtonClick(){
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
            SlantChangesUBI = false;
            BalanceMaxWithPoverty = false;
            BalancePovertyWithMax = false;
            DontAdjustBracketCount = false;
            OnPropertyChange("DontAdjustBracketCount");
            LockTaxRates = false;
            LockNumberSlider = 0;
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
            this.MaxUBIBracketCountSlider = 0;
            this.MinUBIBracketCountSlider = 3;
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

        public void resetTaxRatesButtonClick() {
            if (SelectedTaxPlanName.Contains("Slant "))
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
            else if (SelectedTaxPlanName.Contains("Flat Tax"))
            {
                FlatTaxSlider = 0;
                OnPropertyChange("TaxRateSlider");
                OnPropertyChange("SelectedTaxRate");
            }
            else {
                TaxPlansModel.TaxPlans.TryGetValue(SelectedTaxPlanName, out IndividualTaxPlanModel selectedTaxPlan);
                for (int i = 0; i < selectedTaxPlan.TaxRates.Count; i++){
                    selectedTaxPlan.TaxRates[i] = 0;
                    DataModel.NewTaxPctByBracket[i] = 0;
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
            OnPropertyChange("BalanceMaxWithPoverty");
            OnPropertyChange("BalancePovertyWithMax");
        }

        // Auto-fit current tax plan (slant and flat tax plans only)
        public void autoFitTaxButtonClick()
        {
            if(SelectedTaxPlanName == "Flat Tax")
            {
                autoFitFlatTaxButtonClick();
            }
            else if (SelectedTaxPlanName == "Slant Tax" || this.SelectedTaxPlanName == "Slant Mod 1" || this.SelectedTaxPlanName == "Slant Mod 2")
            {
                autoFitSlantTaxButtonClick();
            }
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

    public enum EditingMode
    {
        Tax,
        UBI
    }

}