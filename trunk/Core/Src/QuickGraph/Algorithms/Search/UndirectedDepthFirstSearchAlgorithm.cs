﻿using System;
using System.Collections.Generic;

namespace Topology.Graph.Algorithms.Search
{
    /// <summary>
    /// A depth first search algorithm for undirected graphs
    /// </summary>
    /// <reference-ref
    ///     idref="gross98graphtheory"
    ///     chapter="4.2"
    ///     />
    [Serializable]
    public sealed class UndirectedDepthFirstSearchAlgorithm<TVertex, TEdge> :
        RootedAlgorithmBase<TVertex, IUndirectedGraph<TVertex, TEdge>>,
        IDistanceRecorderAlgorithm<TVertex, TEdge>,
        IVertexColorizerAlgorithm<TVertex, TEdge>,
        IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>,
        IVertexTimeStamperAlgorithm<TVertex, TEdge>,
        ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private IDictionary<TVertex, GraphColor> colors;
        private int maxDepth = int.MaxValue;

        public UndirectedDepthFirstSearchAlgorithm(IUndirectedGraph<TVertex, TEdge> g)
            :this(g, new Dictionary<TVertex, GraphColor>())
        {
        }

        public UndirectedDepthFirstSearchAlgorithm(
            IUndirectedGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, GraphColor> colors
            )
            :base(visitedGraph)
        {
            if (colors == null)
                throw new ArgumentNullException("VertexColors");

            this.colors = colors;
        }

        public IDictionary<TVertex, GraphColor> VertexColors
        {
            get
            {
                return this.colors;
            }
        }

        public int MaxDepth
        {
            get
            {
                return this.maxDepth;
            }
            set
            {
                this.maxDepth = value;
            }
        }

        public event VertexEventHandler<TVertex> InitializeVertex;
        private void OnInitializeVertex(TVertex v)
        {
            if (InitializeVertex != null)
                InitializeVertex(this, new VertexEventArgs<TVertex>(v));
        }

        public event VertexEventHandler<TVertex> StartVertex;
        private void OnStartVertex(TVertex v)
        {
            if (StartVertex != null)
                StartVertex(this, new VertexEventArgs<TVertex>(v));
        }

        public event VertexEventHandler<TVertex> DiscoverVertex;
        private void OnDiscoverVertex(TVertex v)
        {
            if (DiscoverVertex != null)
                DiscoverVertex(this, new VertexEventArgs<TVertex>(v));
        }

        public event EdgeEventHandler<TVertex, TEdge> ExamineEdge;
        private void OnExamineEdge(TEdge e)
        {
            if (ExamineEdge != null)
                ExamineEdge(this, new EdgeEventArgs<TVertex, TEdge>(e));
        }

        public event EdgeEventHandler<TVertex, TEdge> TreeEdge;
        private void OnTreeEdge(TEdge e)
        {
            if (TreeEdge != null)
                TreeEdge(this, new EdgeEventArgs<TVertex, TEdge>(e));
        }

        public event EdgeEventHandler<TVertex, TEdge> BackEdge;
        private void OnBackEdge(TEdge e)
        {
            if (BackEdge != null)
                BackEdge(this, new EdgeEventArgs<TVertex, TEdge>(e));
        }

        public event EdgeEventHandler<TVertex, TEdge> ForwardOrCrossEdge;
        private void OnForwardOrCrossEdge(TEdge e)
        {
            if (ForwardOrCrossEdge != null)
                ForwardOrCrossEdge(this, new EdgeEventArgs<TVertex, TEdge>(e));
        }

        public event VertexEventHandler<TVertex> FinishVertex;
        private void OnFinishVertex(TVertex v)
        {
            if (FinishVertex != null)
                FinishVertex(this, new VertexEventArgs<TVertex>(v));
        }

        protected override void InternalCompute()
        {
            // put all vertex to white
            Initialize();

            // if there is a starting vertex, start whith him:
            if (this.RootVertex != null)
            {
                OnStartVertex(this.RootVertex);
                Visit(this.RootVertex, 0);
            }

            // process each vertex 
            foreach (TVertex u in VisitedGraph.Vertices)
            {
                if (this.IsAborting)
                    return;
                if (VertexColors[u] == GraphColor.White)
                {
                    OnStartVertex(u);
                    Visit(u, 0);
                }
            }
        }

        public void Initialize()
        {
            foreach (TVertex u in VisitedGraph.Vertices)
            {
                if (this.IsAborting)
                    return;
                VertexColors[u] = GraphColor.White;
                OnInitializeVertex(u);
            }
        }

        public void Visit(TVertex u, int depth)
        {
            if (depth > this.maxDepth)
                return;
            if (u == null)
                throw new ArgumentNullException("u");
            if (this.IsAborting)
                return;

            VertexColors[u] = GraphColor.Gray;
            OnDiscoverVertex(u);

            TVertex v = default(TVertex);
            foreach (TEdge e in VisitedGraph.AdjacentEdges(u))
            {
                if (this.IsAborting)
                    return;

                OnExamineEdge(e);
                if (u.Equals(e.Source))
                    v = e.Target;
                else
                    v = e.Source;

                GraphColor c = VertexColors[v];
                if (c == GraphColor.White)
                {
                    OnTreeEdge(e);
                    Visit(v, depth + 1);
                }
                else if (c == GraphColor.Gray)
                {
                    OnBackEdge(e);
                }
                else
                {
                    OnForwardOrCrossEdge(e);
                }
            }

            VertexColors[u] = GraphColor.Black;
            OnFinishVertex(u);
        }
    }
}
