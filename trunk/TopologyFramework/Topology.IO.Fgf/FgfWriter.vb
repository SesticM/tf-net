Imports Topology.IO
Imports Topology.Geometries

Namespace IO

    Public Class FgfWriter
        Inherits GeometryReaderWriter

        Private FgfGeometryFactory As OSGeo.FDO.Geometry.FgfGeometryFactory

        Sub New()
            Me.FgfGeometryFactory = New OSGeo.FDO.Geometry.FgfGeometryFactory
        End Sub

#Region " GetOrdinateSequence "

        Private Function GetOrdinateSequence(ByVal geometry As Geometry) As OrdinateSequence
            Dim m_Result As New OrdinateSequence
            Dim m_Ordinates As New List(Of Double)

            m_Result.Dimensionality = 1

            For Each m_Coordinate As Coordinate In geometry.Coordinates
                m_Ordinates.Add(m_Coordinate.X)
                m_Ordinates.Add(m_Coordinate.Y)
                If m_Coordinate.Z.ToString <> Double.NaN.ToString Then
                    m_Ordinates.Add(m_Coordinate.Z)
                Else
                    m_Result.Dimensionality = 0
                End If
            Next

            m_Result.OrdinateNumber = m_Ordinates.Count
            m_Result.Ordinates = m_Ordinates.ToArray

            Return m_Result
        End Function

#End Region


#Region " WriteFgf "

        Public Function WriteFgf(ByVal geometry As IGeometry) As Byte()
            Return Me.FgfGeometryFactory.GetFgf(Me.Write(geometry))
        End Function

        Public Function WriteFgf(ByVal point As IPoint) As Byte()
            Return Me.FgfGeometryFactory.GetFgf(Me.Write(point))
        End Function

        Public Function WriteFgf(ByVal lineString As ILineString) As Byte()
            Return Me.FgfGeometryFactory.GetFgf(Me.Write(lineString))
        End Function

        Public Function WriteFgf(ByVal linearRing As ILinearRing) As Byte()
            Return Me.FgfGeometryFactory.GetFgf(Me.Write(linearRing))
        End Function

        Public Function WriteFgf(ByVal polygon As IPolygon) As Byte()
            Return Me.FgfGeometryFactory.GetFgf(Me.Write(polygon))
        End Function

#End Region

#Region " WriteWkb "

        Public Function WriteWkb(ByVal geometry As IGeometry) As Byte()
            Return Me.FgfGeometryFactory.GetWkb(Me.Write(geometry))
        End Function

        Public Function WriteWkb(ByVal point As IPoint) As Byte()
            Return Me.FgfGeometryFactory.GetWkb(Me.Write(point))
        End Function

        Public Function WriteWkb(ByVal lineString As ILineString) As Byte()
            Return Me.FgfGeometryFactory.GetWkb(Me.Write(lineString))
        End Function

        Public Function WriteWkb(ByVal linearRing As ILinearRing) As Byte()
            Return Me.FgfGeometryFactory.GetWkb(Me.Write(linearRing))
        End Function

        Public Function WriteWkb(ByVal polygon As IPolygon) As Byte()
            Return Me.FgfGeometryFactory.GetWkb(Me.Write(polygon))
        End Function

#End Region

#Region " Write "

        Public Function Write(ByVal geometry As IGeometry) As OSGeo.FDO.Geometry.IGeometry
            If Not geometry.IsEmpty Then
                If TypeOf geometry Is Point Then
                    Dim m_Geometry As Point = CType(geometry, Point)
                    Return Me.Write(m_Geometry)
                End If

                If TypeOf geometry Is LineString Then
                    Dim m_Geometry As LineString = CType(geometry, LineString)
                    Return Me.Write(m_Geometry)
                End If

                If TypeOf geometry Is LinearRing Then
                    Dim m_Geometry As LinearRing = CType(geometry, LinearRing)
                    Return Me.Write(m_Geometry)
                End If

                If TypeOf geometry Is Polygon Then
                    Dim m_Geometry As Polygon = CType(geometry, Polygon)
                    Return Me.Write(m_Geometry)
                End If

                Throw New ArgumentException("Conversion from " + geometry.GeometryType + " to OSGeo.FDO.Geometry.IGeometry is not supported.")
            Else
                Throw New ArgumentException("Geometry " + geometry.GeometryType + " is empty.")
            End If

            Return Nothing
        End Function

        Public Function Write(ByVal point As IPoint) As OSGeo.FDO.Geometry.IPoint
            Dim m_OrdinateSequence As OrdinateSequence = Me.GetOrdinateSequence(point)
            Return Me.FgfGeometryFactory.CreatePoint(m_OrdinateSequence.Dimensionality, m_OrdinateSequence.Ordinates)
        End Function

        Public Function Write(ByVal lineString As ILineString) As OSGeo.FDO.Geometry.ILineString
            Dim m_OrdinateSequence As OrdinateSequence = Me.GetOrdinateSequence(lineString)
            Return Me.FgfGeometryFactory.CreateLineString(m_OrdinateSequence.Dimensionality, m_OrdinateSequence.OrdinateNumber, m_OrdinateSequence.Ordinates)
        End Function

        Public Function Write(ByVal linearRing As ILinearRing) As OSGeo.FDO.Geometry.ILinearRing
            Dim m_OrdinateSequence As OrdinateSequence = Me.GetOrdinateSequence(linearRing)
            Return Me.FgfGeometryFactory.CreateLinearRing(m_OrdinateSequence.Dimensionality, m_OrdinateSequence.OrdinateNumber, m_OrdinateSequence.Ordinates)
        End Function

        Public Function Write(ByVal polygon As IPolygon) As OSGeo.FDO.Geometry.IPolygon
            Dim m_ExteriorRing As OSGeo.FDO.Geometry.ILinearRing = Me.Write(polygon.Shell)

            Dim m_InteriorRingCollection As New OSGeo.FDO.Geometry.LinearRingCollection
            For Each m_Hole As LinearRing In polygon.Holes
                Dim m_InteriorRing As OSGeo.FDO.Geometry.ILinearRing = Me.Write(m_Hole)
                m_InteriorRingCollection.Add(m_InteriorRing)
            Next

            Return Me.FgfGeometryFactory.CreatePolygon(m_ExteriorRing, m_InteriorRingCollection)
        End Function

#End Region

    End Class

End Namespace
