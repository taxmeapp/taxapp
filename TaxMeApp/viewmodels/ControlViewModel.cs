using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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


        public ICommand SettingsBtnCommand { get; set; }
        public ICommand AddTaxPlanBtnCommand { get; set; }
        public ICommand SaveTaxPlanBtnCommand { get; set; }
        public ICommand DeleteTaxPlanBtnCommand { get; set; }
        public ICommand ResetSettingsBtnCommand { get; set; }
        public ICommand ResetTaxRatesBtnCommand { get; set; }
        public ICommand AutoFitSlantTaxBtnCommand { get; set; }
        public ICommand AutoFitBudgetBtnCommand { get; set; }


        public ControlViewModel()
        {

            SettingsBtnCommand = new RelayCommand(o => settingsButtonClick(""));
            AddTaxPlanBtnCommand = new RelayCommand(o => addTaxPlanButtonClick());
            SaveTaxPlanBtnCommand = new RelayCommand(o => saveTaxPlanButtonClick());
            DeleteTaxPlanBtnCommand = new RelayCommand(o => deleteTaxPlanButtonClick());
            ResetSettingsBtnCommand = new RelayCommand(o => resetSettingsButtonClick());
            ResetTaxRatesBtnCommand = new RelayCommand(o => resetTaxRatesButtonClick());
            AutoFitSlantTaxBtnCommand = new RelayCommand(o => autoFitSlantTaxButtonClick());
            AutoFitBudgetBtnCommand = new RelayCommand(o => autoFitBudgetButtonClick());
        }

        // ControlPanel Init
        public void ControlInit()
        {

            // Automatically select the first item in the list
            SelectedYear = YearsModel.YearList[0];

            //Add Slant Tax to the list and select it when ControlViewModel is Inited
            List<List<double>> slantTaxData = DataVM.calculateSlantTaxData();
            List<double> slantTaxRates = slantTaxData[0];
            TaxPlansModel.TaxPlans.Add("Slant Tax", new IndividualTaxPlanModel("Slant Tax", new ObservableCollection<double>(slantTaxRates)));
            SelectedTaxPlanName = "Slant Tax";

            PlanLoader.LoadPlans(this);

            //for (int i = 0; i < DataModel.NewTaxPctByBracket.Count; i++) {
            //    Console.WriteLine("Bracket {0}, Value {1}", i, DataModel.NewTaxPctByBracket[i]);
            //}

            //Set selected bracket to first one
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
                //Trace.WriteLine("Changing selected year to: " + value);

                // Save it in our data model
                YearsModel.SelectedYear = value;

                totalGraphReset();

            }
        }

        public List<string> GovProgramList
        {
            get
            {
                return OptionsModel.GetGovProgramList();
            }
        }
        public string SelectedGovProgram
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
                double ans = OptionsModel.GetSelectedTargetFunding(GovProgramList.IndexOf(SelectedGovProgram));
                if (ans == -1) {
                    SelectedGovProgram = OptionsModel.GetGovProgramList()[GovProgramList.Count-1];
                    OnPropertyChange("SelectedGovProgram");
                    ans = OptionsModel.GetSelectedTargetFunding(GovProgramList.IndexOf(SelectedGovProgram));
                }

                return ans;
            }
        }

        public string SelectedTargetBudget
        {
            get
            {
                return OptionsModel.GetSelectedTargetBudget(GovProgramList.IndexOf(SelectedGovProgram));
            }
        }

        public double TargetFundingSlider {
            get {
                return OptionsModel.GetSelectedTargetFunding(GovProgramList.IndexOf(SelectedGovProgram));
            }
            set {
                //int priority, bool ischecked, string name, double cost, double tFunding)
                int priority = GovProgramList.IndexOf(SelectedGovProgram);
                bool isChecked = OptionsModel.listOfCosts[priority].ischecked;
                string name = SelectedGovProgram;
                double cost = OptionsModel.listOfCosts[priority].cost;

                OptionsModel.listOfCosts[GovProgramList.IndexOf(SelectedGovProgram)] = (priority, isChecked, name, cost, value);
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

                    DataModel.NewRevenueByBracket = DataVM.calculateNewRevenues(selectedTaxPlan.TaxRates);
                    OnPropertyChange("SelectedTaxRate");
                    OutputVM.Update();
                    customGraphReset();
                }
            }
        }

        public bool locked = false;
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

        int editingBrackets = 0;
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
                if (SelectedTaxPlanName == null || !SelectedTaxPlanName.Equals("Slant Tax"))
                {
                    return true;
                }

                return false;
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
                TaxPlansModel.SelectedTaxPlanName = value;
                if (value != "Slant Tax")
                {
                    OptionsModel.MaxTaxRate = MaxTaxRate;
                    OptionsModel.MaxBracketCount = MaxBracketCountSlider;
                    MaxTaxRate = 0;
                    MaxBracketCountSlider = 0;

                    if (SelectedTaxPlanName != null)
                    {
                        TaxPlansModel.TaxPlans.TryGetValue(SelectedTaxPlanName, out IndividualTaxPlanModel selectedTaxPlan);
                        for (int i = 0; i < selectedTaxPlan.TaxRates.Count; i++)
                        {
                            DataModel.NewTaxPctByBracket[i] = selectedTaxPlan.TaxRates[i];

                        }
                        DataModel.NewRevenueByBracket = DataVM.calculateNewRevenues(selectedTaxPlan.TaxRates);
                        customGraphReset();
                    }

                }
                else
                {
                    MaxTaxRate = (int)OptionsModel.MaxTaxRate;
                    MaxBracketCountSlider = OptionsModel.MaxBracketCount;
                    DataVM.calculateNewRevenues(TaxPlansModel.SelectedTaxPlan.TaxRates);
                }

                OnPropertyChange("SelectedTaxRate");
                OnPropertyChange("TaxRateSlider");
                OnPropertyChange("MaxBracketCountSlider");
                OnPropertyChange("SelectedTaxPlanName");
                OnPropertyChange("SelectedBracket");
                OnPropertyChange("DeleteTaxPlanBtnEnabled");
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
                DataModel.MaxTaxRate = value;

                newDataGraphReset();

                OnPropertyChange("MaxTaxRate");

                List<List<double>> slantTaxData = DataVM.calculateSlantTaxData();
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

                newDataGraphReset();

                OnPropertyChange("MaxUBIBracketCount");
                OnPropertyChange("MaxBracketUBICountSlider");
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

                newDataGraphReset();

                OnPropertyChange("MinUBIBracketCount");
                OnPropertyChange("MinBracketUBICountSlider");
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

                newDataGraphReset();

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

                OnPropertyChange("MaxBracketCountSlider");


                //UBI sometimes changes when poverty brackets change
                //OnPropertyChange("MaxUBIBracketCount");
                //OnPropertyChange("MaxUBIBracketCountSlider");
                //OnPropertyChange("MaxUBISlider");

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

            }
        }

        /*
         
                Button Logic
        */

        // Settings button logic

        private void settingsButtonClick(object sender)
        {

            MainVM.TabSelected = 1;

        }

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
            addButton.Click += (sender, EventArgs) => { AddButton_Click(sender, EventArgs, createNewTaxPlan, textInput); };
            addButton.Width = 180;
            addButton.Margin = defaultMargin;
            content.Children.Add(addButton);

            Button cancelButton = new Button();
            cancelButton.Content = "Cancel";
            cancelButton.Click += (sender, EventArgs) => { CancelButton_Click(sender, EventArgs, createNewTaxPlan); };
            cancelButton.Width = 180;
            cancelButton.Margin = defaultMargin;
            content.Children.Add(cancelButton);

            createNewTaxPlan.Child = content;
            createNewTaxPlan.IsOpen = true;
        }

        private void AddButton_Click(object sender, EventArgs e, Popup p, TextBox tb)
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
        private void CancelButton_Click(object sender, EventArgs e, Popup p)
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
            if (GraphVM != null)
            {
                GraphVM.ClearSeries();

                GraphVM.GraphAllChecked();
            }
        }

        private void customGraphReset(List<double> customRates) 
        {
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
            MaxBracketCountSlider = 0;
            MaxTaxRate = 0;
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
        }

        public void autoFitSlantTaxButtonClick() {
            double budget = OptionsModel.GetTotalBudget();
            double revenue = 0;

            this.MaxBracketCountSlider = 0;
            this.MaxTaxRate = 0;
            revenue = DataModel.TotalRevenueNew;

            while (revenue < budget) {
                for (int i = 0; i < 11; i++) {
                    MaxBracketCountSlider = i;
                    revenue = DataModel.TotalRevenueNew;
                    //Console.WriteLine("TaxRate = {0}, Brackets = {1}, Revenue = {2}, Budget = {3}", MaxTaxRate, MaxBracketCountSlider, revenue, budget);
                    if (revenue >= budget) {
                        break;
                    }
                }
                if (revenue < budget) {
                    this.MaxTaxRate += 1;
                    //revenue = DataModel.TotalRevenueNew;
                }
                if (this.MaxTaxRate > 100) {
                    break;
                }
            }
            if (this.MaxTaxRate > 100) {
                this.MaxTaxRate = 100;
            }
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
    }
}