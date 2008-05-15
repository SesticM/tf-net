using System;
using System.Collections.Generic;

namespace Topology.Graph
{
    public interface IImplicitGraph<TVertex,TEdge> : IGraph<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
    {
        bool IsOutEdgesEmpty(TVertex v);
        int OutDegree(TVertex v);
        IEnumerable<TEdge> OutEdges(TVertex v);
        TEdge OutEdge(TVertex v, int index);
    }
}
