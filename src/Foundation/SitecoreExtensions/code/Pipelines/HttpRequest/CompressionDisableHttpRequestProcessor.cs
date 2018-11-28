using Sitecore.Configuration;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Linq;

namespace Wageworks.Foundation.SitecoreExtensions.Pipelines.HttpRequest
{
    public class CompressionDisableHttpRequestProcessor : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            var paths = Settings.GetSetting("CompressionExclusionPaths")?.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (paths == null || !paths.Any()) return;

            if (args.RequestUrl.PathAndQuery.ToLower().StartsWith("/sitecore/service/keepalive.aspx")) return;

            if (!paths.Any(p => args.RequestUrl.PathAndQuery.ToLower().StartsWith(p))) return;

            try
            {
                args.HttpContext.Request.Headers["Accept-Encoding"] = "";
            }
            catch (Exception ex)
            {
                var log = ex; // TODO: Log exception
            }
        }
    }
}