using System;
namespace Topology.Graph
{
    [Serializable]
    public delegate TVertex CreateVertexDelegate<TVertex, TEdge>(
        IVertexListGraph<TVertex,TEdge> g) 
    where TEdge : IEdge<TVertex>;
}
