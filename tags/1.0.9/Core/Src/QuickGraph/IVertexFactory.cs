using System;

namespace Topology.Graph
{
    public interface IVertexFactory<TVertex>
    {
        TVertex CreateVertex();
    }
}
