using System;
using System.Collections.Generic;
using System.Text;

namespace Topology.Graph
{
    public interface IVertexAndEdgeSet<TVertex,TEdge> :
        IVertexSet<TVertex>,
        IEdgeListGraph<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
    {
    }
}
