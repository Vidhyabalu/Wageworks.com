namespace Wageworks.Feature.News
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("news-fetch", "api/feature/news/fetch", new { controller = "News", action = "PagedNewsList", id = UrlParameter.Optional });
        }
    }
}