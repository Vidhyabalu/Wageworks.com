using Sitecore.Links;
using System.Web;
using Wageworks.Foundation.Commerce.Extensions;

namespace Wageworks.Feature.Navigation.Repositories
{
    using Sitecore;
    using Sitecore.Data.Items;
    using Sitecore.Mvc.Presentation;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Wageworks.Feature.Navigation.Models;
    using Wageworks.Foundation.DependencyInjection;
    using Wageworks.Foundation.SitecoreExtensions.Extensions;

    [Service(typeof(INavigationRepository), Lifetime = Lifetime.Transient)]
    public class NavigationRepository : INavigationRepository
    {
        public Item ContextItem => RenderingContext.Current?.Rendering.Item ?? Sitecore.Context.Item;

        public Item NavigationRoot { get; }

        public NavigationRepository()
        {
            this.NavigationRoot = this.GetNavigationRoot(this.ContextItem);
            if (this.NavigationRoot == null)
            {
                throw new InvalidOperationException($"Cannot determine navigation root from '{this.ContextItem.Paths.FullPath}'");
            }
        }

        public Item GetNavigationRoot(Item contextItem)
        {
            return contextItem.GetAncestorOrSelfOfTemplate(Templates.NavigationRoot.ID) ?? Context.Site.GetContextItem(Templates.NavigationRoot.ID);
        }

        public NavigationItems GetBreadcrumb()
        {
            var items = new NavigationItems
            {
                NavItems = this.GetNavigationHierarchy(true).Where(item => !item.IsWildcard && item.ShowInBreadcrumb).Reverse().ToList()
            };

            for (var i = 0; i < items.NavItems.Count - 1; i++)
            {
                items.NavItems[i].Level = i;
                items.NavItems[i].IsActive = i == items.NavItems.Count - 1;
            }

            return items;
        }

        public NavigationItems GetPrimaryMenu()
        {
            var navItems = this.GetChildNavigationItems(this.NavigationRoot, 0, 1);

            this.AddRootToPrimaryMenu(navItems);
            return navItems;
        }

        private void AddRootToPrimaryMenu(NavigationItems navItems)
        {
            if (!this.IncludeInNavigation(this.NavigationRoot))
            {
                return;
            }

            var navigationItem = this.CreateNavigationItem(this.NavigationRoot, 0, 0);
            //Root navigation item is only active when we are actually on the root item
            navigationItem.IsActive = this.ContextItem.ID == this.NavigationRoot.ID;
            navItems?.NavItems?.Insert(0, navigationItem);
        }

        private bool IncludeInNavigation(Item item, bool forceShowInMenu = false)
        {
            var result = item.HasContextLanguage()
                   && (item.IsDerived(Templates.Navigable.ID) || item.IsDerived(Templates.NavigationRoot.ID))
                   && (forceShowInMenu || MainUtil.GetBool(item[Templates.Navigable.Fields.ShowInNavigation], false));
            return result;
        }

        public NavigationItem GetSecondaryMenuItem()
        {
            var rootItem = this.GetSecondaryMenuRoot();
            return rootItem == null ? null : this.CreateNavigationItem(rootItem, 0, 3);
        }

        public NavigationItems GetLinkMenuItems(Item menuRoot, int level = 0, int maxLevel = 0)
        {
            if (menuRoot == null)
            {
                throw new ArgumentNullException(nameof(menuRoot));
            }
            return this.GetChildNavigationItems(menuRoot, level, maxLevel);
        }

        public NavigationItems GetMegaMenu()
        {
            var navItems = this.GetChildNavigationItems(this.NavigationRoot, 0, 5);

            //this.AddRootToPrimaryMenu(navItems);
            return navItems;
        }

        private Item GetSecondaryMenuRoot()
        {
            return this.FindActivePrimaryMenuItem();
        }

        private Item FindActivePrimaryMenuItem()
        {
            var primaryMenuItems = this.GetPrimaryMenu();
            //Find the active primary menu item
            var activePrimaryMenuItem = primaryMenuItems.NavItems.FirstOrDefault(i => i.Item.ID != this.NavigationRoot.ID && i.IsActive);
            return activePrimaryMenuItem?.Item;
        }

        private IEnumerable<NavigationItem> GetNavigationHierarchy(bool forceShowInMenu = false)
        {
            var item = this.ContextItem;
            while (item != null)
            {
                if (this.IncludeInNavigation(item, forceShowInMenu))
                {
                    yield return this.CreateNavigationItem(item, 0);
                }

                item = item.Parent;
            }
        }

        private NavigationItem CreateNavigationItem(Item item, int level, int maxLevel = -1)
        {
            var targetItem = item.IsDerived(Templates.Link.ID) ? item.TargetItem(Templates.Link.Fields.Link) : item;
            return new NavigationItem
            {
                Item = item,
                Url = GetItemUrl(item),
                Target = item.IsDerived(Templates.Link.ID) ? item.LinkFieldTarget(Templates.Link.Fields.Link) : "",
                TargetItem = item.IsDerived(Templates.Link.ID) ? item.LinkFieldTargetItem(Templates.Link.Fields.Link): null,
                Anchor = item.IsDerived(Templates.Link.ID) ? item.LinkFieldAnchor(Templates.Link.Fields.Link) : "",
                CssClass = item.IsDerived(Templates.Link.ID) ? item.LinkFieldClass(Templates.Link.Fields.Link) : "",
                IsActive = this.IsItemActive(targetItem ?? item),
                Children = this.GetChildNavigationItems(item, level + 1, maxLevel),
                ShowChildren = !item.IsDerived(Templates.Navigable.ID) || item.Fields[Templates.Navigable.Fields.ShowChildren].IsChecked(),
                Description = item.IsDerived(Templates.LinkDescription.ID) ? item[Templates.LinkDescription.Fields.Description] : string.Empty,
                ImageUrl = item.IsDerived(Templates.LinkMenuItem.ID) ? item.ImageUrl(Templates.LinkMenuItem.Fields.Icon) : string.Empty,
                IsWildcard = GetIsWildcard(item),
                NavigationTitle = GetNavigationTitle(item),
                ShowInBreadcrumb = GetShowInBreadCrumb(item)
            };
        }

        private bool GetShowInBreadCrumb(Item item)
        {
            return item.IsDerived(Templates.Navigable.ID) && item.Fields[Templates.Navigable.Fields.ShowInBreadcrumb].IsChecked();
        }

        private string GetItemUrl(Item item)
        {
            if (item.Name != "*" || !item.IsProductDetailsPage())
                return item.IsDerived(Templates.Link.ID) ? item.LinkFieldUrl(Templates.Link.Fields.Link) : item.Url();

            var contextItem =
                CommerceExtensions.GetContextItem(HttpContext.Current.Request);
            if (contextItem != null && (contextItem.IsProduct() || contextItem.IsProductVariant()))
            {
                return LinkManager.GetItemUrl(contextItem);
            }

            return item.Url();
        }

        private bool GetIsWildcard(Item item)
        {
            if (item.Name != "*") return false;

            if (!item.IsProductDetailsPage()) return true;

            var contextItem =
                CommerceExtensions.GetContextItem(HttpContext.Current.Request);
            return contextItem == null || (!contextItem.IsProduct() && !contextItem.IsProductVariant());
        }

        private string GetNavigationTitle(Item item)
        {
            if (item.Name != "*" || !item.IsProductDetailsPage()) return item[Templates.Navigable.Fields.NavigationTitle];

            var contextItem =
                CommerceExtensions.GetContextItem(HttpContext.Current.Request);

            if (contextItem != null && (contextItem.IsProduct() || contextItem.IsProductVariant()))
            {
                return CommerceExtensions.GetProductTitle(contextItem);
            }

            return item[Templates.Navigable.Fields.NavigationTitle];

        }
        private NavigationItems GetChildNavigationItems(Item parentItem, int level, int maxLevel)
        {
            if (level > maxLevel || !parentItem.HasChildren)
            {
                return null;
            }
            var childItems = parentItem.Children.Where(item => this.IncludeInNavigation(item)).Select(i => this.CreateNavigationItem(i, level, maxLevel));
            var result = new NavigationItems
            {
                NavItems = childItems.ToList()
            };
            return result;
        }

        private bool IsItemActive(Item item)
        {
            return this.ContextItem.ID == item.ID || this.ContextItem.Axes.GetAncestors().Any(a => a.ID == item.ID);
        }
    }
}