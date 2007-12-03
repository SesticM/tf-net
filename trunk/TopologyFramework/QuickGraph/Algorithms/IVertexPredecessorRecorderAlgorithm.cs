using System;

namespace Topology.Graph.Algorithms
{
    public interface IVertexPredecessorRecorderAlgorithm<TVertex,TEdge> :
        ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        event VertexEventHandler<TVertex> StartVertex;
        event VertexEventHandler<TVertex> FinishVertex;
    }
}
