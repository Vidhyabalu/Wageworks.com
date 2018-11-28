using System;
using System.Net;
using System.Web;
using Sitecore.Configuration;
using Sitecore.Pipelines.HttpRequest;

namespace Wageworks.Foundation.SitecoreExtensions.Pipelines.ErrorHandling
{
    public class Set404StatusCode : HttpRequestBase
    {
        protected override void Execute(HttpRequestArgs args)
        {
            var context = HttpContext.Current;
            // retain 500 response if previously set
            if (HttpContext.Current.Response.StatusCode >= 500 || context.Request.RawUrl == "/")
            {
                return;
            }

            // return if request does not end with value set in ItemNotFoundUrl, i.e. successful page
            if (!context.Request.Url.LocalPath.EndsWith(Settings.ItemNotFoundUrl, StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            _404Logger.Log.Warn("Page Not Found: " + context.Request.RawUrl + ", current status: " + HttpContext.Current.Response.StatusCode);
            HttpContext.Current.Response.TrySkipIisCustomErrors = true;
            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.NotFound;
            HttpContext.Current.Response.StatusDescription = "Page not found";
        }
    }
}