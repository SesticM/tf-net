Imports Topology.Features
Imports Topology.Geometries
Imports Topology.Planargraph

Namespace Conflation.EdgeMerge

    Public Class MergeGraph
        Inherits Topology.Planargraph.PlanarGraph

        Sub New()
        End Sub

        Public Overloads Sub Add(ByVal feature As Feature)
            Dim line As LineString = CType(feature.Geometry, LineString)
            Dim linePts As Coordinate() = CoordinateArrays.RemoveRepeatedPoints(line.Coordinates)
            Dim startPt As Coordinate = linePts(0)
            Dim endPt As Coordinate = linePts(linePts.Length - 1)

            Dim nStart As Node = Me.GetNode(startPt)
            Dim nEnd As Node = Me.GetNode(endPt)

            Dim de0 As DirectedEdge = New DirectedEdge(nStart, nEnd, linePts(1), True)
            Dim de1 As DirectedEdge = New DirectedEdge(nEnd, nStart, linePts(linePts.Length - 2), False)
            Dim edge As Edge = New MergeEdge(feature)
            edge.SetDirectedEdges(de0, de1)
            Me.Add(edge)
        End Sub

        Private Function GetNode(ByVal pt As Coordinate) As Node
            Dim node As Node = Me.FindNode(pt)
            If node Is Nothing Then
                node = New Node(pt)
                ' ensure node is only added once to graph
                Me.Add(node)
            End If
            Return node
        End Function

    End Class

End Namespace
