using Newtonsoft.Json;
using Sitecore.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Wageworks.Feature.News.Models.Json;

namespace Wageworks.Feature.News.Repositories
{
    public class ExternalNewsRepository
    {

        public List<ExternalNewsModel> GetNews()
        {
            // TODO: move to sitecore config
            var setting = $"Wageworks.Feature.News.ExternalNewsFeedUrl.{Sitecore.Context.Site.Name}";

            var config = Settings.GetSetting(setting);
            if (string.IsNullOrEmpty(config))
                return new List<ExternalNewsModel>();

            List<ExternalNewsModel> result;

            try
            {
                using (var client = new WebClient())
                {

                    var response = client.DownloadString(config);
                    var encoded = Encoding.UTF8.GetString(Encoding.Default.GetBytes(response));
                    result = JsonConvert.DeserializeObject<List<ExternalNewsModel>>(encoded);
                }
            }
            catch (Exception)
            {
                // TODO: log exception
                result = new List<ExternalNewsModel>();
            }

            return result.OrderByDescending(n => n.DATE_DISPLAYED).ToList();
        }
    }
}