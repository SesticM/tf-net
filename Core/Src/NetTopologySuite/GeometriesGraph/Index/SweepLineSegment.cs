using System;
using System.Collections;
using System.Text;



using Topology.Geometries;
using Topology.GeometriesGraph;

namespace Topology.GeometriesGraph.Index
{
    /// <summary>
    /// 
    /// </summary>
    public class SweepLineSegment
    {
        private Edge edge;
        private ICoordinate[] pts;
        int ptIndex;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="ptIndex"></param>
        public SweepLineSegment(Edge edge, int ptIndex)
        {
            this.edge = edge;
            this.ptIndex = ptIndex;
            pts = edge.Coordinates;
        }

        /// <summary>
        /// 
        /// </summary>
        public double MinX
        {
            get
            {
                double x1 = pts[ptIndex].X;
                double x2 = pts[ptIndex + 1].X;
                return x1 < x2 ? x1 : x2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double MaxX
        {
            get
            {
                double x1 = pts[ptIndex].X;
                double x2 = pts[ptIndex + 1].X;
                return x1 > x2 ? x1 : x2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ss"></param>
        /// <param name="si"></param>
        public void ComputeIntersections(SweepLineSegment ss, SegmentIntersector si)
        {
            si.AddIntersections(edge, ptIndex, ss.edge, ss.ptIndex);
        }
    }
}
