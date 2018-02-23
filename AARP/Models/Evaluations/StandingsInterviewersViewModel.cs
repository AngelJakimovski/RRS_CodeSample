using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AARP.Models.Evaluations
{
    public class StandingsInterviewersViewModel
    {
        public int InterviewerId { get; set; }

        public string Name { get; set; }

        public int Position { get; set; }

        public double AvgRating { get; set; }

        public StandingsAttitudeViewModel GeneralAttitude { get; set; }

        public List<StandingsInterviewStageViewModel> InterviewStages { get; set; }

        public Chart.Chart SeniorityChart { set; get; }
        public Chart.Chart StageChart { set; get; }
        public Chart.Chart LengthChart { set; get; }
    }

    public class StandingsInterviewStageViewModel
    {
        public int StageId { get; set; }

        public string StageName { get; set; }

        public StandingsAttitudeViewModel AttitudeStat { get; set; }

        public List<StandingsInterviewRatingViewModel> Ratings { get; set; }
    }

    public class StandingsAttitudeViewModel
    {
        public double Positive { get; set; }

        public double Neutral { get; set; }

        public double Negative { get; set; }
    }

    public class StandingsInterviewRatingViewModel
    {
        /// <summary>
        /// Rating number
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Number of interviewees which set this rating
        /// </summary>
        public int RatingCount { get; set; }
    }
}