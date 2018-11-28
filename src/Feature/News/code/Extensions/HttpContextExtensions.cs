using System;
using System.Web;

namespace Wageworks.Feature.News.Extensions
{
    public static class HttpContextExtensions
    {
        public static void SetCookie(this HttpContext context, string key, string value, int dayExpires)
        {
            context.Response.Cookies[key].Value = value;
            context.Response.Cookies[key].Expires = DateTime.Now.AddDays(dayExpires);
        }

        public static void SetCookie(this HttpContext context, string key, string value)
        {
            SetCookie(context, key, value, 360);
        }

        public static string GetCookie(this HttpContext context, string key)
        {
            if (context.Request.Cookies[key] != null)
            {
                return context.Request.Cookies[key].Value;
            }
            else
                return null;
        }
    }
}