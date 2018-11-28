using log4net;
using Sitecore.Diagnostics;

namespace Vista.Foundation.SitecoreExtensions.Logging
{
    public class VistaLogger
    {
        public static ILog Log => LogManager.GetLogger("VistaLogger") ?? LoggerFactory.GetLogger(typeof(VistaLogger));
    }
}