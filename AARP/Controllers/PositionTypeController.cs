using AARP.Models;
using AARP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AARP.Controllers
{
    [Authorize]
    public class PositionTypeController : BaseController
    {
        public ActionResult Index()
        {
            var service = new JobPositionTypeProvider();
            return View(service.GetList());
        }

        [HttpPost]
        public ActionResult Create(JobPositionType jobCatg)
        {
            if (ModelState.IsValid)
            {
                var service = new JobPositionTypeProvider();
                var results = service.Add(jobCatg);
                return Json(results);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        public ActionResult Edit(int id)
        {
            var service = new JobPositionTypeProvider();
            var positionType = service.Get(id);
            if (positionType == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            return Json(positionType);
        }

        [HttpPost]
        public ActionResult Edit(JobPositionType jobCatg)
        {
            if (ModelState.IsValid)
            {
                var service = new JobPositionTypeProvider();
                var results = service.Update(jobCatg);
                return Json(results);
            }

            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var service = new JobPositionTypeProvider();
            service.Delete(id);
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        public ActionResult Get(int id)
        {
            var service = new JobPositionTypeProvider();
            var results = service.Get(id);
            if (results == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);

            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}
