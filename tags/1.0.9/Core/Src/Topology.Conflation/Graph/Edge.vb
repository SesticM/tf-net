
Namespace Conflation.Graph

    Public Class Edge

        Protected dirEdge As DirectedEdge()

        Sub New()
        End Sub

        Sub New(ByVal de0 As DirectedEdge, ByVal de1 As DirectedEdge)
            SetDirectedEdges(de0, de1)
        End Sub

        Public Sub SetDirectedEdges(ByVal de0 As DirectedEdge, ByVal de1 As DirectedEdge)
            dirEdge = New DirectedEdge() {de0, de1}
            de0.SetEdge(Me)
            de1.SetEdge(Me)
            de0.SetSym(de1)
            de1.SetSym(de0)
            de0.GetFromNode().AddOutEdge(de0)
            de1.GetFromNode().AddOutEdge(de1)
        End Sub

        ''' <summary>
        ''' Gets one of the DirectedEdges associated with this edge.
        ''' </summary>
        ''' <param name="i">0 or 1</param>
        ''' <returns>A DirectedEdge.</returns>
        ''' <remarks></remarks>
        Public Function GetDirEdge(ByVal i As Integer) As DirectedEdge
            Return dirEdge(i)
        End Function

        ''' <summary>
        ''' Finds the {@link DirectedEdge} that starts from the given node.
        ''' </summary>
        ''' <param name="fromNode">The {@link Node} the Directed edge starts from.</param>
        ''' <returns>The {@link DirectedEdge} starting from the node.</returns>
        ''' <remarks></remarks>
        Public Function GetDirEdge(ByVal fromNode As Node) As DirectedEdge
            If dirEdge(0).GetFromNode().Equals(fromNode) Then
                Return dirEdge(0)
            End If
            If dirEdge(1).GetFromNode().Equals(fromNode) Then
                Return dirEdge(1)
            End If
            ' node not found
            ' possibly should throw an exception here?
            Return Nothing
        End Function

        Public Function GetOppositeNode(ByVal node As Node) As Node
            If dirEdge(0).GetFromNode().Equals(node) Then
                Return dirEdge(0).GetToNode()
            End If
            If dirEdge(1).GetFromNode().Equals(node) Then
                Return dirEdge(1).GetToNode()
            End If
            ' node not found
            ' possibly should throw an exception here?
            Return Nothing
        End Function

    End Class

End Namespace
