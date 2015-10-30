//original license: MIT
//
//See the file license.txt for copying permission.

using System;

namespace screenium.Reports
{
    class DateSupport
    {
        internal static string ToString(DateTime date)
        {
            //just use local culture:
            return date.ToString();
        }

        internal static string ToString(TimeSpan timeSpan)
        {
            //assuming the test completes within 1 day!
            return timeSpan.Hours + "h " + timeSpan.Minutes + "min " + timeSpan.Seconds + "s";
        }
    }
}
