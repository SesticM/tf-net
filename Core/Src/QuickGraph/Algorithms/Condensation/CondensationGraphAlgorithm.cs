﻿using System;
using System.Collections.Generic;
using Topology.Graph.Collections;

namespace Topology.Graph.Algorithms.Condensation
{
    public sealed class CondensationGraphAlgorithm<TVertex,TEdge,TGraph> :
        AlgorithmBase<IVertexAndEdgeListGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
        where TGraph : IMutableVertexAndEdgeListGraph<TVertex, TEdge>, new()
    {
        private bool stronglyConnected = true;
        private IConnectedComponentAlgorithm<TVertex,TEdge,IVertexListGraph<TVertex,TEdge>> componentAlgorithm = null;

        private IMutableBidirectionalGraph<
            TGraph,
            CondensatedEdge<TVertex, TEdge, TGraph>
            > condensatedGraph;

        public CondensationGraphAlgorithm(IVertexAndEdgeListGraph<TVertex, TEdge> visitedGraph)
            :base(visitedGraph)
        {}

        public IMutableBidirectionalGraph<
            TGraph,
            CondensatedEdge<TVertex, TEdge,TGraph>
            > CondensatedGraph
        {
            get { return this.condensatedGraph; }
        }

        public bool StronglyConnected
        {
            get { return this.stronglyConnected; }
            set { this.stronglyConnected = value; }
        }

        protected override void InternalCompute()
        {
            // create condensated graph
            this.condensatedGraph = new BidirectionalGraph<
                TGraph,
                CondensatedEdge<TVertex, TEdge,TGraph>
                >(false);
            if (this.VisitedGraph.VertexCount == 0)
                return;

            // compute strongly connected components
            Dictionary<TVertex,int> components = new Dictionary<TVertex,int>(this.VisitedGraph.VertexCount);
            int componentCount;
            lock (this.SyncRoot)
            {
                if (this.StronglyConnected)
                    this.componentAlgorithm = new StronglyConnectedComponentsAlgorithm<TVertex, TEdge>(this.VisitedGraph, components);
                else
                    this.componentAlgorithm = new WeaklyConnectedComponentsAlgorithm<TVertex, TEdge>(this.VisitedGraph, components);
            }
            this.componentAlgorithm.Compute();
            componentCount = this.componentAlgorithm.ComponentCount;
            lock (SyncRoot)
            {
                this.componentAlgorithm = null;
            }
            if (this.IsAborting)
                return;

            // create list vertices
            Dictionary<int, TGraph> condensatedVertices = new Dictionary<int, TGraph>(componentCount);
            for (int i = 0; i < componentCount; ++i)
            {
                if (this.IsAborting)
                    return;

                TGraph v = new TGraph();
                condensatedVertices.Add(i, v);
                this.condensatedGraph.AddVertex(v);
            }

            // addingvertices
            foreach (TVertex v in this.VisitedGraph.Vertices)
            {
                condensatedVertices[components[v]].AddVertex(v);
            }
            if (this.IsAborting)
                return;

            // condensated edges
            Dictionary<EdgeKey, CondensatedEdge<TVertex,TEdge,TGraph>> condensatedEdges = new Dictionary<EdgeKey,CondensatedEdge<TVertex,TEdge,TGraph>>(componentCount);

            // iterate over edges and condensate graph
            foreach (TEdge edge in this.VisitedGraph.Edges)
            {
                if (this.IsAborting)
                    return;

                // get component ids
                int sourceID = components[edge.Source];
                int targetID = components[edge.Target];

                // get vertices
                TGraph sources = condensatedVertices[sourceID];
                if (sourceID == targetID)
                {
                    sources.AddEdge(edge);
                    continue;
                }

                //
                TGraph targets = condensatedVertices[targetID];

                // at last add edge
                EdgeKey edgeKey = new EdgeKey(sourceID, targetID);
                CondensatedEdge<TVertex,TEdge,TGraph> condensatedEdge;
                if (!condensatedEdges.TryGetValue(edgeKey, out condensatedEdge))
                {
                    condensatedEdge = new CondensatedEdge<TVertex, TEdge,TGraph>(sources, targets);
                    condensatedEdges.Add(edgeKey, condensatedEdge);
                    this.condensatedGraph.AddEdge(condensatedEdge);
                }
                condensatedEdge.Edges.Add(edge);
            }
        }

        public override void Abort()
        {
            if (this.componentAlgorithm != null)
            {
                this.componentAlgorithm.Abort();
                this.componentAlgorithm = null;
            }
            base.Abort();
        }

        private struct EdgeKey : IComparable<EdgeKey>
        {
            int SourceID;
            int TargetID;

            public EdgeKey(int sourceID, int targetID)
            {
                SourceID = sourceID;
                TargetID = targetID;
            }

            public int CompareTo(EdgeKey other)
            {
                int compare = SourceID.CompareTo(other.SourceID);
                if (compare != 0)
                    return compare;
                return TargetID.CompareTo(other.TargetID);                
            }

            public bool Equals(EdgeKey other)
            {
                return SourceID == other.SourceID && TargetID == other.TargetID;
            }
        }
    }
}
