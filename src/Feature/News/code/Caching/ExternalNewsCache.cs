using Sitecore.Caching;
using Sitecore.Data;
using System.Collections.Generic;
using Vista.Feature.News.Models.Json;

namespace Vista.Feature.News.Caching
{
    public class ExternalNewsCache : CustomCache
    {
        public ExternalNewsCache(long maxSize) : base("Vista.Feature.News.ExternalNews", maxSize)
        {
        }

        public object Get(string cacheKey)
        {
            return (object)this.GetObject(cacheKey.ToString());
        }

        public void Set(string cacheKey, object requirementList)
        {
            this.SetObject(cacheKey.ToString(), requirementList);
        }

        public List<ExternalNewsModel> Get(ID cacheKey)
        {
            return (List<ExternalNewsModel>)this.GetObject(cacheKey.ToString());
        }

        public void Set(ID cacheKey, List<ExternalNewsModel> requirementList)
        {
            this.SetObject(cacheKey.ToString(), requirementList);
        }
    }
}