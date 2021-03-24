using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxMeApp.models;

namespace TaxMeApp.viewmodels
{
    public class DataViewModel : MainViewModel
    {

        public bool SlantChangesUBI { get; set; } = false;
        public bool UBIChangesSlant { get; set; } = false;


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

                if (OutputVM != null)
                {
                    OutputVM.Update();
                }

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
                GraphModel.MaxBracketCount = value;               
                ControlVM.MaxBracketCount = value;
            }
        }

        // Number of brackets recieving $0 UBI 
        private int maxUBIBracketCount
        {
            get
            {
                return GraphModel.MaxUBIBracketCount;
            }
            set
            {
                ControlVM.MaxUBIBracketCount = value;
                GraphModel.MaxUBIBracketCount = value;
            }
        }

        // Number of brackets recieving full UBI 
        private int minUBIBracketCount
        {
            get
            {
                return GraphModel.MinUBIBracketCount;
            }
            set
            {
                ControlVM.MinUBIBracketCount = value;
                GraphModel.MinUBIBracketCount = value;
            }
        }

        // max UBI rate 
        private int maxUBI
        {
            get
            {
                return GraphModel.MaxUBI;
            }
            set
            {
                ControlVM.MaxUBI = value;
                GraphModel.MaxUBI = value;
            }
        }

        // Population at max tax rate
        private int numMaxPop
        {
            set
            {
                DataModel.NumMaxPop = value;
                // force update of OutputVM
                if (OutputVM != null)
                {
                    OutputVM.Update();
                }

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
                if (OutputVM != null)
                {
                    OutputVM.Update();
                }

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
        
        // How much money each person would recieve based on the bracket that they are in
        private List<double> uBIPayOutByBracket
        {
            get
            {
                return DataModel.UBIPayOutByBracket;
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
            get
            {
                return DataModel.TotalRevenueNew;
            }
            set
            {
                DataModel.TotalRevenueNew = value;
                // force update of OutputVM
                if (OutputVM != null)
                {
                    OutputVM.Update();
                }

            }
        }

        // Total cost of implemting UBI
        private long totalUBICost
        {
            set
            {
                DataModel.TotalUBICost = value;
                // force update of OutputVM
                OutputVM.Update();
            }
        }

        // Mean income before tax
        private double PreTaxMean
        {
            get
            {
                return DataModel.PreTaxMean;
            }
            set
            {
                DataModel.PreTaxMean = value;
            }
        }

        // Median income before tax
        private double PreTaxMedian
        {
            get
            {
                return DataModel.PreTaxMedian;
            }
            set
            {
                DataModel.PreTaxMedian = value;
            }
        }

        // Median income after tax
        private double PostTaxMedian
        {
            get
            {
                return DataModel.PostTaxMedian;
            }
            set
            {
                DataModel.PostTaxMedian = value;
                OutputVM.Update();
            }
        }

        // Mean income after tax
        private double PostTaxMean
        {
            get
            {
                return DataModel.PostTaxMean;
            }
            set
            {
                DataModel.PostTaxMean = value;
            }
        }

        // Mean income after tax including each bracket's UBI payout
        private double PostTaxMeanUBI
        {
            get
            {
                return DataModel.PostTaxMeanWithUBI;
            }
            set
            {
                DataModel.PostTaxMeanWithUBI = value;
            }
        }

        // Median income after tax including each bracket's UBI payout
        private double PostTaxMedianUBI
        {
            get
            {
                return DataModel.PostTaxMedianWithUBI;
            }
            set
            {
                DataModel.PostTaxMedianWithUBI = value;
            }
        }

        // Bracket containing pre-tax mean
        private int PreTaxMeanBracket
        {
            get
            {
                return GraphModel.PreTaxMeanLine;
            }
            set
            {
                GraphVM.PreTaxMeanLine = value;
            }
        }

        // Bracket containing pre-tax median
        private int PreTaxMedianBracket
        {
            get
            {
                return GraphModel.PreTaxMedianLine;
            }
            set
            {
                GraphVM.PreTaxMedianLine = value;
            }
        }

        // Bracket containing post-tax mean
        private int PostTaxMeanBracket
        {
            get
            {
                return GraphModel.PostTaxMeanLine;
            }
            set
            {
                GraphVM.PostTaxMeanLine = value;
            }
        }

        // Bracket containing post-tax median
        private int PostTaxMedianBracket
        {
            get
            {
                return GraphModel.PostTaxMedianLine;
            }
            set
            {
                GraphVM.PostTaxMedianLine = value;
            }
        }

        // Bracket containing post-tax mean plus UBI
        private int PostTaxMeanUBIBracket
        {
            get
            {
                return GraphModel.PostTaxMeanUBILine;
            }
            set
            {
                GraphVM.PostTaxMeanUBILine = value;
            }
        }

        // Bracket containing post-tax mean plus UBI
        private int PostTaxMedianUBIBracket
        {
            get
            {
                return GraphModel.PostTaxMedianUBILine;
            }
            set
            {
                GraphVM.PostTaxMedianUBILine = value;
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
            //COMMENTED OUT so that the program wouldn't change max tax brackets when the poverty line changes
            //determineBaselineMaxBracketCount();
            


            

            // Count how many people are at max tax rate
            countPopulationAtMaxRate();

            // Calculate how much tax revenue was generated under current plan
            calculateOldTaxData();

            // Calculate how much tax revenue was generated under new plan
            calculateNewTaxData();

            // Calculate UBI cost
            calculateUBIByBracket();

            //Set default UBI max
            maxUBI = 1500;

            //Set default min UBIBrackets
            minUBIBracketCount = 3;

            // Calculate statistics pre/post-tax
            calculateMeanMedian();

        }

        public void NewDataRecalcuation()
        {

            // Count how many people are at max tax rate
            countPopulationAtMaxRate();

            // Calculate how much tax revenue was generated under new plan
            calculateNewTaxData();

            // Calculate UBI cost
            calculateUBIByBracket();

            // Calculate statistics pre/post-tax
            calculateMeanMedian();

        }

        // When variables, such as UBI or tax rates, are manually changed and don't require slant tax recalculation
        public void CustomDataRecalcuation()
        {

            // Calculate UBI cost
            calculateUBIByBracket();

            // Calculate statistics pre/post-tax
            calculateMeanMedian();

        }

        public void calculateMeanMedian()
        {
            // Calculate mean before tax
            CalculatePreTaxMean();

            // Calculate median before tax
            CalculatePreTaxMedian();

            // Calculate mean after tax
            CalculatePostTaxMean();

            // Calculate median after tax
            CalculatePostTaxMedian();
        }

        //Use to calculate revenue for custom tax plan
        public List<long> CalculateNewRevenues(ObservableCollection<double> taxRates)
        {

            totalRevenueNew = 0;

            List<long> ans = new List<long>();
            for (int i = 0; i < selectedBrackets.Count; i++)
            {
                ans.Add(Convert.ToInt64(selectedBrackets[i].TaxableIncome * 1000 * taxRates[i] / 100));
                totalRevenueNew += Convert.ToInt64(selectedBrackets[i].TaxableIncome * 1000 * taxRates[i] / 100);
            }
            return ans;
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
            for (int i = 0; i <= povertyBrackets; i++)
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
            this.maxUBIBracketCount = maxBracketCount;

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

            long totalRevenueNew = 0;
            double rate = 0;

            if (ControlVM.SelectedTaxPlanName == "Flat Tax")
            {
                rate = OptionsModel.FlatTaxRate;

                for(int i = 0; i < selectedBrackets.Count; i++)
                {
                    long bracketRevenue = (long)(selectedBrackets[i].TaxableIncome * 10 * rate);
                    totalRevenueNew += bracketRevenue;
                    newRevenueByBracket.Add(bracketRevenue);
                    newTaxPctByBracket.Add(rate);
                }
            }
            else
            {
                // The number of brackets that will be taxed incrementally:
                int middleCount = selectedBrackets.Count - maxBracketCount - (povertyBrackets + 1);

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
                int divisions = middleCount + 1;

                // Determine the rate at how much to increment, and round to 1 decimal place
                double increment = Math.Round(maxTaxRate / (double)divisions, 1);

                // Incremental brackets
                for (; i < selectedBrackets.Count - maxBracketCount; i++)
                {

                    rate = rate + increment;

                    // Revenue is Taxable Income * Tax Rate
                    long bracketRevenue = (long)(selectedBrackets[i].TaxableIncome * 10 * rate);
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
            }

            if (OptionsModel.YangRemoveChecked)
            {
                totalRevenueNew += 1066000000000;
            }

            // Save our total revenue calculation
            this.totalRevenueNew = totalRevenueNew;
        }

        public List<List<double>> CalculateSlantTaxData()
        {
            List<List<double>> ans = new List<List<double>>();
            List<double> rates = new List<double>();
            List<double> revenueByBracket = new List<double>();

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
                revenueByBracket.Add(0);
                // Rate is 0
                rates.Add(0);

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

                revenueByBracket.Add(bracketRevenue);

                // Rate is incremental
                rates.Add(rate);

            }

            // Max rate:
            for (; i < selectedBrackets.Count; i++)
            {

                // Revenue is Taxable Income * Max Rate
                long bracketRevenue = selectedBrackets[i].TaxableIncome * 1000 * maxTaxRate / 100;
                totalRevenueNew += bracketRevenue;

                revenueByBracket.Add(bracketRevenue);

                // Rate is max
                rates.Add(maxTaxRate);

            }

            if (OptionsModel.YangRemoveChecked)
            {
                totalRevenueNew += 1066000000000;
            }

            // Save our total revenue calculation
            this.totalRevenueNew = totalRevenueNew;

            ans.Add(rates);
            ans.Add(revenueByBracket);
            return ans;
        }

        //builds list of UBI by bracket
        private void calculateUBIByBracket()
        {

            // Clear our bracket lists so we can add to fresh lists
            uBIPayOutByBracket.Clear();

            // Initialize our variables
            int middleCount = selectedBrackets.Count - maxUBIBracketCount;
            long totalUBICost = 0;
            //double maxUBI = 1500;
            double rate = 0;

            // Initialize our index counter that is shared for all three loops
            int i = 0;

            // Handle poverty brackets
            for (; i <= minUBIBracketCount; i++)
            {

                // Revenue is 0
                uBIPayOutByBracket.Add(maxUBI);
                totalUBICost += (long)((long)maxUBI * (long)population[i]);
            }

            // Determine how many divisons we want to spread our increment over
            int divisions = middleCount - i + 1;

            // Determine the rate at how much to increment, and round to 1 decimal place
            double increment = Math.Round(1 / (double)divisions, 2);

            // Incremental brackets
            for (; i < middleCount; i++)
            {

                rate = rate + increment;

                // Revenue is Taxable Income * Tax Rate
                double bracketUBI = maxUBI - (rate * maxUBI);
                long bracketUBICost = (long)(bracketUBI * population[i]);
                totalUBICost += bracketUBICost;

                uBIPayOutByBracket.Add(bracketUBI);
            }

            // Max rate:
            for (; i < selectedBrackets.Count; i++)
            {

                uBIPayOutByBracket.Add(0);

            }

            // Save our total revenue calculation
            this.totalUBICost = totalUBICost;

        }

        private void CalculatePreTaxMedian()
        {
            int frequency, totalFreq = 0, medianBracketIndex = 0;
            int[] cumulativeBracketFrequency = new int[selectedBrackets.Count];

            // get the total number of observations in the income year
            foreach (BracketModel bracket in selectedBrackets)
            {
                totalFreq += bracket.NumReturns;
            }

            // find the index of the bracket that contains the median
            // median bracket is first bracket that has a cumulative frequency that is greater than total num of observations (totalFreq) divided by 2
            // cumulative frequency is total number (sum) of observations (numReturns) up to (and including) the current bracket
            for (int i = 0; i < cumulativeBracketFrequency.Length; i++)
            {
                frequency = selectedBrackets[i].NumReturns;
                if (i == 0)
                {
                    cumulativeBracketFrequency[i] = frequency;
                    if(cumulativeBracketFrequency[i] > (totalFreq / 2))
                    {
                        medianBracketIndex = i;
                        break;
                    }
                }
                else
                {
                    cumulativeBracketFrequency[i] = cumulativeBracketFrequency[i-1] + frequency;
                    if (cumulativeBracketFrequency[i] > (totalFreq / 2))
                    {
                        medianBracketIndex = i;
                        break;
                    }
                }
            }

            // create an instance of the median bracket to retrieve relevant information
            BracketModel medianBracket = selectedBrackets[medianBracketIndex];
            frequency = medianBracket.NumReturns;

            // calculate width of median bracket's range
            int width = medianBracket.UpperBound - medianBracket.LowerBound;

            // find the difference between the total number of observations divided by 2 and the cumulative frequency of the bracket preceding the median bracket
            double difference = (totalFreq / 2) - cumulativeBracketFrequency[medianBracketIndex - 1];

            // calculate post tax median using grouped data median formula
            this.PreTaxMedian = medianBracket.LowerBound + (difference / frequency) * width;
            this.PreTaxMedianBracket = DetermineMeanMedianBracket(PreTaxMedian);
            //Console.WriteLine("Pre-tax median bracket = {0} | median: ${1}", PreTaxMedianBracket, PreTaxMedian);
        }

        private void CalculatePreTaxMean()
        {
            int frequency, totalFreq = 0;
            double midpoint, totalMidFreq = 0;

            foreach (BracketModel bracket in selectedBrackets)
            {
                // frequency equals total number of observations per bracket
                frequency = bracket.NumReturns;

                // calculate midpoint of bracket's range
                midpoint = (bracket.LowerBound + bracket.UpperBound) / 2;

                // increment running total of midpoint frequency and frequency by current bracket's
                totalMidFreq += (frequency * midpoint);
                totalFreq += frequency;
            }

            // calculate post-tax mean by dividing summation of midpoint-frequency by summation of frequency
            this.PreTaxMean = totalMidFreq / totalFreq;
            this.PreTaxMeanBracket = DetermineMeanMedianBracket(PreTaxMean);
            //Console.WriteLine("Pre-tax mean: ${0}, Bracket: {1}", PreTaxMean, PreTaxMeanBracket);
        }

        private void CalculatePostTaxMedian()
        {
            int frequency, totalFreq = 0, medianBracketIndex = 0;
            int[] cumulativeBracketFrequency = new int[selectedBrackets.Count];

            // get the total number of observations in the income year
            foreach (BracketModel bracket in selectedBrackets)
            {
                totalFreq += bracket.NumReturns;
            }

            // find the index of the bracket that contains the median
            // median bracket is first bracket that has a cumulative frequency that is greater than total num of observations (totalFreq) divided by 2
            // cumulative frequency is total number (sum) of observations (numReturns) up to (and including) the current bracket
            for (int i = 0; i < cumulativeBracketFrequency.Length; i++)
            {
                frequency = selectedBrackets[i].NumReturns;
                if (i == 0)
                {
                    cumulativeBracketFrequency[i] = frequency;
                    if (cumulativeBracketFrequency[i] > (totalFreq / 2))
                    {
                        medianBracketIndex = i;
                        break;
                    }
                }
                else
                {
                    cumulativeBracketFrequency[i] = cumulativeBracketFrequency[i - 1] + frequency;
                    if (cumulativeBracketFrequency[i] > (totalFreq / 2))
                    {
                        medianBracketIndex = i;
                        break;
                    }
                }
            }

            // create an instance of the median bracket to retrieve relevant information
            BracketModel medianBracket = selectedBrackets[medianBracketIndex];
            frequency = medianBracket.NumReturns;

            // calculate width of new (post-tax) bracket bounds after applying tax rates to pre-tax bounds
            double newLowerBound = medianBracket.LowerBound * (100 - newTaxPctByBracket[medianBracketIndex]) / 100;
            double newUpperBound = medianBracket.UpperBound * (100 - newTaxPctByBracket[medianBracketIndex]) / 100;
            double width = newUpperBound - newLowerBound;

            double newLowerBoundWithUBI = newLowerBound + uBIPayOutByBracket[medianBracketIndex];
            double newUpperBoundWithUBI = newUpperBound + uBIPayOutByBracket[medianBracketIndex];
            double widthUBI = newUpperBoundWithUBI - newLowerBoundWithUBI;

            // find the difference between the total number of observations divided by 2 and the cumulative frequency of the bracket preceding the median bracket
            double difference = (totalFreq / 2) - cumulativeBracketFrequency[medianBracketIndex - 1];

            // calculate post tax median using grouped data median formula
            this.PostTaxMedianUBI = newLowerBoundWithUBI + (difference / frequency) * widthUBI;
            this.PostTaxMedian = newLowerBound + (difference / frequency) * width;
            this.PostTaxMedianBracket = DetermineMeanMedianBracket(PostTaxMedian);
            this.PostTaxMedianUBIBracket = DetermineMeanMedianBracket(PostTaxMedianUBI);
            //Console.WriteLine("Post-tax median bracket = {0} | median: ${1}", PostTaxMedianBracket, PostTaxMedian);
        }

        private void CalculatePostTaxMean()
        {
            int totalFreq = 0, index = 0;
            double totalMidFreq = 0, totalMidFreqWithUBI = 0;

            foreach (BracketModel bracket in selectedBrackets)
            {
                // frequency equals total number of observations per bracket
                int frequency = bracket.NumReturns;

                // calculate new (post-tax) bracket range after applying tax rate to bounds
                double newLowerBound = bracket.LowerBound * (100 - newTaxPctByBracket[index]) / 100;
                double newUpperBound = bracket.UpperBound * (100 - newTaxPctByBracket[index]) / 100;
                double newLowerBoundWithUBI = newLowerBound + uBIPayOutByBracket[index];
                double newUpperBoundWithUBI = newUpperBound + uBIPayOutByBracket[index];

                // find midpoint of new bracket range
                double midpoint = (newLowerBound + newUpperBound) / 2;
                double midpointWithUBI = (newLowerBoundWithUBI + newUpperBoundWithUBI) / 2;

                // increment running total of midpoint frequency and frequency by current bracket's 
                totalMidFreq += (frequency * midpoint);
                totalMidFreqWithUBI += (frequency * midpointWithUBI);
                totalFreq += frequency;
                ++index;
            }

            // calculate post-tax mean by dividing summation of midpoint-frequency by summation of frequency
            this.PostTaxMean = totalMidFreq / totalFreq;
            this.PostTaxMeanUBI = totalMidFreqWithUBI / totalFreq;
            this.PostTaxMeanBracket = DetermineMeanMedianBracket(PostTaxMean);
            this.PostTaxMeanUBIBracket = DetermineMeanMedianBracket(PostTaxMeanUBI);
            //Console.WriteLine("Post-tax mean: ${0}, Bracket: {1}", PostTaxMean, PostTaxMeanBracket);
        }

        private int DetermineMeanMedianBracket(double income) {
            int bracketIndex = 0;
            foreach(BracketModel bracket in selectedBrackets)
            {
                if(income >= bracket.LowerBound && income <= bracket.UpperBound)
                {
                    bracketIndex = selectedBrackets.IndexOf(bracket);
                }
            }
            return bracketIndex;
        }

    }
}