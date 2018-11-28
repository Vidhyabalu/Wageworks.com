using Sitecore.Mvc.Presentation;

namespace Wageworks.Foundation.SitecoreExtensions.Repositories
{
    public interface IRenderingPropertiesRepository
  {
    T Get<T>(Rendering rendering);
  }
}