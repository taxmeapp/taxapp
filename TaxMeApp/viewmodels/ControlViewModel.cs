using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using TaxMeApp.Helpers;
using TaxMeApp.models;

namespace TaxMeApp.viewmodels
{
    public class ControlViewModel : MainViewModel
    {


        public ICommand SettingsBtnCommand { get; set; }

        public ControlViewModel()
        {

            SettingsBtnCommand = new RelayCommand(o => settingsButtonClick(""));

        }

        // ControlPanel Init
        public void ControlInit()
        {

            // Automatically select the first item in the list
            SelectedYear = YearsModel.YearList[0];

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

            }
        }

        // Number of brackets living at/under poverty line
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

        // Updates graph after number of brackets at/under poverty is changed
        public int PovertyLineIndexSlider
        {
            get
            {
                return GraphModel.PovertyLineIndex;
            }
            set
            {
                GraphModel.PovertyLineIndex = value;

                totalGraphReset(); // may only need to update poverty population

                OnPropertyChange("PovertyLineIndex");

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

    }

}
