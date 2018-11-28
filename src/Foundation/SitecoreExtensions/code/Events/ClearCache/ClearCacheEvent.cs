using System.Runtime.Serialization;

namespace Vista.Foundation.SitecoreExtensions.Events.ClearCache
{
    [DataContract]
    public class ClearCacheEvent
    {
        public ClearCacheEvent(string[] cacheNames)
        {
            this.CacheNames = cacheNames;
        }

        //Properties
        [DataMember]
        public string[] CacheNames { get; set; }
    }
}
