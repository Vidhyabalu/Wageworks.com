using Sitecore.Mvc.Presentation;

namespace Vista.Foundation.SitecoreExtensions.Repositories
{
    public interface IRenderingPropertiesRepository
  {
    T Get<T>(Rendering rendering);
  }
}