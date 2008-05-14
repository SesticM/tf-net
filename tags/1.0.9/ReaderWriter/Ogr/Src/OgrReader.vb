Imports Topology.IO
Imports Topology.Geometries

''' <summary>
''' Reads GDAL/OGR geometries and creates geometric representation of the features based on JTS model. 
''' </summary>
''' <remarks></remarks>
Public Class OgrReader
    Inherits GeometryReader

#Region " CTOR "

    Sub New()
        MyBase.New()
    End Sub

#End Region

#Region " ReadGeometry "

    ''' <summary>
    ''' Returns <see cref="Geometry"/> object converted from <see cref="OSGeo.OGR.Geometry"/>.
    ''' If geometry conversion is not possible or input geometry is empty, returns empty <see cref="Geometry"/>.
    ''' </summary>
    ''' <param name="geometry">A <see cref="OSGeo.OGR.Geometry"/> object reference.</param>
    ''' <returns>A <see cref="Geometry"/> object.</returns>
    ''' <remarks></remarks>
    Public Function ReadGeometry(ByVal geometry As OSGeo.OGR.Geometry) As IGeometry
        Dim wkt As String = String.Empty
        geometry.ExportToWkt(wkt)
        Dim reader As New WKTReader
        Return reader.Read(wkt)
    End Function

#End Region

End Class


