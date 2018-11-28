using System.Collections.Generic;
using Wageworks.Feature.News.Models.Json;

namespace Wageworks.Feature.News.Models
{
    public class ExternalNewsViewModel
    {
        public ExternalNewsModel SelectedNews { get; set; }
        public IEnumerable<ExternalNewsModel> NewsList { get; set; } = new List<ExternalNewsModel>();
    }
}