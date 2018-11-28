using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Vista.Foundation.SitecoreExtensions.Helpers
{
    public static class Utilities
    {
        public static string GetUserIP(HttpContext context)
        {
            try
            {
                var trueip = context.Request.Headers["True-Client-IP"];
                string ip = "";
                string rawip = "";


                if (!String.IsNullOrEmpty(trueip) && !CheckIsIPV6(trueip))
                {
                    Logging.VistaLogger.Log.Info("Use True IP");
                    ip = trueip;
                }
                else {
                    Logging.VistaLogger.Log.Info("TrueIP is IPV6 use X-Forwarded-For");

                    rawip = (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null
                        && context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "")
                        ? context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
                        : context.Request.ServerVariables["REMOTE_ADDR"];

                    
                    ip = rawip.Split(',').Last().Trim();
                }
                
                Logging.VistaLogger.Log.Info("CF-Psuedo-IPV4: " + context.Request.Headers["Cf-Pseudo-IPv4"]);
                Logging.VistaLogger.Log.Info("True-Client-IP: " + trueip);
                Logging.VistaLogger.Log.Info("GetUserIp Raw: " + rawip);
                
                if (string.Equals(ip, "::1"))
                    ip = "127.0.0.1";

                IPAddress address;
                if (IPAddress.TryParse(ip, out address))
                {
                    switch (address.AddressFamily)
                    {
                        case System.Net.Sockets.AddressFamily.InterNetwork:
                            // we have IPv4
                            break;
                        case System.Net.Sockets.AddressFamily.InterNetworkV6:
                            Logging.VistaLogger.Log.Info("GetUserIp Have IPv6: " + ip);
                            ip = "127.0.0.1";
                            break;
                    }
                }
                Logging.VistaLogger.Log.Info("GetUserIp response: " + ip);
                return ip;
            }
            catch (Exception ex)
            {
                Logging.VistaLogger.Log.Error("GetUserIp: " + ex.ToString());
                return string.Empty;
            }
        }

        private static bool CheckIsIPV6(string ip) {

            IPAddress address;
            if (IPAddress.TryParse(ip, out address))
            {
                switch (address.AddressFamily)
                {
                    case System.Net.Sockets.AddressFamily.InterNetwork:
                        return false;
                    case System.Net.Sockets.AddressFamily.InterNetworkV6:
                        return true;
                }
            }

            return false;
        }

        public static string BuildUrl(string baseUrl, NameValueCollection query)
        {
            if (string.IsNullOrWhiteSpace(baseUrl) || query == null || !query.HasKeys())
                return baseUrl;

            var hasQuestionMark = baseUrl.Contains("?");
            var first = true;
            var sb = new StringBuilder(baseUrl);
            foreach(var key in query.Cast<string>().Where(k => !string.IsNullOrWhiteSpace(query[k])))
            {
                if (first && !hasQuestionMark)
                    sb.Append("?");
                else
                    sb.Append("&");
                sb.Append(key);
                sb.Append("=");
                sb.Append(query[key]);
                first = false;
            }
            return sb.ToString();
        }
    }
}