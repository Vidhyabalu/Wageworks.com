namespace Wageworks.Feature.Navigation.Extensions
{
    public static class StringExtensions
    {
        public static string ToSocialAttribute(this string cssClass)
        {
            if (string.IsNullOrEmpty(cssClass)) return string.Empty;

            var css = cssClass.ToLower().Replace("network-", "");
            switch (css)
            {
                case "facebook":
                case "twitter":
                case "email":
                    return $"data-network={css}";

                default:
                    return string.Empty;
            }
        }
    }
}