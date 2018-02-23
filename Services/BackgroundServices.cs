//using AARP.Models;
//using AARP.Models.Emails;
//using Postal;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Reflection;
//using System.Threading.Tasks;
//using System.Web;

//namespace AARP.Services
//{
//    public static class BackgroundServices
//    {
//        static Random random = new Random();
//        static CommonAppConfigurations _configs = CommonAppConfigurations.Instance;
//        static string[] _delimiterStr = new string[] { ",", ";" };

//        public static GreenHouseAPI.Services.IGreenHouseDataStore GreenHouseData { get; private set; }

//        public static DAL.IRepositoryFactory RepositoryFactoryAAPR { get; private set; }

//        #region constructors
//        static BackgroundServices()
//        {
//            var appSettings = AARP.Properties.Settings.Default;
//            var greenHouseAPI = new GreenHouseAPI.Services.GreenHouseAPI(appSettings.GreenHouse_HarvestAPIBaseAddess, appSettings.GreenHouse_APIToken);
//            GreenHouseData = new AARP.GreenHouseAPI.Services.GreenHouseDataStore(greenHouseAPI);
//            RepositoryFactoryAAPR = AARP.DAL.RepositoryFactory.Instance;
//        }
//        #endregion

//        #region basics
//        [LogFailure]
//        [DisplayName("Search and distribute new applicants to reviewers")]
//        public static void DistributeApplicantsToReviewers()
//        {
//            //if (DateTime.Now.IsWeekend())
//            //    return;

//            RefreshGreenHouseData(true);
//            var newApplications = GetNewAppliedApplications();

//            foreach (var application in newApplications)
//            {
//                //Get job applied of application
//                var jobApplied = GreenHouseData.Jobs.FirstOrDefault(m => m.Id.ToString() == application.JobId);
//                if (jobApplied == null)
//                    continue;

//                //Get Job Category string
//                var jobCatgStr = string.Empty;
//                jobApplied.CustomFields.TryGetValue(AARP.Properties.Settings.Default.CustomField_PositionType, out jobCatgStr);
//                if (string.IsNullOrEmpty(jobCatgStr))
//                    continue;

//                //Get Job Category configured from our Database
//                var jobCatg = RepositoryFactoryAAPR.PositionTypeRepository.List().FirstOrDefault(m => string.Equals(m.Name, jobCatgStr));
//                if (jobCatg == null)
//                    continue;

//                //Get reviewer emails of Job Category
//                var reviewerEmails = jobCatg.Interviewers.Split(_delimiterStr, StringSplitOptions.RemoveEmptyEntries).Select(m => m.Trim()).ToArray();
//                if (reviewerEmails == null || reviewerEmails.Count() == 0)
//                    continue;

//                //select a random email and associated reviewer
//                var selectedReviewerEmail = reviewerEmails[random.Next(0, reviewerEmails.Count())];
//                var selectedReviewer = FindReviewerInDbByEmail(selectedReviewerEmail);
//                if (selectedReviewer == null)
//                    continue;

//                //Adding additional information for application: Selected Reviewer, Review Status
//                application.ReviewerId = selectedReviewer.Id;
//                application.ReviewStatus = DAL.ApplicationReviewStatus.Assigned;
//                selectedReviewer.AssignedCount++;

//                //Store the application into the local database
//                RepositoryFactoryAAPR.JobApplicationRepository.Add(application);
//                RepositoryFactoryAAPR.ReviewerRepository.Update(selectedReviewer);

//                //Sending email to reviewer
//                var email = new ReviewApplicationEmail()
//                {
//                    Reviewer = selectedReviewer,
//                    JobApplication = application,
//                    Job = GreenHouseData.Jobs.FirstOrDefault(m => m.Id.ToString() == application.JobId),
//                };
//                email.SendAsync();
//                System.Diagnostics.Trace.WriteLine("New review email has been sent to the reviewer {0}", email.Reviewer.Name);
//            }
//        }

//        [LogFailure]
//        [DisplayName("Refresh/Update status of assigned applicants")]
//        public static void RefreshStateOfApplicants()
//        {
//            RefreshGreenHouseData(false);

//            var applications = RepositoryFactoryAAPR.JobApplicationRepository.List().Where(m => m.ReviewStatus == DAL.ApplicationReviewStatus.Assigned);

//            foreach (var application in applications)
//            {
//                var newState = GreenHouseData.API.GetApplicationById(application.Id.ToString());
//                if (newState == null || newState.Id == 0 || string.IsNullOrEmpty(application.CandidateId))
//                {
//                    application.ReviewStatus = DAL.ApplicationReviewStatus.Deleted;
//                    application.RejectionReason = "Application has been deleted from Greenhouse database";
//                    RepositoryFactoryAAPR.JobApplicationRepository.Update(application);
//                    continue;
//                }

//                var reviewer = RepositoryFactoryAAPR.ReviewerRepository.FindById(application.ReviewerId.ToString());

//                if (newState.CurrentStage.Name != AARP.Properties.Settings.Default.StageName_ApplicationReviewCV)
//                {
//                    application.ReviewStatus = DAL.ApplicationReviewStatus.Accepted;
//                    reviewer.AdvanceCount++;
//                }
//                else if (newState.CurrentStage.Name == AARP.Properties.Settings.Default.StageName_ApplicationReviewCV && newState.RejectedAt.HasValue)
//                {
//                    application.RejectAt = newState.RejectedAt;
//                    application.RejectionReason = newState.RejectionReason;
//                    application.ReviewStatus = DAL.ApplicationReviewStatus.Rejected;

//                    reviewer.RejectCount++;
//                }

//                if (application.ReviewStatus == DAL.ApplicationReviewStatus.Assigned && IsJobClosed(int.Parse(application.JobId)))
//                    application.ReviewStatus = DAL.ApplicationReviewStatus.JobClosed;

//                RepositoryFactoryAAPR.JobApplicationRepository.Update(application);
//                RepositoryFactoryAAPR.ReviewerRepository.Update(reviewer);
//            }

//            if (CommonAppConfigurations.Instance.RemindReviewers_IsEnabled)
//            {
//                Hangfire.BackgroundJob.Enqueue(() => SendReminders());
//            }

//            if (CommonAppConfigurations.Instance.EscalatingApplications_IsEnabled)
//            {
//                Hangfire.BackgroundJob.Enqueue(() => SendEscalationEmails());
//            }
//        }

//        [LogFailure]
//        [DisplayName("Send reminder emails to assigned reviewers")]
//        public static void SendReminders()
//        {
//            //if (DateTime.Now.IsWeekend())
//            //    return;

//            RefreshGreenHouseData();

//            var applications = RepositoryFactoryAAPR.JobApplicationRepository.List().Where(m => m.ReviewStatus == DAL.ApplicationReviewStatus.Assigned).ToArray();
//            foreach (var application in applications)
//            {
//                if (application.ReviewStatus != DAL.ApplicationReviewStatus.Assigned)
//                    continue;


//                if (DateTimeUtils.IsTimeOut(application.RecorededAt, _configs.RemindReviewers_DelayTime, incluedWeekend: false))
//                {
//                    var reviewer = RepositoryFactoryAAPR.ReviewerRepository.FindById(application.ReviewerId.ToString());
//                    if (reviewer == null)
//                    {
//                        System.Diagnostics.Trace.WriteLine(string.Format("Not found reviewer with the Id {0}", application.ReviewerId));
//                        continue;
//                    }
//                    application.RemindCount++;
//                    RepositoryFactoryAAPR.JobApplicationRepository.Update(application);
//                    var email = new RemindReviewApplicationEmail()
//                    {
//                        JobApplication = application,
//                        Reviewer = reviewer,
//                        Job = GreenHouseData.Jobs.FirstOrDefault(m => m.Id.ToString() == application.JobId),
//                    };
//                    email.SendAsync();
//                    System.Diagnostics.Trace.WriteLine(string.Format("New remind email has been sent to reviewer Id {0}", application.ReviewerId));
//                }
//            }
//        }

//        [LogFailure]
//        [DisplayName("Check and send escalation emails")]
//        public static void SendEscalationEmails()
//        {
//            RefreshGreenHouseData();
//            var applications = RepositoryFactoryAAPR.JobApplicationRepository.List().Where(m => m.ReviewStatus == DAL.ApplicationReviewStatus.Assigned).ToArray();
//            foreach (var application in applications)
//            {
//                if (application.ReviewStatus != DAL.ApplicationReviewStatus.Assigned)
//                    continue;

//                if (DateTimeUtils.IsTimeOut(application.RecorededAt, _configs.EscalatingApplications_DelayTime, incluedWeekend: false))
//                {
//                    var reviewer = RepositoryFactoryAAPR.ReviewerRepository.FindById(application.ReviewerId.ToString());
//                    if (reviewer == null)
//                    {
//                        System.Diagnostics.Trace.WriteLine(string.Format("Not found reviewer with the Id {0}", application.ReviewerId));
//                        continue;
//                    }

//                    var escalateTo = _configs.EscalatingApplications_ToEmails;
//                    if (string.IsNullOrEmpty(escalateTo))
//                    {
//                        System.Diagnostics.Trace.WriteLine("Escalation Email is null");
//                        return;
//                    }
//                    var email = new TimeOutApplicationEscalationEmail()
//                    {
//                        JobApplication = application,
//                        Reviewer = reviewer,
//                        EscalationTo = escalateTo,
//                    };
//                    email.SendAsync();
//                    System.Diagnostics.Trace.WriteLine(string.Format("New escaltion email has been sent to reviewer Id {0}", application.ReviewerId));
//                }
//            }
//        }
//        #endregion

//        #region helpers
//        public static IList<AARP.DAL.JobApplication> GetNewAppliedApplications()
//        {
//            RefreshGreenHouseData();
//            var results = new List<AARP.DAL.JobApplication>();

//            var openningJobs = GreenHouseData.Jobs.Where(m => m.Status != null && m.Status == AARP.Properties.Settings.Default.GreenHouseJobOpen).ToList();
//            if (openningJobs == null || openningJobs.Count == 0)
//                return results;

//            //get un-assigned applications
//            var newApplications = GreenHouseData.Applicants.Where(m => IsNewApplication(m)).ToList();

//            foreach (var jobApplication in newApplications)
//            {
//                var application2Store = new AARP.DAL.JobApplication()
//                {
//                    Id = jobApplication.Id,
//                    CandidateId = jobApplication.CandidateId,
//                    CandidateName = jobApplication.Name,
//                    AppliedAt = jobApplication.AppliedAt,
//                    RejectAt = jobApplication.RejectedAt,
//                    LastActivity = jobApplication.LastActivityAt,
//                    UrlCV = "",
//                    RejectionReason = "",
//                    RecorededAt = DateTime.Now,
//                    ReviewStatus = DAL.ApplicationReviewStatus.New,
//                    JobId = jobApplication.Jobs.First().Id.ToString(),
//                };

//                var candidate = GreenHouseData.Candidates.FirstOrDefault(m => m.Id == application2Store.CandidateId);
//                var job = GreenHouseData.Jobs.FirstOrDefault(m => m.Id.ToString() == application2Store.JobId);

//                application2Store.JobName = job != null ? job.Name : "";
//                application2Store.CandidateName = candidate.FirstName + (string.IsNullOrEmpty(candidate.LastName) ? "" : (" " + candidate.LastName));
                
//                results.Add(application2Store);
//            }

//            return results;
//        }

//        private static AARP.DAL.Reviewer FindReviewerInDbByEmail(string email)
//        {
//            return RepositoryFactoryAAPR.ReviewerRepository.List()
//                    .FirstOrDefault(m => m.Email.Split(_delimiterStr, StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim()).ToArray()
//                        .Contains(email));
//        }

//        public static void RefreshGreenHouseData(bool isForced = false)
//        {
//            var timeTheshold = AARP.Properties.Settings.Default.DataLifeTime;

//            if (isForced)
//                GreenHouseData.Refresh();
//            else if(!GreenHouseData.LastUpdateAt.HasValue || GreenHouseData.LastUpdateAt.Value < DateTime.Now - timeTheshold)
//                GreenHouseData.Refresh();
//        }

//        private static bool IsJobClosed(int jobId)
//        {
//           var job = GreenHouseData.API.GetJobById(jobId.ToString());
//           return job == null ? true : job.Status == "closed";
//        }

//        private static bool IsNewApplication(AARP.GreenHouseAPI.Models.GhJobApplication jobApplication)
//        {
//            return jobApplication.Status != null && jobApplication.Status == AARP.Properties.Settings.Default.GreenHouseActiveStatus
//              && jobApplication.Jobs != null && jobApplication.Jobs.Count != 0
//              && jobApplication.CurrentStage != null && jobApplication.CurrentStage.Name != null && jobApplication.CurrentStage.Name == AARP.Properties.Settings.Default.StageName_ApplicationReviewCV
//              && jobApplication.RejectedAt == null
//              && RepositoryFactoryAAPR.JobApplicationRepository.FindById(jobApplication.Id.ToString()) == null;
//        }
//        #endregion
//    }
//}