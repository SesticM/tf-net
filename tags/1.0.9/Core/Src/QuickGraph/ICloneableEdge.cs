using System;

namespace Topology.Graph
{
    public interface ICloneableEdge<TVertex> : IEdge<TVertex>
    {
        ICloneableEdge<TVertex> Clone(TVertex source, TVertex target);
    }
}
