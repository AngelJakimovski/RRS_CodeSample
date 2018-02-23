using AARP.Models;
using AARP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AARP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConfigurationController : Controller
    {

        public ActionResult Index()
        {
            return View(new AppConfigurationsViewModel());
        }

        [HttpPost]
        public ActionResult Index(AppConfigurationsViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.SaveChanges();
                BackgroundJobsManager.Initialize();
                //TO DO: update background jobs here
                TempData["Message"] = new MessageViewModel()
                {
                    Type = MessageType.Success,
                    Title = "Success",
                    Content = string.Format("Your configuration has been updated at {0} UTC ... New changes will take effect in 1 min", DateTime.UtcNow),
                };

                return RedirectToAction("Index");
            }
            return View(vm);
        }
    }
}