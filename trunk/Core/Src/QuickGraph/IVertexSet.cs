using System;
using System.Collections.Generic;
using System.Text;

namespace Topology.Graph
{
    public interface IVertexSet<TVertex>
    {
        bool IsVerticesEmpty { get;}
        int VertexCount { get;}
        IEnumerable<TVertex> Vertices { get;}
        bool ContainsVertex(TVertex vertex);
    }
}
