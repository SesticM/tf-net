Imports Topology.Geometries

Namespace Conflation.Algorithm.LinearReference

    ''' <summary>
    ''' Provides various ways of computing the actual value of a point a given length along a line.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class LocatePoint

        Private pt As Coordinate
        Private index As Integer

        Sub New(ByVal line As LineString, ByVal length As Double)
            Me.compute(line, length)
        End Sub

        ''' <summary>
        ''' Computes the location of a point a given length along a {@link LineSegment}.
        ''' If the length exceeds the length of the line segment the last point of the segment is returned.
        ''' If the length is negative the first point of the segment is returned.
        ''' </summary>
        ''' <param name="seg">The line segment.</param>
        ''' <param name="length">The length to the desired point.</param>
        ''' <returns>{@link Coordinate} of the desired point.</returns>
        ''' <remarks></remarks>
        Public Shared Function PointAlongSegment(ByVal seg As LineSegment, ByVal length As Double) As Coordinate
            Return PointAlongSegment(seg.P0, seg.P1, length)
        End Function

        ''' <summary>
        ''' Computes the location of a point a given length along a line segment.
        ''' If the length exceeds the length of the line segment the last point of the segment is returned.
        ''' If the length is negative the first point of the segment is returned.
        ''' </summary>
        ''' <param name="p0">The first point of the line segment.</param>
        ''' <param name="p1">The last point of the line segment.</param>
        ''' <param name="length">The length to the desired point.</param>
        ''' <returns>{@link Coordinate} of the desired point.</returns>
        ''' <remarks></remarks>
        Public Shared Function PointAlongSegment(ByVal p0 As Coordinate, ByVal p1 As Coordinate, ByVal length As Double) As Coordinate
            Dim segLen As Double = p1.Distance(p0)
            Dim frac As Double = (length / segLen)

            If (frac <= 0) Then Return p0
            If (frac >= 1) Then Return p1

            Dim x As Double = (((p1.X - p0.X) * frac) + p0.X)
            Dim y As Double = (((p1.Y - p0.Y) * frac) + p0.Y)

            Return New Coordinate(x, y)
        End Function

        ''' <summary>
        ''' Computes the {@link Coordinate} of the point a given length along a {@link LineString}.
        ''' </summary>
        ''' <param name="line">The LineString to measure.</param>
        ''' <param name="length">The length to the desired point.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function pointAlongLine(ByVal line As LineString, ByVal length As Double) As Coordinate
            Dim loc As LocatePoint = New LocatePoint(line, length)
            Return loc.getPoint
        End Function

        Private Sub Compute(ByVal line As LineString, ByVal length As Double)
            ' <TODO> handle negative distances (measure from opposite end of line)
            Dim totalLength As Double = 0
            Dim coord() As ICoordinate = line.Coordinates
            Dim i As Integer = 0
            Do While (i < (coord.Length - 1))
                Dim p0 As Coordinate = coord(i)
                Dim p1 As Coordinate = coord((i + 1))
                Dim segLen As Double = p1.Distance(p0)
                If (totalLength + segLen) > length Then
                    pt = PointAlongSegment(p0, p1, (length - totalLength))
                    index = i
                    Return
                End If
                totalLength = (totalLength + segLen)
                i = (i + 1)
            Loop
            ' distance is greater than line length
            pt = New Coordinate(coord((coord.Length - 1)))
            index = coord.Length
        End Sub

        Public Function GetPoint() As Coordinate
            Return pt
        End Function

        ''' <summary>
        ''' Returns the index of the segment containing the computed point.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetIndex() As Integer
            Return index
        End Function

    End Class

End Namespace
