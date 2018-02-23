using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AARP.Models.Evaluations
{
    public class InterviewRowItemViewModel
    {
        public int InterviewId { get; set; }

        /// <summary>
        /// Date of interview
        /// </summary>
        public DateTime Date { get; set; }

        public string IntervieweeName { get; set; }

        public string InterviewStage { get; set; }

        public string Seniority { get; set; }

        public string Technology { get; set; }

        public string Status { get; set; }

        public int Rating { get; set; }


        public string Attitude { get; set; }

        public bool? TestTask { get; set; }

        public string Difficulty { get; set; }

        public int? Length { get; set; } 

        public string Feedback { get; set; }

    }
}