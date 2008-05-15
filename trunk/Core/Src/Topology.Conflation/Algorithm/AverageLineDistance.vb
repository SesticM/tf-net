Imports Topology.Geometries
Imports Topology.Conflation.Algorithm.LinearReference

Namespace Conflation.Algorithm

    ''' <summary>
    ''' Computes the "average" distance between two {@link LineString}s, based on the distance between each vertex
    ''' and a point the same distance along the other line.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class AverageLineDistance

        Public Shared Factory As GeometryFactory = New GeometryFactory()
        Private avgLineDistance As Double

        Sub New(ByVal line0 As LineString, ByVal line1 As LineString)
            avgLineDistance = Compute(line0, line1)
        End Sub

        Public Function GetDistance() As Double
            Return avgLineDistance
        End Function

        Private Function Compute(ByVal line0 As LineString, ByVal line1 As LineString) As Double
            Dim lines As LineString() = Normalize(line0, line1)
            Dim distance As Double = AverageIntervalDistance(lines, 1.0)
            Return distance
        End Function

        Public Function ClosestPairIndex(ByVal pts1 As Coordinate(), ByVal pts2 As Coordinate()) As Integer()
            Dim minDistance As Double = Double.MaxValue
            Dim result(2) As Integer

            Dim i As Integer = 0
            Do While (i < 2)
                Dim j As Integer = 0
                Do While (j < 2)
                    Dim dist As Double = pts1(i).Distance(pts2(j))
                    If (dist < minDistance) Then
                        result(0) = i
                        result(1) = j
                        minDistance = dist
                    End If
                    j = (j + 1)
                Loop
                i = (i + 1)
            Loop

            Return result
        End Function

        Public Shared Function Reverse(ByVal line As LineString) As LineString
            Dim revLine As LineString = line.Clone
            Dim pts As Coordinate() = revLine.Coordinates
            CoordinateArrays.Reverse(pts)
            Return revLine
        End Function

        Private Function AverageIntervalDistance(ByVal lines As LineString(), ByVal intervalLen As Double) As Double
            Dim len0 As Double = lines(0).Length
            Dim len1 As Double = lines(1).Length
            Dim currLen As Double = 0.0
            Dim totalDistance As Double = 0.0
            Dim intervalCount As Integer = 0

            While ((currLen < len0) AndAlso (currLen < len1))
                Dim locate0 As LocatePoint = New LocatePoint(lines(0), currLen)
                Dim p0 As Coordinate = locate0.getPoint
                Dim locate1 As LocatePoint = New LocatePoint(lines(1), currLen)
                Dim p1 As Coordinate = locate1.getPoint
                Dim intervalDistance As Double = p0.Distance(p1)
                totalDistance = (totalDistance + intervalDistance)
                currLen = (currLen + intervalLen)
                intervalCount = (intervalCount + 1)
            End While

            Return totalDistance / intervalCount
        End Function

        Private Function Normalize(ByVal line0 As LineString, ByVal line1 As LineString) As LineString()
            Dim lines(2) As LineString
            Dim edge0Pts() As Coordinate = line0.Coordinates
            Dim edge1Pts() As Coordinate = line1.Coordinates

            ' find closest endpoint pair
            Dim closestPairIndex() As Integer = Me.ClosestPairIndex( _
                New Coordinate() {edge0Pts(0), edge0Pts((edge0Pts.Length - 1))}, _
                New Coordinate() {edge1Pts(0), edge1Pts((edge1Pts.Length - 1))})

            lines(0) = line0
            If (closestPairIndex(0) <> 0) Then
                lines(0) = Reverse(line0)
            End If

            lines(1) = line1
            If (closestPairIndex(1) <> 0) Then
                lines(1) = Reverse(line1)
            End If

            Return lines
        End Function

    End Class

End Namespace
