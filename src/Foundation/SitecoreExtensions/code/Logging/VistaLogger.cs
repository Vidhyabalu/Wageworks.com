using log4net;
using Sitecore.Diagnostics;

namespace Wageworks.Foundation.SitecoreExtensions.Logging
{
    public class WageworksLogger
    {
        public static ILog Log => LogManager.GetLogger("WageworksLogger") ?? LoggerFactory.GetLogger(typeof(WageworksLogger));
    }
}