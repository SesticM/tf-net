using System;
using System.Collections;
using System.Text;



using Topology.Geometries;
using Topology.GeometriesGraph;
using Topology.GeometriesGraph.Index;
using Topology.Algorithm;
using Topology.Operation;
using Topology.Utilities;

namespace Topology.Operation.Overlay
{
    /// <summary>
    /// The spatial functions supported by this class.
    /// These operations implement various bool combinations of the resultants of the overlay.
    /// </summary>
    public enum SpatialFunctions
    {
        /// <summary>
        /// 
        /// </summary>
        Intersection  = 1,

        /// <summary>
        /// 
        /// </summary>
        Union         = 2,

        /// <summary>
        /// 
        /// </summary>
        Difference    = 3,

        /// <summary>
        /// 
        /// </summary>
        SymDifference = 4,
    }

    /// <summary>
    /// Computes the overlay of two <c>Geometry</c>s.  The overlay
    /// can be used to determine any bool combination of the geometries.
    /// </summary>
    public class OverlayOp : GeometryGraphOperation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geom0"></param>
        /// <param name="geom1"></param>
        /// <param name="opCode"></param>
        /// <returns></returns>
        public static IGeometry Overlay(IGeometry geom0, IGeometry geom1, SpatialFunctions opCode)
        {
            OverlayOp gov = new OverlayOp(geom0, geom1);
            IGeometry geomOv = gov.GetResultGeometry(opCode);
            return geomOv;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="opCode"></param>
        /// <returns></returns>
        public static bool IsResultOfOp(Label label, SpatialFunctions opCode)
        {
            Locations loc0 = label.GetLocation(0);
            Locations loc1 = label.GetLocation(1);
            return IsResultOfOp(loc0, loc1, opCode);
        }

        /// <summary>
        /// This method will handle arguments of Location.NULL correctly.
        /// </summary>
        /// <returns><c>true</c> if the locations correspond to the opCode.</returns>
        public static bool IsResultOfOp(Locations loc0, Locations loc1, SpatialFunctions opCode)
        {
            if (loc0 == Locations.Boundary) 
                loc0 = Locations.Interior;
            if (loc1 == Locations.Boundary) 
                loc1 = Locations.Interior;
            
            switch (opCode) 
            {
                case SpatialFunctions.Intersection:
                    return loc0 == Locations.Interior && loc1 == Locations.Interior;
                case SpatialFunctions.Union:
                    return loc0 == Locations.Interior || loc1 == Locations.Interior;
                case SpatialFunctions.Difference:
                    return loc0 == Locations.Interior && loc1 != Locations.Interior;
                case SpatialFunctions.SymDifference:
                    return   (loc0 == Locations.Interior &&  loc1 != Locations.Interior)
                          || (loc0 != Locations.Interior &&  loc1 == Locations.Interior);
	            default:
                    return false;
            }            
        }

        private readonly PointLocator ptLocator = new PointLocator();
        private IGeometryFactory geomFact;
        private IGeometry resultGeom;

        private PlanarGraph graph;
        private EdgeList edgeList     = new EdgeList();

        private IList resultPolyList   = new ArrayList();
        private IList resultLineList   = new ArrayList();
        private IList resultPointList  = new ArrayList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g0"></param>
        /// <param name="g1"></param>
        public OverlayOp(IGeometry g0, IGeometry g1) : base(g0, g1)
        {            
            graph = new PlanarGraph(new OverlayNodeFactory());

            /*
            * Use factory of primary point.
            * Note that this does NOT handle mixed-precision arguments
            * where the second arg has greater precision than the first.
            */
            geomFact = g0.Factory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="funcCode"></param>
        /// <returns></returns>
        public IGeometry GetResultGeometry(SpatialFunctions funcCode)
        {
            ComputeOverlay(funcCode);
            return resultGeom;
        }

        /// <summary>
        /// 
        /// </summary>
        public PlanarGraph Graph
        {
            get
            {
                return graph;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opCode"></param>
        private void ComputeOverlay(SpatialFunctions opCode)
        {
            // copy points from input Geometries.
            // This ensures that any Point geometries
            // in the input are considered for inclusion in the result set
            CopyPoints(0);
            CopyPoints(1);

            // node the input Geometries
            arg[0].ComputeSelfNodes(lineIntersector, false);
            arg[1].ComputeSelfNodes(lineIntersector, false);            

            // compute intersections between edges of the two input geometries
            arg[0].ComputeEdgeIntersections(arg[1], lineIntersector, true);

            IList baseSplitEdges = new ArrayList();
            arg[0].ComputeSplitEdges(baseSplitEdges);            
            arg[1].ComputeSplitEdges(baseSplitEdges);            
            // add the noded edges to this result graph
            InsertUniqueEdges(baseSplitEdges);

            ComputeLabelsFromDepths();
            ReplaceCollapsedEdges();        

            graph.AddEdges(edgeList.Edges);
            ComputeLabelling();
            LabelIncompleteNodes();

            /*
            * The ordering of building the result Geometries is important.
            * Areas must be built before lines, which must be built before points.
            * This is so that lines which are covered by areas are not included
            * explicitly, and similarly for points.
            */
            FindResultAreaEdges(opCode);
            CancelDuplicateResultEdges();
            PolygonBuilder polyBuilder = new PolygonBuilder(geomFact);
            polyBuilder.Add(graph);
            resultPolyList = polyBuilder.Polygons;

            LineBuilder lineBuilder = new LineBuilder(this, geomFact, ptLocator);
            resultLineList = lineBuilder.Build(opCode);

            PointBuilder pointBuilder = new PointBuilder(this, geomFact, ptLocator);
            resultPointList = pointBuilder.Build(opCode);

            // gather the results from all calculations into a single Geometry for the result set
            resultGeom = ComputeGeometry(resultPointList, resultLineList, resultPolyList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="edges"></param>
        private void InsertUniqueEdges(IList edges)
        {
            for (IEnumerator i = edges.GetEnumerator(); i.MoveNext(); ) 
            {
                Edge e = (Edge) i.Current;
                InsertUniqueEdge(e);
            }
        }

        /// <summary>
        /// Insert an edge from one of the noded input graphs.
        /// Checks edges that are inserted to see if an
        /// identical edge already exists.
        /// If so, the edge is not inserted, but its label is merged
        /// with the existing edge.
        /// </summary>
        /// <param name="e"></param>
        protected void InsertUniqueEdge(Edge e)
        {
            int foundIndex = edgeList.FindEdgeIndex(e);
            // If an identical edge already exists, simply update its label
            if (foundIndex >= 0)
            {
                Edge existingEdge = (Edge) edgeList[foundIndex];
                Label existingLabel = existingEdge.Label;

                Label labelToMerge = e.Label;
                // check if new edge is in reverse direction to existing edge
                // if so, must flip the label before merging it
                if (!existingEdge.IsPointwiseEqual(e))
                {
                    labelToMerge = new Label(e.Label);
                    labelToMerge.Flip();
                }
                Depth depth = existingEdge.Depth;
                // if this is the first duplicate found for this edge, initialize the depths
                if (depth.IsNull())
                    depth.Add(existingLabel);
                depth.Add(labelToMerge);
                existingLabel.Merge(labelToMerge);
            }
            else
            {
                // no matching existing edge was found
                // add this new edge to the list of edges in this graph
                edgeList.Add(e);
            }
        }

        /// <summary>
        /// Update the labels for edges according to their depths.
        /// For each edge, the depths are first normalized.
        /// Then, if the depths for the edge are equal,
        /// this edge must have collapsed into a line edge.
        /// If the depths are not equal, update the label
        /// with the locations corresponding to the depths
        /// (i.e. a depth of 0 corresponds to a Location of Exterior,
        /// a depth of 1 corresponds to Interior)
        /// </summary>
        private void ComputeLabelsFromDepths()
        {
            for (IEnumerator it = edgeList.GetEnumerator(); it.MoveNext(); ) 
            {
                Edge e = (Edge) it.Current;
                Label lbl = e.Label;
                Depth depth = e.Depth;
                /*
                * Only check edges for which there were duplicates,
                * since these are the only ones which might
                * be the result of dimensional collapses.
                */
                if (!depth.IsNull()) 
                {
                    depth.Normalize();                    
                    for (int i = 0; i < 2; i++) 
                    {
                        if (!lbl.IsNull(i) && lbl.IsArea() && ! depth.IsNull(i)) 
                        {                            
                            /*
                             * if the depths are equal, this edge is the result of
                             * the dimensional collapse of two or more edges.
                             * It has the same location on both sides of the edge,
                             * so it has collapsed to a line.
                             */
                            if (depth.GetDelta(i) == 0)                   
                                lbl.ToLine(i);
                            else
                            {                                
                                /*
                                * This edge may be the result of a dimensional collapse,
                                * but it still has different locations on both sides.  The
                                * label of the edge must be updated to reflect the resultant
                                * side locations indicated by the depth values.
                                */
                                Assert.IsTrue(!depth.IsNull(i, Positions.Left), "depth of Left side has not been initialized");
                                lbl.SetLocation(i, Positions.Left, depth.GetLocation(i, Positions.Left));
                                Assert.IsTrue(!depth.IsNull(i, Positions.Right), "depth of Right side has not been initialized");
                                lbl.SetLocation(i, Positions.Right, depth.GetLocation(i, Positions.Right));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// If edges which have undergone dimensional collapse are found,
        /// replace them with a new edge which is a L edge
        /// </summary>
        private void ReplaceCollapsedEdges()
        {
            IList newEdges = new ArrayList();
            IList edgesToRemove = new ArrayList();
            IEnumerator it = edgeList.GetEnumerator();
            while (it.MoveNext()) 
            {
                Edge e = (Edge) it.Current;
                if (e.IsCollapsed) 
                {
                    // edgeList.Remove(it.Current as Edge); 
                    // Diego Guidi says:
                    // This instruction throws a "System.InvalidOperationException: Collection was modified; enumeration operation may not execute".
                    // i try to not modify edgeList here, and remove all elements at the end of iteration.
                    edgesToRemove.Add((Edge)it.Current);
                    newEdges.Add(e.CollapsedEdge);
                }
            }
            // Removing all collapsed edges at the end of iteration.
            foreach (Edge obj in edgesToRemove)
                edgeList.Remove(obj);            
            foreach (object obj in newEdges)
                edgeList.Add((Edge) obj);
        }

        /// <summary>
        /// Copy all nodes from an arg point into this graph.
        /// The node label in the arg point overrides any previously computed
        /// label for that argIndex.
        /// (E.g. a node may be an intersection node with
        /// a previously computed label of Boundary,
        /// but in the original arg Geometry it is actually
        /// in the interior due to the Boundary Determination Rule)
        /// </summary>
        /// <param name="argIndex"></param>
        private void CopyPoints(int argIndex)
        {
            IEnumerator i = arg[argIndex].GetNodeEnumerator();
            while (i.MoveNext()) 
            {
                Node graphNode = (Node) i.Current;
                Node newNode = graph.AddNode(graphNode.Coordinate);
                newNode.SetLabel(argIndex, graphNode.Label.GetLocation(argIndex));
            }
        }

        /// <summary> 
        /// Compute initial labelling for all DirectedEdges at each node.
        /// In this step, DirectedEdges will acquire a complete labelling
        /// (i.e. one with labels for both Geometries)
        /// only if they
        /// are incident on a node which has edges for both Geometries
        /// </summary>
        private void ComputeLabelling()
        {
            IEnumerator nodeit = graph.Nodes.GetEnumerator();
            while (nodeit.MoveNext()) 
            {
                Node node = (Node) nodeit.Current;
                node.Edges.ComputeLabelling(arg);
            }
            MergeSymLabels();
            UpdateNodeLabelling();
        }

        /// <summary> 
        /// For nodes which have edges from only one Geometry incident on them,
        /// the previous step will have left their dirEdges with no labelling for the other
        /// Geometry.  However, the sym dirEdge may have a labelling for the other
        /// Geometry, so merge the two labels.
        /// </summary>
        private void MergeSymLabels()
        {
            IEnumerator nodeit = graph.Nodes.GetEnumerator();
            while (nodeit.MoveNext()) 
            {
                Node node = (Node) nodeit.Current;
                ((DirectedEdgeStar) node.Edges).MergeSymLabels();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateNodeLabelling()
        {
            // update the labels for nodes
            // The label for a node is updated from the edges incident on it
            // (Note that a node may have already been labelled
            // because it is a point in one of the input geometries)
            IEnumerator nodeit = graph.Nodes.GetEnumerator();
            while (nodeit.MoveNext()) 
            {
                Node node = (Node) nodeit.Current;
                Label lbl = ((DirectedEdgeStar) node.Edges).Label;
                node.Label.Merge(lbl);
            }
        }

        /// <summary>
        /// Incomplete nodes are nodes whose labels are incomplete.
        /// (e.g. the location for one Geometry is null).
        /// These are either isolated nodes,
        /// or nodes which have edges from only a single Geometry incident on them.
        /// Isolated nodes are found because nodes in one graph which don't intersect
        /// nodes in the other are not completely labelled by the initial process
        /// of adding nodes to the nodeList.
        /// To complete the labelling we need to check for nodes that lie in the
        /// interior of edges, and in the interior of areas.
        /// When each node labelling is completed, the labelling of the incident
        /// edges is updated, to complete their labelling as well.
        /// </summary>
        private void LabelIncompleteNodes()
        {
            IEnumerator ni = graph.Nodes.GetEnumerator();
            while (ni.MoveNext()) 
            {
                Node n = (Node) ni.Current;
                Label label = n.Label;
                if (n.IsIsolated) 
                {
                    if (label.IsNull(0))
                         LabelIncompleteNode(n, 0);
                    else LabelIncompleteNode(n, 1);
                }
                // now update the labelling for the DirectedEdges incident on this node
                ((DirectedEdgeStar) n.Edges).UpdateLabelling(label);
            }
        }

        /// <summary>
        /// Label an isolated node with its relationship to the target point.
        /// </summary>
        private void LabelIncompleteNode(Node n, int targetIndex)
        {
            Locations loc = ptLocator.Locate(n.Coordinate, arg[targetIndex].Geometry);
            n.Label.SetLocation(targetIndex, loc);
        }

        /// <summary>
        /// Find all edges whose label indicates that they are in the result area(s),
        /// according to the operation being performed.  Since we want polygon shells to be
        /// oriented CW, choose dirEdges with the interior of the result on the RHS.
        /// Mark them as being in the result.
        /// Interior Area edges are the result of dimensional collapses.
        /// They do not form part of the result area boundary.
        /// </summary>
        private void FindResultAreaEdges(SpatialFunctions opCode)
        {
            IEnumerator it = graph.EdgeEnds.GetEnumerator();
            while (it.MoveNext()) 
            {
                DirectedEdge de = (DirectedEdge) it.Current;
                // mark all dirEdges with the appropriate label
                Label label = de.Label;
                if (label.IsArea() && !de.IsInteriorAreaEdge &&
                    IsResultOfOp(label.GetLocation(0, Positions.Right), label.GetLocation(1, Positions.Right), opCode))                 
                        de.InResult = true;                            
            }
        }

        /// <summary>
        /// If both a dirEdge and its sym are marked as being in the result, cancel
        /// them out.
        /// </summary>
        private void CancelDuplicateResultEdges()
        {
            // remove any dirEdges whose sym is also included
            // (they "cancel each other out")
            IEnumerator it = graph.EdgeEnds.GetEnumerator();
            while (it.MoveNext()) 
            {
                DirectedEdge de = (DirectedEdge) it.Current;
                DirectedEdge sym = de.Sym;
                if (de.IsInResult && sym.IsInResult)
                {
                    de.InResult = false;
                    sym.InResult = false;            
                }
            }
        }

        /// <summary>
        /// This method is used to decide if a point node should be included in the result or not.
        /// </summary>
        /// <returns><c>true</c> if the coord point is covered by a result Line or Area point.</returns>
        public bool IsCoveredByLA(ICoordinate coord)
        {
            if (IsCovered(coord, resultLineList)) 
                return true;
            if (IsCovered(coord, resultPolyList)) 
                return true;
            return false;
        }
        /// <summary>
        /// This method is used to decide if an L edge should be included in the result or not.
        /// </summary>
        /// <returns><c>true</c> if the coord point is covered by a result Area point.</returns>
        public bool IsCoveredByA(ICoordinate coord)
        {
            if (IsCovered(coord, resultPolyList)) 
                return true;
            return false;
        }

        /// <returns>
        /// <c>true</c> if the coord is located in the interior or boundary of
        /// a point in the list.
        /// </returns>
        private bool IsCovered(ICoordinate coord, IList geomList)
        {
            IEnumerator it = geomList.GetEnumerator();
            while (it.MoveNext()) 
            {
                IGeometry geom = (IGeometry) it.Current;
                Locations loc = ptLocator.Locate(coord, geom);
                if (loc != Locations.Exterior) 
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultPointList"></param>
        /// <param name="resultLineList"></param>
        /// <param name="resultPolyList"></param>
        /// <returns></returns>
        private IGeometry ComputeGeometry(IList resultPointList, IList resultLineList, IList resultPolyList)
        {
            ArrayList geomList = new ArrayList();
            // element geometries of the result are always in the order Point,Curve,A
            //geomList.addAll(resultPointList);
            foreach (object obj in resultPointList)
                geomList.Add(obj);

            //geomList.addAll(resultLineList);
            foreach (object obj in resultLineList)
                geomList.Add(obj);

            //geomList.addAll(resultPolyList);
            foreach (object obj in resultPolyList)
                geomList.Add(obj);

            // build the most specific point possible
            return geomFact.BuildGeometry(geomList);
        }
    }
}
