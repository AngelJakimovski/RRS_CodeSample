using AARP.Models;
using AARP.Services.AppConfigs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Services
{
    public class ReviewerProvider : BaseModelProvider<Reviewer>
    {
        public Reviewer GetByEmail(string email)
        {
            using (var dbContext = new AARPDbContext())
            {
                return dbContext.Reviewers.FirstOrDefault(r => r.Email.ToLower().Contains(email.ToLower()));
            }
        }

        public Reviewer[] GetWorkingReviewers(CommonConfigs commonConfigs)
        {
            using (var dbContext = new AARPDbContext())
            {
                return dbContext.Reviewers
                                .Where(rvr => rvr.IsInWorkingHours(commonConfigs.StartTimeOfDay, commonConfigs.EndTimeOfDay, commonConfigs.EnabledDays))
                                .ToArray();
            }
        }

        public int GetNextId()
        {
            using (var dbContext = new AARPDbContext())
            {
                if (dbContext.Reviewers.Count() == 0)
                    return 1;

                return dbContext.Reviewers.Select(t => t.Id).Max() + 1;
            }
        }
    }
}
