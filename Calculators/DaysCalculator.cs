using System;

namespace HouseDashboardServer.Utils
{
    public class DaysCalculator
    {
        private const int DEFAULT_DAYS = 3;
        
        public static int DaysSinceDateFrom(DateTime dateFrom)
        {
            var days = DEFAULT_DAYS;

            if (dateFrom != DateTime.Today)
            {
                var ts = dateFrom.Subtract(DateTime.Today);
                days = Math.Abs(ts.Days); // fromDate in future be damned
            }

            return days;
        }
    }
}