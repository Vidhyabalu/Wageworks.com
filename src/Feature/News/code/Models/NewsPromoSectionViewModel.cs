using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;

namespace Vista.Feature.News.Models
{
    public class NewsPromoSectionViewModel
    {
        public string Title { get; set; }
        public List<NewsListModel> Articles { get; set; } = new List<NewsListModel>();
        public Link DetailsPage { get; set; } = new Link();
    }
}