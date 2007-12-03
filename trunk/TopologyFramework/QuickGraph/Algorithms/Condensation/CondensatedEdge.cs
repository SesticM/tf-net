using System;
using System.Collections.Generic;
using Topology.Graph.Collections;

namespace Topology.Graph.Algorithms.Condensation
{
    [Serializable]
    public sealed class CondensatedEdge<TVertex, TEdge, TGraph> : Edge<TGraph>
        where TEdge : IEdge<TVertex>
        where TGraph : IMutableVertexAndEdgeListGraph<TVertex, TEdge>, new()
    {
        private List<TEdge> edges = new List<TEdge>();
        public CondensatedEdge(TGraph source, TGraph target)
            :base(source,target)
        { }

        public IList<TEdge> Edges
        {
            get { return this.edges; }
        }
    }
}
