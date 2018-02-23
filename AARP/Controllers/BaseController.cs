using System;
using System.Web.Mvc;
using log4net;

namespace AARP.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ILog _logger = LogManager.GetLogger(typeof(BaseController));

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }
            LogError(filterContext);
            filterContext.ExceptionHandled = true;
            base.OnException(filterContext);
        }

        private void LogError(ExceptionContext exceptionContext)
        {
            var message = exceptionContext.Exception.Message + Environment.NewLine +
                          exceptionContext.Exception.StackTrace;
            if (exceptionContext.Exception.InnerException != null)
            {
                message += Environment.NewLine + exceptionContext.Exception.InnerException.Message + Environment.NewLine +
                           exceptionContext.Exception.InnerException.StackTrace;
            }

            _logger.Error(message);
        }
    }
}