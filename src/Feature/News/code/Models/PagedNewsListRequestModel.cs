using System.Collections.Generic;

namespace Wageworks.Feature.News.Models
{
    public class PagedNewsListRequestModel
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 6; // TODO: Retrieve from settings
        public string ParentPage { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public bool VideosOnly { get; set; }
    }
}