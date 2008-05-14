using System;
using System.Collections.Generic;
using System.Text;

namespace Topology.Graph.Algorithms
{
    public delegate void AlgorithmEventHandler<TGraph>(
        IAlgorithm<TGraph> sender,
        EventArgs e);
}
