using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Maintenance;
using Sitecore.ContentSearch.SolrProvider;

namespace Wageworks.Foundation.Solr.SpatialSearch.Provider
{
    public class SolrSearchIndexWithSpatial : SolrSearchIndex
    {
        public SolrSearchIndexWithSpatial(string name, string core, IIndexPropertyStore propertyStore, string @group) : base(name, core, propertyStore, @group)
        {
        }

        public SolrSearchIndexWithSpatial(string name, string core, IIndexPropertyStore propertyStore) : base(name, core, propertyStore)
        {
        }

        public override IProviderSearchContext CreateSearchContext(Sitecore.ContentSearch.Security.SearchSecurityOptions options = Sitecore.ContentSearch.Security.SearchSecurityOptions.EnableSecurityCheck)
        {
            return new SolrSearchWithSpatialContext(this,options);
        }
    }
}
