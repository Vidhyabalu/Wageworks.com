using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.SecurityModel;
using Sitecore.StringExtensions;

namespace Wageworks.Foundation.SitecoreExtensions.Extensions
{
    public static class LocalizationExtensions
    {
        private const double MetersToKm = 1000;
        private const double MetersToMiles = 1609.344;

        public static double GetLocalizedDistanceFromMeters(this double distanceInMeters)
        {
            var culture = Context.Culture;
            var region = new RegionInfo(culture.LCID);

            if (region.IsMetric) return distanceInMeters / MetersToKm;

            return distanceInMeters / MetersToMiles;
        }

        public static double GetDistanceInKilometersFromLocalizedDistance(this double distance)
        {
            var culture = Context.Culture;
            var region = new RegionInfo(culture.LCID);

            if (region.IsMetric) return distance;

            return distance * MetersToMiles / MetersToKm;
        }

        public static bool IsOfSupportedTemplate(Item item)
        {
            var template = TemplateManager.GetTemplate(item.TemplateID, item.Database);
            return SupportedTemplateIDs.Any(template.DescendsFromOrEquals);
        }

        public static IEnumerable<ID> SupportedTemplateIDs
        {
            get
            {
                var templateIds = MainUtil.RemoveEmptyStrings(EnforceVersionPresenceTemplates.ToLower().Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries));
                return from templateId in templateIds where ID.IsID(templateId) select ID.Parse(templateId);
            }
        }

        public static string EnforceVersionPresenceTemplates => Settings.GetSetting("Fallback.EnforceVersionPresenceTemplates");

        public static bool DisableFieldFallbackCheck => Sitecore.Context.Site == null || Sitecore.Context.Site.SiteInfo.Properties["enableFieldLanguageFallback"] == null;

        public static bool EnforceVersionPresence => Sitecore.Context.Site != null &&
                                                     Sitecore.Context.Site.SiteInfo.Properties["enforceVersionPresence"] != null &&
                                                     Sitecore.Context.Site.SiteInfo.Properties["enforceVersionPresence"].Equals("true", StringComparison.InvariantCultureIgnoreCase);

        public static bool EnableFallback => Sitecore.Context.Site != null &&
                                             Sitecore.Context.Site.SiteInfo.Properties["enableFieldLanguageFallback"] != null &&
                                             Sitecore.Context.Site.SiteInfo.Properties["enableFieldLanguageFallback"].Equals("true", StringComparison.InvariantCultureIgnoreCase);

        public static bool HasFallbackAssigned(this Language language, Database database)
        {
            var fallbackLanguage = GetFallbackLanguage(language, database);
            return (fallbackLanguage != null) && !String.IsNullOrEmpty(fallbackLanguage.Name);
        }

        public static Language GetFallbackLanguage(this Language language, Database database)
        {
            var sourceLangItem = GetLanguageDefinitionItem(language, database);
            return sourceLangItem != null ? sourceLangItem.GetFallbackLanguage() : null;
        }

        public static Item GetLanguageDefinitionItem(this Language language, Database database)
        {
            var sourceLanguageItemId = LanguageManager.GetLanguageItemId(language, database);
            return ID.IsNullOrEmpty(sourceLanguageItemId) ? null : ItemManager.GetItem(sourceLanguageItemId, Language.Parse("en"), Sitecore.Data.Version.Latest, database, SecurityCheck.Disable);
        }


        public static Language GetFallbackLanguage(this Item langItem)
        {
            Assert.IsNotNull(langItem, "langItem cannot be null");

            var fallbackLangName = langItem[FallbackLanguage];
            Language fallbackLang;
            return Language.TryParse(fallbackLangName, out fallbackLang) ? fallbackLang : null;
        }

        public static Item GetFallbackItem(this Item item)
        {
            Assert.IsNotNull(item, "item cannot be null");

            var fallbackLang = item.Language.GetFallbackLanguage(item.Database);

            if (fallbackLang != null &&
                !fallbackLang.Name.IsNullOrEmpty() &&
                !fallbackLang.Equals(item.Language))
            {
                return item.Database.GetItem(item.ID, fallbackLang, Sitecore.Data.Version.Latest);
            }

            return null;
        }

        public static readonly string FallbackLanguage = "{892975AC-496F-4AC9-8826-087095C68E1D}";
    }
}