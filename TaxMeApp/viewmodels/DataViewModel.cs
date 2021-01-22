using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxMeApp.models;

namespace TaxMeApp.viewmodels
{
    public class DataViewModel : MainViewModel
    {

        public void DataInit()
        {

            ControlVM.ControlInit();
            GraphVM.GraphInit();

        }


        /*
         
                Interaction with Models

        */


        // Brackets for selected year
        private ObservableCollection<BracketModel> selectedBrackets
        {
            get
            {
                return YearsModel.SelectedIncomeYearModel.Brackets;
            }
        }

        // Population array
        private ObservableCollection<int> population
        {
            get
            {
                return DataModel.Population;
            }
        }

        // Population count at or below poverty line
        private int numPovertyPop
        {
            get
            {
                return DataModel.NumPovertyPop;
            }
            set
            {
                DataModel.NumPovertyPop = value;
                // force update of OutputVM
                OutputVM.Update();
            }
        }

        // Number of brackets at max rate
        private int maxBracketCount
        {
            get
            {
                return GraphModel.MaxBracketCount;
            }
            set
            {
                ControlVM.MaxBracketCount = value;               
            }
        }

        // Population at max tax rate
        private int numMaxPop
        {
            set
            {
                DataModel.NumMaxPop = value;
                // force update of OutputVM
                OutputVM.Update();
            }
        }

        // Original revenue data
        private List<long> oldRevenueByBracket
        {
            get
            {
                return DataModel.OldRevenueByBracket;
            }
        }

        // Original % of income data
        private List<double> oldTaxPctByBracket
        {
            get
            {
                return DataModel.OldTaxPctByBracket;
            }
        }

        // Original Total Revenue
        private long totalRevenueOld
        {
            set
            {
                DataModel.TotalRevenueOld = value;
                // force update of OutputVM
                OutputVM.Update();
            }
        }

        // New revenue data
        private List<long> newRevenueByBracket
        {
            get
            {
                return DataModel.NewRevenueByBracket;
            }
        }

        // New % of total income
        private List<double> newTaxPctByBracket
        {
            get
            {
                return DataModel.NewTaxPctByBracket;
            }
        }

        // # of brackets in poverty
        private int povertyBrackets
        {
            get
            {
                return GraphModel.PovertyLineIndex;
            }
        }

        // Slant tax maximum rate that the user can adjust via slider on control panel
        private int maxTaxRate
        {
            get
            {
                return DataModel.MaxTaxRate;
            }
        }

        // New total revenue
        private long totalRevenueNew
        {
            set
            {
                DataModel.TotalRevenueNew = value;
                // force update of OutputVM
                OutputVM.Update();
            }
        }

        /*
         
                Calculation Logic

        */


        // Public Collection of calls to update all data (i.e. user selects new year):
        public void TotalRecalculation()
        {

            // Update our population counts
            // CalculatePopulation();
            calculatePopulation();

            // Count how many people are under poverty line
            countUnderPoverty();

            // Determine our baseline number of brackets at max rate
            determineBaselineMaxBracketCount();

            // Count how many people are at max tax rate
            countPopulationAtMaxRate();

            // Calculate how much tax revenue was generated under current plan
            calculateOldTaxData();

            // Calculate how much tax revenue was generated under new plan
            calculateNewTaxData();

        }

        public void NewDataRecalcuation()
        {

            // Count how many people are at max tax rate
            countPopulationAtMaxRate();

            // Calculate how much tax revenue was generated under new plan
            calculateNewTaxData();

        }


        // Private calls to do the actual updating:

        // Recalculate population collection
        private void calculatePopulation()
        {

            // Clear our current count
            population.Clear();

            // Iterating through all brackets:
            for (int i = 0; i < selectedBrackets.Count; i++)
            {

                population.Add(selectedBrackets[i].NumReturns);

            }

        }

        // Count the population that is included in the poverty brackets
        private void countUnderPoverty()
        {
            int povertyPop = 0;

            // Count the number of returns up to and including the designated poverty bracket
            //for (int i = 0; i <= povertyBrackets; i++)
            for (int i = 0; i <= 3; i++)
            {
                povertyPop += population[i];
            }

            numPovertyPop = povertyPop;

            //Trace.WriteLine("There are " + NumPovertyPop + " returns under poverty line.");

        }

        private void determineBaselineMaxBracketCount()
        {

            // Initialize population and bracket counters
            int maxPopCount = 0;
            int maxBracketCount = 0;

            int brackets = population.Count - 1;

            // Starting at the end of array, work backwards
            // as long as our max population count is below our poverty population count
            for (int i = brackets; maxPopCount < numPovertyPop && i >= 0; i--)
            {

                // Add to our counters
                maxPopCount += population[i];
                maxBracketCount++;

            }

            //Trace.WriteLine("There are " + maxBracketCount + " brackets at max rate.");

            // Save our value 
            this.maxBracketCount = maxBracketCount;

        }

        // Count the population that is at max tax rate
        private void countPopulationAtMaxRate()
        {

            // Initialize population and bracket counters
            int maxPopCount = 0;

            int brackets = population.Count - 1;

            // Starting at the end of array, work backwards
            // as long as our max population count is below our poverty population count
            for (int i = brackets; i > brackets - maxBracketCount; i--)
            {

                // Add to our counters
                maxPopCount += population[i];

            }

            //Trace.WriteLine("There are " + maxPopCount + " returns at max rate.");

            // Save our value
            numMaxPop = maxPopCount;

        }

        // Accumulate the data we need from original tax data
        private void calculateOldTaxData()
        {

            // Clear our old bracket data
            oldRevenueByBracket.Clear();
            oldTaxPctByBracket.Clear();

            long totalRevenueOld = 0;

            for (int i = 0; i < selectedBrackets.Count; i++)
            {

                // Get the actual value of income tax paid (values stored as 1000)
                long bracketIncomeTax = selectedBrackets[i].IncomeTax * 1000;

                // Add it to our count
                totalRevenueOld += bracketIncomeTax;

                // Add to our revenue bracket list
                oldRevenueByBracket.Add(bracketIncomeTax);

                // Get the percent of income gross and add to our Pct bracket list
                oldTaxPctByBracket.Add(selectedBrackets[i].PercentOfGrossIncomePaid);

            }

            // Save our value for display in output region
            this.totalRevenueOld = totalRevenueOld;

        }

        private void calculateNewTaxData()
        {

            // Clear our bracket lists so we can add to fresh lists
            newRevenueByBracket.Clear();
            newTaxPctByBracket.Clear();

            // Initialize our variables
            int middleCount = selectedBrackets.Count - maxBracketCount;
            long totalRevenueNew = 0;
            double rate = 0;

            // Initialize our index counter that is shared for all three loops
            int i = 0;

            // Handle poverty brackets
            for (; i <= povertyBrackets; i++)
            {

                // Revenue is 0
                newRevenueByBracket.Add(0);
                // Rate is 0
                newTaxPctByBracket.Add(0);

            }

            // Determine how many divisons we want to spread our increment over
            int divisions = middleCount - i + 1;

            // Determine the rate at how much to increment, and round to 1 decimal place
            double increment = Math.Round(maxTaxRate / (double)divisions, 1);

            // Incremental brackets
            for (; i < middleCount; i++)
            {

                rate = rate + increment;

                // Revenue is Taxable Income * Tax Rate
                long bracketRevenue = (long)(selectedBrackets[i].TaxableIncome * 1000 * rate / 100);
                totalRevenueNew += bracketRevenue;

                newRevenueByBracket.Add(bracketRevenue);

                // Rate is incremental
                newTaxPctByBracket.Add(rate);

            }

            // Max rate:
            for (; i < selectedBrackets.Count; i++)
            {

                // Revenue is Taxable Income * Max Rate
                long bracketRevenue = selectedBrackets[i].TaxableIncome * 1000 * maxTaxRate / 100;
                totalRevenueNew += bracketRevenue;

                newRevenueByBracket.Add(bracketRevenue);

                // Rate is max
                newTaxPctByBracket.Add(maxTaxRate);

            }

            // Save our total revenue calculation
            this.totalRevenueNew = totalRevenueNew;

        }



    }
}
