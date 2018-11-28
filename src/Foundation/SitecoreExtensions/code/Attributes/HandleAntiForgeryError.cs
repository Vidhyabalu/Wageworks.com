using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Configuration;

namespace Wageworks.Foundation.SitecoreExtensions.Attributes
{
    public class HandleAntiForgeryError : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!(filterContext.Exception is HttpAntiForgeryException)) return;

            var routeValues = new RouteValueDictionary
            {
                ["controller"] = "Account",
                ["action"] = "Login"
            };
            filterContext.Result = new RedirectResult(System.Web.HttpContext.Current.Request.Url.PathAndQuery);
            filterContext.ExceptionHandled = true;
        }
    }
}