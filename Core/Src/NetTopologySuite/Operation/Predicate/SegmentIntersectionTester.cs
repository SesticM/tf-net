using System;
using System.Collections;



using Topology.Algorithm;
using Topology.Geometries;

namespace Topology.Operation.Predicate
{
    /// <summary>
    /// Tests if any line segments in two sets of CoordinateSequences intersect.
    /// Optimized for small geometry size.
    /// Short-circuited to return as soon an intersection is found.
    /// </summary>
    public class SegmentIntersectionTester 
    {
        // for purposes of intersection testing, don't need to set precision model
        private LineIntersector li = new RobustLineIntersector();

        private bool hasIntersection = false;
        private Coordinate pt00 = new Coordinate();
        private Coordinate pt01 = new Coordinate();
        private Coordinate pt10 = new Coordinate();
        private Coordinate pt11 = new Coordinate();

        /// <summary>
        /// 
        /// </summary>
        public SegmentIntersectionTester() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        public bool HasIntersectionWithLineStrings(ICoordinateSequence seq, IList lines)
        {
            for (IEnumerator i = lines.GetEnumerator(); i.MoveNext(); ) 
            {
                ILineString line = (ILineString) i.Current;
                HasIntersection(seq, line.CoordinateSequence);
                if (hasIntersection)
                    break;
            }
            return hasIntersection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seq0"></param>
        /// <param name="seq1"></param>
        /// <returns></returns>
        public bool HasIntersection(ICoordinateSequence seq0, ICoordinateSequence seq1) 
        {
            for (int i = 1; i < seq0.Count && ! hasIntersection; i++) 
            {
                seq0.GetCoordinate(i - 1, pt00);
                seq0.GetCoordinate(i, pt01);
                for (int j = 1; j < seq1.Count && ! hasIntersection; j++) 
                {
                    seq1.GetCoordinate(j - 1, pt10);
                    seq1.GetCoordinate(j, pt11);
                    li.ComputeIntersection(pt00, pt01, pt10, pt11);
                    if (li.HasIntersection)
                        hasIntersection = true;
                }
            }
            return hasIntersection;
        }
    }
}
