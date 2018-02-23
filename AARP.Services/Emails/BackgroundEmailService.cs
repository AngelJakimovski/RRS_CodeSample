using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using Postal;

namespace AARP.Services.Emails
{
    public static class BackgroundEmailService
    {
        public static IEmailService Create()
        {
            // Prepare Postal classes to work outside of ASP.NET request
            var viewsPath = Path.GetFullPath(HostingEnvironment.MapPath(@"~/Views/Emails"));
            var engines = new ViewEngineCollection {new FileSystemRazorViewEngine(viewsPath)};

            return new EmailService(engines);
        }
    }
}
