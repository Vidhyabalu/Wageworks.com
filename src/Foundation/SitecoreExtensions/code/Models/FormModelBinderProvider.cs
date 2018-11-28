using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Wageworks.Foundation.SitecoreExtensions.Models
{
    public class FormModelBinderProvider: Dictionary<Type, IModelBinder>, IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            var binders = from binder in this
                where binder.Key.IsAssignableFrom(modelType)
                select binder.Value;

            return binders.FirstOrDefault();
        }
    }
}