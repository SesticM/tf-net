Imports Topology.Geometries

Namespace IO

    Public MustInherit Class GeometryWriter
        Inherits GeometryReaderWriter

#Region " CTOR "

        Sub New()
            MyBase.New()
        End Sub

        Sub New(ByVal factory As IGeometryFactory)
            MyBase.New(factory)
        End Sub

#End Region

    End Class

End Namespace
