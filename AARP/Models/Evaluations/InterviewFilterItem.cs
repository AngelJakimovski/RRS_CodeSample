using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AARP.Models.Evaluations
{
    public class InterviewFilterItem
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public int Count { get; set; }
    }

    public class InterviewFilter
    {
        public InterviewFilter()
        {
            StageIds = new List<int>();
            SeniorityIds = new List<int>();
            TechnologyIds = new List<int>();
        }

        public List<int> StageIds { get; set; }

        public List<int> SeniorityIds { get; set; }

        public List<int> TechnologyIds { get; set; }
    }
}