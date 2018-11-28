using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.Caching;
using Sitecore.Caching.Generics;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Wageworks.Foundation.SitecoreExtensions.Extensions
{
    public static class JsonExtensions
    {
        //TODO: Clean up cache usage
        public static ICache<string> IndexingCache;
        private const string _indexingCacheName = "indexing_cache";
        private const long _cacheSize = 128000000;


        public static IEnumerable<string> GetTokenValues(Item item, string fieldName, string jsonPath)
        {
            try
            {
                if (item == null || string.IsNullOrWhiteSpace(fieldName) || string.IsNullOrWhiteSpace(jsonPath))
                    return new List<string>();


                IndexingCache = CacheManager.FindCacheByName<string>(_indexingCacheName) ?? new Cache<string>(_indexingCacheName, _cacheSize);

                var itemField = item.Fields[fieldName];

                if (itemField == null)
                {
                    return null;
                }
                JObject json = null;
                var cacheKey = $"{item.ID.Guid}-{fieldName}";
                if (IndexingCache.ContainsKey(cacheKey))
                {
                    json = IndexingCache.GetValue(cacheKey) as JObject;
                }

                if (json == null)
                {
                    var fieldValue = itemField.Value;
                    if (string.IsNullOrEmpty(fieldValue))
                    {
                        return null;
                    }
                    try
                    {
                        json = JObject.Parse(fieldValue);
                    }
                    catch (Exception e)
                    {
                        Log.Warn("Could not parse json field to JObject., Exception: " + e.Message, e, typeof(JsonExtensions));
                        return null;
                    }

                    if (json != null)
                        IndexingCache.Add(cacheKey, json, new TimeSpan(2, 0, 0));
                }

                if (json == null) return null;

                var tokens = json.SelectTokens(jsonPath)
                    .Where(e => e != null && !string.IsNullOrEmpty(e.Value<string>()))
                    .Select(x => x.Value<string>());

                return tokens != null && tokens.Any() ? tokens.ToList<string>() : null;
            }
            catch (Exception e)
            {
                Log.Warn("Could not get value from product json field., Exception: " + e.Message, e, typeof(JsonExtensions));
                return null;
            }
      
        }

        public static string SerializeObjectToJson<T>(this T toSerialize)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, toSerialize);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;

        }

        public static T DeserializJsonToObject<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }

        public static T LoadJsonFile<T>(string filePath)
        {
            T items = default(T);
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<T>(json);
            }

            return items;
        }
    }
}