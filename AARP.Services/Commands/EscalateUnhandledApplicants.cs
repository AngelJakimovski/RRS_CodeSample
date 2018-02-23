using AARP.Models;
using AARP.Services.Emails;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Services.Commands
{
    public class EscalateUnhandledApplicants : ICommand
    {
        private static ILog _logger = LogManager.GetLogger(typeof(EscalateUnhandledApplicants));

        public bool CanExecute()
        {
            var selfConfigs = AppConfigsProvider.EscalationConfigs;
            return selfConfigs.IsEnabled && selfConfigs.GetReportToEmails().Count() != 0;
        }

        public void Execute()
        {
            var applicantsProvider = new JobApplicationProvider();
            var reviewerProvider = new ReviewerProvider();
            var configs = AppConfigsProvider.EscalationConfigs;
            Reviewer reviewer = null;

            var unhandledApplicants = applicantsProvider
                .GetList(a => a.ReviewStatus == Models.ReviewStatus.Assigned
                           && a.RemindCount >= configs.RemindTheshold);

            if (unhandledApplicants.Count() == 0)
                return;

            foreach (var applicant in unhandledApplicants)
            {
                if (applicant.ReviewerId.HasValue)
                    reviewer = reviewerProvider.Get(applicant.ReviewerId.Value);

                var email = new TimeOutApplicationEscalationEmail()
                   {
                       JobApplication = applicant,
                       Reviewer = reviewer,
                       EscalationTo = configs.ReportToEmails,
                   };
                BackgroundEmailService.Create().Send(email);

                _logger.InfoFormat("Reviewing for applicant {0} has been escalated as reviewer {1} did not finish his review after {2} of reminds", applicant.Id, reviewer.Id, applicant.RemindCount);
            }
        }
    }
}
