Imports Topology.Geometries

Namespace Conflation.Algorithm

    ''' <summary>
    ''' Contains a pair of points and the distance between them.
    ''' Provides methods to update with a new point pair with either maximum or minimum distance.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PointPairDistance

        Private pt(2) As Coordinate
        Private distance As Double = 0
        Private isNull As Boolean = True

        Public Sub New()
        End Sub

        Public Sub Initialize()
            isNull = True
        End Sub

        Public Sub Initialize(ByVal p0 As Coordinate, ByVal p1 As Coordinate)
            pt(0) = p0
            pt(1) = p1
            distance = p0.Distance(p1)
            isNull = False
        End Sub

        ''' <summary>
        ''' Initializes the points, avoiding recomputing the distance.
        ''' </summary>
        ''' <param name="p0"></param>
        ''' <param name="p1"></param>
        ''' <param name="distance">The distance between p0 and p1.</param>
        ''' <remarks></remarks>
        Private Sub Initialize(ByVal p0 As Coordinate, ByVal p1 As Coordinate, ByVal distance As Double)
            pt(0) = p0
            pt(1) = p1
            Me.distance = distance
            isNull = False
        End Sub

        Public Function GetDistance() As Double
            Return distance
        End Function

        Public Function GetCoordinates() As Coordinate()
            Return pt
        End Function

        Public Function GetCoordinate(ByVal i As Integer) As Coordinate
            Return pt(i)
        End Function

        Public Sub SetMaximum(ByVal ptDist As PointPairDistance)
            SetMaximum(ptDist.pt(0), ptDist.pt(1))
        End Sub

        Public Sub SetMaximum(ByVal p0 As Coordinate, ByVal p1 As Coordinate)
            If isNull Then
                Initialize(p0.Clone(), p1.Clone())
                Return
            End If
            Dim dist As Double = p0.Distance(p1)
            If (dist > distance) Then
                Initialize(p0.Clone(), p1.Clone(), dist)
            End If
        End Sub

        Public Sub SetMinimum(ByVal ptDist As PointPairDistance)
            SetMinimum(ptDist.pt(0), ptDist.pt(1))
        End Sub

        Public Sub SetMinimum(ByVal p0 As Coordinate, ByVal p1 As Coordinate)
            If isNull Then
                Initialize(p0.Clone(), p1.Clone())
                Return
            End If
            Dim dist As Double = p0.Distance(p1)
            If (dist < distance) Then
                Initialize(p0.Clone(), p1.Clone(), dist)
            End If
        End Sub
    End Class

End Namespace
