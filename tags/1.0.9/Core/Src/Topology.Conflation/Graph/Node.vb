Imports Topology.Collections
Imports Topology.Geometries

Namespace Conflation.Graph

    Public Class Node

        Protected pt As Coordinate
        Protected deStar As DirectedEdgeStar

        Sub New(ByVal pt As Coordinate)
            Me.New(pt, New DirectedEdgeStar)
        End Sub

        Sub New(ByVal pt As Coordinate, ByVal deStar As DirectedEdgeStar)
            Me.pt = pt
            Me.deStar = deStar
        End Sub

        ' The input nodes are assumed to be different
        Public Shared Function GetEdgesBetween(ByVal node0 As Node, ByVal node1 As Node) As ICollection
            Dim edges0 As IList = DirectedEdge.ToEdges(node0.GetOutEdges().GetEdges())
            Dim commonEdges As ISet = New HashedSet(edges0)
            Dim edges1 As IList = DirectedEdge.ToEdges(node1.GetOutEdges().GetEdges())
            commonEdges.RetainAll(edges1)
            Return commonEdges
        End Function

        Public Function GetCoordinate() As Coordinate
            Return pt
        End Function

        Public Sub AddOutEdge(ByVal de As DirectedEdge)
            deStar.add(de)
        End Sub

        Public Function GetOutEdges() As DirectedEdgeStar
            Return deStar
        End Function

        Public Function GetDegree() As Integer
            Return deStar.GetDegree
        End Function

        Public Function GetIndex(ByVal edge As Edge) As Integer
            Return deStar.GetIndex(edge)
        End Function

    End Class

End Namespace
