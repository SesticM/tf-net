Imports Topology.Features
Imports Topology.Geometries
Imports Topology.Planargraph

Namespace Conflation.EdgeMerge

    Public Class MergeEdge
        Inherits Topology.Planargraph.Edge

        Private Feature As Feature

        Sub New(ByVal feature As Feature)
            MyBase.New()
            Me.Feature = feature
        End Sub

        Public Function GetGeometry() As Geometry
            Return Me.Feature.Geometry
        End Function

    End Class

End Namespace
