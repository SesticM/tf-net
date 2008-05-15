using System;
using System.Collections.Generic;

namespace Topology.Graph
{
    public interface IIndexedVertexListGraph<TVertex, TEdge> :
        IVertexListGraph<TVertex, TEdge>,
        IIndexedImplicitGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
    }
}
