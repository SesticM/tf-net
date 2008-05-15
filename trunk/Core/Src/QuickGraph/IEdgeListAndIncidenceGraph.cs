namespace Topology.Graph
{
    public interface IEdgeListAndIncidenceGraph<TVertex,TEdge> :
        IEdgeListGraph<TVertex,TEdge>, IIncidenceGraph<TVertex,TEdge>
        where TEdge : IEdge<TVertex>
    {
    }
}
