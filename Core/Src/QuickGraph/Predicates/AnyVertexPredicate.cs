using System;

namespace Topology.Graph.Predicates
{
    [Serializable]
    public sealed class AnyVertexPredicate<TVertex>
    {
        public bool Test(TVertex vertex)
        {
            return true;
        }
    }
}
