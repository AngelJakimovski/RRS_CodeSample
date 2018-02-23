using AARP.Models;
using AARP.Models.Evaluations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AARP.Logic
{
    public static class EvaluationStandings
    {
        public static List<StandingsInterviewersViewModel> GetAllStandings()
        {
            return EvaluationStandings.GetAllStandings(new DateRange());
        }

        public static List<StandingsInterviewersViewModel> GetAllStandings(DateRange period)
        {
            List<StandingsInterviewersViewModel> standings = null;

            using (var dbContext = new AARPDbContext())
            {
                var standingsResult =
                (from interviewer in dbContext.Interviewers
                 join interview in dbContext.Interviews on interviewer.Id equals interview.InterviewerId
                 join ratings in dbContext.InterviewRatings on interview.RatingId equals ratings.Id
                 join istage in dbContext.InterviewStages on interview.InterviewStageId equals istage.Id
                 where (period.dateFrom == null || interview.Date >= period.dateFrom)
                       && (period.dateTo == null || interview.Date >= period.dateTo)
                 select new
                 {
                     interviewer.Id,
                     interviewer.InterviewerName,
                     istage.Stage,
                     ratings.Rating,
                     ratingId = ratings.Id,
                     InterviewerAttitude = interview.InterviewerAttitude.Trim(),
                 });

                standings = standingsResult.GroupBy(g => g.Id).Select(s => new StandingsInterviewersViewModel()
                {
                    InterviewerId = s.Key,
                    AvgRating = s.Average(a => a.Rating),
                    Name = s.FirstOrDefault() == null ? "" : s.FirstOrDefault().InterviewerName,
                    GeneralAttitude = new StandingsAttitudeViewModel()
                    {
                        Negative = (double)s.Count(w => w.InterviewerAttitude.Equals(DbConstants.AttitudeNegative)) / s.Count(),
                        Neutral = (double)s.Count(w => w.InterviewerAttitude.Equals(DbConstants.AttitudeNeutral)) / s.Count(),
                        Positive = (double)s.Count(w => w.InterviewerAttitude.Equals(DbConstants.AttitudePositive)) / s.Count(),
                    },
                    InterviewStages = s.GroupBy(g => g.Stage).Select(st => new StandingsInterviewStageViewModel()
                    {
                        AttitudeStat = new StandingsAttitudeViewModel()
                        {
                            Negative = (double)st.Count(w => w.InterviewerAttitude.Equals(DbConstants.AttitudeNegative)) / st.Count(),
                            Neutral = (double)st.Count(w => w.InterviewerAttitude.Equals(DbConstants.AttitudeNeutral)) / st.Count(),
                            Positive = (double)st.Count(w => w.InterviewerAttitude.Equals(DbConstants.AttitudePositive)) / st.Count(),
                        },
                        StageName = st.Key,
                        Ratings = st.GroupBy(g => g.Rating).Select(rt => new StandingsInterviewRatingViewModel()
                        {
                            RatingCount = rt.Count(),
                            Rating = rt.Key
                        }).ToList(),
                    }).ToList(),
                }).ToList();
            }

            return standings;
        }
    }
}