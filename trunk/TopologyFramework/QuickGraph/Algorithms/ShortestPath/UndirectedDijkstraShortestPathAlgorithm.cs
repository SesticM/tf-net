using System;
using System.Collections.Generic;
using Topology.Graph.Algorithms.Search;
using Topology.Graph.Algorithms.Observers;
using Topology.Graph.Collections;

namespace Topology.Graph.Algorithms.ShortestPath
{
    /// <summary>
    /// A single-source shortest path algorithm for undirected graph
    /// with positive distance.
    /// </summary>
    /// <reference-ref
    ///     idref="lawler01combinatorial"
    ///     />
    [Serializable]
    public sealed class UndirectedDijkstraShortestPathAlgorithm<TVertex, TEdge> :
        ShortestPathAlgorithmBase<TVertex, TEdge, IUndirectedGraph<TVertex, TEdge>>,
        IVertexColorizerAlgorithm<TVertex, TEdge>,
        IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>,
        IDistanceRecorderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private PriorithizedVertexBuffer<TVertex, double> vertexQueue;

        public UndirectedDijkstraShortestPathAlgorithm(
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TEdge, double> weights,
            IDistanceRelaxer distanceRelaxer
            )
            : base(visitedGraph, weights, distanceRelaxer)
        { }

        public event VertexEventHandler<TVertex> InitializeVertex;
        public event VertexEventHandler<TVertex> StartVertex;
        public event VertexEventHandler<TVertex> DiscoverVertex;
        public event VertexEventHandler<TVertex> ExamineVertex;
        public event EdgeEventHandler<TVertex, TEdge> ExamineEdge;
        public event VertexEventHandler<TVertex> FinishVertex;

        public event EdgeEventHandler<TVertex, TEdge> TreeEdge;
        private void OnTreeEdge(TEdge e)
        {
            if (TreeEdge != null)
                TreeEdge(this, new EdgeEventArgs<TVertex, TEdge>(e));
        }
        public event EdgeEventHandler<TVertex, TEdge> EdgeNotRelaxed;
        private void OnEdgeNotRelaxed(TEdge e)
        {
            if (EdgeNotRelaxed != null)
                EdgeNotRelaxed(this, new EdgeEventArgs<TVertex, TEdge>(e));
        }

        private void InternalTreeEdge(Object sender, EdgeEventArgs<TVertex, TEdge> args)
        {
            bool decreased = Relax(args.Edge);
            if (decreased)
                OnTreeEdge(args.Edge);
            else
                OnEdgeNotRelaxed(args.Edge);
        }

        private void InternalGrayTarget(Object sender, EdgeEventArgs<TVertex, TEdge> args)
        {
            bool decreased = Relax(args.Edge);
            if (decreased)
            {
                this.vertexQueue.Update(args.Edge.Target);
                OnTreeEdge(args.Edge);
            }
            else
            {
                OnEdgeNotRelaxed(args.Edge);
            }
        }

        public void Initialize()
        {
            this.VertexColors.Clear();
            this.Distances.Clear();
            // init color, distance
            foreach (TVertex u in VisitedGraph.Vertices)
            {
                this.VertexColors.Add(u, GraphColor.White);
                this.Distances.Add(u, double.MaxValue);
            }
        }

        protected override void InternalCompute()
        {
            if (this.RootVertex == null)
                throw new InvalidOperationException("RootVertex not initialized");

            this.Initialize();
            this.VertexColors[this.RootVertex] = GraphColor.Gray;
            this.Distances[this.RootVertex] = 0;
            ComputeNoInit(this.RootVertex);
        }

        public void ComputeNoInit(TVertex s)
        {
            this.vertexQueue = new PriorithizedVertexBuffer<TVertex, double>(this.Distances);
            UndirectedBreadthFirstSearchAlgorithm<TVertex, TEdge> bfs = new UndirectedBreadthFirstSearchAlgorithm<TVertex, TEdge>(
                this.VisitedGraph,
                this.vertexQueue,
                VertexColors
                );

            try
            {
                bfs.InitializeVertex += this.InitializeVertex;
                bfs.DiscoverVertex += this.DiscoverVertex;
                bfs.StartVertex += this.StartVertex;
                bfs.ExamineEdge += this.ExamineEdge;
                bfs.ExamineVertex += this.ExamineVertex;
                bfs.FinishVertex += this.FinishVertex;

                bfs.TreeEdge += new EdgeEventHandler<TVertex, TEdge>(this.InternalTreeEdge);
                bfs.GrayTarget += new EdgeEventHandler<TVertex, TEdge>(this.InternalGrayTarget);

                bfs.Visit(s);
            }
            finally
            {
                if (bfs != null)
                {
                    bfs.InitializeVertex -= this.InitializeVertex;
                    bfs.DiscoverVertex -= this.DiscoverVertex;
                    bfs.StartVertex -= this.StartVertex;
                    bfs.ExamineEdge -= this.ExamineEdge;
                    bfs.ExamineVertex -= this.ExamineVertex;
                    bfs.FinishVertex -= this.FinishVertex;

                    bfs.TreeEdge -= new EdgeEventHandler<TVertex, TEdge>(this.InternalTreeEdge);
                    bfs.GrayTarget -= new EdgeEventHandler<TVertex, TEdge>(this.InternalGrayTarget);
                }
            }
        }

        private bool Relax(TEdge e)
        {
            double du = this.Distances[e.Source];
            double dv = this.Distances[e.Target];
            double we = this.Weights[e];

            if (Compare(Combine(du, we), dv))
            {
                this.Distances[e.Target] = Combine(du, we);
                return true;
            }
            else
                return false;
        }
    }
}
