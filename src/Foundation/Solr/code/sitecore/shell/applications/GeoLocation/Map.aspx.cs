using System;

namespace Wageworks.Foundation.Solr.SpatialSearch.sitecore.shell.applications.GeoLocation
{
    public partial class Map : System.Web.UI.Page
    {
        public string ApiKey;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ApiKey = Sitecore.Configuration.Settings.GetSetting("Locations.MapApiKey");
        }
    }
}