using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Models
{
    public partial class JobApplication : IAARPEntity
    {
        public bool NeedToRemind(TimeSpan delayTime)
        {
            if (AssignedToReviewerAt == null)
                return false;

            var recentActivityAt = RecentRemindAt.HasValue ? RecentRemindAt.Value : AssignedToReviewerAt.Value;
            return DateTime.UtcNow >= recentActivityAt + delayTime;
        }
    }
}
