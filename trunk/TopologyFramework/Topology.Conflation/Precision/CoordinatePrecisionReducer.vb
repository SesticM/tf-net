Imports Topology.Geometries

Namespace Conflation.Precision

    Public Class CoordinatePrecisionReducer

        Private npr As NumberPrecisionReducer

        Public Sub New(ByVal npr As NumberPrecisionReducer)
            Me.npr = npr
        End Sub

        Public Sub ReducePrecision(ByVal pt As Coordinate)
            pt.X = npr.reducePrecision(pt.X)
            pt.Y = npr.reducePrecision(pt.Y)
        End Sub

    End Class

End Namespace
