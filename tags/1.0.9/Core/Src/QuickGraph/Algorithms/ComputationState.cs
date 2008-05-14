using System;

namespace Topology.Graph.Algorithms
{
    [Serializable]
    public enum ComputationState
    {
        NotRunning,
        Running,
        PendingAbortion,
        Finished,
        Aborted
    }
}
