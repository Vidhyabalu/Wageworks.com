using Glass.Mapper.Sc.Fields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using Wageworks.Foundation.SitecoreExtensions.Extensions;

namespace Wageworks.Feature.News.Models
{
    public class NewsListViewModel
    {
        public List<NewsListModel> PagedItems { get; set; } = new List<NewsListModel>();
        public int Total { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get { return (int)Math.Ceiling((double)Total / PageSize); } }
        public int CurrentPage { get; set; }
        public string ParentPage { get; set; }
        public List<NewsFilterItem> Tags { get; set; } = new List<NewsFilterItem>();
        public bool VideosOnly { get; set; }
        public bool HideContainer { get; set; }
    }

    public class NewsFilterItem
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public bool Selected { get; set; }
    }

    public class NewsListModel
    {
        public NewsListModel()
        {

        }

        public NewsListModel(Item article)
        {
            this.Title = article[Templates.NewsArticle.Fields.Title];
            this.Summary = article[Templates.NewsArticle.Fields.Summary];

            // TODO: add to constants or retrieve from settings/localization
            this.DetailsLink = new Glass.Mapper.Sc.Fields.Link() { Url = article.Url(), Text = "Read More" };
            ImageField imgField = string.IsNullOrEmpty(article[Templates.NewsArticle.Fields.ThumbnailImage]) ? article.Fields[Templates.NewsArticle.Fields.Image] : article.Fields[Templates.NewsArticle.Fields.ThumbnailImage];

            if (imgField != null && imgField.MediaItem != null)
            {
                this.BackgroundImage = Sitecore.Resources.Media.HashingUtils.ProtectAssetUrl(MediaManager.GetMediaUrl(imgField.MediaItem, new MediaUrlOptions() { MaxWidth = 640 }));
            }
        }

        public string Title { get; internal set; }
        public string Summary { get; internal set; }
        public string BackgroundImage { get; internal set; }
        public Link DetailsLink { get; internal set; } = new Link();
    }
}