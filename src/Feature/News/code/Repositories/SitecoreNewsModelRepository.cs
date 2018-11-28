using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Wageworks.Feature.News.Models;
using Wageworks.Foundation.ORM.Context;
using Wageworks.Foundation.SitecoreExtensions.Extensions;

namespace Wageworks.Feature.News.Repositories
{
    public class SitecoreNewsModelRepository : INewsModelRepository
    {
        private INewsRepository Repository { get; }
        IControllerSitecoreContext context;


        private void PopulateCategories(NewsListViewModel model, List<string> filters, Item parentItem, string queryFilter)
        {
            if (filters == null) filters = new List<string>();
            var articleTags = parentItem[Templates.NewsTaxonomy.Fields.Category]?.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries) ?? new List<string>().ToArray();

            foreach (string tag in articleTags)
            {
                var tagItem = context.Database.GetItem(tag);
                if (tagItem == null) continue;

                var tagModel = new NewsFilterItem();
                tagModel.ID = tagItem.ID.ToString();
                tagModel.Name = tagItem[Templates.Taxonomy.Fields.Title];
                tagModel.Key = Regex.Replace(tagModel.Name, @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-");
                tagModel.Selected = filters.Contains(tagModel.ID) || tagModel.Key.Equals(queryFilter, StringComparison.OrdinalIgnoreCase);
                if (tagModel.Selected && !filters.Contains(tagModel.ID))
                {
                    filters.Add(tag);
                }
                model.Tags.Add(tagModel);
            }
        }

        public SitecoreNewsModelRepository(INewsRepository newsRepository, IControllerSitecoreContext context)
        {
            this.Repository = newsRepository;
            this.context = context;
        }

        public NewsListViewModel GetNewsList(string parentId, int page = 1, int pageSize = 6, List<string> filters = null, bool videosOnly = false, string queryFilter = "")
        {
            var vm = new NewsListViewModel();
            vm.PageSize = pageSize;
            vm.CurrentPage = page;
            vm.VideosOnly = videosOnly;
            vm.ParentPage = parentId;

            var parentItem = context.Database.GetItem(parentId);
            var items = this.Repository.Get(parentItem);

            if (filters == null) filters = new List<string>();

            PopulateCategories(vm, filters, parentItem, queryFilter);

            var articles = items.AsQueryable();
            if (filters?.Any() ?? false)
            {
                articles = articles.Where(a => filters.Any(f => a[Templates.NewsArticle.Fields.Category].Contains(f)));
            }

            if (videosOnly)
            {
                articles = articles.Where(a => !string.IsNullOrWhiteSpace(a[Templates.NewsArticle.Fields.MediaVideoLink]));
            }

            vm.Total = articles.Count();

            foreach (var article in articles.Skip(pageSize * (page - 1)).Take(vm.PageSize))
            {
                var model = new NewsListModel(article);
                vm.PagedItems.Add(model);
            }

            return vm;
        }

        public NewsPromoSectionViewModel GetNewsPromos()
        {
            var contextItem = RenderingContext.Current.Rendering.Item;

            var vm = new NewsPromoSectionViewModel();
            vm.Title = contextItem[Templates.ExpertAdviceGroup.Fields.Title];
            LinkField lnk = contextItem.Fields[Templates.ExpertAdviceGroup.Fields.ListPage];
            vm.DetailsPage.Text = lnk.Text;
            vm.DetailsPage.Url = lnk.GetFriendlyUrl();

            var articleIds = contextItem[Templates.ExpertAdviceGroup.Fields.Articles]?.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var articleId in articleIds)
            {
                try
                {
                    var article = context.Database.GetItem(articleId);
                    if (article != null)
                    {
                        var model = new NewsListModel(article);
                        vm.Articles.Add(model);
                    }
                }
                catch (Exception)
                {
                    // TODO: Log error
                }
            }

            return vm;
        }

        public NewsPromoSectionViewModel GetRelatedNews()
        {
            var contextItem = RenderingContext.Current.Rendering.Item;

            var vm = new NewsPromoSectionViewModel();

            // TODO: add to constants or retrieve from settings/localization, or retrieve from field
            if (!string.IsNullOrEmpty(contextItem[Templates.NewsArticle.Fields.NewsListPage]))
            {


                vm.DetailsPage.Url = contextItem.LinkFieldUrl(Templates.NewsArticle.Fields.NewsListPage);
                vm.DetailsPage.Text = contextItem.LinkFieldText(Templates.NewsArticle.Fields.NewsListPage);
                vm.DetailsPage.Target = contextItem.LinkFieldTarget(Templates.NewsArticle.Fields.NewsListPage);

            }

            var articleIds = contextItem[Templates.NewsArticle.Fields.RelatedArticles]?.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var articleId in articleIds)
            {
                try
                {
                    var article = context.Database.GetItem(articleId);
                    if (article != null)
                    {
                        var model = new NewsListModel(article);
                        vm.Articles.Add(model);
                    }
                }
                catch (Exception)
                {
                    // TODO: log error
                }
            }

            return vm;
        }
    }
}