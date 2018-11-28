using Sitecore.ContentSearch.Linq.Nodes;
using Sitecore.ContentSearch.Linq.Solr;
using Wageworks.Foundation.Solr.SpatialSearch.Nodes;
using WithinRadiusNode = Wageworks.Foundation.Solr.SpatialSearch.Nodes.WithinRadiusNode;

namespace Wageworks.Foundation.Solr.SpatialSearch.Indexing
{
    public class SpatialSolrQueryOptimizer : SolrQueryOptimizer
    {
        protected override QueryNode Visit(QueryNode node, SolrQueryOptimizerState state)
        {
            if (node.NodeType == QueryNodeType.Custom)
            {
                if (node is WithinRadiusNode)
                {
                    return VisitWithinRadius((WithinRadiusNode)node, state);
                }
            }
           
            return base.Visit(node, state);
        }

        private QueryNode VisitWithinRadius(WithinRadiusNode radiusNode, SolrQueryOptimizerState state)
        {
            return new WithinRadiusNode(this.Visit(radiusNode.SourceNode, state), radiusNode.Field, radiusNode.Lat, radiusNode.Lon, radiusNode.Radius);
        }
    }
}