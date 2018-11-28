using Sitecore;

namespace Wageworks.Feature.News.Infrastructure.Pipelines
{
    using Sitecore.Pipelines;
    using System.Web.Routing;
    using Wageworks.Foundation.DependencyInjection;

    [Service]
    public class InitializeRoutes
    {
        public void Process(PipelineArgs args)
        {
            if (!Context.IsUnitTesting)
            {
                RouteConfig.RegisterRoutes(RouteTable.Routes);
            }
        }
    }
}