using AARP.Models;
using AARP.Models.Evaluations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AARP.Repositories
{
    public class InterviewsRepository : Repository, IInterviewsRepository
    {
        public InterviewsRepository(AARPDbContext context)
        {
            this.Context = context;
        }

        public List<InterviewRowItemViewModel> GetInterviews(int interviewerId, InterviewFilter filter, out int total, int start = 0, int size = 10, string sortBy = "", bool sortDir = false)
        {
            var query = from i in Context.Interviews
                        join ivr in Context.Interviewers on i.InterviewerId equals ivr.Id
                        join ivw in Context.Interviewees on i.IntervieweeId equals ivw.Id
                        join sn in Context.Seniorities on i.SeniorityId equals sn.Id
                        join intech in Context.InterviewTechnologies on i.TechnologyId equals intech.Id
                        join status in Context.InterviewStatus on i.StatusId equals status.Id
                        join rt in Context.InterviewRatings on i.RatingId equals rt.Id
                        join stage in Context.InterviewStages on i.InterviewStageId equals stage.Id
                        where i.InterviewerId == interviewerId
                                && (filter.StageIds.Count == 0 || filter.StageIds.Contains(i.InterviewStageId)) 
                                && (filter.TechnologyIds.Count == 0 || filter.TechnologyIds.Contains(i.TechnologyId))
                                && (filter.SeniorityIds.Count == 0 || filter.SeniorityIds.Contains(i.SeniorityId))
                        select new InterviewRowItemViewModel
                        {
                            Date = i.Date,
                            IntervieweeName = ivw.Name.Trim(),
                            InterviewId = i.Id,
                            InterviewStage = stage.Stage.Trim(),
                            Rating = rt.Rating,
                            Seniority = sn.SeniorityName.Trim(),
                            Status = status.Status.Trim(),
                            Technology = intech.Technology.Trim(),
                            Attitude = i.InterviewerAttitude.Trim(),
                            Difficulty = i.TaskDifficultyLevel.Trim(),
                            Length = i.InterviewLengthId,
                            TestTask = i.TestTask,
                            Feedback= i.NoteFromInterviewee.Trim(),
                        };

            total = query.Count();
            var result = query.OrderBy(sortBy, sortDir).Skip(start).Take(size).ToList();

            return result;
        }

        public List<InterviewRowItemViewModel> GetInterviews(DateTime from, DateTime to, out int total, int start = 0, int size = 10)
        {
            throw new NotImplementedException();
        }

        public List<InterviewReportRowItemViewModel> GetReportItems(out int total, int start = 0, int size = 10, string sortBy = "", bool sortDir = false)
        {
            var query = from i in Context.Interviews
                        join ivr in Context.Interviewers on i.InterviewerId equals ivr.Id
                        join ivw in Context.Interviewees on i.IntervieweeId equals ivw.Id
                        join sn in Context.Seniorities on i.SeniorityId equals sn.Id
                        join stage in Context.InterviewStages on i.InterviewStageId equals stage.Id
                        select new InterviewReportRowItemViewModel
                        {
                            Date = i.Date,
                            Email = ivr.InterviewerEmail,
                            FeedbackStatus = i.SurveyStatus,
                            IntervieweeName = ivw.Name.Trim(),
                            InterviewId = i.Id,
                            Seniority = sn.SeniorityName.Trim(),
                            InterviewerName = ivr.InterviewerName,
                            Stage = stage.Stage,
                            Link = i.FeedbackLink
                        };

            total = query.Count();
            var result = query.OrderBy(sortBy, sortDir).Skip(start).Take(size).ToList();

            return result;

        }
    }
}