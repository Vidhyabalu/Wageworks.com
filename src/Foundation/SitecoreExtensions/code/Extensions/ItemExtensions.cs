using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Resources.Media;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Wageworks.Foundation.SitecoreExtensions.Services;

namespace Wageworks.Foundation.SitecoreExtensions.Extensions
{
    public static class ItemExtensions
    {
        public static string Url(this Item item, UrlOptions options = null)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (options != null)
            {
                return LinkManager.GetItemUrl(item, options);
            }
            return !item.Paths.IsMediaItem ? LinkManager.GetItemUrl(item) : MediaManager.GetMediaUrl(item);
        }

        public static string ImageUrl(this Item item, ID imageFieldId, MediaUrlOptions options = null)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var imageField = (ImageField)item.Fields[imageFieldId];
            return imageField?.MediaItem == null ? string.Empty : imageField.ImageUrl(options);
        }

        public static string ImageUrl(this MediaItem mediaItem, int width, int height)
        {
            if (mediaItem == null)
            {
                throw new ArgumentNullException(nameof(mediaItem));
            }

            var options = new MediaUrlOptions { Height = height, Width = width };
            var url = MediaManager.GetMediaUrl(mediaItem, options);
            var cleanUrl = StringUtil.EnsurePrefix('/', url);
            var hashedUrl = HashingUtils.ProtectAssetUrl(cleanUrl);

            return hashedUrl;
        }


        public static Item TargetItem(this Item item, ID linkFieldId)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            if (item.Fields[linkFieldId] == null || !item.Fields[linkFieldId].HasValue)
            {
                return null;
            }
            return ((LinkField)item.Fields[linkFieldId]).TargetItem ?? ((ReferenceField)item.Fields[linkFieldId]).TargetItem;
        }

        public static string MediaUrl(this Item item, ID mediaFieldId, MediaUrlOptions options = null)
        {
            var targetItem = item.TargetItem(mediaFieldId);
            return targetItem == null ? string.Empty : (MediaManager.GetMediaUrl(targetItem) ?? string.Empty);
        }


        public static bool IsImage(this Item item)
        {
            return new MediaItem(item).MimeType.StartsWith("image/", StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsVideo(this Item item)
        {
            return new MediaItem(item).MimeType.StartsWith("video/", StringComparison.InvariantCultureIgnoreCase);
        }

        public static Item GetAncestorOrSelfOfTemplate(this Item item, ID templateID)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return item.IsDerived(templateID) ? item : item.Axes.GetAncestors().LastOrDefault(i => i.IsDerived(templateID));
        }

        public static IList<Item> GetAncestorsAndSelfOfTemplate(this Item item, ID templateID)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var returnValue = new List<Item>();
            if (item.IsDerived(templateID))
            {
                returnValue.Add(item);
            }

            returnValue.AddRange(item.Axes.GetAncestors().Reverse().Where(i => i.IsDerived(templateID)));
            return returnValue;
        }

        public static List<Item> GetDescendantsAndSelf(this Item item, string templateId = "")
        {
            var items = new List<Item>();
            if (item == null || item.Database == null) return items;

            items.Add(item);
            var path = StringUtil.EnsurePostfix('/', item.Paths.FullPath);
            path = StringUtil.EnsurePrefix('/', path);
            path = path.FastQueryEscape();
            string query = string.Format("fast:{0}/*", path);
            if (!string.IsNullOrEmpty(templateId))
            {
                query += string.Format("[@@templateid='{0}']", templateId);
            }

            items.AddRange(item.Database.SelectItems(query));
            return items;
        }

        public static Item LinkFieldTargetItem(this Item item, ID fieldId)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            if (ID.IsNullOrEmpty(fieldId))
            {
                throw new ArgumentNullException(nameof(fieldId));
            }
            var field = item.Fields[fieldId];
            if (field == null || !(FieldTypeManager.GetField(field) is LinkField))
            {
                return null;
            }
            LinkField linkField = field;
            return linkField?.TargetItem;
        }

        public static string LinkFieldUrl(this Item item, ID fieldId)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            if (ID.IsNullOrEmpty(fieldId))
            {
                throw new ArgumentNullException(nameof(fieldId));
            }
            var field = item.Fields[fieldId];
            if (field == null || !(FieldTypeManager.GetField(field) is LinkField))
            {
                return string.Empty;
            }
            LinkField linkField = field;
            switch (linkField.LinkType.ToLower())
            {
                case "internal":
                    // Use LinkMananger for internal links, if link is not empty
                    //return linkField.TargetItem != null ? LinkManager.GetItemUrl(linkField.TargetItem) : string.Empty;
                    return linkField.TargetItem != null ? linkField.GetFriendlyUrl() : string.Empty;
                case "media":
                    // Use MediaManager for media links, if link is not empty
                    return linkField.TargetItem != null ? MediaManager.GetMediaUrl(linkField.TargetItem) : string.Empty;
                case "external":
                    // Just return external links
                    return linkField.Url;
                case "anchor":
                    // Prefix anchor link with # if link if not empty
                    return !string.IsNullOrEmpty(linkField.Anchor) ? "#" + linkField.Anchor : string.Empty;
                case "mailto":
                    // Just return mailto link
                    return linkField.Url;
                case "javascript":
                    // Just return javascript
                    return linkField.Url;
                default:
                    // Just please the compiler, this
                    // condition will never be met
                    return linkField.Url;
            }
        }

        public static string LinkFieldClass(this Item item, ID fieldID)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            if (ID.IsNullOrEmpty(fieldID))
            {
                throw new ArgumentNullException(nameof(fieldID));
            }
            var field = item.Fields[fieldID];
            if (field == null || !(FieldTypeManager.GetField(field) is LinkField))
            {
                return string.Empty;
            }
            LinkField linkField = field;
            return linkField != null ? linkField.Class : string.Empty;
        }

        public static string LinkFieldTarget(this Item item, ID fieldID)
        {
            return item.LinkFieldOptions(fieldID, LinkFieldOption.Target);
        }

        public static string LinkFieldAnchor(this Item item, ID fieldID)
        {
            return item.LinkFieldOptions(fieldID, LinkFieldOption.Anchor);
        }

        public static string LinkFieldText(this Item item, ID fieldID)
        {
            return item.LinkFieldOptions(fieldID, LinkFieldOption.Text);
        }

        public static string LinkFieldOptions(this Item item, ID fieldID, LinkFieldOption option)
        {
            XmlField field = item.Fields[fieldID];
            switch (option)
            {
                case LinkFieldOption.Text:
                    return field?.GetAttribute("text");
                case LinkFieldOption.LinkType:
                    return field?.GetAttribute("linktype");
                case LinkFieldOption.Class:
                    return field?.GetAttribute("class");
                case LinkFieldOption.Alt:
                    return field?.GetAttribute("title");
                case LinkFieldOption.Target:
                    return field?.GetAttribute("target");
                case LinkFieldOption.QueryString:
                    return field?.GetAttribute("querystring");
                case LinkFieldOption.Anchor:
                    return field?.GetAttribute("anchor");
                default:
                    throw new ArgumentOutOfRangeException(nameof(option), option, null);
            }
        }

        public static bool HasLayout(this Item item)
        {
            return item?.Visualization?.Layout != null;
        }


        public static bool IsDerived(this Item item, ID templateId)
        {
            if (item == null)
            {
                return false;
            }

            return !templateId.IsNull && item.IsDerived(item.Database.Templates[templateId]);
        }

        private static bool IsDerived(this Item item, Item templateItem)
        {
            if (item == null)
            {
                return false;
            }

            if (templateItem == null)
            {
                return false;
            }

            var itemTemplate = TemplateManager.GetTemplate(item) ?? TemplateManager.GetTemplate(item.TemplateID, item.Database);
            return itemTemplate != null && (itemTemplate.ID == templateItem.ID || itemTemplate.DescendsFrom(templateItem.ID));
        }

        public static bool FieldHasValue(this Item item, ID fieldID)
        {
            return item.Fields[fieldID] != null && !string.IsNullOrWhiteSpace(item.Fields[fieldID].Value);
        }

        public static int? GetInteger(this Item item, ID fieldId)
        {
            int result;
            return !int.TryParse(item.Fields[fieldId].Value, out result) ? new int?() : result;
        }

        public static double? GetDouble(this Item item, ID fieldId)
        {
            var value = item?.Fields[fieldId]?.Value;
            if (value == null)
            {
                return null;
            }

            double num;
            if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out num) || double.TryParse(value, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out num) || double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out num))
            {
                return num;
            }
            return null;
        }

        public static IEnumerable<Item> GetMultiListValueItems(this Item item, ID fieldId)
        {
            return new MultilistField(item.Fields[fieldId]).GetItems();
        }

        public static bool HasContextLanguage(this Item item)
        {
            // get the latest version and count, count should be 0 if it doesn't exist in the current language
            Item latestVersion = item.Versions.GetLatestVersion();
            var hasVersionInCurrentLanguage = (latestVersion != null) && (latestVersion.Versions.Count > 0);

            // if fallback config is not enabled, return true
            // if it has a version in the current language, then return true
            // if it DOESNT have a version in the current language and a version MUST exist (in the least, a blank version in the language) 
            // because this item's template (or base templates) is in the list of supported templates, return false
            // if it DOESNT have a version in the current language and fallback is NOT enabled, return false
            if (LocalizationExtensions.DisableFieldFallbackCheck)
                return true;
            if (hasVersionInCurrentLanguage)
                return true;
            else if (!hasVersionInCurrentLanguage && LocalizationExtensions.EnforceVersionPresence && LocalizationExtensions.IsOfSupportedTemplate(item))
                return false;
            else if (!LocalizationExtensions.EnableFallback)
                return false;

            // Note, the following only applies if enforceVersionPresence is false, 
            // because if it is true, then straight up if the language version doesn't exist, it should be treated like it doesn't exist

            // But even if we aren't enforcing version presence, we don't want it to return a blank item, 
            // and by default sitecore would do so if the language version doesn't exist AND the language doesn't fall back.

            // so if we have gotten this far, we know it doesn't have a version in the current language
            // we have already checked if EnableFallback isn't turned on, and we are returning false in that case, 
            // because if it can't fallback, then without a language version it will definitely return blank.

            // but there is the case that if this item is falling back, then eventually it might get to a language version that does exist
            // so we run the following code, which only is necessary if fallback is enabled AND if the current language has been configured to fallback
            // we can then check that fallback language version, and so on, recursively

            // assumes at this point that version presence is NOT enforced AND it doesn't have a version in the current language
            // it should be the case that fallback is enabled, in order to get here, but let's check just in case
            if (LocalizationExtensions.EnableFallback)
            {
                try
                {
                    // check if the current language falls back and get the version of the item it falls back to
                    bool currentLanguageFallsBack = Sitecore.Context.Language.HasFallbackAssigned(Sitecore.Context.Database);
                    var fallbackItem = LocalizationExtensions.GetFallbackItem(item);

                    // if the current language doesn't fall back at all and we already know if we have gotten this far, 
                    // there isn't a version in the current language, so return false
                    if (!currentLanguageFallsBack)
                        return false;
                    else
                    {
                        // if there is no latest version for this item in the current language (which we know is the case otherwise we wouldn't have gotten this far), '
                        // check to see if it falls back to another language version and if so then return the latest version check on that
                        // don't want to say there is no version for this context if the language it falls back to does have a version, because it will load that one.
                        // recursively will do this in case one language falls back to another which falls back to another

                        if (fallbackItem != null)
                            return HasContextLanguage(fallbackItem);
                    }

                }
                catch
                {

                }
            }

            return true;
        }

        public static HtmlString Field(this Item item, ID fieldId)
        {
            Assert.IsNotNull(item, "Item cannot be null");
            Assert.IsNotNull(fieldId, "FieldId cannot be null");
            return new HtmlString(FieldRendererService.RenderField(item, fieldId));
        }

        public static SiteInfo GetSite(this Item item)
        {
            var siteInfoList = Sitecore.Configuration.Factory.GetSiteInfoList();

            SiteInfo currentSiteinfo = null;
            var matchLength = 0;
            foreach (var siteInfo in siteInfoList)
            {
                if (item.Paths.FullPath.StartsWith(siteInfo.RootPath, StringComparison.OrdinalIgnoreCase) && siteInfo.RootPath.Length > matchLength)
                {
                    matchLength = siteInfo.RootPath.Length;
                    currentSiteinfo = siteInfo;
                }
            }

            return currentSiteinfo;
        }
    }

    public enum LinkFieldOption
    {
        Text,
        LinkType,
        Class,
        Alt,
        Target,
        QueryString,
        Anchor
    }
}