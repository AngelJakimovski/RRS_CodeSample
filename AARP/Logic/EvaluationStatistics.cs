using AARP.Models;
using AARP.Models.Evaluations;
using AARP.Models.Evaluations.Chart;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace AARP.Logic
{
    public static class EvaluationStatistics
    {
        public static StatsSubHeaderViewModel GenerateStatsSubHeader(DateTime startDate, DateTime endDate)
        {
            StatsSubHeaderViewModel result = new StatsSubHeaderViewModel();
            using (var dbContext = new AARPDbContext())
            {
                result.TotalInterviewers = dbContext.Interviewers.Count();
                result.TotalNumberOfInterviews = dbContext.Interviews.Where(x => x.Date >= startDate && x.Date <= endDate).Count();
                result.TotalInterview1 = dbContext.Interviews.Where(x => x.Date >= startDate && x.Date <= endDate && x.InterviewStageId == 1).Count();
                result.TotalInterview2 = dbContext.Interviews.Where(x => x.Date >= startDate && x.Date <= endDate && x.InterviewStageId == 2).Count();
                result.TotalInterviewers = dbContext.Interviews.Where(x => x.Date >= startDate && x.Date <= endDate).Select(y => y.InterviewerId).Distinct().Count();
                result.PositiveImpression = dbContext.Interviews.Where(x => x.Date >= startDate && x.Date <= endDate && x.InterviewerAttitude == DbConstants.AttitudePositive).Count();
                result.NeutralImpression = dbContext.Interviews.Where(x => x.Date >= startDate && x.Date <= endDate && x.InterviewerAttitude == DbConstants.AttitudeNeutral).Count();
                result.NegativeImpression = dbContext.Interviews.Where(x => x.Date >= startDate && x.Date <= endDate && x.InterviewerAttitude == DbConstants.AttitudeNegative).Count();
            }

            return result;
        }

        public static StatsBodyViewModel GenerateStatsBody(DateRange period)
        {
            StatsBodyViewModel result = new StatsBodyViewModel();
            result.Top5Interviewers = EvaluationStandings.GetAllStandings(period).Take(5).ToList();

            foreach (var interviewer in result.Top5Interviewers)
            {
                interviewer.SeniorityChart = GetSeniorityChartData(period, interviewer.InterviewerId);
                interviewer.LengthChart = GetLengthChartData(period, interviewer.InterviewerId);
                interviewer.StageChart = GetStageChartData(period, interviewer.InterviewerId);
            }

            return result;
        }

        public static StatsViewModel GenerateStats(DateRange period)
        {
            StatsViewModel result = new StatsViewModel();
            result.header = GenerateStatsSubHeader(period.dateFrom != null ? period.dateFrom.Value : DateTime.MinValue, period.dateTo != null ? period.dateTo.Value : DateTime.Today);
            result.body = GenerateStatsBody(period);

            return result;
        }

        public static Models.Evaluations.Chart.Chart GetStatsHeaderBarChartData(DateRange period)
        {

            StandingsAttitudeViewModel attitude = new StandingsAttitudeViewModel();

            using (var dbContext = new AARPDbContext())
            {
                var standingsResult = (from interview in dbContext.Interviews
                                       where interview.Date >= period.dateFrom && interview.Date <= period.dateTo
                                       select new
                                       {
                                           interview.Id,
                                           interview.InterviewerAttitude,
                                       }).Where(x => x.InterviewerAttitude != null).ToList();

                if (standingsResult.Count > 0)
                {
                    attitude.Positive = ((double)standingsResult.Count(x => x.InterviewerAttitude.Trim() == DbConstants.AttitudePositive) / (double)standingsResult.Count()) * 100;
                    attitude.Neutral = ((double)standingsResult.Count(x => x.InterviewerAttitude.Trim() == DbConstants.AttitudeNeutral) / (double)standingsResult.Count()) * 100;
                    attitude.Negative = ((double)standingsResult.Count(x => x.InterviewerAttitude.Trim() == DbConstants.AttitudeNegative) / (double)standingsResult.Count()) * 100;
                }
                else attitude.Positive = attitude.Neutral = attitude.Negative = 0;
            }

            if (attitude != null)
            {

                Models.Evaluations.Chart.Chart _chart = new Models.Evaluations.Chart.Chart();
                _chart.labels = new string[] { ((int)attitude.Positive).ToString(), ((int)attitude.Neutral).ToString(), ((int)attitude.Negative).ToString() };
                _chart.datasets = new List<Datasets>();
                List<Datasets> _dataSet = new List<Datasets>();
                _dataSet.Add(new Datasets()
                {
                    label = "",
                    data = new int[] { (int)attitude.Positive, (int)attitude.Neutral, (int)attitude.Negative },
                    backgroundColor = new string[] { "#9068be", "#e62739", "#3fb0ac", "ba9077", "6534ff" },
                    //borderColor = new string[] { "green", "grey", "red" },
                    borderWidth = "1"
                });
                _chart.datasets = _dataSet;
                return _chart;
            }
            else
            {
                Models.Evaluations.Chart.Chart _chart = new Models.Evaluations.Chart.Chart();
                _chart.labels = new string[] { "0", "0", "0" };
                _chart.datasets = new List<Datasets>();
                List<Datasets> _dataSet = new List<Datasets>();
                _dataSet.Add(new Datasets()
                {
                    label = "",
                    data = new int[] { 0, 0, 0 },
                    backgroundColor = new string[] { "#9068be", "#e62739", "#3fb0ac", "ba9077", "6534ff" },
                    //borderColor = new string[] { "green", "grey", "red" },
                    borderWidth = "1"
                });
                _chart.datasets = _dataSet;
                return _chart;
            }
        }

        public static Models.Evaluations.Chart.Chart GetRatingChartData(DateRange period, int interviewerID = default(int))
        {
            List<StandingsInterviewRatingViewModel> OverallRatings = new List<StandingsInterviewRatingViewModel>();

            using (var dbContext = new AARPDbContext())
            {
                var standingsResult = (from interview in dbContext.Interviews
                                       join ratings in dbContext.InterviewRatings on interview.RatingId equals ratings.Id
                                       where interview.Date >= period.dateFrom && interview.Date <= period.dateTo
                                                && (interviewerID > 0 ? interview.Id == interviewerID : true)
                                       select new
                                       {
                                           interview.Id,
                                           interview.RatingId,
                                           ratings.Rating,
                                       }).Where(x => x.RatingId != 0).ToList();

                var test = standingsResult.GroupBy(x => x.RatingId).ToList();
                foreach (var item in test)
                {
                    OverallRatings.Add(new StandingsInterviewRatingViewModel() { Rating = item.Key, RatingCount = item.Count() });
                }

            }

            Models.Evaluations.Chart.Chart _chart = new Models.Evaluations.Chart.Chart();
            _chart.labels = OverallRatings.Select(x => x.Rating.ToString()).ToArray();
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Ratings",
                data = OverallRatings.Select(x => x.RatingCount).ToArray(),
                backgroundColor = new string[] { "#9068be", "#e62739", "#3fb0ac", "ba9077", "6534ff" },
                // borderColor = new string[] { "#FF0000", "#800000", "#808000"},
                borderWidth = "1"
            });
            _chart.datasets = _dataSet;
            return _chart;
        }

        public static Models.Evaluations.Chart.Chart GetSeniorityChartData(DateRange period, int interviewerID = default(int))
        {
            List<StatsSeniorityViewModel> OverallSeniorities = new List<StatsSeniorityViewModel>();

            using (var dbContext = new AARPDbContext())
            {
                var standingsResult = (from interview in dbContext.Interviews
                                       join seniority in dbContext.Seniorities on interview.SeniorityId equals seniority.Id
                                       where interview.Date >= period.dateFrom && interview.Date <= period.dateTo
                                       && (interviewerID > 0 ? interview.Id == interviewerID : true)
                                       select new
                                       {
                                           interview.SeniorityId,
                                           interview.Id,
                                           seniority.SeniorityName,
                                       }).Where(x => x.SeniorityId != 0).ToList();

                var groupedSeniorities = standingsResult.GroupBy(x => x.SeniorityName).ToList();
                foreach (var item in groupedSeniorities)
                {
                    OverallSeniorities.Add(new StatsSeniorityViewModel() { Seniority = item.Key.Trim(), Percent = item.Count() });
                }

            }

            Models.Evaluations.Chart.Chart _chart = new Models.Evaluations.Chart.Chart();
            _chart.labels = OverallSeniorities.Select(x => x.Seniority.ToString()).ToArray();
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Seniority",
                data = OverallSeniorities.Select(x => (int)Math.Round(x.Percent)).ToArray(),
                backgroundColor = new string[] { "#9068be", "#e62739", "#3fb0ac", "ba9077", "6534ff" },
                // borderColor = new string[] { "#FF0000", "#800000", "#808000"},
                borderWidth = "1"
            });
            _chart.datasets = _dataSet;
            return _chart;
        }

        public static Models.Evaluations.Chart.Chart GetStageChartData(DateRange period, int interviewerID = default(int))
        {
            List<StatsStageViewModel> OverallStages = new List<StatsStageViewModel>();

            using (var dbContext = new AARPDbContext())
            {
                var standingsResult = (from interview in dbContext.Interviews
                                       join stages in dbContext.InterviewStages on interview.InterviewStageId equals stages.Id
                                       where interview.Date >= period.dateFrom && interview.Date <= period.dateTo
                                        && (interviewerID > 0 ? interview.Id == interviewerID : true)
                                       select new
                                       {
                                           interview.InterviewStageId,
                                           interview.Id,
                                           stages.Stage,
                                       }).Where(x => x.InterviewStageId != 0).ToList();

                var groupedStages = standingsResult.GroupBy(x => x.Stage).ToList();
                foreach (var item in groupedStages)
                {
                    OverallStages.Add(new StatsStageViewModel() { Stage = item.Key.Trim(), Percent = item.Count() });
                }

            }

            Models.Evaluations.Chart.Chart _chart = new Models.Evaluations.Chart.Chart();
            _chart.labels = OverallStages.Select(x => x.Stage.ToString()).ToArray();
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Stage",
                data = OverallStages.Select(x => (int)Math.Round(x.Percent)).ToArray(),
                backgroundColor = new string[] { "#9068be", "#e62739", "#3fb0ac", "ba9077", "6534ff" },
                // borderColor = new string[] { "#FF0000", "#800000", "#808000"},
                borderWidth = "1"
            });
            _chart.datasets = _dataSet;
            return _chart;
        }

        public static Models.Evaluations.Chart.Chart GetLengthChartData(DateRange period, int interviewerID = default(int))
        {
            List<StatsLengthViewModel> OverallLengths = new List<StatsLengthViewModel>();

            using (var dbContext = new AARPDbContext())
            {
                var standingsResult = (from interview in dbContext.Interviews
                                       join length in dbContext.InterviewLengths on interview.InterviewLengthId equals length.Id
                                       where interview.Date >= period.dateFrom && interview.Date <= period.dateTo
                                        && (interviewerID > 0 ? interview.Id == interviewerID : true)
                                       select new
                                       {
                                           interview.InterviewLengthId,
                                           interview.Id,
                                           length.Length,
                                       }).Where(x => x.InterviewLengthId != 0).ToList();

                var groupedLenghts = standingsResult.GroupBy(x => x.Length).ToList();
                foreach (var item in groupedLenghts)
                {
                    OverallLengths.Add(new StatsLengthViewModel() { Length = item.Key.Trim(), Percent = item.Count() });
                }

            }

            Models.Evaluations.Chart.Chart _chart = new Models.Evaluations.Chart.Chart();
            _chart.labels = OverallLengths.Select(x => x.Length.ToString()).ToArray();
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Length",
                data = OverallLengths.Select(x => (int)Math.Round(x.Percent)).ToArray(),
                backgroundColor = new string[] { "#9068be", "#e62739", "#3fb0ac", "ba9077", "6534ff" },
                // borderColor = new string[] { "#FF0000", "#800000", "#808000"},
                borderWidth = "1"
            });
            _chart.datasets = _dataSet;
            return _chart;
        }

        public static Models.Evaluations.Chart.Chart GetInterviewsChartData(DateRange period, int stageID = default(int))
        {

            using (var dbContext = new AARPDbContext())
            {
                var standingsResult = (from interview in dbContext.Interviews
                                       join ratings in dbContext.InterviewRatings on interview.RatingId equals ratings.Id
                                       where interview.Date >= period.dateFrom && interview.Date <= period.dateTo
                                        && (stageID > 0 ? interview.InterviewStageId == stageID : true)

                                       select new
                                       {
                                           interview.Id,
                                           interview.RatingId,
                                           ratings.Rating,
                                       }).Where(x => x.RatingId != 0).ToList();


                Models.Evaluations.Chart.Chart _chart = new Models.Evaluations.Chart.Chart();
                _chart.labels = standingsResult.Select(x => x.Rating.ToString()).ToArray();
                _chart.datasets = new List<Datasets>();
                List<Datasets> _dataSet = new List<Datasets>();
                _dataSet.Add(new Datasets()
                {
                    label = "Ratings",
                    data = standingsResult.Select(x => x.RatingId).ToArray(),
                    backgroundColor = new string[] { "#9068be", "#e62739", "#3fb0ac", "ba9077", "6534ff" },
                    // borderColor = new string[] { "#FF0000", "#800000", "#808000"},
                    borderWidth = "1"
                });
                _chart.datasets = _dataSet;
                return _chart;
            }
        }

        public static Models.Evaluations.Chart.Chart GetInterviewsMultiChartData(DateRange period)
        {

            using (var dbContext = new AARPDbContext())
            {
                var standingsResult = (from interview in dbContext.Interviews
                                       join ratings in dbContext.InterviewRatings on interview.RatingId equals ratings.Id
                                       where interview.Date >= period.dateFrom && interview.Date <= period.dateTo

                                       select new
                                       {
                                           interview.Id,
                                           interview.InterviewStageId,
                                           interview.RatingId,
                                           ratings.Rating,
                                       }).Where(x => x.RatingId != 0).ToList();


                Models.Evaluations.Chart.Chart _chart = new Models.Evaluations.Chart.Chart();
                _chart.labels = standingsResult.Select(x => x.Rating.ToString()).ToArray();
                _chart.datasets = new List<Datasets>();
                List<Datasets> _dataSet = new List<Datasets>();
                _dataSet.Add(new Datasets()
                {
                    label = "Tech Interview 1",
                    data = standingsResult.Where(s=>s.InterviewStageId==1).Select(x => x.RatingId).ToArray(),
                    backgroundColor = new string[] { "#9068be", "#e62739", "#3fb0ac", "ba9077", "6534ff" },
                    // borderColor = new string[] { "#FF0000", "#800000", "#808000"},
                    borderWidth = "1"
                });
                _dataSet.Add(new Datasets()
                {
                    label = "Tech Interview 2",
                    data = standingsResult.Where(s => s.InterviewStageId == 2).Select(x => x.RatingId).ToArray(),
                    backgroundColor = new string[] { "#e62739", "#3fb0ac", "ba9077", "6534ff", "#9068be" },
                    // borderColor = new string[] { "#FF0000", "#800000", "#808000"},
                    borderWidth = "1"
                });
                _chart.datasets = _dataSet;
                return _chart;
            }
        }

    }
}