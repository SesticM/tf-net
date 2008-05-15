using System;
using System.Collections;
using System.Text;



using Topology.Geometries;

namespace Topology.Utilities
{
    /// <summary>
    /// A <c>CoordinateFilter</c> that creates an array containing every coordinate in a <c>Geometry</c>.
    /// </summary>
    public class CoordinateArrayFilter : ICoordinateFilter 
    {
        Coordinate[] pts = null;
        int n = 0;

        /// <summary>
        /// Constructs a <c>CoordinateArrayFilter</c>.
        /// </summary>
        /// <param name="size">The number of points that the <c>CoordinateArrayFilter</c> will collect.</param>
        public CoordinateArrayFilter(int size) 
        {
            pts = new Coordinate[size];
        }

        /// <summary>
        /// Returns the <c>Coordinate</c>s collected by this <c>CoordinateArrayFilter</c>.
        /// </summary>
        public Coordinate[] Coordinates
        {
            get
            {
                return pts;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coord"></param>
        public void Filter(ICoordinate coord) 
        {
            pts[n++] = (Coordinate) coord;
        }
    }
}
