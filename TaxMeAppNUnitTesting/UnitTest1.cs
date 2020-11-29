using TaxMeApp;
using TaxMeApp.Helpers;
using NUnit.Framework;
using System;

namespace TaxMeAppNUnitTesting
{
    public class Tests
    {        
        [SetUp]
        public void Setup()
        {
        }

        //Testing Helpers
        [Test]
        public void TestFormatter()
        {
            Random r = new Random();
            long t1, t2, t3, t4, t5, t6, t7, t8;
            string t3Ans, t4Ans, t5Ans, t6Ans, t7Ans, t8Ans;

            //Generate Random numbers to test
            //1st Set of Cases, numbers < 1,000 and numbers > 1,000 but < 1,000,000
            t1 = r.Next(1, 999);
            t2 = r.Next(1, 999) * (long) Math.Pow(10, 3);
            //2nd Set of Cases, numbers in the millions
            t3 = r.Next(1, 999) * (long)Math.Pow(10, 6);
            t4 = r.Next(1, 999) * (long)Math.Pow(10, 6);
            //3rd Set of Cases, numbers in the billions
            t5 = r.Next(1, 999) * (long)Math.Pow(10, 9);
            t6 = r.Next(1, 999) * (long)Math.Pow(10, 9);
            //4th Set of Cases, numbers in the trillions
            t7 = r.Next(1, 999) * (long)Math.Pow(10, 12);
            t8 = r.Next(1, 999) * (long)Math.Pow(10, 12);

            //For the millions, billions and trillions format the answer manually
            t3Ans = (t3 / (1 * Math.Pow(10, 6))).ToString(".00") + " million";
            t4Ans = (t4 / (1 * Math.Pow(10, 6))).ToString(".00") + " million";
            t5Ans = (t5 / (1 * Math.Pow(10, 9))).ToString(".00") + " billion";
            t6Ans = (t6 / (1 * Math.Pow(10, 9))).ToString(".00") + " billion";
            t7Ans = (t7 / (1 * Math.Pow(10, 12))).ToString(".00") + " trillion";
            t8Ans = (t8 / (1 * Math.Pow(10, 12))).ToString(".00") + " trillion";

            //Test Each Case
            Assert.AreEqual(t1.ToString(), Formatter.Format(t1));
            Assert.AreEqual(t2.ToString("#,##0"), Formatter.Format(t2));
            Assert.AreEqual(t3Ans, Formatter.Format(t3));
            Assert.AreEqual(t4Ans, Formatter.Format(t4));
            Assert.AreEqual(t5Ans, Formatter.Format(t5));
            Assert.AreEqual(t6Ans, Formatter.Format(t6));
            Assert.AreEqual(t7Ans, Formatter.Format(t7));
            Assert.AreEqual(t8Ans, Formatter.Format(t8));
        }

        [Test]
        public void TestLoader()
        {
            Assert.Pass();
        }
    }
}