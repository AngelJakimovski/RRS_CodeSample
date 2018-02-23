using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AARP.Services
{
    public static class DateTimeUtils
    {
        public static bool IsTimeOut(this DateTime recorded, TimeSpan timeAllowed, bool incluedWeekend = true)
        {
            if (incluedWeekend)
            {
                if (recorded + timeAllowed >= DateTime.Now)
                    return true;
                else
                    return false;
            }
            else
            {
                var recordedWeek = recorded.GetWeekOfYear();
                var currentWeek = DateTime.Now.GetWeekOfYear();
                var bufferDays = (currentWeek - recordedWeek) * 2;
                var bufferTime = new TimeSpan(bufferDays, 0, 0, 0);

                if (recorded + timeAllowed + bufferTime >= DateTime.Now)
                    return true;

                return false;
            }
        }
        public static int GetWeekOfYear(this DateTime d)
        {
            return System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(d, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        public static bool IsWeekend(this DateTime time)
        {
            return time.DayOfWeek == DayOfWeek.Saturday || time.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}