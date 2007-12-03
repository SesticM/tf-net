using System;
using System.Collections.Generic;

namespace Topology.Graph
{
    public interface IVertexListGraph<TVertex, TEdge> : 
        IIncidenceGraph<TVertex, TEdge>,
        IVertexSet<TVertex>
        where TEdge : IEdge<TVertex>
    {
    }
}
