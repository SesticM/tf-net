using System;

namespace Topology.Graph
{
    [Serializable]
    public sealed class EdgeFactory<TVertex> : IEdgeFactory<TVertex, Edge<TVertex>>
    {
        public Edge<TVertex> CreateEdge(TVertex source, TVertex target)
        {
            return new Edge<TVertex>(source, target);
        }
    }
}
