using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using TaxMeApp.Helpers;
using TaxMeApp.models;

namespace TaxMeApp.viewmodels
{
    public class ControlViewModel : MainViewModel
    {


        public ICommand SettingsBtnCommand { get; set; }
        public ICommand AddTaxPlanBtnCommand { get; set; }
        public ICommand DeleteTaxPlanBtnCommand { get; set; }
        public ICommand ResetSettingsBtnCommand { get; set; }
        public ICommand ResetTaxRatesBtnCommand { get; set; }

        public ControlViewModel()
        {

            SettingsBtnCommand = new RelayCommand(o => settingsButtonClick(""));
            AddTaxPlanBtnCommand = new RelayCommand(o => addTaxPlanButtonClick());
            DeleteTaxPlanBtnCommand = new RelayCommand(o => deleteTaxPlanButtonClick());
            ResetSettingsBtnCommand = new RelayCommand(o => resetSettingsButtonClick());
            ResetTaxRatesBtnCommand = new RelayCommand(o => resetTaxRatesButtonClick());
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
                    Console.WriteLine("Selected Bracket = {0}, Tax Rate = {1}", SelectedBracket, selectedTaxPlan.TaxRates[BracketList.IndexOf(SelectedBracket)]);
                    return selectedTaxPlan.TaxRates[BracketList.IndexOf(SelectedBracket)];
                }
                return 0;
            }
            set
            {
                if (SelectedTaxPlanName != null)
                {
                    TaxPlansModel.TaxPlans.TryGetValue(SelectedTaxPlanName, out IndividualTaxPlanModel selectedTaxPlan);
                    selectedTaxPlan.TaxRates[BracketList.IndexOf(SelectedBracket)] = value;
                    DataModel.NewTaxPctByBracket[BracketList.IndexOf(SelectedBracket)] = value;
                    DataModel.NewRevenueByBracket = DataVM.calculateNewRevenues(selectedTaxPlan.TaxRates);
                    OnPropertyChange("SelectedTaxRate");
                    OutputVM.Update();
                    customGraphReset();
                }
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
                TaxPlansModel.TaxPlans.TryGetValue("Slant Tax", out IndividualTaxPlanModel stax);
                stax.TaxRates = new ObservableCollection<double>(slantTaxRates);
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
                GraphModel.MaxBracketCount = value;

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

                TaxPlansModel.TaxPlans.Add(tb.Text, new IndividualTaxPlanModel(tb.Text, defaultValues));
                TaxPlansList.Add(tb.Text);
                tb.Text = "";
                p.IsOpen = false;
                SelectedTaxPlanName = TaxPlansList[0];
                SelectedBracket = BracketList[0];
            }
            //If the name is already being used then clear the text entry and print something out
            else
            {
                Console.WriteLine("\nERROR: The Name: {0} Is Already Taken\n", tb.Text);
                tb.Text = "";
            }
        }

        //The cancel button just closes the popup window
        private void CancelButton_Click(object sender, EventArgs e, Popup p)
        {
            p.IsOpen = false;
        }

        //The Delete Tax Plan button deletes the selected tax plan but it can't delete the default slant tax plan
        private void deleteTaxPlanButtonClick()
        {
            if (SelectedTaxPlanName != "Slant Tax")
            {
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
            DataVM.TotalRecalculation();

            GraphVM.ClearSeries();

            GraphVM.GraphAllChecked();


        }

        //Graph reset that doesn't recalculate Slant Tax Data
        //Used for manually changing tax rates
        private void customGraphReset()
        {
            GraphVM.ClearSeries();
            GraphVM.GraphAllChecked();
        }

        private void customGraphReset(List<double> customRates) {
            GraphVM.ClearSeries();
            GraphVM.GraphAllChecked(customRates);
        }

        // Collections of calls to update only the new data (e.g. max % or # of max brackets changed)
        private void newDataGraphReset()
        {

            // Recalculate all the *new* data only
            DataVM.NewDataRecalcuation();

            // Clear the graph
            GraphVM.ClearSeries();

            // Graph everything that is checked
            GraphVM.GraphAllChecked();

        }

        // Colections of calls to update only what is shown on the graph (e.g. checkboxes ticked/unticked)
        private void displayOnlyGraphReset()
        {

            // Clear the graph
            GraphVM.ClearSeries();

            // Graph everything that is checked
            GraphVM.GraphAllChecked();

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
    }

}