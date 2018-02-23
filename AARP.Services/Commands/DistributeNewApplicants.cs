using AARP.Models;
using AARP.Services.Emails;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AARP.WebApi.Commons;
using AARP.WebApi.Models;

namespace AARP.Services.Commands
{
    public class DistributeNewApplicants: ICommand
    {
        private static log4net.ILog _logger = LogManager.GetLogger(typeof(DistributeNewApplicants));
        public bool CanExecute()
        {
            return AppConfigsProvider.GetConfigs<AppConfigs.DistributeApplicantsConfigs>().IsEnabled;
        }

        public void Execute()
        {
            if (!CanExecute())
                return;

            var applicantService = new JobApplicationProvider();
            JobApplication[] freshApplicants = applicantService.GetList(ap => ap.ReviewStatus == Models.ReviewStatus.New);

            foreach (var applicant in freshApplicants)
            {
                AssignToReviewer(applicant);
            }
        }

        private void AssignToReviewer(Models.JobApplication applicant)
        {
            if (applicant == null || applicant.ReviewStatus != ReviewStatus.New)
                return;
            var applicantService = new JobApplicationProvider();
            var positionTypeService = new JobPositionTypeProvider();
            var reviewerService = new ReviewerProvider();
            var ghDataStore = GreenHouseDataStore.Instance;
            var job = ghDataStore.GetJobById(applicant.JobId);

            if (job == null || job.Status != JobStates.Open)
            {
                applicant.ReviewStatus = ReviewStatus.JobClosed;
                applicantService.Update(applicant);

                _logger.InfoFormat("Applicant {0} is closed because it's job ({1}) is either deleted or not opened", applicant.Id, job?.Id);
                return;
            }

            if (applicant.Source == ApplicantSources.Referral && AppConfigsProvider.ReferralsHandlingConfigs.IsEnabled)
            {
                applicant.ReviewStatus = ReviewStatus.HandledAsReferral;
                applicantService.Update(applicant);
                SendEmailForReferral(applicant, job);

                _logger.InfoFormat("Applicant {0} of job {1} has been handled as a referral", applicant.Id, job.Id);
                return;
            }

            var positionTypeStr = job.GetCustomFieldValue(JobCustomFields.PositionType);
            var positionType = positionTypeService.GetList(p => p.Name == positionTypeStr).FirstOrDefault();
            if (positionType == null)
            {
                _logger.WarnFormat("Position Type '{0}' is not available.", positionTypeStr);
                return;
            }
            var selectedReviewer = GetNextReviewer(positionType);
            if (selectedReviewer == null)
            {
                _logger.WarnFormat("Can not get any reviewers for the applicant {0}. It's either because there is no reviewers configured for this position type or all reviewers all not in working hours.", applicant.Id);
                return;
            }

            selectedReviewer.RecentAssignedAt = DateTime.UtcNow;
            selectedReviewer.AssignedCount++;

            applicant.ReviewerId = selectedReviewer.Id;
            applicant.ReviewStatus = ReviewStatus.Assigned;
            applicant.AssignedToReviewerAt = DateTime.UtcNow;

            applicantService.Update(applicant);
            reviewerService.Update(selectedReviewer);

            _logger.InfoFormat("Applicant {0} has been assigned to reviewer {1}", applicant.Id, selectedReviewer.Id);
            SendAssignmentEmail(selectedReviewer, applicant, job);
        }

        private void SendEmailForReferral(JobApplication applicant, ApiJob job)
        {
            if (applicant == null || applicant.Source != ApplicantSources.Referral)
                return;
            var referralEmail = new NotificationOfNewReferralEmail()
            {
                Applicant = applicant,
                Job = job,
                SendTo = AppConfigsProvider.ReferralsHandlingConfigs.ReportToEmails
            };
            BackgroundEmailService.Create().Send(referralEmail);
        }

        public Reviewer GetNextReviewer(JobPositionType positionType)
        {
            var reviewerService = new ReviewerProvider();
            var commonConfigs = AppConfigsProvider.CommonConfigs;

            if (positionType == null || string.IsNullOrEmpty(positionType.Interviewers))
                return null;

            var reviewers = positionType.Interviewers.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(email => email.ToLower().Trim())
                .Select(email => reviewerService.GetByEmail(email))
                .Where(rvr => rvr != null && rvr.IsInWorkingHours(commonConfigs.StartTimeOfDay, commonConfigs.EndTimeOfDay, commonConfigs.EnabledDays))
                .Distinct();

            return reviewers.OrderBy(rvr => rvr.RecentAssignedAt ?? DateTime.MinValue)
                .FirstOrDefault();
        }

        private void SendAssignmentEmail(Reviewer to, JobApplication applicant, ApiJob job)
        {
            var email = new ReviewApplicationEmail()
            {
                Reviewer = to,
                JobApplication = applicant,
                Job = job,
            };
            BackgroundEmailService.Create().Send(email);
            System.Diagnostics.Trace.WriteLine("New review email has been sent to the reviewer {0}", email.Reviewer.Name);
        }
    }
}
