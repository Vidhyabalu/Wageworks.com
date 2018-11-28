using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Sitecore.Caching;
using Sitecore.Caching.Generics;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Commerce.Engine.Connect;
using Sitecore.Commerce.Engine.Connect.DataProvider;
using System.Net.Http;
using Sitecore.Configuration;

namespace Wageworks.Foundation.SitecoreExtensions.Events.ClearCache
{
    public class ClearCacheEventHandler
    {
        public static void Run(ClearCacheEvent @event)
        {
            Log.Info("ClearCacheEventhandler - Run", typeof(ClearCacheEventHandler));
            ClearCacheEventArgs args = new ClearCacheEventArgs(@event.CacheNames);
            Event.RaiseEvent("Wageworks:clearcacheremote", new object[] { args });
        }
        public virtual void OnClearCacheRemote(object sender, EventArgs e)
        {
            var args = (ClearCacheEventArgs)e;
            var cacheRegions = args.CacheNames;

            foreach (var cacheName in cacheRegions)
            {
                switch (cacheName)
                {
                    case "All":
                        InvalidateCache();
                        break;
                    case "Commerce":
                        InvalidateCommerceCache();
                        break;
                    default:
                        InvalidateCache(cacheName);
                        break;

                }
            }
        }

        private void InvalidateCache()
        {
            //clear Sitecore Cache
            CacheManager.ClearAllCaches();

            //clear HttpRuntime Cache
            IDictionaryEnumerator enumerator = HttpRuntime.Cache.GetEnumerator();

            while (enumerator.MoveNext())
            {
                HttpRuntime.Cache.Remove((string)enumerator.Key);
            }
            Log.Info("Manually Cleared All Caches via Sitecore Interface", this);

            InvalidateCommerceCache();

        }

        private void InvalidateCache(string cacheName)
        {
            //clear Sitecore Cache
            var cacheList = CacheManager.GetAllCaches().Where(c => c.Name.StartsWith(cacheName));
            foreach (var cache in cacheList)
            {
                cache.Clear();
                Log.Info(string.Format("Manually Cleared Sitecore Cache ({0}) via Sitecore Interface", cache.Name), this);
            }

            //clear HttpRuntime Cache
            var cacheNames = new List<string>();
            IDictionaryEnumerator enumerator = HttpRuntime.Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var key = (string)enumerator.Key;
                if (key.StartsWith(cacheName))
                {
                    cacheNames.Add(key);
                }
            }
            foreach (var name in cacheNames)
            {
                HttpRuntime.Cache.Remove(name);
                Log.Info(string.Format("Manually Cleared HttpRuntime Cache ({0}) via Sitecore Interface", name), this);
            }


        }

        private void InvalidateCommerceCache()
        {
            ICache<string> cache = CacheManager.FindCacheByName<string>("CommerceCache.Default");
            if (cache != null)
            {
                cache.Clear();
                Log.Info(string.Format("Manually Cleared Commerce Cache ({0}) via Sitecore Interface", cache.Name), this);
            }


            CatalogRepository.MappingEntries = null;
            Log.Info("Manually Cleared Commerce Cache (CatalogRepository.MappingEntries) via Sitecore Interface", this);

            EngineConnectUtility.RefreshSitecoreCaches("web");
            Log.Info("Manually Cleared Commerce Cache ( EngineConnectUtility.RefreshSitecoreCaches[web]) via Sitecore Interface", this);

            var ceConfig = (CommerceEngineConfiguration)Factory.CreateObject("commerceEngineConfiguration", true);
            var client = GetClient(ceConfig);
            var commerceEndpoint = "RequestCacheReset()";

            // var opsApi = "commerceops";

            if (string.IsNullOrEmpty(commerceEndpoint))
            {
                throw new ConfigurationErrorsException("Missing Commerce endpoint configuration.");
            }

            var content = new MultipartFormDataContent
            {
                { new StringContent(ceConfig.DefaultEnvironment), "environmentName" },
                { new StringContent(ceConfig.DefaultEnvironment), "cacheStoreName" }
            };

            var result = client.PutAsync(commerceEndpoint, content).Result;
            if (result.IsSuccessStatusCode)
            {
                Log.Info("Manually Cleared Commerce Cache (Shops Cache) via Sitecore Interface", this);
            }
            else
            {
                var error = result.Content.ReadAsStringAsync().Result;
                Log.Error($"Could not clear shops cache, error: {error}", this);
            }


        }

        private static HttpClient GetClient(CommerceEngineConfiguration config)
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = new System.Uri(config.CommerceOpsServiceUrl)
            };

            //TODO: populate below from context
            httpClient.DefaultRequestHeaders.Add("ShopName", config.DefaultShopName);
            httpClient.DefaultRequestHeaders.Add("Language", "en-US");
            httpClient.DefaultRequestHeaders.Add("Currency", config.DefaultShopCurrency);
            httpClient.DefaultRequestHeaders.Add("Environment", config.DefaultEnvironment);

            string certificate = config.GetCertificate();
            if (certificate != null)
                httpClient.DefaultRequestHeaders.Add(config.CertificateHeaderName, certificate);
            httpClient.Timeout = new System.TimeSpan(0, 0, 600);
            return httpClient;
        }
    }
}
