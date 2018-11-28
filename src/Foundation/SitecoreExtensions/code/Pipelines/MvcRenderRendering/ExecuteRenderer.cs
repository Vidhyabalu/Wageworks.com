using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Wageworks.Foundation.SitecoreExtensions.Pipelines.MvcRenderRendering
{
    public class ExecuteRenderer : Sitecore.Mvc.Pipelines.Response.RenderRendering.ExecuteRenderer
    {
        public ExecuteRenderer(IRendererErrorStrategy errorStrategy) : base(errorStrategy)
        {
        }

        public override void Process(RenderRenderingArgs args)
        {
            try
            {
                base.Process(args);
            }
            catch (HttpAntiForgeryException forgeryException)
            {
                Redirect(forgeryException);
            }
            catch (Exception ex) when (ex.InnerException is HttpAntiForgeryException)
            {
                Redirect(ex);
            }
        }

        private void Redirect(Exception ex)
        {
            Log.Warn("Caught HttpAntiForgeryException", ex, this);
            var referrer = HttpContext.Current.Request.UrlReferrer;
            string returnUrl = string.Empty;
            if (referrer != null && !string.IsNullOrWhiteSpace(referrer.Query))
            {
                returnUrl = System.Web.HttpUtility.ParseQueryString(referrer.Query)["returnUrl"];
            }
          
            var redirectLink = string.IsNullOrWhiteSpace(returnUrl) ? System.Web.HttpContext.Current.Request.Url.PathAndQuery : returnUrl;
            Log.Info("Redirecting due to AntiForgeryException. URL: " + redirectLink, this);
            HttpContext.Current.Response.Redirect(redirectLink, true);
        }

    }
}