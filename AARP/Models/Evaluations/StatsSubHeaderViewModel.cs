using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AARP.Models.Evaluations
{
    public class StatsSubHeaderViewModel
    {
        public float AverageInterviewRating { set; get; }
        public int TotalNumberOfInterviews { set; get; }
        public int TotalInterview1 { set; get; }
        public int TotalInterview2 { set; get; }
        public int TotalInterviewers { set; get; }
        public int PositiveImpression { set; get; }
        public int NeutralImpression { set; get; }
        public int NegativeImpression { set; get; }
    }
}