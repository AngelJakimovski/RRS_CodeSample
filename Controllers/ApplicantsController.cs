using AARP.Models;
using AARP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AARP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ApplicantsController : Controller
    {
        public ActionResult Index(string order)
        {
            var jobApplicantService = new JobApplicationProvider();
            var reviewerResourceServuce = new ReviewerProvider();
            var results = jobApplicantService.GetList().OrderByDescending(m => m.AppliedAt)
                .Select(applicant => new ApplicantViewModel()
                {
                    Applicant = applicant,
                    Reviewer = applicant.ReviewerId.HasValue ? reviewerResourceServuce.Get(applicant.ReviewerId.Value) : null
                });

            return View(results);
        }

        [HttpPost]
        public ActionResult UpdateApplicants()
        {
            BackgroundJobsManager.RefreshAssignedApplicantsState();

            var message = new MessageViewModel()
            {
                Type = MessageType.Success,
                Title = "Success",
                Content = string.Format("All assigned applicants have beed updated at {0}", DateTime.Now),
            };
            TempData["Message"] = message;


            return RedirectToAction("Index");
        }
    }
}