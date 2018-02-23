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
    public class ConfigurationController : BaseController
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
                AARP.WebApi.RecruiterBoxPhantomJsApi.Instance.Reinitialize(vm.RecruiterBox.Login, vm.RecruiterBox.Password);
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

        public ActionResult CheckConnection(string login, string password)
        {
            bool connected = AARP.WebApi.RecruiterBoxPhantomJsApi.Instance.ConnectionTest(login, password);
            if (connected)
            {
                return Json(new { Type = MessageType.Success, Title = "Success", Content = "Successfully connected to RecruiterBox Api" }, JsonRequestBehavior.AllowGet);
                    //TempData["CheckMessage"] = new MessageViewModel()
                    //{
                    //    Type = MessageType.Success,
                    //    Title = "Success",
                    //    Content = "Successfully connected to RecruiterBox Api",
                    //};
                }
            else
            {
                return Json(new { Type = MessageType.Error, Title = "Error", Content = "Wrong connection settings..." }, JsonRequestBehavior.AllowGet);
                //TempData["CheckMessage"] = new MessageViewModel()
                //{
                //    Type = MessageType.Error,
                //    Title = "Error",
                //    Content = "Wrong connection settings...",
                //};
            }
            
            //return Content(connected.ToString());
            //return new EmptyResult();
            //return RedirectToAction("Index"); 

        }
    }
}