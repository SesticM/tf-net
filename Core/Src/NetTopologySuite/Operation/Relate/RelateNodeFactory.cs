using System;
using System.Collections;
using System.Text;



using Topology.Geometries;
using Topology.GeometriesGraph;

namespace Topology.Operation.Relate
{
    /// <summary>
    /// Used by the <c>NodeMap</c> in a <c>RelateNodeGraph</c> to create <c>RelateNode</c>s.
    /// </summary>
    public class RelateNodeFactory : NodeFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public override Node CreateNode(ICoordinate coord)
        {
            return new RelateNode(coord, new EdgeEndBundleStar());
        }
    }
}
