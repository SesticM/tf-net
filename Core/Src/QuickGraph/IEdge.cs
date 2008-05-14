namespace Topology.Graph
{
    public interface IEdge<TVertex>
    {
        TVertex Source { get;}
        TVertex Target { get;}
    }
}
