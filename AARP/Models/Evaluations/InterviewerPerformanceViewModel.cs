using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AARP.Models.Evaluations
{
    public class InterviewerPerformanceViewModel
    {
        public string Name { get; set; }

        public double AvgRating { get; set; }

        public StandingsAttitudeViewModel Attitude { get; set; }

        public List<StandingsInterviewRatingViewModel> Standings { get; set; }

        public bool IsPositiveTrend
        {
            get
            {
                if (AvgRating >= 3.5)
                    return true;
                return false;
            }
        }
    }
}