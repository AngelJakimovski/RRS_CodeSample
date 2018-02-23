using AARP.Infrashtructure.DI;
using AARP.Models;
using AARP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AARP.WebApi;
using System.Data.Entity.SqlServer;

namespace AARP.Controllers
{
    [Authorize]
    public class ReviewerController : BaseController
    {
        IWebApiClient greenHouseAPI = DiContainers.Global.Resolve<IWebApiClient>();
        Random random = new Random();

        public ActionResult Index()
        {
            var reviewers = new ReviewerProvider();
            var jobApplicants = new JobApplicationProvider();

           
            var avgDurations = jobApplicants.GetList()
                .GroupBy(rvw => rvw.ReviewerId)
                .Select(g => new
                {
                    g.Key,
                    AvgDuration =
                        g.Average(
                            x =>
                                ((x.RejectedOrAcceptedAt ?? DateTime.UtcNow) - x.AssignedToReviewerAt).HasValue
                                    ? Math.Max(
                                        ((x.RejectedOrAcceptedAt ?? DateTime.UtcNow) - x.AssignedToReviewerAt).Value
                                            .TotalSeconds/3600, 0)
                                    : 0)
                });

            //var results = reviewers.GetList()
            //    .Join(avgDurations, rvw => rvw.Id, ad => ad.Key, (rvw, ad) => new ReviewerViewModel { Reviewer = rvw, AvgTime = ad.AvgDuration });

            var results = reviewers.GetList()
                .GroupJoin(avgDurations,
                            rvw => rvw.Id,
                            ad => ad.Key,
                            (rvw, ms) => new { rvw, ms = ms.DefaultIfEmpty() })
                            .SelectMany(
                                  z => z.ms.Select(ad => new { rvw = z.rvw, ad }))
                                  .Select(x=> new ReviewerViewModel() {
                                      Reviewer = x.rvw,
                                      AvgTime = x.ad == null? 0 : Math.Round(x.ad.AvgDuration, 2)
                                  });



            return View(results.ToList());
        }

        public ActionResult Create()
        {
            var provider = new ReviewerProvider();
            return View(new Reviewer());
        }

        [HttpPost]
        public ActionResult Create(Reviewer reviewer)
        {
            Random random = new Random();
            if (ModelState.IsValid)
            {
                var provider = new ReviewerProvider();
                reviewer.Id = provider.GetNextId();
                provider.Add(reviewer);
                return RedirectToAction("Index");
            }
            return View(reviewer);
        }

        public ActionResult Details(int id)
        {
            var service = new ReviewerProvider();
            var reviewer = service.Get(id);
            if (reviewer == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            else
                return View(reviewer);
        }

        public ActionResult Edit(int id)
        {
            var service = new ReviewerProvider();
            var reviewer = service.Get(id);
            if (reviewer == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            return View(reviewer);
        }

        [HttpPost]
        public ActionResult Edit(Reviewer reviewer)
        {
            if (ModelState.IsValid)
            {
                var service = new ReviewerProvider();
                service.Update(reviewer);
                var message = new MessageViewModel()
                {
                    Type = MessageType.Success,
                    Title = "Success",
                    Content = string.Format("Reviewer {0} ({1}) has been updated...", reviewer.Name, reviewer.Email),
                };
                TempData["Message"] = message;

                return RedirectToAction("Index");
            }
            return View(reviewer);
        }

        public ActionResult Delete(int id)
        {
            var service = new ReviewerProvider();
            return View(service.Get(id));
        }

        [HttpPost]
        public ActionResult Delete(Reviewer reviewer)
        {
            var service = new ReviewerProvider();
                service.Delete(reviewer.Id);
                var message = new MessageViewModel()
                {
                    Type = MessageType.Warning,
                    Title = "Deleted",
                    Content = string.Format("A reviewer has been deleted...", reviewer.Name, reviewer.Email),
                };
                TempData["Message"] = message;
                return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SyncData()
        {
            var count = 0;
            var reviewerProvider = new ReviewerProvider();
            var message = new MessageViewModel();
            var usersGreenhouse = greenHouseAPI.GetUsers();
            if (usersGreenhouse == null || usersGreenhouse.Count == 0)
            {
                message.Type = MessageType.Error;
                message.Title = "Not Found";
                message.Content = "There're not data found vie GreenHouse API ";
                TempData["Message"] = message;
                return RedirectToAction("Index");
            }

            foreach (var user in usersGreenhouse)
            {
                var reviewer = reviewerProvider.Get(int.Parse(user.Id));
                if (reviewer != null)
                    continue;
                reviewer = new Reviewer()
                {
                    Id = int.Parse(user.Id),
                    Name = user.Name,
                    Email = string.Join(";", user.Emails),
                };
                reviewerProvider.Add(reviewer);
                count++;
            }

            if (count == 0)
            {
                message.Type = MessageType.Info;
                message.Title = "Finished";
                message.Content = string.Format("No more updates found...", count);
            }
            else
            {
                message.Type = MessageType.Success;
                message.Title = "Success";
                message.Content = string.Format("Added new {0} reviewer(s)...", count);
            }

            TempData["Message"] = message;

            return RedirectToAction("Index");
        }

        public ActionResult Get()
        {
            var service = new ReviewerProvider();
            return Json(service.GetList(), JsonRequestBehavior.AllowGet);
        }
    }
}
