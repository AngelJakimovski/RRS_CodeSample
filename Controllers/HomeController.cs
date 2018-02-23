using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AARP.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Title = "Home";
            return View();
        }

        public ActionResult SendTestEmail()
        {
            dynamic email = new Postal.Email("Test");
            email.To = "vu.l@scopicsoftware.com";
            email.Message = "How do you feel to day. Sent at " + DateTime.Now.ToString();
            email.Send();
            return RedirectToAction("Index");
        }
    }
}