using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Diagnostics;

namespace Wageworks.Foundation.SitecoreExtensions.Extensions
{
    public static class StringExtensions
    {
        public static string Humanize(this string input)
        {
            return Regex.Replace(input, "(\\B[A-Z])", " $1");
        }

        public static string ToCssUrlValue(this string url)
        {
            return string.IsNullOrWhiteSpace(url) ? "none" : $"url('{url}')";
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
  (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }

        }
        public static string FastQueryEscape(this string str)
        {
            return Regex.Replace(str.Replace("-", " "), @"[^/]*(\s[^/]*)+", "#$&#");
        }

        public static string ToCleanUrl(this string str)
        {
            return Regex.Replace(str, @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-").ToLower();
        }

        public static List<T> ToTypedList<T>(this ArrayList list)
        {
            var result = new List<T>();
            foreach (T item in list)
            {
                result.Add(item);
            }

            return result;
        }

        static Regex matcher = new Regex(@"(?:youtu\.be\/|youtube.com\/(?:watch\?.*\bv=|embed\/|v\/)|ytimg\.com\/vi\/)(.+?)(?:[^-a-zA-Z0-9_]|$)");
        public static string ToYoutubeId(this string url)
        {
            var youtubeId = string.Empty;

            Match youtubeMatch = matcher.Match(url);

            if (youtubeMatch.Success)
                youtubeId = youtubeMatch.Groups[1].Value;

            return youtubeId;
        }

        //public static IEnumerable<T> OrderBySequence<T, TId>(
        //    this IEnumerable<T> source,
        //    IEnumerable<TId> order,
        //    Func<T, TId> idSelector)
        //{
        //    var lookup = source.ToLookup(idSelector, t => t);
        //    foreach (var id in order)
        //    {
        //        foreach (var t in lookup[id])
        //        {
        //            yield return t;
        //        }
        //    }
        //}
        public static List<T> OrderBySequence<T, TId>(
            this IEnumerable<T> source,
            IEnumerable<TId> order,
            Func<T, TId> idSelector)
        {
            var lookup = source.ToLookup(idSelector, t => t);
            var finalList = order.SelectMany(id => lookup[id]).ToList();

            //foreach (var id in order)
            //{
            //    foreach (var t in lookup[id])
            //    {
            //        finalList.Add(t);
            //    }
            //}

            foreach (var item in source)
            {
                if (!finalList.Contains(item))
                    finalList.Add(item);
            }

            return finalList;

        }

        public static string ReplaceTagTokens(this string tag)
        {
            var sessionToken = "[SessionID]";
            try
            {
                tag = tag.Replace(sessionToken, HttpContext.Current.Session.SessionID);
            }
            catch (Exception e)
            {
                Log.Error("Could not replace tag token. Raw tag: " + tag, e, typeof(StringExtensions));
                return tag;
            }
           

            return tag;
        }

    }
}