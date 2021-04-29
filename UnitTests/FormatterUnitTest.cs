using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TaxMeApp.Helpers;

namespace UnitTests
{
    [TestClass]
    public class FormatterUnitTest
    {
        
        [TestMethod]
        public void TC001_TrillionOneDecimal()
        {
            // One trillion + 1 decimal place
            Assert.AreEqual("1.20 trillion", Formatter.Format(1200000000000));
        }

        [TestMethod]
        public void TC002_TrillionTwoDecimal()
        {
            // One trillion + 2 decimal places
            Assert.AreEqual("1.23 trillion", Formatter.Format(1230000000000));
        }

        [TestMethod]
        public void TC003_TrillionThreeDecimalNoRound()
        {
            // One trillion + 2 decimal places
            Assert.AreEqual("1.23 trillion", Formatter.Format(1230000000000));
        }

        [TestMethod]
        public void TC004_TrillionThreeDecimalRounding()
        {
            // One trillion + 3 decimal places, no round
            Assert.AreEqual("1.23 trillion", Formatter.Format(1234000000000));
        }

        [TestMethod]
        public void TC005_TrillionLowestValue()
        {
            // Lowest value for trillion:
            // 999,500,000,000 <- this should round to trillion
            Assert.AreEqual("1.00 trillion", Formatter.Format(999500000000));
        }

        [TestMethod]
        public void TC006_BillionHighestValue()
        {
            // Highest value for billion:
            // 999,459,999,999
            Assert.AreEqual("999.50 billion", Formatter.Format(999499999999));
        }

        [TestMethod]
        public void TC007_BillionOneDecimal()
        {
            // One billion + 1 decimal place
            Assert.AreEqual("1.20 billion", Formatter.Format(1200000000));
        }

        [TestMethod]
        public void TC008_BillionTwoDecimal()
        {
            // One billion + 2 decimal places
            Assert.AreEqual("1.23 billion", Formatter.Format(1230000000));
        }

        [TestMethod]
        public void TC009_BillionThreeDecimalNoRound()
        {
            // One billion + 3 decimal places, no round
            Assert.AreEqual("1.23 billion", Formatter.Format(1234000000));
        }

        [TestMethod]
        public void TC010_BillionThreeDecimalRounding()
        {
            // One billion + 3 decimal places, with round
            Assert.AreEqual("1.24 billion", Formatter.Format(1235000000));
        }

        [TestMethod]
        public void TC011_BillionLowestValue()
        {
            // Lowest value for billion:
            // 999,500,000 <- this should round to billion
            Assert.AreEqual("1.00 billion", Formatter.Format(999500000));
        }


        [TestMethod]
        public void TC012_MillionHighestValue()
        {
            // Highest value for million:
            // 999,499,999 
            Assert.AreEqual("999.50 million", Formatter.Format(999499999));
        }

        [TestMethod]
        public void TC013_MillionOneDecimal()
        {
            // One million + 1 decimal place
            Assert.AreEqual("1.20 million", Formatter.Format(1200000));
        }

        [TestMethod]
        public void TC014_MillionTwoDecimal()
        {
            // One million + 2 decimal places
            Assert.AreEqual("1.23 million", Formatter.Format(1230000));
        }

        [TestMethod]
        public void TC015_MillionThreeDecimalNoRound()
        {
            // One million + 3 decimal places, no round
            Assert.AreEqual("1.23 million", Formatter.Format(1234000));
        }

        [TestMethod]
        public void TC016_MillionThreeDecimalRounding()
        {
            // One million + 3 decimal places, with round
            Assert.AreEqual("1.24 million", Formatter.Format(1235000));
        }

        [TestMethod]
        public void TC017_MillionLowestValue()
        {
            // Lowest value for million:
            // 1,000,000 (because we catch 999,999 and below explicitly before we evaluate value) 
            Assert.AreEqual("1.00 million", Formatter.Format(1000000));
        }

        [TestMethod]
        public void TC018_StraightHighestValue()
        {
            // 999,999 Highest value for just commas
            Assert.AreEqual("999,999", Formatter.Format(999999));
        }

        [TestMethod]
        public void TC019_StraightLowestValue()
        {
            // 0 lowest value for just commas
            Assert.AreEqual("0", Formatter.Format(0));
        }

        [TestMethod]
        public void TC020_NegativeStraightHighestValue()
        {
            // -1 Highest negative value for just commas
            Assert.AreEqual("-1", Formatter.Format(-1));
        }

        [TestMethod]
        public void TC021_NegativeStraightLowestValue()
        {
            // -999,999 Lowest value for just commas
            Assert.AreEqual("-999,999", Formatter.Format(-999999));
        }

        [TestMethod]
        public void TC022_NegativeMillionHighestValue()
        {
            // -1 million Highest for negative million
            Assert.AreEqual("-1.00 million", Formatter.Format(-1000000));
        }

        [TestMethod]
        public void TC023_NegativeMillionLowestValue()
        {
            // -999,499,999 Lowest for negative million
            Assert.AreEqual("-999.50 million", Formatter.Format(-999499999));
        }

        [TestMethod]
        public void TC024_NegativeBillionHighestValue()
        {
            // -999,500,000 Highest for negative billion
            Assert.AreEqual("-1.00 billion", Formatter.Format(-999500000));
        }

        [TestMethod]
        public void TC025_NegativeBillionLowestValue()
        {
            // -999,459,999,999 Lowest for negative billion
            Assert.AreEqual("-999.50 billion", Formatter.Format(-999499999999));
        }

        [TestMethod]
        public void TC026_NegativeTillionHighestValue()
        {
            // -999,500,000,000 Highest for negative trillion
            Assert.AreEqual("-1.00 trillion", Formatter.Format(-999500000000));
        }

    }
}
