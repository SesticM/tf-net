Imports Topology.IO
Imports Topology.Geometries

Namespace IO

    Public Class FgfReader
        Inherits GeometryReaderWriter

        Private m_ArcTesselationMaxSpacing As Double
        Private m_ArcTesselationMaxOffset As Double

        Sub New()
            MyBase.New()
        End Sub

        Sub New(ByVal geometryFactory As Geometries.GeometryFactory)
            MyBase.New(geometryFactory)
        End Sub

#Region " ArcTesselationMaxSpacing "

        Public Property ArcTesselationMaxSpacing() As Double
            Get
                Return m_ArcTesselationMaxSpacing
            End Get
            Set(ByVal value As Double)
                m_ArcTesselationMaxSpacing = value
            End Set
        End Property

#End Region

#Region " ArcTesselationMaxOffset "

        Public Property ArcTesselationMaxOffset() As Double
            Get
                Return m_ArcTesselationMaxOffset
            End Get
            Set(ByVal value As Double)
                m_ArcTesselationMaxOffset = value
            End Set
        End Property

#End Region

#Region " Read "

        Public Function Read(ByVal point As OSGeo.FDO.Geometry.IPoint) As Point
            If point.Dimensionality = 0 Then
                Return New Point(point.Position.X, point.Position.Y)
            Else
                Return New Point(point.Position.X, point.Position.Y, point.Position.Z)
            End If
        End Function

        Public Function Read(ByVal lineString As OSGeo.FDO.Geometry.ILineString) As LineString
            Dim m_Coordinates As New List(Of Coordinate)

            If lineString.Dimensionality = 0 Then
                For Each m_Position As OSGeo.FDO.Geometry.IDirectPosition In lineString.Positions
                    m_Coordinates.Add(New Coordinate(m_Position.X, m_Position.Y))
                Next
            Else
                For Each m_Position As OSGeo.FDO.Geometry.IDirectPosition In lineString.Positions
                    m_Coordinates.Add(New Coordinate(m_Position.X, m_Position.Y, m_Position.Z))
                Next
            End If

            Return New LineString(m_Coordinates.ToArray)
        End Function

        Public Function Read(ByVal linearRing As OSGeo.FDO.Geometry.ILinearRing) As LinearRing
            Dim m_Coordinates As New List(Of Coordinate)

            If linearRing.Dimensionality = 0 Then
                For Each m_Position As OSGeo.FDO.Geometry.IDirectPosition In linearRing.Positions
                    m_Coordinates.Add(New Coordinate(m_Position.X, m_Position.Y))
                Next
            Else
                For Each m_Position As OSGeo.FDO.Geometry.IDirectPosition In linearRing.Positions
                    m_Coordinates.Add(New Coordinate(m_Position.X, m_Position.Y, m_Position.Z))
                Next
            End If

            Return New LinearRing(m_Coordinates.ToArray)
        End Function

        Public Function Read(ByVal polygon As OSGeo.FDO.Geometry.IPolygon) As Polygon
            Dim m_Shell As LinearRing = Me.Read(polygon.ExteriorRing)

            Dim m_Holes As New List(Of LinearRing)
            For i As Integer = 0 To polygon.InteriorRingCount - 1
                Dim m_InteriorRing As OSGeo.FDO.Geometry.ILinearRing = polygon.GetInteriorRing(i)
                m_Holes.Add(Me.Read(m_InteriorRing))
            Next

            Return New Polygon(m_Shell, m_Holes.ToArray)
        End Function

        'Public Function Read(ByVal circularArcSegment As OSGeo.FDO.Geometry.ICircularArcSegment) As LineString
        '    Dim m_Geometry As OSGeo.FDO.Geometry.IGeometry = OSGeo.FDO.Spatial.SpatialUtility.TesselateCurve(circularArcSegment)
        '    Select Case m_Geometry.DerivedType
        '        Case OSGeo.FDO.Common.GeometryType.GeometryType_LineString
        '            Return Me.Read(CType(m_Geometry, OSGeo.FDO.Geometry.ILineString))
        '        Case Else
        '            Return LineString.Empty
        '    End Select
        'End Function

#End Region

    End Class

End Namespace

