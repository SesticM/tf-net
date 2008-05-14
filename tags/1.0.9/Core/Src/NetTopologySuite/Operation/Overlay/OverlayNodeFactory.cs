using System;
using System.Collections;
using System.Text;



using Topology.Geometries;
using Topology.GeometriesGraph;

namespace Topology.Operation.Overlay
{
    /// <summary>
    /// Creates nodes for use in the <c>PlanarGraph</c>s constructed during
    /// overlay operations.
    /// </summary>
    public class OverlayNodeFactory : NodeFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public override Node CreateNode(ICoordinate coord)
        {
            return new Node(coord, new DirectedEdgeStar());
        }
    }
}
