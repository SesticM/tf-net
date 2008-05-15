using System;
using System.Collections.Generic;

namespace Topology.Graph.Algorithms
{
    public interface IEdgeColorizerAlgorithm<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
    {
        IDictionary<TEdge, GraphColor> EdgeColors { get;}
    }
}
