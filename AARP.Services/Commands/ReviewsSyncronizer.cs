using System;
using AARP.Infrashtructure.DI;
using AARP.Models;
using AARP.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using log4net;
using System.Threading.Tasks;

namespace AARP.Services.Commands
{
    public class ReviewsSyncronizer : ICommand
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ReviewsSyncronizer));

        public bool CanExecute()
        {
            return true;
        }

        public void Execute()
        {
            //SyncUsers();
            //SyncJobtitles();
            //SyncInterviewers();
            //SyncInterviewees();
            //SyncInterviews();
            //SendSurveyEmails();
        }

        /// <summary>
        /// Sync Users
        /// </summary>
        private static void SyncUsers()
        {
            try
            {
                var webApiClient = DiContainers.Global.Resolve<IWebApiClient>();

                var users = webApiClient.GetUsers();

                using (var dbContext = new AARPDbContext())
                {
                    foreach (var user in users)
                    {
                        if (user.Disabled)
                            continue;

                        var currentInterviewer = dbContext.Interviewers.FirstOrDefault(x => x.Id.ToString() == user.Id);
                        if (currentInterviewer != null) continue;


                        currentInterviewer = new Interviewer
                        {
                            Id = int.Parse(user.Id),
                            InterviewerName = user.Name ?? string.Empty,
                            InterviewerEmail = user.Emails.Count > 0 ? user.Emails[0] : string.Empty
                        };

                        dbContext.Interviewers.Add(currentInterviewer);
                        dbContext.SaveChanges();
                    }


                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, e);
            }
        }

        private static void SyncInterviews()
        {
            var webApiClient = DiContainers.Global.Resolve<IWebApiClient>();

            var apiInterviews = webApiClient.GetInterviews();
            if (apiInterviews == null || apiInterviews.Count == 0)
            {
                return;
            }
            using (var dbContext = new AARPDbContext())
            {
                try
                {
                    foreach (var apiInterview in apiInterviews)
                    {
                        foreach (var invitee in apiInterview.Invitees)
                        {
                            var emails = invitee.Where(x => x.Key == "email").Select(y => y.Value);
                            foreach (var email in emails)
                            {
                                if (!EvaluationsHelper.IsTechInterviewer(email)) continue;

                                var interviewer = dbContext.Interviewers.FirstOrDefault(x => x.InterviewerEmail.ToLower().Trim() == email.ToLower().Trim());
                                if (interviewer == null) continue;

                                var currentInterview = dbContext.Interviews.FirstOrDefault(x => x.Id == apiInterview.Id);
                                if (currentInterview != null) continue;

                                currentInterview = new Interview
                                {

                                    Id = apiInterview.Id,
                                    InterviewerId = interviewer.Id
                                };


                                var candidateId = apiInterview.Candidate["id"];
                                var candidate = dbContext.Interviewees.FirstOrDefault(c => c.Id == candidateId);
                                if (candidate != null)
                                {

                                    var jobPosition = dbContext.JobPositions.First(x => x.Id == candidate.OpeningID);
                                    currentInterview.IntervieweeId = candidate.Id;
                                    currentInterview.Date = apiInterview.Time;
                                    currentInterview.SurveyStatus = "New";
                                    currentInterview.InterviewStageId = 1;
                                    currentInterview.SeniorityId = jobPosition.Name != null ?
                                        (dbContext.Seniorities.FirstOrDefault(x => jobPosition.Name.ToLower().Contains(x.SeniorityName.ToLower().Trim())) != null ?
                                            dbContext.Seniorities.First(x => jobPosition.Name.ToLower().Contains(x.SeniorityName.ToLower().Trim())).Id : 0) : 0;


                                    dbContext.Interviews.Add(currentInterview);
                                    dbContext.SaveChanges();
                                }
                                else
                                {
                                    Logger.Info($"failed candidate found, id: {candidateId}");
                                }


                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message, e);
                }
            }
        }

        /// <summary>
        /// Sync Interviewers
        /// </summary>
        private static void SyncInterviewers()
        {
            var webApiClient = DiContainers.Global.Resolve<IWebApiClient>();

            var apiInterviewers = webApiClient.GetInterviewers();
            if (apiInterviewers == null || apiInterviewers.Count == 0)
            {
                return;
            }
            using (var dbContext = new AARPDbContext())
            {
                try
                {
                    foreach (var apiInterviewer in apiInterviewers)
                    {
                        foreach (var reviewer in apiInterviewer.Feedback)
                        {
                            var currentInterviewer =
                                dbContext.Interviewers.FirstOrDefault(x => x.Id.ToString() == reviewer.SubmittedBy.Id);
                            if (currentInterviewer != null) continue;

                            currentInterviewer = new Interviewer
                            {
                                Id = int.Parse(reviewer.SubmittedBy.Id),
                                InterviewerName = reviewer.SubmittedBy.Name ?? string.Empty,
                                InterviewerEmail = reviewer.SubmittedBy.Email ?? string.Empty
                            };

                            dbContext.Interviewers.Add(currentInterviewer);
                            dbContext.SaveChanges();


                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message, e);
                }
            }

        }

        /// <summary>
        /// Sync Interviewees
        /// </summary>
        private static void SyncInterviewees()
        {
            var webApiClient = DiContainers.Global.Resolve<IWebApiClient>();

            var apiInterviewees = webApiClient.GetInterviewees();
            if (apiInterviewees == null || apiInterviewees.Count == 0)
            {
                return;
            }
            using (var dbContext = new AARPDbContext())
            {
                try
                {
                    foreach (var apiInterviewee in apiInterviewees)
                    {
                        var currentInterview = dbContext.Interviewees.FirstOrDefault(x => x.Id.ToString() == apiInterviewee.Id);

                        if (currentInterview != null) continue;


                        var interviewee = new Interviewee
                        {
                            Id = int.Parse(apiInterviewee.Id),
                            Name = apiInterviewee.GetFullName(),
                            Email = apiInterviewee.Email,
                            OpeningID = apiInterviewee.OpeningID,
                            StageID = apiInterviewee.StageID
                        };

                        dbContext.Interviewees.Add(interviewee);
                        dbContext.SaveChanges();


                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message, e);
                }
            }
        }

        /// <summary>
        /// Syncronize JobTitles
        /// </summary>
        private static void SyncJobtitles()
        {
            var webApiClient = DiContainers.Global.Resolve<IWebApiClient>();

            var apiJobTitles = webApiClient.GetJobTitles();
            if (apiJobTitles == null || apiJobTitles.Count == 0)
            {
                return;
            }
            using (var dbContext = new AARPDbContext())
            {
                try
                {
                    foreach (var job in apiJobTitles)
                    {
                        var currentJob = dbContext.JobPositions.FirstOrDefault(x => x.Id == job.Id);

                        if (currentJob != null) continue;


                        currentJob = new JobPosition
                        {
                            Id = job.Id,
                            Name = job.Title,
                            Description = job.Description,
                            Interviewers = ""
                        };
                        dbContext.JobPositions.Add(currentJob);
                        dbContext.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message, e);
                }
            }
        }

        private static void SendSurveyEmails()
        {
            using (var dbContext = new AARPDbContext())
            {
                var newReviews = dbContext.Interviews.Where(x => x.SurveyStatus.Trim() == "New" && x.StatusId == 0).ToList();
                EvaluationsEmailService service = new EvaluationsEmailService();

                foreach (var review in newReviews)
                {
                    service.SendEvaluationInvitationEmail(review.InterviewerId, review.IntervieweeId);
                    review.StatusId = 1;
                    review.SurveyStatus = "EmailSent";
                    dbContext.Entry(review).State = System.Data.Entity.EntityState.Modified;
                }
                dbContext.SaveChanges();
            }
        }
    }
}
