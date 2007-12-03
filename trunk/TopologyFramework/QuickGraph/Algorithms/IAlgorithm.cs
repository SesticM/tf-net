using System;

namespace Topology.Graph.Algorithms
{
    public interface IAlgorithm<TGraph>
    {
        TGraph VisitedGraph { get;}

        object SyncRoot { get;}
        ComputationState State { get;}

        void Compute();
        void Abort();

        event EventHandler StateChanged;
        event EventHandler Started;
        event EventHandler Finished;
        event EventHandler Aborted;
    }
}
