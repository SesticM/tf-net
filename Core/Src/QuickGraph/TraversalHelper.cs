using System;
using System.Collections.Generic;
using System.Text;

namespace Topology.Graph
{
    public static class TraversalHelper
    {
        public static TVertex GetFirstVertex<TVertex,TEdge>(IVertexListGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            foreach (TVertex v in g.Vertices)
                return v;
            return default(TVertex);
        }

        public static TVertex GetFirstVertex<TVertex, TEdge>(IUndirectedGraph<TVertex, TEdge> g)
            where TEdge : IEdge<TVertex>
        {
            foreach (TVertex v in g.Vertices)
                return v;
            return default(TVertex);
        }
    }
}
