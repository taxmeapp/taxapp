using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxMeApp.Helpers
{
    public static class Formatter
    {

        public static string Format(long value)
        {

            // we're going to want a decimal representation
            double newValue = value;

            if (Math.Abs(newValue) <= 999999)
            {
                // if  between 0 - 999,999. Format it with commas only
                return newValue.ToString("#,##0");
            }

            // Take the absolute value (to handle the negative cases) and compare to the value

            // Because of the rounding in the logic, cutoff values are at the rounding point (999.50)

            // a trillion or more 
            if (Math.Abs(newValue) >= 999500000000)
            {
                newValue = Math.Round(newValue / 1000000000000, 2);

                return newValue.ToString("F" + 2) + " trillion";
            }

            // a billion or more
            if (Math.Abs(newValue) >= 999500000)
            {
                newValue = Math.Round(newValue / 1000000000, 2);

                return newValue.ToString("F" + 2) + " billion";
            }

            // a million or more - explicit cutoff because 999,999 and below is handled exactly
            if (Math.Abs(newValue) >= 1000000)
            {
                newValue = Math.Round(newValue / 1000000, 2);

                return newValue.ToString("F" + 2) + " million";
            }

            return "error";

        }
        public static string Format(double value) {
            // we're going to want a decimal representation
            double newValue = value;

            if (Math.Abs(newValue) <= 999999)
            {
                // if  between 0 - 999,999. Format it with commas only
                return newValue.ToString("#,##0");
            }

            // Take the absolute value (to handle the negative cases) and compare to the value

            // Because of the rounding in the logic, cutoff values are at the rounding point (999.50)

            // a trillion or more 
            if (Math.Abs(newValue) >= 999500000000)
            {
                newValue = Math.Round(newValue / 1000000000000, 2);

                return newValue.ToString("F" + 2) + " trillion";
            }

            // a billion or more
            if (Math.Abs(newValue) >= 999500000)
            {
                newValue = Math.Round(newValue / 1000000000, 2);

                return newValue.ToString("F" + 2) + " billion";
            }

            // a million or more - explicit cutoff because 999,999 and below is handled exactly
            if (Math.Abs(newValue) >= 1000000)
            {
                newValue = Math.Round(newValue / 1000000, 2);

                return newValue.ToString("F" + 2) + " million";
            }

            return "error";
        }

    }
}
