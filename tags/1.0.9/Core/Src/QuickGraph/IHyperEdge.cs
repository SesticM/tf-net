using System;
using System.Collections.Generic;

namespace Topology.Graph
{
    public interface IHyperEdge<TVertex>
    {
        int EndPointCount { get;}
        IEnumerable<TVertex> EndPoints { get;}
    }
}
