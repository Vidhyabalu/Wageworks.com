using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc;
using Sitecore.Mvc.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Wageworks.Foundation.SitecoreExtensions.Attributes;
using Wageworks.Foundation.SitecoreExtensions.Controls;
using Wageworks.Foundation.SitecoreExtensions.Models;

namespace Wageworks.Foundation.SitecoreExtensions.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static HtmlString ImageField(this SitecoreHelper helper, ID fieldID, int mh = 0, int mw = 0, string cssClass = null, bool disableWebEditing = false)
        {
            return helper.Field(fieldID.ToString(), new
            {
                mh,
                mw,
                DisableWebEdit = disableWebEditing,
                @class = cssClass ?? ""
            });
        }

        public static HtmlString ImageField(this SitecoreHelper helper, ID fieldID, Item item, int mh = 0, int mw = 0, string cssClass = null, bool disableWebEditing = false)
        {
            return helper.Field(fieldID.ToString(), item, new
            {
                mh,
                mw,
                DisableWebEdit = disableWebEditing,
                @class = cssClass ?? ""
            });
        }

        public static HtmlString ImageField(this SitecoreHelper helper, string fieldName, Item item, int mh = 0, int mw = 0, string cssClass = null, bool disableWebEditing = false)
        {
            return helper.Field(fieldName, item, new
            {
                mh,
                mw,
                DisableWebEdit = disableWebEditing,
                @class = cssClass ?? ""
            });
        }

        public static EditFrameRendering BeginEditFrame<T>(this HtmlHelper<T> helper, string dataSource, string buttons)
        {
            var frame = new EditFrameRendering(helper.ViewContext.Writer, dataSource, buttons);
            return frame;
        }

        public static HtmlString DynamicPlaceholder(this SitecoreHelper helper, string placeholderName, bool useStaticPlaceholderNames = false)
        {
            return useStaticPlaceholderNames ? helper.Placeholder(placeholderName) : helper.DynamicPlaceholder(placeholderName);
        }

        public static HtmlString DynamicPlaceholder(this SitecoreHelper helper, string placeholderName, string className, int count)
        {
            return helper.DynamicPlaceholder(placeholderName, CreateColumnWrapper(className), count: count);
        }
        private static TagBuilder CreateColumnWrapper(string className)
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass(className);
            return tagBuilder;
        }


        public static HtmlString Field(this SitecoreHelper helper, ID fieldID)
        {
            Assert.ArgumentNotNullOrEmpty(fieldID, nameof(fieldID));
            return helper.Field(fieldID.ToString());
        }

        public static HtmlString Field(this SitecoreHelper helper, ID fieldID, object parameters)
        {
            Assert.ArgumentNotNullOrEmpty(fieldID, nameof(fieldID));
            Assert.IsNotNull(parameters, nameof(parameters));
            return helper.Field(fieldID.ToString(), parameters);
        }

        public static HtmlString Field(this SitecoreHelper helper, ID fieldID, Item item, object parameters)
        {
            Assert.ArgumentNotNullOrEmpty(fieldID, nameof(fieldID));
            Assert.IsNotNull(item, nameof(item));
            Assert.IsNotNull(parameters, nameof(parameters));
            return helper.Field(fieldID.ToString(), item, parameters);
        }

        public static HtmlString Field(this SitecoreHelper helper, ID fieldID, Item item)
        {
            Assert.ArgumentNotNullOrEmpty(fieldID, nameof(fieldID));
            Assert.IsNotNull(item, nameof(item));
            return helper.Field(fieldID.ToString(), item);
        }

        /// <summary>
        /// Generates a hidden form field for use with form validation
        /// Required for the <see cref="ValidateRenderingIdAttribute">ValidateRenderingIdAttribute</see> to work
        /// </summary>
        /// <param name="htmlHelper">Html Helper class</param>
        /// <returns>Hidden form field with unique ID</returns>
        public static MvcHtmlString AddUniqueFormId(this HtmlHelper htmlHelper)
        {
            var uid = htmlHelper.Sitecore().CurrentRendering?.UniqueId;
            return !uid.HasValue ? null : htmlHelper.Hidden(ValidateRenderingIdAttribute.FormUniqueid, uid.Value);
        }

        public static MvcHtmlString ValidationErrorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string error)
        {
            return htmlHelper.HasError(ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData), ExpressionHelper.GetExpressionText(expression)) ? new MvcHtmlString(error) : null;
        }

        public static bool HasError(this HtmlHelper htmlHelper, ModelMetadata modelMetadata, string expression)
        {
            var modelName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expression);
            var formContext = htmlHelper.ViewContext.FormContext;
            if (formContext == null)
            {
                return false;
            }

            if (!htmlHelper.ViewData.ModelState.ContainsKey(modelName))
            {
                return false;
            }

            var modelState = htmlHelper.ViewData.ModelState[modelName];
            var modelErrors = modelState?.Errors;
            return modelErrors?.Count > 0;
        }


        public static HtmlString RenderDate(this SitecoreHelper sitecoreHelper, ID fieldId, Item item, string format = "D", bool disableWebEdit = false, bool setCulture = true, SafeDictionary<string> parameters = null)
        {
            return RenderDate(
              sitecoreHelper,
              fieldId.ToString(),
              item,
              format,
              disableWebEdit,
              setCulture,
              parameters);
        }

        public static HtmlString RenderDate(this SitecoreHelper sitecoreHelper, string fieldNameOrId, Item item, string format = "D", bool disableWebEdit = false, bool setCulture = true, SafeDictionary<string> parameters = null)
        {
            if (setCulture)
            {
                Thread.CurrentThread.CurrentUICulture =
                  new CultureInfo(Sitecore.Context.Language.Name);
                Thread.CurrentThread.CurrentCulture =
                  CultureInfo.CreateSpecificCulture(Sitecore.Context.Language.Name);
            }

            if (parameters == null)
            {
                parameters = new SafeDictionary<string>();
            }

            parameters["format"] = format;
            return RenderField(sitecoreHelper, fieldNameOrId, item, disableWebEdit, parameters);
        }

        public static HtmlString RenderField(this SitecoreHelper sitecoreHelper, string fieldNameOrId, Item item, bool disableWebEdit = false, SafeDictionary<string> parameters = null)
        {
            if (parameters == null)
            {
                parameters = new SafeDictionary<string>();
            }

            return sitecoreHelper.Field(fieldNameOrId,
              new
              {
                  DisableWebEdit = disableWebEdit,
                  Parameters = parameters
              });
        }

        public static MvcHtmlString TextBoxDisabledFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool disabled, object htmlAttributes = null)
        {
            return TextBoxDisabledFor(htmlHelper, expression, disabled, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString TextBoxDisabledFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool disabled, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null)
                htmlAttributes = new Dictionary<string, object>();
            if (disabled)
                htmlAttributes["disabled"] = "disabled";
            return htmlHelper.TextBoxFor(expression, htmlAttributes);
        }

        public static MvcHtmlString TextBoxReadOnlyFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool readOnly, object htmlAttributes = null)
        {
            return TextBoxReadOnlyFor(htmlHelper, expression, readOnly, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString TextBoxReadOnlyFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool readOnly, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null)
                htmlAttributes = new Dictionary<string, object>();
            if (readOnly)
                htmlAttributes["readonly"] = "readonly";
            return htmlHelper.TextBoxFor(expression, htmlAttributes);
        }

        public static HtmlString FormatForPhone(this HtmlHelper helper, string phone)
        {
            string s = string.Format("{0}", phone).Trim();
            phone = phone.Replace("-", ".");
            phone = phone.Replace(" ", ".");
            phone = phone.Replace("(", "");
            phone = phone.Replace(")", "");

            return new HtmlString(phone);
        }

        public static MvcHtmlString FormData(this HtmlHelper htmlHelper, IFormModel model)
        {
            Assert.ArgumentNotNull(model, "model");
            Assert.IsNotNull(model.Item, "IFormModel.Item cannot be null");
            return htmlHelper.Hidden("__AjaxFormData", model.Item.ID.ToShortID().ToString());
        }

        public static MvcHtmlString RadioButtonsListForEnum<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
        {
            var sb = new StringBuilder();
            sb.Append("<ul>");
            foreach (var value in Enum.GetValues(typeof(TEnum)))
            {
                var radio = htmlHelper.RadioButtonFor(expression, value).ToHtmlString();

                //radio = radio.Replace("checked=\"checked\"", "data-val-mandatory=\"This field is required\"");

                sb.AppendFormat(
                    "<li><label>{0} {1}</label></li>",
                    radio,
                    ((Enum)value).GetEnumDisplayName()
                );
            }
            sb.Append("</ul");
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString CheckBoxListForEnum<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TEnum>>> expression, IDictionary<string, object> htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            //var enumValue = Convert.ToInt32(metadata.Model);
            var model = metadata.Model as IEnumerable<TEnum>;
            var sb = new StringBuilder();
            sb.Append("<ul>");
            foreach (var value in Enum.GetValues(typeof(TEnum)))
            {
                var val = Convert.ToInt32(value);
                var checkbox = new TagBuilder("input");
                bool ischecked = (model == null) ? false : model.Any(x => x.ToString() == value.ToString());

                checkbox.MergeAttribute("type", "checkbox");
                checkbox.MergeAttribute("value", value.ToString());
                checkbox.MergeAttribute("name", htmlHelper.NameFor(expression).ToString());

                if (ischecked)
                    checkbox.MergeAttribute("checked", "checked");
                if (htmlAttributes != null)
                    checkbox.MergeAttributes(htmlAttributes);

                var label = new TagBuilder("label") { InnerHtml = checkbox + ((Enum)value).GetEnumDisplayName() };
                sb.AppendFormat("<li>{0}</li>", label);
            }
            sb.Append("</ul");
            return new MvcHtmlString(sb.ToString());
        }

        public static string GetEnumDisplayName(this Enum enumType)
        {
            return enumType.GetType().GetMember(enumType.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                .Name;
        }
    }
}