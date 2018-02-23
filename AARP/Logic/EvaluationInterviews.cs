using AARP.Models;
using AARP.Models.Evaluations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AARP.Logic
{
    public static class EvaluationInterviews
    {
        public static InterviewsViewModel GetAllInterviewsList(InterviewsFiltersPostViewModel filters)
        {
            InterviewsViewModel result = new InterviewsViewModel();

            if (filters.selectedstages == null)
                filters.selectedstages = new List<int>();

            if (filters.selectedseniorities == null)
            {
                filters.selectedseniorities = new List<int>();
            }

            using (var dbContext = new AARPDbContext())
            {
                var interviewers = dbContext.Interviewers.ToList();
                var interviewees = dbContext.Interviewees.ToList();
                var stages = dbContext.InterviewStages.ToList();
                var seniorities = dbContext.Seniorities.ToList();
                var ratings = dbContext.InterviewRatings.ToList();

                var interviews = dbContext.Interviews.Where(x => filters.selectedstages.Contains(x.InterviewStageId) &&
                                                             filters.selectedseniorities.Contains(x.SeniorityId)
                                                            && (filters.dateFrom != null ? x.Date >= filters.dateFrom : true)
                                                            && (filters.dateTo != null ? x.Date <= filters.dateTo : true)
                                                            );

                foreach (var interview in interviews)
                {
                    InterviewViewModel interviewVM = new InterviewViewModel();

                    var target = interviewers.FirstOrDefault(f => f.Id == interview.InterviewerId);

                    if(target != null)
                    {
                        interviewVM.Interviewer = interviewers.Where(x => x.Id == interview.InterviewerId).First().InterviewerName;
                        interviewVM.Interviewee = interviewees.Where(x => x.Id == interview.IntervieweeId).First().Name;
                        interviewVM.IntervieweeID = interviewees.Where(x => x.Id == interview.IntervieweeId).First().Id;
                        interviewVM.InterviewDate = interview.Date;
                        interviewVM.InterviewStage = stages.Where(x => x.Id == interview.InterviewStageId).First().Stage;
                        interviewVM.InterviewStatus = interview.SurveyStatus;

                        var seniority = seniorities.FirstOrDefault(x => x.Id == interview.SeniorityId);

                        if (seniority != null)
                        {
                            interviewVM.Seniority = seniority.SeniorityName;
                        }

                        interviewVM.Rating = interview.RatingId > 0 ? ratings.Where(x => x.Id == interview.RatingId).First().Rating : 0;
                        result.Interviews.Add(interviewVM);
                    }
                }

            }

            return result;
        }

        public static InterviewsFiltersViewModel GetAllInterviewsFilters()
        {
            InterviewsFiltersViewModel result = new InterviewsFiltersViewModel();

            using (var dbContext = new AARPDbContext())
            {
                var stages = dbContext.InterviewStages.ToList();
                foreach (var stage in stages)
                {
                    result.StageFilter.Add(stage, true);
                }

                var seniorities = dbContext.Seniorities.ToList();
                foreach (var seniority in seniorities)
                {
                    result.SeniorityFilter.Add(seniority, true);
                }

            }

            return result;
        }
    }
}