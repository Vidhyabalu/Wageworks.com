using System.Collections.Generic;
using Vista.Feature.News.Models.Json;

namespace Vista.Feature.News.Models
{
    public class ExternalNewsViewModel
    {
        public ExternalNewsModel SelectedNews { get; set; }
        public IEnumerable<ExternalNewsModel> NewsList { get; set; } = new List<ExternalNewsModel>();
    }
}