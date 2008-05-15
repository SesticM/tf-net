Imports Topology.IO
Imports Topology.Geometries

''' <summary>
''' Reads OSGeo Feature Data Objects (FDO) geometries and creates geometric representation
''' of the features based on JTS model. Curve-based geometries are currently not supported.
''' </summary>
''' <remarks></remarks>
Public Class FgfReader
    Inherits GeometryReader

    Private m_Factory As OSGeo.FDO.Geometry.FgfGeometryFactory

#Region " CTOR "

    Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="FgfReader"/> class, using
    ''' user-supplied <see cref="GeometryFactory"/> for geometry processing. 
    ''' </summary>
    ''' <param name="factory">A <see cref="GeometryFactory"/>.</param>
    ''' <remarks></remarks>
    Sub New(ByVal factory As IGeometryFactory)
        MyBase.New(factory)
    End Sub

#End Region

#Region " Factory "

    ''' <summary>
    ''' Returns current <see cref="OSGeo.FDO.Geometry.FgfGeometryFactory"/> used to build geometries.
    ''' </summary>
    ''' <value></value>
    ''' <returns>Current <see cref="OSGeo.FDO.Geometry.FgfGeometryFactory"/> instance.</returns>
    ''' <remarks>
    ''' If there's no <see cref="OSGeo.FDO.Geometry.FgfGeometryFactory"/> set within class constructor,
    ''' a <c>Default</c> factory will be automatically instantiated. Otherwise,
    ''' user-supplied <see cref="OSGeo.FDO.Geometry.FgfGeometryFactory"/> will be used during geometries
    ''' building process.
    ''' </remarks>
    Private ReadOnly Property Factory() As OSGeo.FDO.Geometry.FgfGeometryFactory
        Get
            If m_Factory Is Nothing Then
                m_Factory = New OSGeo.FDO.Geometry.FgfGeometryFactory
            End If
            Return m_Factory
        End Get
    End Property

#End Region

#Region " ReadCoordinate "

    ''' <summary>
    ''' Returns <see cref="Coordinate"/> structure converted from <see cref="OSGeo.FDO.Geometry.IDirectPosition"/>.
    ''' Supported dimensionalities are <c>XY</c> and <c>XYZ</c> only.
    ''' </summary>
    ''' <param name="directPosition">A <see cref="OSGeo.FDO.Geometry.IDirectPosition"/> structure.</param>
    ''' <returns>A <see cref="Coordinate"/> structure.</returns>
    ''' <remarks></remarks>
    Public Function ReadCoordinate(ByVal directPosition As OSGeo.FDO.Geometry.IDirectPosition) As ICoordinate
        If directPosition.Dimensionality = 0 Then
            Return New Coordinate(directPosition.X, directPosition.Y)
        Else
            Return New Coordinate(directPosition.X, directPosition.Y, directPosition.Z)
        End If
    End Function

#End Region

#Region " ReadPoint "

    ''' <summary>
    ''' Returns <see cref="Point"/> geometry converted from <see cref="OSGeo.FDO.Geometry.IPoint"/>.
    ''' </summary>
    ''' <param name="point">A <see cref="OSGeo.FDO.Geometry.IPoint"/> geometry.</param>
    ''' <returns>A <see cref="Point"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadPoint(ByVal point As OSGeo.FDO.Geometry.IPoint) As IPoint
        Dim coordinate As ICoordinate = Me.ReadCoordinate(point.Position)
        Return Me.GeometryFactory.CreatePoint(coordinate)
    End Function

#End Region

#Region " ReadLineString "

    ''' <summary>
    ''' Returns <see cref="LineString"/> geometry converted from <see cref="OSGeo.FDO.Geometry.ILineString"/>.
    ''' </summary>
    ''' <param name="lineString">A <see cref="OSGeo.FDO.Geometry.ILineString"/> geometry.</param>
    ''' <returns>A <see cref="LineString"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadLineString(ByVal lineString As OSGeo.FDO.Geometry.ILineString) As ILineString
        Dim coordinates As New CoordinateList

        For Each pos As OSGeo.FDO.Geometry.IDirectPosition In lineString.Positions
            coordinates.Add(Me.ReadCoordinate(pos), Me.AllowRepeatedCoordinates)
        Next

        Return Me.GeometryFactory.CreateLineString(coordinates.ToCoordinateArray)
    End Function

#End Region

#Region " ReadLinearRing "

    ''' <summary>
    ''' Returns <see cref="LinearRing"/> geometry converted from <see cref="OSGeo.FDO.Geometry.ILinearRing"/>.
    ''' </summary>
    ''' <param name="linearRing">A <see cref="OSGeo.FDO.Geometry.ILinearRing"/> geometry.</param>
    ''' <returns>A <see cref="LinearRing"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadLinearRing(ByVal linearRing As OSGeo.FDO.Geometry.ILinearRing) As ILinearRing
        Dim coordinates As New CoordinateList

        For Each pos As OSGeo.FDO.Geometry.IDirectPosition In linearRing.Positions
            coordinates.Add(Me.ReadCoordinate(pos), Me.AllowRepeatedCoordinates)
        Next

        Return Me.GeometryFactory.CreateLinearRing(coordinates.ToCoordinateArray)
    End Function

#End Region

#Region " ReadPolygon "

    ''' <summary>
    ''' Returns <see cref="Polygon"/> geometry converted from <see cref="OSGeo.FDO.Geometry.IPolygon"/>.
    ''' </summary>
    ''' <param name="polygon">A <see cref="OSGeo.FDO.Geometry.IPolygon"/> geometry.</param>
    ''' <returns>A <see cref="Polygon"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadPolygon(ByVal polygon As OSGeo.FDO.Geometry.IPolygon) As IPolygon
        Dim shell As LinearRing = Me.ReadLinearRing(polygon.ExteriorRing)
        Dim holes As New List(Of LinearRing)

        If polygon.InteriorRingCount > 0 Then
            For i As Integer = 0 To polygon.InteriorRingCount - 1
                Dim hole As LinearRing = Me.ReadLinearRing(polygon.GetInteriorRing(i))
                holes.Add(hole)
            Next
        End If

        Return Me.GeometryFactory.CreatePolygon(shell, holes.ToArray)
    End Function

#End Region

#Region " ReadGeometry "

    ''' <summary>
    ''' Returns <see cref="Geometry"/> object converted from <see cref="OSGeo.FDO.Geometry.IGeometry"/>.
    ''' <para>
    ''' Supported geometry types:
    ''' <list type="table">
    '''    <listheader>
    '''       <term>Geometry Type</term>
    '''       <term>Description</term>
    '''    </listheader>
    '''    <item>
    '''       <term><c>Point</c></term>
    '''       <term>A single point.</term>
    '''    </item>
    '''    <item>
    '''       <term><c>LineString</c></term>
    '''       <term>Series of connected line segments.</term>
    '''    </item>
    '''    <item>
    '''       <term><c>Polygon</c></term>
    '''       <term>A polygon with sides formed from linear rings.</term>
    '''    </item>
    '''    <item>
    '''       <term><c>MultiPoint</c></term>
    '''       <term>Collection of points.</term>
    '''    </item>
    '''    <item>
    '''       <term><c>MultiLineString</c></term>
    '''       <term>Collection of line strings.</term>
    '''    </item>
    '''    <item>
    '''       <term><c>MultiPolygon</c></term>
    '''       <term>Collection of polygons.</term>
    '''    </item>
    ''' </list>
    ''' </para>
    ''' </summary>
    ''' <param name="geometry">A <see cref="OSGeo.FDO.Geometry.IGeometry"/> object.</param>
    ''' <returns>A <see cref="Geometry"/> object.</returns>
    ''' <remarks></remarks>
    Public Function ReadGeometry(ByVal geometry As OSGeo.FDO.Geometry.IGeometry) As IGeometry
        Select Case geometry.DerivedType
            Case 1      'Point
                Return Me.ReadPoint(CType(geometry, OSGeo.FDO.Geometry.IPoint))
            Case 2      'LineString
                Return Me.ReadLineString(CType(geometry, OSGeo.FDO.Geometry.ILineString))
            Case 3      'Polygon
                Return Me.ReadPolygon(CType(geometry, OSGeo.FDO.Geometry.IPolygon))
            Case 4      'MultiPoint
                Return Me.ReadMultiPoint(CType(geometry, OSGeo.FDO.Geometry.IMultiPoint))
            Case 5      'MultiLineString
                Return Me.ReadMultiLineString(CType(geometry, OSGeo.FDO.Geometry.IMultiLineString))
            Case 6      'MultiPolygon
                Return Me.ReadMultiPolygon(CType(geometry, OSGeo.FDO.Geometry.IMultiPolygon))
            Case Else
                Throw New ArgumentException(String.Format("Reading of geometry type {0} is not supported.", geometry.Text))
        End Select

        Return Nothing
    End Function

    Public Function ReadGeometry(ByVal stream As Byte()) As IGeometry
        Dim geom As OSGeo.FDO.Geometry.IGeometry = Me.Factory.CreateGeometryFromFgf(stream)
        Return Me.ReadGeometry(geom)
    End Function

    '''' <summary>
    '''' Returns <see cref="Geometry"/> object given <see cref="MgFeatureReader"/> and a
    '''' name of the feature class geometry property.
    '''' </summary>
    '''' <param name="featureReader">A <see cref="MgFeatureReader"/> object.</param>
    '''' <param name="geometryPropertyName">Name of the feature class geometry property (i.e. "Geometry").</param>
    '''' <returns>A <see cref="Geometry"/> object.</returns>
    '''' <remarks></remarks>
    'Public Function ReadGeometry(ByRef featureReader As OSGeo.FDO.Common.Io., ByVal geometryPropertyName As String) As IGeometry
    '    Dim reader As MgByteReader = featureReader.GetGeometry(geometryPropertyName)
    '    Dim agfReader As MgAgfReaderWriter = New MgAgfReaderWriter
    '    Dim geometry As OSGeo.FDO.Geometry.IGeometry = agfReader.Read(reader)
    '    Return Me.ReadGeometry(geometry)
    'End Function

#End Region


#Region " ReadMultiPoint "

    ''' <summary>
    ''' Returns <see cref="MultiPoint"/> geometry converted from <see cref="OSGeo.FDO.Geometry.IMultiPoint"/>.
    ''' </summary>
    ''' <param name="multiPoint">A <see cref="OSGeo.FDO.Geometry.IMultiPoint"/> geometry.</param>
    ''' <returns>A <see cref="MultiPoint"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadMultiPoint(ByVal multiPoint As OSGeo.FDO.Geometry.IMultiPoint) As IMultiPoint
        Dim points As New List(Of Point)

        For i As Integer = 0 To multiPoint.Count - 1
            points.Add(Me.ReadPoint(multiPoint.Item(i)))
        Next

        Return Me.GeometryFactory.CreateMultiPoint(points.ToArray)
    End Function

#End Region

#Region " ReadMultiLineString "

    ''' <summary>
    ''' Returns <see cref="MultiLineString"/> geometry converted from <see cref="OSGeo.FDO.Geometry.IMultiLineString"/>.
    ''' </summary>
    ''' <param name="multiLineString">A <see cref="OSGeo.FDO.Geometry.IMultiLineString"/> geometry.</param>
    ''' <returns>A <see cref="MultiLineString"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadMultiLineString(ByVal multiLineString As OSGeo.FDO.Geometry.IMultiLineString) As MultiLineString
        Dim lineStrings As New List(Of LineString)

        For i As Integer = 0 To multiLineString.Count - 1
            lineStrings.Add(Me.ReadLineString(multiLineString.Item(i)))
        Next

        Return Me.GeometryFactory.CreateMultiLineString(lineStrings.ToArray)
    End Function

#End Region

#Region " ReadMultiPolygon "

    ''' <summary>
    ''' Returns <see cref="MultiPolygon"/> geometry converted from <see cref="OSGeo.FDO.Geometry.IMultiPolygon"/>.
    ''' </summary>
    ''' <param name="multiPolygon">A <see cref="OSGeo.FDO.Geometry.IMultiPolygon"/> geometry.</param>
    ''' <returns>A <see cref="MultiPolygon"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadMultiPolygon(ByVal multiPolygon As OSGeo.FDO.Geometry.IMultiPolygon) As MultiPolygon
        Dim polygons As New List(Of Polygon)

        For i As Integer = 0 To multiPolygon.Count - 1
            polygons.Add(Me.ReadPolygon(multiPolygon.Item(i)))
        Next

        Return Me.GeometryFactory.CreateMultiPolygon(polygons.ToArray)
    End Function

#End Region


#Region " ReadCoordinateCollection "

    ''' <summary>
    ''' Returns array of <see cref="Coordinate"/>s converted from <see cref="OSGeo.FDO.Geometry.DirectPositionCollection"/>.
    ''' </summary>
    ''' <param name="directPositions">A <see cref="OSGeo.FDO.Geometry.DirectPositionCollection"/> collection.</param>
    ''' <returns>Array of <see cref="Coordinate"/> structures.</returns>
    ''' <remarks></remarks>
    Public Function ReadCoordinateCollection(ByVal directPositions As OSGeo.FDO.Geometry.DirectPositionCollection) As ICoordinate()
        Dim collection As New CoordinateList
        For Each pos As OSGeo.FDO.Geometry.IDirectPosition In directPositions
            collection.Add(Me.ReadCoordinate(pos), Me.AllowRepeatedCoordinates)
        Next
        Return collection.ToCoordinateArray
    End Function

#End Region

#Region " ReadGeometryCollection "

    ''' <summary>
    ''' Returns <see cref="GeometryCollection"/> converted from <see cref="OSGeo.FDO.Geometry.GeometryCollection"/>.
    ''' </summary>
    ''' <param name="geometries">A <see cref="OSGeo.FDO.Geometry.GeometryCollection"/> collection.</param>
    ''' <returns>A <see cref="GeometryCollection"/> collection.</returns>
    ''' <remarks></remarks>
    Public Function ReadGeometryCollection(ByVal geometries As OSGeo.FDO.Geometry.GeometryCollection) As GeometryCollection
        Dim collection As New List(Of IGeometry)

        For Each geometry As OSGeo.FDO.Geometry.IGeometry In geometries
            collection.Add(Me.ReadGeometry(geometry))
        Next

        Return Me.GeometryFactory.CreateGeometryCollection(collection.ToArray)
    End Function

    ''' <summary>
    ''' Returns <see cref="GeometryCollection"/> converted from <see cref="OSGeo.FDO.Geometry.IMultiGeometry"/> geometry.
    ''' </summary>
    ''' <param name="multiGeometry">A <see cref="OSGeo.FDO.Geometry.IMultiGeometry"/> geometry.</param>
    ''' <returns>A <see cref="GeometryCollection"/> collection.</returns>
    ''' <remarks></remarks>
    Public Function ReadGeometryCollection(ByVal multiGeometry As OSGeo.FDO.Geometry.IMultiGeometry) As GeometryCollection
        Dim geometries As New List(Of Geometries.Geometry)

        For i As Integer = 0 To multiGeometry.Count - 1
            geometries.Add(Me.ReadGeometry(multiGeometry.Item(i)))
        Next

        Return Me.GeometryFactory.CreateGeometryCollection(geometries.ToArray)
    End Function

#End Region


#Region " ReadLineSegment "

    ''' <summary>
    ''' Returns <see cref="LineSegment"/> geometry converted from <see cref="OSGeo.FDO.Geometry.ILineStringSegment"/> geometry.
    ''' </summary>
    ''' <param name="lineStringSegment">A <see cref="OSGeo.FDO.Geometry.ILineStringSegment"/> geometry.</param>
    ''' <returns>A <see cref="LineSegment"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadLineSegment(ByVal lineStringSegment As OSGeo.FDO.Geometry.ILineStringSegment) As LineSegment
        Return New LineSegment(Me.ReadCoordinate(lineStringSegment.StartPosition), Me.ReadCoordinate(lineStringSegment.EndPosition))
    End Function

#End Region

#Region " ReadEnvelope "

    ''' <summary>
    ''' Returns <see cref="Envelope"/> structure converted from <see cref="OSGeo.FDO.Geometry.IEnvelope"/>.
    ''' </summary>
    ''' <param name="envelope">A <see cref="OSGeo.FDO.Geometry.IEnvelope"/> structure.</param>
    ''' <returns>An <see cref="Envelope"/> structure.</returns>
    ''' <remarks></remarks>
    Public Function ReadEnvelope(ByVal envelope As OSGeo.FDO.Geometry.IEnvelope) As IEnvelope
        Return New Envelope( _
            New Coordinate(envelope.MinX, envelope.MinY, envelope.MinZ), _
            New Coordinate(envelope.MaxX, envelope.MaxY, envelope.MaxZ))
    End Function

#End Region

    'Public Function Read(ByVal circularArcSegment As OSGeo.FDO.Geometry.ICircularArcSegment) As LineString
    '    Dim m_Geometry As OSGeo.FDO.Geometry.IGeometry = OSGeo.FDO.Spatial.SpatialUtility.TesselateCurve(circularArcSegment)
    '    Select Case m_Geometry.DerivedType
    '        Case OSGeo.FDO.Common.GeometryType.GeometryType_LineString
    '            Return Me.Read(CType(m_Geometry, OSGeo.FDO.Geometry.ILineString))
    '        Case Else
    '            Return LineString.Empty
    '    End Select
    'End Function

End Class


