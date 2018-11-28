using System;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Wageworks.Foundation.SitecoreExtensions
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

        /// <summary>
        /// resets AntiForgery validation token and update a cookie
        /// The new antiforgery cookie is set as the results and sent
        /// back to client with Ajax
        /// </summary>
        /// <param name="Request">request from current context</param>
        /// <returns>string - a form token to pass to AJAX response</returns>
        public static string UpdateRequestVerificationToken(HttpRequestBase Request)
        {
            string formToken;
            string cookieToken;
            const string __RequestVerificationToken = "__RequestVerificationToken";
            AntiForgery.GetTokens(Request.Form[__RequestVerificationToken], out cookieToken, out formToken);
            if (!Request.Cookies.AllKeys.Contains(__RequestVerificationToken)) return formToken;
            HttpCookie cookie = Request.Cookies[__RequestVerificationToken];
            if (cookie == null) return formToken;
            cookie.HttpOnly = true;
            cookie.Name = __RequestVerificationToken;
            cookie.Value = cookieToken;
            HttpContext.Current.Response.Cookies.Add(cookie);
            return formToken;
        }
    }
}