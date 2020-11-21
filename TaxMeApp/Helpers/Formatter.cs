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

            // Take the absolute value (to handle the negative cases) and compare to the value

            // a trillion or more 
            if (Math.Abs(newValue) >= 1000000000000)
            {
                newValue = Math.Round(newValue / 1000000000000, 2);

                return newValue.ToString("F" + 2) + " trillion";
            }

            // a billion or more
            if (Math.Abs(newValue) >= 1000000000)
            {
                newValue = Math.Round(newValue / 1000000000, 2);

                return newValue.ToString("F" + 2) + " billion";
            }

            // a million or more
            if (Math.Abs(newValue) >= 1000000)
            {
                newValue = Math.Round(newValue / 1000000, 2);

                return newValue.ToString("F" + 2) + " million";
            }

            // otherwise it's 0 - 999,999. Format it appropriately
            return newValue.ToString("#,##0");

        }

    }
}
