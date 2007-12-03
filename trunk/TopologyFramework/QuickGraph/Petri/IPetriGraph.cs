using System;

namespace Topology.Graph.Petri
{
    public interface IPetriGraph<Token> : IMutableBidirectionalGraph<IPetriVertex, IArc<Token>>
    {}
}
