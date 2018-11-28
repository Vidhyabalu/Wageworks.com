namespace Wageworks.Feature.Navigation.Models
{
    using Sitecore.Data.Items;

    public class NavigationItem
    {
        public Item Item { get; set; }
        public Item TargetItem { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public int Level { get; set; }
        public NavigationItems Children { get; set; }
        public string Target { get; set; }
        public bool ShowChildren { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string CssClass { get; set; }
        public string NavigationTitle { get; set; }

        public bool IsWildcard { get; set; }
        public bool ShowInBreadcrumb { get; set; }
        public string Anchor { get; set; }

    }
}