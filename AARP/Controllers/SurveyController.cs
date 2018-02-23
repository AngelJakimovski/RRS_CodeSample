using AARP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AARP.Controllers
{
    public class SurveyController : BaseController
    {
        public ActionResult Index(int intervieweeId, int interviewerId)
        {
            using (var dbContext = new AARPDbContext())
            {
                var interview = dbContext.Interviews.Where(t => t.IntervieweeId == intervieweeId && t.InterviewerId == interviewerId)
                    .OrderByDescending(o => o.Date).FirstOrDefault();

                if (interview != null)
                {
                    var survey = new SurveyViewModel
                    {
                        Id = interview.Id,
                    };

                    ViewBag.InterviewLengths = dbContext.InterviewLengths.ToList();

                    return View(survey);
                }
                else
                {
                    throw new ArgumentException("Interview with specified parameters not found.");
                }

            }
        }

        [HttpPost]
        public ActionResult Save(SurveyViewModel model)
        {
            using (var dbContext = new AARPDbContext())
            {
                var interview = dbContext.Interviews.FirstOrDefault(t => t.Id == model.Id);

                if (interview != null)
                {
                    interview.RatingId = model.RatingId;
                    interview.InterviewerAttitude = model.InterviewerAttitude;
                    interview.TestTask = model.TestTask;
                    interview.TaskDifficultyLevel = model.TaskDifficultyLevel;
                    interview.InterviewLengthId = model.InterviewLengthId;
                    interview.NoteFromInterviewee = model.NoteFromInterviewee;

                    dbContext.SaveChanges();
                }

            }

            return PartialView("Success");
        }
    }
}
