namespace Wageworks.Feature.Navigation.Repositories
{
    using Sitecore.Data.Items;
    using Wageworks.Feature.Navigation.Models;

    public interface INavigationRepository
    {
        Item GetNavigationRoot(Item contextItem);
        NavigationItems GetBreadcrumb();
        NavigationItems GetPrimaryMenu();
        NavigationItem GetSecondaryMenuItem();
        NavigationItems GetLinkMenuItems(Item menuItem, int level = 0, int maxLevel = 0);
        NavigationItems GetMegaMenu();
    }
}