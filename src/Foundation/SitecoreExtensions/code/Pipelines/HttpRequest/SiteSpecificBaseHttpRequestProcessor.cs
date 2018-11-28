using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;

namespace Wageworks.Foundation.SitecoreExtensions.Pipelines.HttpRequest
{
    public abstract class SiteSpecificBaseHttpRequestProcessor : HttpRequestProcessor
    {
        protected abstract void SiteSpecificProcess(HttpRequestArgs args);

        private readonly List<string> _sites;

        /// <summary>
        /// A list of lowercase site names to enable this processor for
        /// </summary>
        protected SiteSpecificBaseHttpRequestProcessor()
        {
            _sites = new List<string>();
        }

        /// <summary>
        /// Processes the specified args when the site name is in the list of sites.
        /// </summary>
        /// <param name="args">The args.</param>
        public override void Process(HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            if (Sitecore.Context.Site != null &&
                _sites.Contains(Sitecore.Context.Site.Name.ToLower()))
            {
                SiteSpecificProcess(args);
            }
        }

        /// <summary>
        /// Adds a site by name to the list of site the processor is active for. Called by Sitecore configuration utility
        /// </summary>
        /// <param name="site">The name of the site to add</param>
        public void AddSite(string site)
        {
            Assert.ArgumentNotNullOrEmpty(site, "site");

            _sites.Add(site.ToLower());
        }
    }
}