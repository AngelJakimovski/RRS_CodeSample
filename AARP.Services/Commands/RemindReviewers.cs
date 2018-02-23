using AARP.Infrashtructure.DI;
using AARP.Models;
using AARP.Services.Commands;
using AARP.Services.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using log4net;

namespace AARP.Services.Commands
{
    public class RemindReviewers : ICommand
    {
        private static ILog _logger = LogManager.GetLogger(typeof(RemindReviewers));

        public bool CanExecute()
        {
            return AppConfigsProvider.RemindReviewersConfigs.IsEnabled;
        }

        public void Execute()
        {
            var applicantsProvider = new JobApplicationProvider();
            var commonConfigs = AppConfigsProvider.CommonConfigs;
            var remindConfigs = AppConfigsProvider.RemindReviewersConfigs;
            var reviewerProvider = new ReviewerProvider();
            //var greenHouseApi = DiContainers.Global.Resolve<IWebApiClient>();
            var applicants = GetTimeOutApplicants();
            var ghStore = GreenHouseDataStore.Instance;

            if (applicants.Count() == 0)
                return;

            Reviewer reviewer = null;
            foreach (var applicant in applicants)
            {
                if (applicant.ReviewerId.HasValue)
                    reviewer = reviewerProvider.Get(applicant.ReviewerId.Value);

                if (reviewer == null)
                {
                    applicant.ReviewStatus = ReviewStatus.Error;
                    applicantsProvider.Update(applicant);
                    continue;
                }

                if (!reviewer.IsInWorkingHours(commonConfigs.StartTimeOfDay, commonConfigs.EndTimeOfDay, commonConfigs.EnabledDays))
                {
                    _logger.InfoFormat("Don't remind reviewer {0} ({1} because he's not in working hour", reviewer.Id, reviewer.Name);
                    continue;
                }

                applicant.RemindCount++;
                applicant.RecentRemindAt = DateTime.UtcNow;
                applicantsProvider.Update(applicant);
                var email = new RemindReviewApplicationEmail()
                {
                    JobApplication = applicant,
                    Reviewer = reviewer,
                    Job = ghStore.GetJobById(applicant.JobId)
                };
                BackgroundEmailService.Create().Send(email);
            }
        }

        public JobApplication[] GetTimeOutApplicants()
        {
            var applicantsProvider = new JobApplicationProvider();
            var remindDelayTime = AppConfigsProvider.RemindReviewersConfigs.DelayTime;

            return applicantsProvider.GetList(a => a.ReviewStatus == Models.ReviewStatus.Assigned)
                                     .Where(a => a.NeedToRemind(remindDelayTime))
                                     .ToArray();
        }
    }
}
