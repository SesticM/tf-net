
Namespace Conflation.Graph

    Public Class DirectedEdgeStar

        Protected outEdges As IList = New ArrayList()
        Private sorted As Boolean = False

        Sub New()
        End Sub

        Sub New(ByVal de As DirectedEdge)
            outEdges.Add(de)
            sorted = False
        End Sub

        Public Sub Add(ByVal de As DirectedEdge)
            outEdges.Add(de)
            sorted = False
        End Sub

        Public Sub Remove(ByVal de As DirectedEdge)
            outEdges.Remove(de)
        End Sub

        Public Function GetEnumerator() As IEnumerator
            sortEdges()
            Return outEdges.GetEnumerator
        End Function

        Public Function GetDegree() As Integer
            Return outEdges.Count
        End Function

        Public Function GetNumEdges() As Integer
            Return outEdges.Count
        End Function

        Public Function GetEdges() As IList
            sortEdges()
            Return outEdges
        End Function

        Public Sub SortEdges()
            If Not sorted Then
                Dim list As ArrayList = CType(outEdges, ArrayList)
                list.Sort()
                sorted = True
            End If
        End Sub

        Public Function GetIndex(ByVal edge As Edge) As Integer
            SortEdges()
            For i As Integer = 0 To outEdges.Count
                Dim de As DirectedEdge = outEdges(i)
                If de.GetEdge.Equals(edge) Then
                    Return i
                End If
            Next
            Return -1
        End Function

        Public Function GetIndex(ByVal dirEdge As DirectedEdge) As Integer
            SortEdges()
            For i As Integer = 0 To outEdges.Count
                Dim de As DirectedEdge = outEdges(i)
                If de.Equals(dirEdge) Then
                    Return i
                End If
            Next
            Return -1
        End Function

        Public Function GetIndex(ByVal i As Integer) As Integer
            Dim modi As Integer = i Mod outEdges.Count
            If modi < 0 Then
                modi += outEdges.Count
            End If
            Return modi
        End Function

        Public Function GetNextEdge(ByVal dirEdge As DirectedEdge) As DirectedEdge
            Dim i As Integer = GetIndex(dirEdge)
            Return outEdges(GetIndex(i + 1))
        End Function

    End Class

End Namespace
