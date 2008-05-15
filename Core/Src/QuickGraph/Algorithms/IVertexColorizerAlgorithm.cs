using System;
using System.Collections.Generic;

namespace Topology.Graph.Algorithms
{
    public interface IVertexColorizerAlgorithm<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
    {
        IDictionary<TVertex, GraphColor> VertexColors { get;}
    }
}
