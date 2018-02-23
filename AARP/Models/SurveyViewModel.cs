using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AARP.Models
{
    public class SurveyViewModel
    {

        public int Id { get; set; }

        [Display(Name = "Rate your interviewer")]
        public int RatingId { get; set; }

        [Display(Name = "How was general attitude of your interviewer?")]
        public string InterviewerAttitude { get; set; }

        [Display(Name = "Rate your task based on difficulty level?")]
        public string TaskDifficultyLevel { get; set; }

        [Display(Name = "Did you have a test task?")]
        public bool? TestTask { get; set; }
        
        [Display(Name = "How long did your interview take?")]
        public int? InterviewLengthId { get; set; }

        [Display(Name = "Describe your experience with NKT Interviewer")]
        public string NoteFromInterviewee { get; set; }

    }
}