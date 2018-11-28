using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Pipelines;
using Wageworks.Foundation.SitecoreExtensions.Models;

namespace Wageworks.Foundation.SitecoreExtensions.Pipelines.Initialize
{
    public class RegisterModelBinder
    {
        public void Process(PipelineArgs args)
        {
            ModelBinderProviders.BinderProviders.Add(new FormModelBinderProvider
            {
                {typeof (IFormModel), new FormModelModelBinder()}
            });
        }
    }
}