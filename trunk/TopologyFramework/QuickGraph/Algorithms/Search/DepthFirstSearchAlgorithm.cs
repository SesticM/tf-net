﻿using System;
using System.Collections.Generic;

namespace Topology.Graph.Algorithms.Search
{
    /// <summary>
    /// A depth first search algorithm for directed graph
    /// </summary>
    /// <typeparam name="Vertex"></typeparam>
    /// <typeparam name="Edge"></typeparam>
    /// <reference-ref
    ///     idref="gross98graphtheory"
    ///     chapter="4.2"
    ///     />
    [Serializable]
    public sealed class DepthFirstSearchAlgorithm<TVertex, TEdge> :
        RootedAlgorithmBase<TVertex,IVertexListGraph<TVertex, TEdge>>,
        IDistanceRecorderAlgorithm<TVertex,TEdge>,
        IVertexColorizerAlgorithm<TVertex,TEdge>,
        IVertexPredecessorRecorderAlgorithm<TVertex,TEdge>,
        IVertexTimeStamperAlgorithm<TVertex,TEdge>,
        ITreeBuilderAlgorithm<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
    {
		private readonly IDictionary<TVertex,GraphColor> colors;
		private int maxDepth = int.MaxValue;

        public DepthFirstSearchAlgorithm(IVertexListGraph<TVertex,TEdge> g)
            :this(g, new Dictionary<TVertex, GraphColor>())
        {}

		public DepthFirstSearchAlgorithm(
            IVertexListGraph<TVertex, TEdge> visitedGraph,
            IDictionary<TVertex, GraphColor> colors
            )
            :base(visitedGraph)
		{
			if (colors == null)
				throw new ArgumentNullException("VertexColors");

			this.colors = colors;
		}

        public IDictionary<TVertex,GraphColor> VertexColors
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
            VertexEventHandler<TVertex> eh = this.InitializeVertex;
			if (eh!=null)
				eh(this, new VertexEventArgs<TVertex>(v));
		}

		public event VertexEventHandler<TVertex> StartVertex;
		private void OnStartVertex(TVertex v)
		{
            VertexEventHandler<TVertex> eh = this.StartVertex;
			if (eh!=null)
				eh(this, new VertexEventArgs<TVertex>(v));
		}

		public event VertexEventHandler<TVertex> DiscoverVertex;
		private void OnDiscoverVertex(TVertex v)
		{
            VertexEventHandler<TVertex> eh = this.DiscoverVertex;
			if (eh!=null)
				eh(this, new VertexEventArgs<TVertex>(v));
		}

		public event EdgeEventHandler<TVertex,TEdge> ExamineEdge;
		private void OnExamineEdge(TEdge e)
		{
            EdgeEventHandler<TVertex, TEdge> eh = this.ExamineEdge;
			if (eh!=null)
				eh(this, new EdgeEventArgs<TVertex,TEdge>(e));
		}

		public event EdgeEventHandler<TVertex,TEdge> TreeEdge;
		private void OnTreeEdge(TEdge e)
		{
            EdgeEventHandler<TVertex, TEdge> eh = this.TreeEdge;
			if (eh!=null)
				eh(this, new EdgeEventArgs<TVertex,TEdge>(e));
		}

		public event EdgeEventHandler<TVertex,TEdge> BackEdge;
		private void OnBackEdge(TEdge e)
		{
            EdgeEventHandler<TVertex, TEdge> eh = this.BackEdge;
			if (eh!=null)
				eh(this, new EdgeEventArgs<TVertex,TEdge>(e));
		}

		public event EdgeEventHandler<TVertex,TEdge> ForwardOrCrossEdge;
		private void OnForwardOrCrossEdge(TEdge e)
		{
            EdgeEventHandler<TVertex, TEdge> eh = this.ForwardOrCrossEdge;
			if (eh!=null)
				eh(this, new EdgeEventArgs<TVertex,TEdge>(e));
		}

		public event VertexEventHandler<TVertex> FinishVertex;
		private void OnFinishVertex(TVertex v)
		{
            VertexEventHandler<TVertex> eh = this.FinishVertex;
			if (eh!=null)
				eh(this, new VertexEventArgs<TVertex>(v));
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
			foreach(TVertex u in VisitedGraph.Vertices)
			{
                if (this.IsAborting)
                    return;
                if (VertexColors[u] == GraphColor.White)
                {
					OnStartVertex(u);
					Visit(u,0);
				}
			}
		}

		public void Initialize()
		{
			foreach(TVertex u in VisitedGraph.Vertices)
			{
                VertexColors[u] = GraphColor.White;
                OnInitializeVertex(u);
			}
		}

        private struct SearchFrame
        {
            public readonly TVertex Vertex;
            public readonly IEnumerator<TEdge> Edges;
            public SearchFrame(TVertex vertex, IEnumerator<TEdge> edges)
            {
                this.Vertex = vertex;
                this.Edges = edges;
            }
        }

		public void Visit(TVertex root, int depth)
		{
			if ((object)root==null)
				throw new ArgumentNullException("root");

            Stack<SearchFrame> todo = new Stack<SearchFrame>();
            this.VertexColors[root] = GraphColor.Gray;
            OnDiscoverVertex(root);

            todo.Push(new SearchFrame(root, this.VisitedGraph.OutEdges(root).GetEnumerator()));
            while (todo.Count > 0)
            {
                if (this.IsAborting)
                    return;

                SearchFrame frame = todo.Pop();

                TVertex u = frame.Vertex;
                IEnumerator<TEdge> edges = frame.Edges;
                while(edges.MoveNext())
                {
                    TEdge e = edges.Current;
                    if (this.IsAborting)
                        return;
                    this.OnExamineEdge(e);
                    TVertex v = e.Target;
                    GraphColor c = this.VertexColors[v];
                    switch (c)
                    {
                        case GraphColor.White:
                            OnTreeEdge(e);
                            todo.Push(new SearchFrame(u, edges));
                            u = v;
                            edges = this.VisitedGraph.OutEdges(u).GetEnumerator();
                            this.VertexColors[u] = GraphColor.Gray;
                            this.OnDiscoverVertex(u);
                            break;
                        case GraphColor.Gray:
                            OnBackEdge(e); break;
                        case GraphColor.Black:
                            OnForwardOrCrossEdge(e); break;
                    }
                }

                this.VertexColors[u] = GraphColor.Black;
                this.OnFinishVertex(u);
            }
		}
    }
}
