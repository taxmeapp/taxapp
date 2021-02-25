using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TaxMeApp.Helpers;

namespace UnitTests
{
    [TestClass]
    public class FormatterUnitTest
    {
        
        [TestMethod]
        public void TrillionOneDecimal()
        {
            // One trillion + 1 decimal place
            Assert.AreEqual("1.20 trillion", Formatter.Format(1200000000000));
        }

        [TestMethod]
        public void TrillionTwoDecimal()
        {
            // One trillion + 2 decimal places
            Assert.AreEqual("1.23 trillion", Formatter.Format(1230000000000));
        }

        [TestMethod]
        public void TrillionThreeDecimalNoRound()
        {
            // One trillion + 2 decimal places
            Assert.AreEqual("1.23 trillion", Formatter.Format(1230000000000));
        }

        [TestMethod]
        public void TrillionThreeDecimalRounding()
        {
            // One trillion + 3 decimal places, no round
            Assert.AreEqual("1.23 trillion", Formatter.Format(1234000000000));
        }

        [TestMethod]
        public void TrillionLowestValue()
        {
            // Lowest value for trillion:
            // 999,500,000,000 <- this should round to trillion
            Assert.AreEqual("1.00 trillion", Formatter.Format(999500000000));
        }

        [TestMethod]
        public void BillionHighestValue()
        {
            // Highest value for billion:
            // 999,459,999,999
            Assert.AreEqual("999.50 billion", Formatter.Format(999499999999));
        }

        [TestMethod]
        public void BillionOneDecimal()
        {
            // One billion + 1 decimal place
            Assert.AreEqual("1.20 billion", Formatter.Format(1200000000));
        }

        [TestMethod]
        public void BillionTwoDecimal()
        {
            // One billion + 2 decimal places
            Assert.AreEqual("1.23 billion", Formatter.Format(1230000000));
        }

        [TestMethod]
        public void BillionThreeDecimalNoRound()
        {
            // One billion + 3 decimal places, no round
            Assert.AreEqual("1.23 billion", Formatter.Format(1234000000));
        }

        [TestMethod]
        public void BillionThreeDecimalRounding()
        {
            // One billion + 3 decimal places, with round
            Assert.AreEqual("1.24 billion", Formatter.Format(1235000000));
        }

        [TestMethod]
        public void BillionLowestValue()
        {
            // Lowest value for billion:
            // 999,500,000 <- this should round to billion
            Assert.AreEqual("1.00 billion", Formatter.Format(999500000));
        }


        [TestMethod]
        public void MillionHighestValue()
        {
            // Highest value for million:
            // 999,499,999 
            Assert.AreEqual("999.50 million", Formatter.Format(999499999));
        }

        [TestMethod]
        public void MillionOneDecimal()
        {
            // One million + 1 decimal place
            Assert.AreEqual("1.20 million", Formatter.Format(1200000));
        }

        [TestMethod]
        public void MillionTwoDecimal()
        {
            // One million + 2 decimal places
            Assert.AreEqual("1.23 million", Formatter.Format(1230000));
        }

        [TestMethod]
        public void MillionThreeDecimalNoRound()
        {
            // One million + 3 decimal places, no round
            Assert.AreEqual("1.23 million", Formatter.Format(1234000));
        }

        [TestMethod]
        public void MillionThreeDecimalRounding()
        {
            // One million + 3 decimal places, with round
            Assert.AreEqual("1.24 million", Formatter.Format(1235000));
        }

        [TestMethod]
        public void MillionLowestValue()
        {
            // Lowest value for million:
            // 1,000,000 (because we catch 999,999 and below explicitly before we evaluate value) 
            Assert.AreEqual("1.00 million", Formatter.Format(1000000));
        }

        [TestMethod]
        public void StraightHighestValue()
        {
            // 999,999 Highest value for just commas
            Assert.AreEqual("999,999", Formatter.Format(999999));
        }

        [TestMethod]
        public void StraightLowestValue()
        {
            // 0 lowest value for just commas
            Assert.AreEqual("0", Formatter.Format(0));
        }

        [TestMethod]
        public void NegativeStraightHighestValue()
        {
            // -1 Highest negative value for just commas
            Assert.AreEqual("-1", Formatter.Format(-1));
        }

        [TestMethod]
        public void NegativeStraightLowestValue()
        {
            // -999,999 Lowest value for just commas
            Assert.AreEqual("-999,999", Formatter.Format(-999999));
        }

        [TestMethod]
        public void NegativeMillionHighestValue()
        {
            // -1 million Highest for negative million
            Assert.AreEqual("-1.00 million", Formatter.Format(-1000000));
        }

        [TestMethod]
        public void NegativeMillionLowestValue()
        {
            // -999,499,999 Lowest for negative million
            Assert.AreEqual("-999.50 million", Formatter.Format(-999499999));
        }

        [TestMethod]
        public void NegativeBillionHighestValue()
        {
            // -999,500,000 Highest for negative billion
            Assert.AreEqual("-1.00 billion", Formatter.Format(-999500000));
        }

        [TestMethod]
        public void NegativeBillionLowestValue()
        {
            // -999,459,999,999 Lowest for negative billion
            Assert.AreEqual("-999.50 billion", Formatter.Format(-999499999999));
        }

        [TestMethod]
        public void NegativeTillionHighestValue()
        {
            // -999,500,000,000 Highest for negative trillion
            Assert.AreEqual("-1.00 trillion", Formatter.Format(-999500000000));
        }

    }
}
