﻿using System;
using System.Collections.Generic;
using Topology.Graph.Algorithms.Search;
using Topology.Graph.Algorithms.Observers;
using Topology.Graph.Collections;

namespace Topology.Graph.Algorithms.ShortestPath
{
    [Serializable]
    public abstract class ShortestPathAlgorithmBase<TVertex, TEdge, TGraph> :
        RootedAlgorithmBase<TVertex,TGraph>
        where TEdge : IEdge<TVertex>
    {
        private readonly IDictionary<TVertex, GraphColor> vertexColors;
        private readonly IDictionary<TVertex, double> distances;
        private readonly IDictionary<TEdge, double> weights;
        private readonly IDistanceRelaxer distanceRelaxer;

        protected ShortestPathAlgorithmBase(
            TGraph visitedGraph,
            IDictionary<TEdge, double> weights
            )
            :this(visitedGraph, weights, new ShortestDistanceRelaxer())
        {}

        protected ShortestPathAlgorithmBase(
            TGraph visitedGraph,
            IDictionary<TEdge, double> weights,
            IDistanceRelaxer distanceRelaxer
            )
            :base(visitedGraph)
        {
            if (weights == null)
                throw new ArgumentNullException("weights");
            if (distanceRelaxer == null)
                throw new ArgumentNullException("distanceRelaxer");

            this.vertexColors = new Dictionary<TVertex, GraphColor>();
            this.distances = new Dictionary<TVertex, double>();
            this.weights = weights;
            this.distanceRelaxer = distanceRelaxer;
        }

        public static Dictionary<TEdge, double> UnaryWeightsFromEdgeList(
            IEdgeListGraph<TVertex, TEdge> graph)
        {
            if (graph == null)
                throw new ArgumentNullException("graph");
            Dictionary<TEdge, double> weights = new Dictionary<TEdge, double>();
            foreach (TEdge e in graph.Edges)
                weights.Add(e, 1);
            return weights;
        }

        public static Dictionary<TEdge, double> UnaryWeightsFromVertexList(
            IVertexListGraph<TVertex, TEdge> graph)
        {
            if (graph == null)
                throw new ArgumentNullException("graph");
            Dictionary<TEdge, double> weights = new Dictionary<TEdge, double>();
            foreach (TVertex v in graph.Vertices)
                foreach (TEdge e in graph.OutEdges(v))
                    weights.Add(e, 1);
            return weights;
        }

        public IDictionary<TVertex, GraphColor> VertexColors
        {
            get
            {
                return this.vertexColors;
            }
        }

        public IDictionary<TVertex, double> Distances
        {
            get
            {
                return this.distances;
            }
        }

        public IDictionary<TEdge, double> Weights
        {
            get { return this.weights; }
        }

        public IDistanceRelaxer DistanceRelaxer
        {
            get { return this.distanceRelaxer; }
        }

        protected bool Compare(double a, double b)
        {
            return this.distanceRelaxer.Compare(a, b);
        }

        protected double Combine(double distance, double weight)
        {
            return this.distanceRelaxer.Combine(distance, weight);
        }
    }
}
