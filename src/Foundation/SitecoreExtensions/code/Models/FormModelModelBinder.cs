using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Data;
using Sitecore.Diagnostics;

namespace Wageworks.Foundation.SitecoreExtensions.Models
{
    public class FormModelModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            object model = base.BindModel(controllerContext, bindingContext);
            if (typeof(IFormModel).IsAssignableFrom(bindingContext.ModelType))
            {
                var formModel = model as IFormModel;
                Assert.IsNotNull(formModel, "viewModel must not be null");
                var formDataValue = controllerContext.HttpContext.Request["__AjaxFormData"];
                ShortID dataSourceId;
                if (ShortID.TryParse(formDataValue, out dataSourceId))
                {
                    if (formModel != null) formModel.Item = Sitecore.Context.Database.GetItem(dataSourceId.ToID());
                }
            }
            return model;
        }
    }
}