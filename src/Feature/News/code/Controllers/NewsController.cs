namespace Wageworks.Feature.News.Controllers
{
    using Sitecore;
    using Sitecore.Data.Fields;
    using Sitecore.Data.Items;
    using Sitecore.Mvc.Presentation;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;
    using Wageworks.Feature.News.Caching;
    using Wageworks.Feature.News.Models;
    using Wageworks.Feature.News.Repositories;
    using Wageworks.Foundation.ORM.Context;
    using Wageworks.Foundation.SitecoreExtensions.Extensions;

    public class NewsController : Controller
    {

        private static readonly ExternalNewsCache _cache = new ExternalNewsCache(StringUtil.ParseSizeString("10MB"));

        IControllerSitecoreContext context;

        public NewsController(INewsRepository newsRepository, IControllerSitecoreContext context)
        {
            this.Repository = newsRepository;
            this.context = context;
        }

        private INewsRepository Repository { get; }

        public ActionResult NewsList()
        {
            var items = this.Repository.Get(RenderingContext.Current.Rendering.Item);
            return this.View("NewsList", items);
        }

        public ActionResult NewsGrouping()
        {
            var model = new NewsGroupingViewModel();

            foreach (Item item in RenderingContext.Current.Rendering.Item?.GetChildren().OrderBy(n => n.Appearance.Sortorder))
            {
                // TODO: verify template of item
                var title = item[Templates.NewsGrouping.Fields.Title];
                if (String.IsNullOrEmpty(title)) continue;

                var newsList = new List<Item>();
                var members = item[Templates.NewsGrouping.Fields.Members].Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var member in members)
                {
                    var page = context.Database.GetItem(member);
                    if (page != null)
                        newsList.Add(page);
                }

                if (!model.Groups.ContainsKey(title))
                    model.Groups.Add(title, newsList);
            }

            return this.View("NewsGrouping", model);
        }

        public ActionResult NewsLandingPage()
        {
            var repo = new SitecoreNewsModelRepository(Repository, context);

            var item = RenderingContext.Current.Rendering.Item;

            int pageSize;
            if (!Int32.TryParse(item[Templates.NewsFolder.Fields.PageSize], out pageSize)) pageSize = 2;

            var q = string.IsNullOrEmpty(Request.QueryString[News.Constants.TopicQueryString]) ? string.Empty : Regex.Replace(Request.QueryString[News.Constants.TopicQueryString], @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-");
            var model = repo.GetNewsList(item.ID.ToString(), 1, pageSize, null, false, q);


            return View(model);

        }

        public ActionResult NewsPromoSection()
        {
            var repo = new SitecoreNewsModelRepository(Repository, context);
            var model = repo.GetNewsPromos();
            return View("RelatedArticles", model);
        }

        public ActionResult RelatedArticles()
        {
            var repo = new SitecoreNewsModelRepository(Repository, context);
            var model = repo.GetRelatedNews();
            return View("RelatedArticles", model);
        }

        [HttpPost]
        public ActionResult PagedNewsList(PagedNewsListRequestModel request)
        {
            var repo = new SitecoreNewsModelRepository(Repository, context);
            var model = repo.GetNewsList(request.ParentPage, request.Page, request.PageSize, request.Tags, request.VideosOnly);
            model.HideContainer = true;
            return View("NewsLandingPage", model);

        }

        public ActionResult LatestNews()
        {
            //TODO: change to parameter template
            var count = RenderingContext.Current.Rendering.GetIntegerParameter("count", 5);
            var items = this.Repository.GetLatest(RenderingContext.Current.Rendering.Item, count);
            return this.View("LatestNews", items);
        }

        public ActionResult ExternalNews(string id)
        {

            var renderingId = RenderingContext.CurrentOrNull.Rendering.RenderingItem.ID;

            var service = new ExternalNewsRepository();
            var news = _cache.Get(renderingId) ?? service.GetNews();

            var model = new ExternalNewsViewModel();
            model.NewsList = news.Take(15);

            if (!string.IsNullOrEmpty(id))
            {
                var selectedNews = news.FirstOrDefault(n => n.ID.ToString() == id);
                if (selectedNews == null)
                    selectedNews = news.FirstOrDefault();

                model.SelectedNews = selectedNews;
            }
            else
                model.SelectedNews = news.FirstOrDefault();

            return View(model);
        }

        public ActionResult RelatedNewsAlert()
        {
            var root = RenderingContext.Current.Rendering.Item;
            if (root == null) return View("AlertMessageBar");

            var product = GetContextItem();
            if (product == null) return View("AlertMessageBar");

            var warnings = Foundation.Commerce.Extensions.CommerceExtensions.GetProductWarnings(root, product);
            if (warnings == null || !warnings.Any()) return View("AlertMessageBar");

            var warning = warnings.First();
            if (warnings.Count == 1) return View("AlertMessageBar", warning);

            foreach (var w in warnings.Skip(1))
            {
                var currentEndDate = (DateField)warning.Fields[Templates.NewsArticle.Fields.Date];
                var otherEndDate = (DateField)w.Fields[Templates.NewsArticle.Fields.Date];

                if (otherEndDate.DateTime > currentEndDate.DateTime)
                    warning = w;

            }

            return View("AlertMessageBar", warning);
        }

        private HttpRequest GetRequest()
        {
            return System.Web.HttpContext.Current.Request;
        }

        private Item GetContextItem()
        {
            return Wageworks.Foundation.Commerce.Extensions.CommerceExtensions.GetContextItem(GetRequest());
        }
    }
}
