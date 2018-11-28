using log4net;
using Sitecore.Diagnostics;

namespace Wageworks.Foundation.SitecoreExtensions.Pipelines.ErrorHandling
{
    public static class _404Logger
    {
        public static ILog Log => LogManager.GetLogger("CustomErrors._404Logger") ?? LoggerFactory.GetLogger(typeof(_404Logger));
    }
}