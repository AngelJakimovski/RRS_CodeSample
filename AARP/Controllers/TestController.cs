using AARP.Models;
using AARP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AARP.Controllers
{
    public class TestController : BaseController
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RunFailedJob()
        {
            TempData["Message"] = new MessageViewModel()
            {
                Title = "Success",
                Content = "A new failed job has been triggered at " + DateTime.Now.ToString(),
                Type = MessageType.Success,
            };

            Hangfire.BackgroundJob.Enqueue(() => RunAFailedJob());

            return RedirectToAction("Index");
        }

        [LogFailure]
        public static void RunAFailedJob()
        {
            throw new NotImplementedException("This job is expected to be failed");
        }
    }
}