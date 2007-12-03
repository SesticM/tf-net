using System;

namespace Topology.Graph
{
    public interface IGraph<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
    {
        bool IsDirected { get;}
        bool AllowParallelEdges { get;}
    }
}
