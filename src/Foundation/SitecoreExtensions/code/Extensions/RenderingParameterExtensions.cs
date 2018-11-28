using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;

namespace Wageworks.Foundation.SitecoreExtensions.Extensions
{
    public static class RenderingParameterExtensions
    {
        public static string ToJson(this RenderingParameters renderingParameters)
        {
            var keyValues = renderingParameters as IEnumerable<KeyValuePair<string, string>>;
            if (keyValues != null)
            {
                var renderingParams = new JObject();
                foreach (var keyValue in keyValues)
                {
                    renderingParams.Add(keyValue.Key, keyValue.Value);
                }

                return JsonConvert.SerializeObject(renderingParams);
            }

            return string.Empty;

        }
        public static string GetCssClassFromParameters(this RenderingParameters parameters)
        {
            if (!parameters.Contains(Constants.PromoLayoutParameters.CssVariant)) return string.Empty;
            var classIds = parameters[Constants.PromoLayoutParameters.CssVariant].Split('|');
#if DEBUG
            foreach (var classId in classIds)
            {
                var guid = Guid.Empty;
                Assert.IsTrue(Guid.TryParse(classId, out guid), "Unable to parse CSS class " + classId + ".");
            }
#endif
            var db = Sitecore.Context.Database;
            var classes = classIds.Select(id => db.GetItem(id)[Constants.PromoLayoutParameters.CssFieldName]);
            var fullClassAssignment = string.Join(" ", classes);
            return fullClassAssignment;
        }
    }
}