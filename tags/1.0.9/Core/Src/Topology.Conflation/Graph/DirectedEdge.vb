Imports Topology.Algorithm
Imports Topology.Geometries
Imports Topology.GeometriesGraph

Namespace Conflation.Graph

    Public Class DirectedEdge
        Implements IComparable(Of DirectedEdge)

        Protected parentEdge As Edge
        Protected from As Node
        Protected [to] As Node
        Protected p0, p1 As Coordinate
        Protected sym As DirectedEdge = Nothing  ' optional
        Protected edgeDirection As Boolean
        Protected quadrant As Integer
        Protected angle As Double

        'Protected Shared cga As NonRobustCGAlgorithms = New NonRobustCGAlgorithms
        'protected static final RobustCGAlgorithms cga = new RobustCGAlgorithms()

        Sub New(ByVal from As Node, ByVal [to] As Node, ByVal directionPt As Coordinate, ByVal edgeDirection As Boolean)
            Me.from = from
            Me.to = [to]
            Me.edgeDirection = edgeDirection
            p0 = from.GetCoordinate()
            p1 = directionPt
            Dim dx As Double = p1.X - p0.X
            Dim dy As Double = p1.Y - p0.Y
            quadrant = QuadrantOp.Quadrant(dx, dy)
            angle = Math.Atan2(dy, dx)
        End Sub

        Public Shared Function ToEdges(ByVal dirEdges As ICollection) As IList
            Dim edges As IList = New ArrayList()
            Dim i As IEnumerator = dirEdges.GetEnumerator
            Do While i.MoveNext
                edges.Add(i.Current.ParentEdge)
            Loop
            Return edges
        End Function

        Public Function GetEdge() As Edge
            Return parentEdge
        End Function

        Public Sub SetEdge(ByVal parentEdge As Edge)
            Me.parentEdge = parentEdge
        End Sub

        Public Function GetEdgeDirection() As Boolean
            Return edgeDirection
        End Function

        Public Function GetFromNode() As Node
            Return from
        End Function

        Public Function GetToNode() As Node
            Return [to]
        End Function

        ''' <summary>
        ''' Returns the starting angle of this DirectedEdge.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAngle() As Double
            Return angle
        End Function

        Public Function GetSym() As DirectedEdge
            Return sym
        End Function

        Public Sub SetSym(ByVal sym As DirectedEdge)
            Me.sym = sym
        End Sub

        Public Function CompareDirection(ByVal e As DirectedEdge) As Integer
            ' if the rays are in different quadrants, determining the ordering is trivial
            If quadrant > e.quadrant Then Return 1
            If quadrant < e.quadrant Then Return -1

            ' vectors are in the same quadrant - check relative orientation of direction vectors
            ' this is > e if it is CCW of e
            ' <TODO>
            'Return cga.computeOrientation(e.p0, e.p1, p1)
            Return 0
        End Function

        Public Function CompareTo(ByVal other As DirectedEdge) As Integer Implements System.IComparable(Of DirectedEdge).CompareTo
            Return CompareDirection(other)
        End Function

        'public void print(PrintStream out)
        '{
        '  String className = getClass().getName();
        '  int lastDotPos = className.lastIndexOf('.');
        '  String name = className.substring(lastDotPos + 1);
        '  out.print("  " + name + ": " + p0 + " - " + p1 + " " + quadrant + ":" + angle);
        '}

    End Class

End Namespace
