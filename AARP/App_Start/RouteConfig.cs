using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AARP
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Survey",
                "{controller}/{intervieweeId}/{interviewerId}",
                new { controller = "Survey", action = "Index" },
                new { intervieweeId = @"\d+", interviewerId = @"\d+" }
            );

            routes.MapRoute(
                "Interviewee",
                "{controller}/{intervieweeId}",
                new { controller = "Evaluations", action = "Interviewee" },
                new { IntervieweeId = @"\d+" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
