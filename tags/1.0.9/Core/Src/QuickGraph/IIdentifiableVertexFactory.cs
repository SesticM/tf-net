using System;
using System.Collections.Generic;
using System.Text;

namespace Topology.Graph
{
    public interface IIdentifiableVertexFactory<TVertex>
        where TVertex : IIdentifiable
    {
        TVertex CreateVertex(string id);
    }
}
