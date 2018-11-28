using Sitecore.Diagnostics;
using Sitecore.Eventing;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using Wageworks.Foundation.SitecoreExtensions.Events.ClearCache;

namespace Wageworks.Foundation.SitecoreExtensions.Commands
{
    public class ClearCache : Command
    {
        public const string CacheRegion = "region";
        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            var cacheRegion = context.Parameters["region"];

            if (string.IsNullOrWhiteSpace(cacheRegion)) return;

            var cacheRegions = cacheRegion.Split('|');

            var cacheEvent = new ClearCacheEvent(cacheRegions);
            //Sitecore.Eventing.EventManager.QueueEvent<ClearCacheEvent>(cacheEvent);
            //var eq = new Sitecore.Eventing.DefaultEventQueueProvider();
            //eq.QueueEvent<ClearCacheEvent>(cacheEvent);

            (new DefaultEventQueueProvider()).QueueEvent<ClearCacheEvent>(cacheEvent, true, true);

            SheerResponse.Alert(
                "The Cache has been queued for clearing. Please allow a few minutes for the process to complete");

            Log.Info(string.Format("Cache queued for clearing. Regions: {0}", cacheRegion), this);

        }
    }
}
