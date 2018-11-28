using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Maintenance;
using Sitecore.ContentSearch.SolrProvider;

namespace Wageworks.Foundation.Solr.SpatialSearch.Provider
{
    public class SwitchOnRebuildSolrSearchIndexWithSpatial : SwitchOnRebuildSolrSearchIndex
	{
        public SwitchOnRebuildSolrSearchIndexWithSpatial(string name, string core, string rebuildcore, IIndexPropertyStore propertyStore) : base(name, core, rebuildcore, propertyStore)
        {
        }

        public override IProviderSearchContext CreateSearchContext(Sitecore.ContentSearch.Security.SearchSecurityOptions options = Sitecore.ContentSearch.Security.SearchSecurityOptions.EnableSecurityCheck)
        {
            return new SolrSearchWithSpatialContext(this,options);
        }
    }
}
