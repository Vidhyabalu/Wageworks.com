using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Items;

namespace Wageworks.Foundation.SitecoreExtensions.Extensions
{
    public class ProductExtensions
    {
        public static string GetTopLevelCategory(IEnumerable<string> pathway)
        {
            //todo; make more configurable. (1) position is the 2nd item in array of pathaways
            if (pathway?.Count() > 1) return pathway.ElementAt(1);

            return string.Empty;
        }
      public static string GetProductFamily(IEnumerable<string> pathway)
        {
            //todo; make more configurable. (1) position is the 2nd item in array of pathaways
            if (pathway?.Count() > 2) return pathway.ElementAt(2);

            return string.Empty;
        }

        public static string GetFirstTokenValue(IEnumerable<string> values)
        {
            return values != null && values.Any() ? values.FirstOrDefault() : string.Empty;
        }

        public static string GetLastTokenValue(IEnumerable<string> values)
        {
            return values != null && values.Any() ? values.LastOrDefault() : string.Empty;
        }

      
    }
}