Imports Topology.Geometries

Namespace Conflation.Algorithm

    ''' <summary>
    ''' Computes the Euclidean distance (L2 metric) from a Point to a Geometry.
    ''' Also computes two points which are separated by the distance.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class EuclideanDistanceToPoint

        Private Shared tempSegment As LineSegment = New LineSegment()

        Public Shared Sub ComputeDistance(ByVal geom As Geometry, ByVal pt As ICoordinate, ByVal ptDist As PointPairDistance)
            If TypeOf (geom) Is LineString Then
                ComputeDistance(CType(geom, LineString), pt, ptDist)
            ElseIf TypeOf (geom) Is Polygon Then
                ComputeDistance(CType(geom, Polygon), pt, ptDist)
            ElseIf TypeOf (geom) Is GeometryCollection Then
                Dim gc As GeometryCollection = CType(geom, GeometryCollection)
                Dim i As Integer = 0
                Do While (i < gc.Geometries.Length)
                    Dim g As Geometry = gc.GetGeometryN(i)
                    ComputeDistance(g, pt, ptDist)
                    i = (i + 1)
                Loop
            Else
                ' assume geom is Point
                ptDist.SetMinimum(geom.Coordinate, pt)
            End If
        End Sub

        Public Shared Sub ComputeDistance(ByVal line As LineString, ByVal pt As Coordinate, ByVal ptDist As PointPairDistance)
            Dim coords() As Coordinate = line.Coordinates
            Dim i As Integer = 0
            Do While (i < (coords.Length - 1))
                tempSegment.SetCoordinates(coords(i), coords((i + 1)))
                ' this is somewhat inefficient - could do better
                Dim closestPt As Coordinate = tempSegment.ClosestPoint(pt)
                ptDist.SetMinimum(closestPt, pt)
                i = (i + 1)
            Loop
        End Sub

        Public Shared Sub ComputeDistance(ByVal segment As LineSegment, ByVal pt As Coordinate, ByVal ptDist As PointPairDistance)
            Dim closestPt As Coordinate = segment.ClosestPoint(pt)
            ptDist.SetMinimum(closestPt, pt)
        End Sub

        '' <TODO> Fix computeDistance for LinearRing
        'Public Overloads Shared Sub computeDistance(ByVal poly As Polygon, ByVal pt As Coordinate, ByVal ptDist As PointPairDistance)
        '    computeDistance(poly.Shell, pt, ptDist)
        '    Dim i As Integer = 0
        '    Do While (i < poly.Holes.Length)
        '        computeDistance(poly.GetInteriorRingN(i), pt, ptDist)
        '        i = (i + 1)
        '    Loop
        'End Sub

    End Class

End Namespace

