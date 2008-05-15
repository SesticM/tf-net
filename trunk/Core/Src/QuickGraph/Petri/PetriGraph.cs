using System;

namespace Topology.Graph.Petri
{
    [Serializable]
    internal sealed class PetriGraph<Token> :
        BidirectionalGraph<IPetriVertex, IArc<Token>>,
        IPetriGraph<Token>
    {
        public PetriGraph()
            :base(true)
        { }
    }
}
