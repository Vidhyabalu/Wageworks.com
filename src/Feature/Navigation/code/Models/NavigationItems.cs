namespace Wageworks.Feature.Navigation.Models
{
  using System.Collections.Generic;
  using Sitecore.Mvc.Presentation;

  public class NavigationItems : RenderingModel
  {
    public IList<NavigationItem> NavItems { get; set; }
  }
}