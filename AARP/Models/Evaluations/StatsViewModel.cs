using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AARP.Models.Evaluations
{
    public class StatsViewModel
    {
        public StatsSubHeaderViewModel header { set; get; }
        public StatsBodyViewModel body { set; get; }
    }

    public class StatsSeniorityViewModel
    {
        public string Seniority { set; get; }
        public double Percent { set; get; }
    }

    public class StatsStageViewModel
    {
        public string Stage { set; get; }
        public double Percent { set; get; }
    }

    public class StatsLengthViewModel
    {
        public string Length { set; get; }
        public double Percent { set; get; }
    }
}