using System.Collections.Generic;
using Wageworks.Feature.News.Models;

namespace Wageworks.Feature.News.Repositories
{
    public class DemoNewsModelRepository : INewsModelRepository
    {
        public NewsListViewModel GetNewsList(string parentId, int page = 1, int pageSize = 6, List<string> tags = null, bool videosOnly = false, string queryFilter = "")
        {
            return GetNewsListInternal(parentId, page, pageSize, tags, videosOnly);
        }

        public NewsPromoSectionViewModel GetNewsPromos()
        {
            return GetNewsSection();
        }

        public NewsPromoSectionViewModel GetRelatedNews()
        {
            return GetNewsSection();
        }

        private NewsListViewModel GetNewsListInternal(string parentId, int page = 1, int pageSize = 6, List<string> tags = null, bool videosOnly = false)
        {
            var vm = new NewsListViewModel();

            for (var i = 0; i < pageSize; i++)
            {
                var model = new NewsListModel();
                model.Title = $"News Item {i + 1}";
                model.Summary = "Lorem ipsum summary for news item in list view.";
                model.BackgroundImage = "https://dummyimage.com/400x300/000/fff&text=News+Image";
                model.DetailsLink.Text = "Read More";
                model.DetailsLink.Url = "#";

                vm.PagedItems.Add(model);
            }

            vm.CurrentPage = page;
            vm.PageSize = pageSize;
            vm.Total = 64;

            vm.Tags.Add(new NewsFilterItem() { Name = "Hunting Tactics", Key = "hunting-tactics" });
            vm.Tags.Add(new NewsFilterItem() { Name = "Gun Maintenance", Key = "gun-maintenance" });
            vm.Tags.Add(new NewsFilterItem() { Name = "Shooting Tips", Key = "shooting-tips" });

            vm.VideosOnly = videosOnly;

            return vm;
        }

        private NewsPromoSectionViewModel GetNewsSection(int page = 1, int pageSize = 3, List<string> filters = null, List<string> types = null)
        {
            var vm = new NewsPromoSectionViewModel();

            for (var i = 0; i < pageSize; i++)
            {
                var model = new NewsListModel();
                model.Title = $"News Item {i + 1}";
                model.Summary = "Lorem ipsum summary for news item in list view.";
                model.BackgroundImage = "https://dummyimage.com/600x400/000/fff&text=News+Image";
                model.DetailsLink.Text = "Read More";
                model.DetailsLink.Url = "/expert-advice";

                vm.Articles.Add(model);
            }

            vm.DetailsPage = new Glass.Mapper.Sc.Fields.Link() { Text = "More", Url = "/expert-advice" };
            vm.Title = "Section Title";

            return vm;
        }
    }
}