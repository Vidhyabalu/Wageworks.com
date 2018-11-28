using System;
using Sitecore.Events;

namespace Vista.Foundation.SitecoreExtensions.Events.ClearCache
{
    [Serializable]
    public class ClearCacheEventArgs : EventArgs, IPassNativeEventArgs
    {
        public string[] CacheNames { get; set; }

        public ClearCacheEventArgs(string[] cacheNames)
        {
            this.CacheNames = cacheNames;
        }
    }
}
