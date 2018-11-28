using Sitecore.Data.Items;

namespace Wageworks.Foundation.SitecoreExtensions.Models
{
    public interface IFormModel
    {
        Item Item { get; set; }
    }
}