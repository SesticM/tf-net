Imports Topology.Geometries

Namespace Conflation.Algorithm

    ''' <summary>
    ''' Computes the "width" of a LineString, as the maximum distance from the vertices to the direction
    ''' line of the LineString.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class LineStringWidth

        Public Shared Function MaxWidth(ByVal line As LineString) As Double
            Dim pts() As Coordinate = line.Coordinates
            Dim directionSeg As LineSegment = New LineSegment(pts(0), pts((pts.Length - 1)))
            Dim maxDistance As Double = 0
            Dim i As Integer = 1
            Do While (i < (pts.Length - 2))
                Dim ptDistance As Double = directionSeg.Distance(pts(i))
                If (ptDistance > maxDistance) Then
                    maxDistance = ptDistance
                End If
                i = (i + 1)
            Loop
            Return maxDistance
        End Function
    End Class

End Namespace
