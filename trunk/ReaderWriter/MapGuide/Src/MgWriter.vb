Imports Topology.IO
Imports Topology.Geometries
Imports OSGeo.MapGuide

''' <summary>
''' Reads features based on JTS model and creates their MapGuide Server representation.
''' </summary>
''' <remarks></remarks>
Public Class MgWriter
    Inherits GeometryWriter

    Private m_Factory As MgGeometryFactory

#Region " CTOR "

    Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="MgWriter"/> class, using
    ''' user-supplied <see cref="MgGeometryFactory"/> for geometry processing. 
    ''' </summary>
    ''' <param name="factory">A <see cref="MgGeometryFactory"/>.</param>
    ''' <remarks></remarks>
    Sub New(ByVal factory As MgGeometryFactory)
        MyBase.New()
        m_Factory = factory
    End Sub

#End Region

#Region " Factory "

    ''' <summary>
    ''' Returns current <see cref="MgGeometryFactory"/> used to build geometries.
    ''' <para>
    ''' It's actually reserved for possible future releases of <see cref="MgGeometryFactory"/>
    ''' that might implement user-defined precision model, or incorporate common coordinate
    ''' reference system (CRS) used when building geometries.
    ''' </para>
    ''' </summary>
    ''' <value></value>
    ''' <returns>Current <see cref="MgGeometryFactory"/> instance.</returns>
    ''' <remarks>
    ''' If there's no <see cref="MgGeometryFactory"/> set within class constructor,
    ''' a <c>Default</c> factory will be automatically instantiated. Otherwise,
    ''' user-supplied <see cref="MgGeometryFactory"/> will be used during geometries
    ''' building process.
    ''' </remarks>
    Private ReadOnly Property Factory() As MgGeometryFactory
        Get
            If m_Factory Is Nothing Then
                m_Factory = New MgGeometryFactory
            End If
            Return m_Factory
        End Get
    End Property

#End Region


#Region " WriteCoordinate "

    ''' <summary>
    ''' Returns <see cref="MgCoordinate"/> structure converted from <see cref="Coordinate"/> structure.
    ''' If <see cref="Coordinate"/> is two-dimensional, resulting <see cref="MgCoordinate"/> is
    ''' created using <c>CreateCoordinateXY()</c>, otherwise <c>CreateCoordinateXYZ()</c> is used.
    ''' </summary>
    ''' <param name="coordinate">A <see cref="Coordinate"/> structure.</param>
    ''' <returns>A <see cref="MgCoordinate"/> structure.</returns>
    ''' <remarks></remarks>
    Public Function WriteCoordinate(ByVal coordinate As ICoordinate) As MgCoordinate
        If Not Double.IsNaN(coordinate.Z) Then
            Return Me.Factory.CreateCoordinateXYZ(coordinate.X, coordinate.Y, coordinate.Z)
        Else
            Return Me.Factory.CreateCoordinateXY(coordinate.X, coordinate.Y)
        End If
    End Function

#End Region

#Region " WritePoint "

    ''' <summary>
    ''' Returns <see cref="MgPoint"/> geometry converted from <see cref="Point"/> geometry.
    ''' </summary>
    ''' <param name="point">A <see cref="Point"/> geometry.</param>
    ''' <returns>A <see cref="MgPoint"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function WritePoint(ByVal point As IPoint) As MgPoint
        Dim coordinate As MgCoordinate = Me.WriteCoordinate(point.Coordinate)
        Return Me.Factory.CreatePoint(coordinate)
    End Function

#End Region

#Region " WriteLineString "

    ''' <summary>
    ''' Returns <see cref="MgLineString"/> geometry converted from <see cref="LineString"/> geometry.
    ''' </summary>
    ''' <param name="lineString">A <see cref="LineString"/> geometry.</param>
    ''' <returns>A <see cref="MgLineString"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function WriteLineString(ByVal lineString As ILineString) As MgLineString
        Dim coordinates As MgCoordinateCollection = Me.WriteCoordinateCollection(lineString.Coordinates)
        Return Me.Factory.CreateLineString(coordinates)
    End Function

    ''' <summary>
    ''' Returns <see cref="MgLineString"/> geometry converted from <see cref="LinearRing"/> geometry.
    ''' </summary>
    ''' <param name="linearRing">A <see cref="LinearRing"/> geometry.</param>
    ''' <returns>A <see cref="MgLineString"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function WriteLineString(ByVal linearRing As ILinearRing) As MgLineString
        Dim coordinates As MgCoordinateCollection = Me.WriteCoordinateCollection(linearRing.Coordinates)
        Return Me.Factory.CreateLineString(coordinates)
    End Function

#End Region

#Region " WriteLinearRing "

    ''' <summary>
    ''' Returns <see cref="MgLinearRing"/> geometry converted from <see cref="LinearRing"/> geometry.
    ''' </summary>
    ''' <param name="linearRing">A <see cref="LinearRing"/> geometry.</param>
    ''' <returns>A <see cref="MgLinearRing"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function WriteLinearRing(ByVal linearRing As ILinearRing) As MgLinearRing
        Dim coordinates As MgCoordinateCollection = Me.WriteCoordinateCollection(linearRing.Coordinates)
        Return Me.Factory.CreateLinearRing(coordinates)
    End Function

#End Region

#Region " WritePolygon "

    ''' <summary>
    ''' Returns <see cref="MgPolygon"/> geometry converted from <see cref="Polygon"/> geometry.
    ''' </summary>
    ''' <param name="polygon">A <see cref="Polygon"/> geometry.</param>
    ''' <returns>A <see cref="MgPolygon"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function WritePolygon(ByVal polygon As IPolygon) As MgPolygon
        Dim outerRing As MgLinearRing = Me.WriteLinearRing(polygon.Shell)
        Dim innerRings As MgLinearRingCollection = Me.WriteLinearRingCollection(polygon.Holes)
        Return Me.Factory.CreatePolygon(outerRing, innerRings)
    End Function

#End Region

#Region " WriteGeometry "

    ''' <summary>
    ''' Returns <see cref="MgGeometry"/> converted from <see cref="Geometry"/>.
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
    '''    <item>
    '''       <term><c>MultiGeometry</c></term>
    '''       <term>Collection of simple geometry types.</term>
    '''    </item>
    ''' </list>
    ''' </para>
    ''' </summary>
    ''' <param name="geometry">A <see cref="Geometry"/> object.</param>
    ''' <returns>A <see cref="MgGeometry"/> object.</returns>
    ''' <remarks></remarks>
    Public Function WriteGeometry(ByVal geometry As IGeometry) As MgGeometry
        Select Case geometry.GeometryType
            Case "Point"
                Return Me.WritePoint(CType(geometry, IPoint))
            Case "MultiPoint"
                Return Me.WriteMultiPoint(CType(geometry, IMultiPoint))
            Case "LineString"
                Return Me.WriteLineString(CType(geometry, ILineString))
            Case "MultiLineString"
                Return Me.WriteMultiLineString(CType(geometry, IMultiLineString))
            Case "LinearRing"
                Return Me.WriteLineString(CType(geometry, ILinearRing))
            Case "Polygon"
                Return Me.WritePolygon(CType(geometry, IPolygon))
            Case "MultiPolygon"
                Return Me.WriteMultiPolygon(CType(geometry, IMultiPolygon))
            Case "MultiSurface"
                Return Me.WriteMultiGeometry(CType(geometry, IMultiSurface))
            Case Else
                Throw New ArgumentException(String.Format("Geometry conversion from {0} to MgGeometry is not supported.", geometry.GeometryType))
        End Select
        Return Nothing
    End Function

#End Region


#Region " WriteMultiPoint "

    ''' <summary>
    ''' Returns <see cref="MgMultiPoint"/> geometry converted from <see cref="MultiPoint"/> geometry.
    ''' </summary>
    ''' <param name="multiPoint">A <see cref="MultiPoint"/> geometry.</param>
    ''' <returns>A <see cref="MgMultiPoint"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function WriteMultiPoint(ByVal multiPoint As IMultiPoint) As MgMultiPoint
        Dim points As New List(Of IPoint)
        For Each geometry As IGeometry In multiPoint.Geometries
            points.Add(CType(geometry, IPoint))
        Next
        Return Me.Factory.CreateMultiPoint(Me.WritePointCollection(points.ToArray))
    End Function

#End Region

#Region " WriteMultiLineString "

    ''' <summary>
    ''' Returns <see cref="MgMultiLineString"/> geometry converted from <see cref="MultiLineString"/> geometry.
    ''' </summary>
    ''' <param name="multiLineString">A <see cref="MultiLineString"/> geometry.</param>
    ''' <returns>A <see cref="MgMultiLineString"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function WriteMultiLineString(ByVal multiLineString As IMultiLineString) As MgMultiLineString
        Dim lineStrings As New List(Of ILineString)
        For Each geometry As IGeometry In multiLineString.Geometries
            lineStrings.Add(CType(geometry, ILineString))
        Next
        Return Me.Factory.CreateMultiLineString(Me.WriteLineStringCollection(lineStrings.ToArray))
    End Function

#End Region

#Region " WriteMultiPolygon "

    ''' <summary>
    ''' Returns <see cref="MgMultiPolygon"/> geometry converted from <see cref="MultiPolygon"/> geometry.
    ''' </summary>
    ''' <param name="multiPolygon">A <see cref="MultiPolygon"/> geometry.</param>
    ''' <returns>A <see cref="MgMultiPolygon"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function WriteMultiPolygon(ByVal multiPolygon As IMultiPolygon) As MgMultiPolygon
        Dim polygons As New List(Of IPolygon)
        For Each geometry As IGeometry In multiPolygon.Geometries
            polygons.Add(CType(geometry, IPolygon))
        Next
        Return Me.Factory.CreateMultiPolygon(Me.WritePolygonCollection(polygons.ToArray))
    End Function

#End Region

#Region " WriteMultiGeometry "

    ''' <summary>
    ''' Returns <see cref="MgMultiGeometry"/> geometry converted from <see cref="IMultiSurface"/> geometry.
    ''' </summary>
    ''' <param name="multiSurface">A <see cref="IMultiSurface"/> geometry.</param>
    ''' <returns>A <see cref="MgMultiGeometry"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function WriteMultiGeometry(ByVal multiSurface As IMultiSurface) As MgMultiGeometry
        Dim geometries As New List(Of IGeometry)
        For Each geometry As IGeometry In multiSurface.Geometries
            geometries.Add(geometry)
        Next
        Return Me.Factory.CreateMultiGeometry(Me.WriteGeometryCollection(geometries.ToArray))
    End Function

#End Region


#Region " WriteCoordinateCollection "

    ''' <summary>
    ''' Returns <see cref="MgCoordinateCollection"/> converted from array of <see cref="Coordinate"/>s.
    ''' </summary>
    ''' <param name="coordinates">An array of <see cref="Coordinate"/>s.</param>
    ''' <returns>A <see cref="MgCoordinateCollection"/> collection.</returns>
    ''' <remarks></remarks>
    Public Function WriteCoordinateCollection(ByVal coordinates() As ICoordinate) As MgCoordinateCollection
        Dim collection As New MgCoordinateCollection
        For Each coordinate As ICoordinate In coordinates
            collection.Add(Me.WriteCoordinate(coordinate))
        Next
        Return collection
    End Function

#End Region

#Region " WritePointCollection "

    ''' <summary>
    ''' Returns <see cref="MgPointCollection"/> converted from array of <see cref="Point"/>s.
    ''' </summary>
    ''' <param name="points">An array of <see cref="Point"/>s.</param>
    ''' <returns>A <see cref="MgPointCollection"/> collection.</returns>
    ''' <remarks></remarks>
    Public Function WritePointCollection(ByVal points() As IPoint) As MgPointCollection
        Dim collection As New MgPointCollection
        For Each point As IPoint In points
            collection.Add(Me.WritePoint(point))
        Next
        Return collection
    End Function

#End Region

#Region " WriteLineStringCollection "

    ''' <summary>
    ''' Returns <see cref="MgLineStringCollection"/> converted from array of <see cref="LineString"/>s.
    ''' </summary>
    ''' <param name="lineStrings">An array of <see cref="LineString"/>s.</param>
    ''' <returns>A <see cref="MgLineStringCollection"/> collection.</returns>
    ''' <remarks></remarks>
    Public Function WriteLineStringCollection(ByVal lineStrings() As ILineString) As MgLineStringCollection
        Dim collection As New MgLineStringCollection
        For Each lineString As ILineString In lineStrings
            collection.Add(Me.WriteLineString(lineString))
        Next
        Return collection
    End Function

    ''' <summary>
    ''' Returns <see cref="MgLineStringCollection"/> converted from array of <see cref="LinearRing"/>s.
    ''' </summary>
    ''' <param name="linearRings">An array of <see cref="LinearRing"/>s.</param>
    ''' <returns>A <see cref="MgLineStringCollection"/> collection.</returns>
    ''' <remarks></remarks>
    Public Function WriteLineStringCollection(ByVal linearRings() As ILinearRing) As MgLineStringCollection
        Dim collection As New MgLineStringCollection
        For Each linearRing As ILinearRing In linearRings
            collection.Add(Me.WriteLineString(linearRing))
        Next
        Return collection
    End Function

#End Region

#Region " WriteLinearRingCollection "

    ''' <summary>
    ''' Returns <see cref="MgLinearRingCollection"/> converted from array of <see cref="LinearRing"/>s.
    ''' </summary>
    ''' <param name="linearRings">An array of <see cref="LinearRing"/>s.</param>
    ''' <returns>A <see cref="MgLinearRingCollection"/> collection.</returns>
    ''' <remarks></remarks>
    Public Function WriteLinearRingCollection(ByVal linearRings() As ILinearRing) As MgLinearRingCollection
        Dim collection As New MgLinearRingCollection
        For Each linearRing As ILinearRing In linearRings
            collection.Add(Me.WriteLinearRing(linearRing))
        Next
        Return collection
    End Function

#End Region

#Region " WritePolygonCollection "

    ''' <summary>
    ''' Returns <see cref="MgPolygonCollection"/> converted from array of <see cref="Polygon"/>s.
    ''' </summary>
    ''' <param name="polygons">An array of <see cref="Polygon"/>s.</param>
    ''' <returns>A <see cref="MgPolygonCollection"/> collection.</returns>
    ''' <remarks></remarks>
    Public Function WritePolygonCollection(ByVal polygons() As IPolygon) As MgPolygonCollection
        Dim collection As New MgPolygonCollection
        For Each polygon As IPolygon In polygons
            collection.Add(Me.WritePolygon(polygon))
        Next
        Return collection
    End Function

#End Region

#Region " WriteGeometryCollection "

    ''' <summary>
    ''' Returns <see cref="MgGeometryCollection"/> converted from <see cref="GeometryCollection"/>.
    ''' </summary>
    ''' <param name="geometries">A <see cref="GeometryCollection"/> collection.</param>
    ''' <returns>A <see cref="MgGeometryCollection"/> collection.</returns>
    ''' <remarks></remarks>
    Public Function WriteGeometryCollection(ByVal geometries As IGeometryCollection) As MgGeometryCollection
        Dim collection As New MgGeometryCollection
        For Each geometry As IGeometry In geometries
            collection.Add(Me.WriteGeometry(geometry))
        Next
        Return collection
    End Function

#End Region


#Region " WriteLinearSegment "

    ''' <summary>
    ''' Returns <see cref="MgLinearSegment"/> geometry converted from <see cref="LineSegment"/> geometry.
    ''' </summary>
    ''' <param name="lineSegment">A <see cref="LineSegment"/> geometry.</param>
    ''' <returns>A <see cref="MgLinearSegment"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function WriteLinearSegment(ByVal lineSegment As LineSegment) As MgLinearSegment
        Dim coordinates As New MgCoordinateCollection
        coordinates.Add(Me.WriteCoordinate(lineSegment.P0))
        coordinates.Add(Me.WriteCoordinate(lineSegment.P1))
        Return Me.Factory.CreateLinearSegment(coordinates)
    End Function

#End Region

#Region " WriteEnvelope "

    ''' <summary>
    ''' Returns <see cref="MgEnvelope"/> structure converted from <see cref="Envelope"/> structure.
    ''' </summary>
    ''' <param name="envelope">An <see cref="Envelope"/> structure.</param>
    ''' <returns>A <see cref="MgEnvelope"/> structure.</returns>
    ''' <remarks></remarks>
    Public Function WriteEnvelope(ByVal envelope As IEnvelope) As MgEnvelope
        Return New MgEnvelope(envelope.MinX, envelope.MaxY, envelope.MaxX, envelope.MaxY)
    End Function

#End Region

End Class

