﻿using System;
using System.Collections.Generic;
namespace Topology.Graph.Predicates
{
    [Serializable]
    public class FilteredVertexListGraph<TVertex, TEdge, Graph> :
        FilteredIncidenceGraph<TVertex,TEdge,Graph>,
        IVertexListGraph<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
        where Graph : IVertexListGraph<TVertex,TEdge>
    {
        public FilteredVertexListGraph(
            Graph baseGraph,
            VertexPredicate<TVertex> vertexPredicate,
            EdgePredicate<TVertex, TEdge> edgePredicate
            )
            :base(baseGraph,vertexPredicate,edgePredicate)
        { }

        public bool IsVerticesEmpty
        {
            get 
            {
                foreach (TVertex v in this.Vertices)
                        return false;
                return true;
            }
        }

        public int VertexCount
        {
            get 
            {
                int count = 0;
                foreach (TVertex v in this.Vertices)
                        count++;
                return count;
            }
        }

        public IEnumerable<TVertex> Vertices
        {
            get 
            {
                foreach (TVertex v in this.BaseGraph.Vertices)
                    if (this.VertexPredicate(v))
                        yield return v;
            }
        }

        public bool ContainsVertex(TVertex vertex)
        {
            if (!this.VertexPredicate(vertex))
                return false;
            return this.ContainsVertex(vertex);
        }
    }
}
