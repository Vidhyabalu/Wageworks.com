using Sitecore.ContentSearch;
using Sitecore.ContentSearch.FieldReaders;
using Sitecore.Data.Fields;

namespace Wageworks.Foundation.Solr.SpatialSearch.FieldReaders
{
    public class SpatialPointFieldReader : FieldReader
    {
        public override object GetFieldValue(IIndexableDataField indexableField)
        {
            if (!(indexableField is SitecoreItemDataField))
                return indexableField.Value;
            var field = (Field)(indexableField as SitecoreItemDataField);
            if (!string.IsNullOrEmpty(field.Value))
                return new SpatialPoint(field.Value);

            return null;
        }
    }
}