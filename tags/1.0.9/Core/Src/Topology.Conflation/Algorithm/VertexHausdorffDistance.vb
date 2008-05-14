Imports Topology.Geometries

Namespace Conflation.Algorithm

    ''' <summary>
    ''' Implements algorithm for computing a distance metric which can be thought of as the "Maximum Vertex Distance".
    ''' This is the Hausdorff distance restricted to vertices for one of the geometries.
    ''' Also computes two points of the Geometries which are separated by the computed distance.
    ''' <para>
    ''' NOTE: This algorithm does NOT compute the full Hausdorff distance correctly, but an approximation that
    ''' is correct for a large subset of useful cases. One important part of this subset is Linestrings that are 
    ''' roughly parallel to each other, and roughly equal in length.
    ''' </para>
    ''' </summary>
    ''' <remarks></remarks>
    Public Class VertexHausdorffDistance

        Private ptDist As PointPairDistance = New PointPairDistance

        Public Sub New(ByVal g0 As Geometry, ByVal g1 As Geometry)
            compute(g0, g1)
        End Sub

        Public Sub New(ByVal seg0 As LineSegment, ByVal seg1 As LineSegment)
            compute(seg0, seg1)
        End Sub

        Public Function Distance() As Double
            Return ptDist.GetDistance
        End Function

        Public Function GetCoordinates() As Coordinate()
            Return ptDist.GetCoordinates
        End Function

        Private Sub Compute(ByVal seg0 As LineSegment, ByVal seg1 As LineSegment)
            computeMaxPointDistance(seg0, seg1, ptDist)
            computeMaxPointDistance(seg1, seg0, ptDist)
        End Sub

        ''' <summary>
        ''' Computes the maximum oriented distance between two line segments,
        ''' as well as the point pair separated by that distance.
        ''' </summary>
        ''' <param name="seg0">The line segment containing the furthest point.</param>
        ''' <param name="seg1">The line segment containing the closest point.</param>
        ''' <param name="ptDist">The point pair and distance to be updated.</param>
        ''' <remarks></remarks>
        Private Sub ComputeMaxPointDistance(ByVal seg0 As LineSegment, ByVal seg1 As LineSegment, ByVal ptDist As PointPairDistance)
            Dim closestPt0 As Coordinate = seg0.ClosestPoint(seg1.P0)
            ptDist.SetMaximum(closestPt0, seg1.P0)
            Dim closestPt1 As Coordinate = seg0.ClosestPoint(seg1.P1)
            ptDist.SetMaximum(closestPt1, seg1.P1)
        End Sub

        Private Sub Compute(ByVal g0 As Geometry, ByVal g1 As Geometry)
            ComputeMaxPointDistance(g0, g1, ptDist)
            ComputeMaxPointDistance(g1, g0, ptDist)
        End Sub

        Private Sub ComputeMaxPointDistance(ByVal pointGeom As Geometry, ByVal geom As Geometry, ByVal ptDist As PointPairDistance)
            Dim distFilter As MaxPointDistanceFilter = New MaxPointDistanceFilter(geom)
            pointGeom.Apply(distFilter)
            ptDist.SetMaximum(distFilter.getMaxPointDistance)
        End Sub


        Public Class MaxPointDistanceFilter
            Implements ICoordinateFilter

            Private maxPtDist As PointPairDistance = New PointPairDistance
            Private minPtDist As PointPairDistance = New PointPairDistance
            Private euclideanDist As EuclideanDistanceToPoint = New EuclideanDistanceToPoint
            Private geom As Geometry

            Public Sub New(ByVal geom As Geometry)
                Me.geom = geom
            End Sub

            Public Sub Filter(ByVal coord As ICoordinate) Implements ICoordinateFilter.Filter
                minPtDist.Initialize()
                EuclideanDistanceToPoint.ComputeDistance(geom, coord, minPtDist)
                maxPtDist.SetMaximum(minPtDist)
            End Sub

            Public Function GetMaxPointDistance() As PointPairDistance
                Return maxPtDist
            End Function

        End Class
    End Class

End Namespace
