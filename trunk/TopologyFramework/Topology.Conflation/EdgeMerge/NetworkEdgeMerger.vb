Imports Topology.Features
Imports Topology.Geometries
Imports Topology.Planargraph

Namespace Conflation.EdgeMerge

    ''' <summary>
    ''' Merges edges of a linear network together based on the differences between edge attributes
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NetworkEdgeMerger

        Private graph As MergeGraph = New MergeGraph()
        Private resultGeom As GeometryCollection

        Sub New(ByVal fc As Feature())
            Me.Build(fc)
        End Sub

        Private Sub Build(ByVal fc As Feature())
            For Each feature As Feature In fc
                Me.graph.Add(feature.Geometry)
            Next
        End Sub

        '        Public Function Merge() As Feature()

        '        End Function

        'public FeatureCollection merge()
        '  {
        '    resultGeom = new ArrayList();
        '    for (Iterator i = graph.nodeIterator(); i.hasNext(); ) {
        '      Node node = (Node) i.next();
        '      if (node.getDegree() > 2)
        '        mergeEdges(node);
        '    }
        '    FeatureCollection fc = FeatureDatasetFactory.createFromGeometry(resultGeom);
        '    return fc;
        '  }

    End Class

End Namespace

