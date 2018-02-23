using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Models
{
    public partial class Reviewer : IAARPEntity
    {
        public bool IsInWorkingHours(TimeSpan startWorkingTime, TimeSpan endWorkingTime, params DayOfWeek[] workingDays)
        {
            if (workingDays == null || workingDays.Count() == 0)
                return false;

            var timeZoneInfo = !string.IsNullOrEmpty(TimeZone) ? TimeZoneInfo.FindSystemTimeZoneById(TimeZone) : null;
            if (timeZoneInfo == null)
                timeZoneInfo = TimeZoneInfo.Utc;

            var localTime = DateTime.UtcNow + timeZoneInfo.BaseUtcOffset;
            var fromTime = localTime - localTime.TimeOfDay + startWorkingTime;
            var toTime = localTime - localTime.TimeOfDay + endWorkingTime;
            return workingDays.Contains(localTime.DayOfWeek)
                && localTime >= fromTime
                && localTime <= toTime;
        }
    }
}
