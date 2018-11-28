using Sitecore;

namespace Wageworks.Feature.Navigation.Controllers
{
    using Sitecore.Mvc.Presentation;
    using System.Web.Mvc;
    using Wageworks.Feature.Navigation.Models;
    using Wageworks.Feature.Navigation.Repositories;
    using Wageworks.Foundation.Alerts.Extensions;
    using Wageworks.Foundation.Alerts.Models;
    using Wageworks.Foundation.Dictionary.Repositories;

    public class NavigationController : Controller
    {
        private readonly INavigationRepository navigationRepository;

        public NavigationController(INavigationRepository navigationRepository)
        {
            this.navigationRepository = navigationRepository;
        }

        public ActionResult Breadcrumb()
        {
            var items = this.navigationRepository.GetBreadcrumb();
            return this.View("Breadcrumb", items);
        }

        public ActionResult PrimaryMenu()
        {
            var items = this.navigationRepository.GetPrimaryMenu();
            return this.View("PrimaryMenu", items);
        }

        public ActionResult SecondaryMenu()
        {
            var item = this.navigationRepository.GetSecondaryMenuItem();
            return this.View("SecondaryMenu", item);
        }


        public ActionResult MegaMenu()
        {
            var items = this.navigationRepository.GetMegaMenu();
            return this.View("MegaMenu", items);
        }

        public ActionResult NavigationLinks()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View(items);
        }

        public ActionResult LinkMenu()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("LinkMenu", items);
        }

        public ActionResult ListingLinks()
        {
            var items = GetSimpleMenu();
            return this.View(items);
        }

        public ActionResult NavList()
        {
            var items = GetSimpleMenu();
            return this.View("NavList", items);
        }

        public ActionResult Simple()
        {
            var items = GetSimpleMenu();
            return this.View("Simple", items);
        }

        private NavigationItems GetSimpleMenu()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 1, 2);
            return items;
        }
    }
}