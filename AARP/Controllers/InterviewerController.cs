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
    public class InterviewerController : BaseController
    {
        [HttpPost]
        public JsonResult GetInterviewer(string key)
        {
            var service = new InterviewerProvider();
            var interviewers = service.SearchInteviewer(key);
            return Json(interviewers, JsonRequestBehavior.AllowGet);
        }
    }
}
