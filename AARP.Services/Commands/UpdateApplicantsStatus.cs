using AARP.Infrashtructure.DI;
using AARP.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AARP.WebApi.Commons;
using AARP.WebApi.Models;

namespace AARP.Services.Commands
{
    public class UpdateApplicantsStatus : ICommand
    {
        private static ILog _logger = LogManager.GetLogger(typeof(UpdateApplicantsStatus));

        public bool CanExecute()
        {
            return AppConfigsProvider.RefreshApplicantsConfigs.IsEnabled;
        }

        public void Execute()
        {
            var applicantsProvider = new JobApplicationProvider();
            var applicants2Update = applicantsProvider.GetList(a => a.ReviewStatus == ReviewStatus.Assigned || a.ReviewStatus == ReviewStatus.New);
            
            if (!applicants2Update.Any())
                return;

            foreach (var applicant in applicants2Update)
                ProcUpdate(applicant);
        }

        private void ProcUpdate(JobApplication applicant)
        {
            var applicantsProvider = new JobApplicationProvider();
            var reviewerProvider = new ReviewerProvider();
            var ghStore = GreenHouseDataStore.Instance;
            Reviewer reviewer = null;

            if (applicant == null || applicant.Id == 0)
                return;
            
            var apiJobApplication = ghStore.GetApplicationById(applicant.Id.ToString());
            var appliedJob = ghStore.GetJobById(applicant.JobId);

            if (applicant.ReviewerId.HasValue)
                reviewer = reviewerProvider.Get(applicant.ReviewerId.Value);

            //applicant deleted
            if (apiJobApplication == null || apiJobApplication.Id == 0)
            {
                applicant.ReviewStatus = Models.ReviewStatus.Deleted;
                applicantsProvider.Update(applicant);

                _logger.WarnFormat("Applicant {0} has been deleted from Greenhosue", applicant.Id);
                return;
            }

            if (apiJobApplication.CurrentStage == null)
            {
                applicant.ReviewStatus = ReviewStatus.Error;
                applicantsProvider.Update(applicant);

                _logger.WarnFormat("Cant not resolve current stage for applicant {0}", applicant.Id);
                return;
            }

            //applicant accepted and moved to another stage
            if (apiJobApplication.CurrentStage.Name != ApplicationStages.ApplicationReviewCV)
            {
                applicant.ReviewStatus = ReviewStatus.Accepted;
                applicant.RejectedOrAcceptedAt = apiJobApplication.AcceptedAt;
                applicantsProvider.Update(applicant);                
                if (reviewer != null)
                {
                    reviewer.AdvanceCount++;
                    reviewerProvider.Update(reviewer);
                }
                _logger.WarnFormat("Applicant {0} has been accepted and moved to next stage by reviewer {1}", applicant.Id, reviewer != null ? reviewer.Id : 0);
            }

            //applicant is still at CV / Review stage and was rejected
            if (apiJobApplication.CurrentStage.Name == ApplicationStages.ApplicationReviewCV
                && apiJobApplication.RejectedAt.HasValue)
            {
                applicant.ReviewStatus = ReviewStatus.Rejected;
                applicant.RejectAt = apiJobApplication.RejectedAt.Value;
                applicant.RejectedOrAcceptedAt = apiJobApplication.RejectedAt.Value;
                applicant.RejectionReason = apiJobApplication.RejectionReason;
                applicantsProvider.Update(applicant);

                if (reviewer != null)
                {
                    reviewer.RejectCount++;
                    reviewerProvider.Update(reviewer);
                }
                _logger.WarnFormat("Applicant {0} has been rejected by reviewer {1}", applicant.Id, reviewer?.Id ?? 0);
            }

            //Applied job was removed or closed
            if (appliedJob == null || appliedJob.Status != JobStates.Open)
            {
                applicant.ReviewStatus = ReviewStatus.JobClosed;
                applicantsProvider.Update(applicant);

                _logger.WarnFormat("Applicant {0} has been closed because its job {1} is either deleted or closed", applicant.Id, applicant.JobId);
            }
        }
    }
}
