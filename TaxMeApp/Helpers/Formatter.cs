using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxMeApp.Helpers
{
    public class Formatter
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

        public static string FormatLabel(double value)
        {
            double newValue;

            // less than a thousand - handle exactly
            if (Math.Abs(value) <= 999)
            {
                return $"{value:c0}";
            }

            // greater than a thousand and less than a million - add a 'k' to signify thousands
            if (Math.Abs(value) <= 999999)
            {
                newValue = value/1000;

                return $"{newValue:c0}k";
            }

            // a million or more - add a 'm' to signify millions
            if (Math.Abs(value) >= 1000000)
            {
                newValue = value / 1000000;

                // if newValue has decimal part != 0, include decimal in output result (ex: 1.5m)
                // otherwise, return as whole number without decimal part (ex: 1m)
                return (newValue % 1 == 0? $"{newValue:c0}m" : $"{newValue:c1}m");
            }

            return "error";
        }
    }
}
