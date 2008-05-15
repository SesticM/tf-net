using System;

namespace Topology.Graph.Algorithms
{
    public interface ITreeBuilderAlgorithm<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
    {
        event EdgeEventHandler<TVertex, TEdge> TreeEdge;
    }
}
