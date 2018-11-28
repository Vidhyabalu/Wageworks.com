using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wageworks.Foundation.SitecoreExtensions.Extensions
{
    public static class LinkExtensions
    {
        public static string RemoveSslPort(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return url;
            }

            if (url.Contains(":443"))
            {
                url = url.Replace(":443", string.Empty);
            }

            return url;
        }

        public static string RemoveLastPart(string filepath, Uri requestUrl)
        {
            var urlSegments = filepath.Split(new char[] {'/'}, StringSplitOptions.RemoveEmptyEntries);

            var lastPart = urlSegments[urlSegments.Length - 1];

            var uri = requestUrl;
            var noLastSegment = uri.GetComponents(UriComponents.SchemeAndServer,
                UriFormat.SafeUnescaped);

            for (int i = 0; i < uri.Segments.Length - 1; i++)
            {
                noLastSegment += uri.Segments[i];
            }

            noLastSegment = noLastSegment.Trim("/".ToCharArray()); // remove trailing `/`

            return noLastSegment;
        }

        public static string RemoveLastPartOfPath(string filePath)
        {
            var urlSegments = filePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            return string.Join("/", urlSegments.Take(urlSegments.Length - 1));

        }

        public static string GetLastPart(string filepath)
        {
            var urlSegments = filepath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            var lastPart = urlSegments[urlSegments.Length - 1];

            return lastPart;
        }

        public static Uri AddParameter(this Uri url, string paramName, string paramValue)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[paramName] = paramValue;
            uriBuilder.Query = query.ToString();

            return uriBuilder.Uri;
        }
    }
}