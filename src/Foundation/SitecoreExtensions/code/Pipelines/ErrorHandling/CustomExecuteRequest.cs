using System.Web;
using Sitecore;
using Sitecore.Abstractions;
using Sitecore.Configuration;
using Sitecore.Web;

namespace Wageworks.Foundation.SitecoreExtensions.Pipelines.ErrorHandling
{
    public class CustomExecuteRequest : global::Sitecore.Pipelines.HttpRequest.ExecuteRequest
    {
        private readonly BaseLinkManager _baseLinkManager;

        public CustomExecuteRequest(BaseSiteManager baseSiteManager, BaseItemManager baseItemManager, BaseLinkManager baseLinkManager) : base(baseSiteManager, baseItemManager)
        {
            _baseLinkManager = baseLinkManager;
        }

        protected override void PerformRedirect(string url)
        {
            if (Context.Site == null || Context.Database == null || Context.Database.Name == "core")
            {
                _404Logger.Log.Info(string.Format("Attempting to redirect url {0}, but no Context Site or DB defined (or core db redirect attempted)",url));
                return;
            }

            // need to retrieve not found item to account for sites utilizing virtualFolder attribute
            var notFoundItem = Context.Database.GetItem(Context.Site.StartPath + Settings.ItemNotFoundUrl.Replace("-", " "));
            if (notFoundItem == null)
            {
                _404Logger.Log.Info(string.Format("No 404 item found on site: {0}",Context.Site.Name));
                return;
            }

            var notFoundUrl = _baseLinkManager.GetItemUrl(notFoundItem);
            if (string.IsNullOrWhiteSpace(notFoundUrl))
            {
                _404Logger.Log.Info(string.Format("Found 404 item for site, but no URL returned: {0}",Context.Site.Name));
                return;
            }

            _404Logger.Log.Info(string.Format("Redirecting to {0}",notFoundUrl));
            if (Settings.RequestErrors.UseServerSideRedirect)
            {
                HttpContext.Current.Server.TransferRequest(notFoundUrl);
            }
            else
            {
                WebUtil.Redirect(notFoundUrl, false);
            }
        }
    }
}