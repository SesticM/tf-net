using System;
using System.Collections.Generic;
using System.Text;

namespace Topology.Graph
{
    public interface IView<TVertex,TEdge> 
        where TEdge : IEdge<TVertex>
    {
        IHierarchy<TVertex, TEdge> Hierarchy { get;}
        void Expand(TVertex vertex);
        void Collapse(TVertex vertex);
    }
}
