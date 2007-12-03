Imports Topology.IO
Imports Topology.Geometries
Imports OSGeo.MapGuide

''' <summary>
''' Reads MapGuide Server geometries and creates geometric representation of the features
''' based on JTS model. Curve-based geometries are currently not supported.
''' </summary>
''' <remarks></remarks>
Public Class MgReader
    Inherits GeometryReader

#Region " CTOR "

    Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="MgReader"/> class, using
    ''' user-supplied <see cref="GeometryFactory"/> for geometry processing. 
    ''' </summary>
    ''' <param name="factory">A <see cref="GeometryFactory"/>.</param>
    ''' <remarks></remarks>
    Sub New(ByVal factory As IGeometryFactory)
        MyBase.New(factory)
    End Sub

#End Region


#Region " ReadCoordinate "

    ''' <summary>
    ''' Returns <see cref="Coordinate"/> structure converted from <see cref="MgCoordinate"/>.
    ''' Supported dimensionalities are <c>XY</c> and <c>XYZ</c> only.
    ''' </summary>
    ''' <param name="coordinate">A <see cref="MgCoordinate"/> structure.</param>
    ''' <returns>A <see cref="Coordinate"/> structure.</returns>
    ''' <remarks></remarks>
    Public Function ReadCoordinate(ByVal coordinate As MgCoordinate) As ICoordinate
        If coordinate.Dimension = 0 Then
            Return New Coordinate(coordinate.X, coordinate.Y)
        Else
            Return New Coordinate(coordinate.X, coordinate.Y, coordinate.Z)
        End If
    End Function

#End Region

#Region " ReadPoint "

    ''' <summary>
    ''' Returns <see cref="Point"/> geometry converted from <see cref="MgPoint"/>.
    ''' </summary>
    ''' <param name="point">A <see cref="MgPoint"/> geometry.</param>
    ''' <returns>A <see cref="Point"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadPoint(ByVal point As MgPoint) As IPoint
        Dim coordinate As ICoordinate = Me.ReadCoordinate(point.Coordinate)
        Return Me.GeometryFactory.CreatePoint(coordinate)
    End Function

#End Region

#Region " ReadLineString "

    ''' <summary>
    ''' Returns <see cref="LineString"/> geometry converted from <see cref="MgLineString"/>.
    ''' </summary>
    ''' <param name="lineString">A <see cref="MgLineString"/> geometry.</param>
    ''' <returns>A <see cref="LineString"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadLineString(ByVal lineString As MgLineString) As ILineString
        Dim coordinates As New CoordinateList

        Dim iterator As MgCoordinateIterator = lineString.GetCoordinates
        Do While iterator.MoveNext
            coordinates.Add(Me.ReadCoordinate(iterator.GetCurrent), Me.AllowRepeatedCoordinates)
        Loop

        Return Me.GeometryFactory.CreateLineString(coordinates.ToCoordinateArray)
    End Function

#End Region

#Region " ReadLinearRing "

    ''' <summary>
    ''' Returns <see cref="LinearRing"/> geometry converted from <see cref="MgLinearRing"/>.
    ''' </summary>
    ''' <param name="linearRing">A <see cref="MgLinearRing"/> geometry.</param>
    ''' <returns>A <see cref="LinearRing"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadLinearRing(ByVal linearRing As MgLinearRing) As ILinearRing
        Dim coordinates As New CoordinateList

        Dim iterator As MgCoordinateIterator = linearRing.GetCoordinates
        Do While iterator.MoveNext
            coordinates.Add(Me.ReadCoordinate(iterator.GetCurrent), Me.AllowRepeatedCoordinates)
        Loop

        Return Me.GeometryFactory.CreateLinearRing(coordinates.ToCoordinateArray)
    End Function

#End Region

#Region " ReadPolygon "

    ''' <summary>
    ''' Returns <see cref="Polygon"/> geometry converted from <see cref="MgPolygon"/>.
    ''' </summary>
    ''' <param name="polygon">A <see cref="MgPolygon"/> geometry.</param>
    ''' <returns>A <see cref="Polygon"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadPolygon(ByVal polygon As MgPolygon) As IPolygon
        Dim shell As LinearRing = Me.ReadLinearRing(polygon.GetExteriorRing)
        Dim holes As New List(Of LinearRing)

        If polygon.GetInteriorRingCount > 0 Then
            For i As Integer = 0 To polygon.GetInteriorRingCount - 1
                Dim hole As LinearRing = Me.ReadLinearRing(polygon.GetInteriorRing(i))
                holes.Add(hole)
            Next
        End If

        Return Me.GeometryFactory.CreatePolygon(shell, holes.ToArray)
    End Function

#End Region

#Region " ReadGeometry "

    ''' <summary>
    ''' Returns <see cref="Geometry"/> object converted from <see cref="MgGeometry"/>.
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
    '''       <term>A polygon with sides formed from linear rings.
    '''             Linear ring is a line string representation where line string's first and last
    '''             point in the coordinate sequence must be equal. Either orientation of the ring is allowed.
    '''             A valid ring must not self-intersect.</term>
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
    ''' <param name="geometry">A <see cref="MgGeometry"/> object.</param>
    ''' <returns>A <see cref="Geometry"/> object.</returns>
    ''' <remarks></remarks>
    Public Function ReadGeometry(ByVal geometry As MgGeometry) As IGeometry
        Select Case geometry.GeometryType
            Case 1      'Point
                Return Me.ReadPoint(CType(geometry, MgPoint))
            Case 2      'LineString
                Return Me.ReadLineString(CType(geometry, MgLineString))
            Case 3      'Polygon
                Return Me.ReadPolygon(CType(geometry, MgPolygon))
            Case 4      'MultiPoint
                Return Me.ReadMultiPoint(CType(geometry, MgMultiPoint))
            Case 5      'MultiLineString
                Return Me.ReadMultiLineString(CType(geometry, MgMultiLineString))
            Case 6      'MultiPolygon
                Return Me.ReadMultiPolygon(CType(geometry, MgMultiPolygon))
            Case Else
                Throw New ArgumentException(String.Format("Reading of geometry type {0} is not supported.", geometry.ToString))
        End Select

        Return Nothing
    End Function

    ''' <summary>
    ''' Returns <see cref="Geometry"/> object given <see cref="MgFeatureReader"/> and a
    ''' name of the feature class geometry property.
    ''' </summary>
    ''' <param name="featureReader">A <see cref="MgFeatureReader"/> object.</param>
    ''' <param name="geometryPropertyName">Name of the feature class geometry property (i.e. "Geometry").</param>
    ''' <returns>A <see cref="Geometry"/> object.</returns>
    ''' <remarks></remarks>
    Public Function ReadGeometry(ByRef featureReader As MgFeatureReader, ByVal geometryPropertyName As String) As IGeometry
        Dim reader As MgByteReader = featureReader.GetGeometry(geometryPropertyName)
        Dim agfReader As MgAgfReaderWriter = New MgAgfReaderWriter
        Dim geometry As MgGeometry = agfReader.Read(reader)
        Return Me.ReadGeometry(geometry)
    End Function

    ''' <summary>
    ''' Returns <see cref="Geometry"/> object given <see cref="MgFeatureReader"/>,
    ''' when feature class geometry property's name is <c>Geometry</c>.
    ''' </summary>
    ''' <param name="featureReader">A <see cref="MgFeatureReader"/> object.</param>
    ''' <returns>A <see cref="Geometry"/> object.</returns>
    ''' <remarks></remarks>
    Public Function ReadGeometry(ByRef featureReader As MgFeatureReader) As IGeometry
        Return Me.ReadGeometry(featureReader, "Geometry")
    End Function

#End Region


#Region " ReadMultiPoint "

    ''' <summary>
    ''' Returns <see cref="MultiPoint"/> geometry converted from <see cref="MgMultiPoint"/>.
    ''' </summary>
    ''' <param name="multiPoint">A <see cref="MgMultiPoint"/> geometry.</param>
    ''' <returns>A <see cref="MultiPoint"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadMultiPoint(ByVal multiPoint As MgMultiPoint) As IMultiPoint
        Dim points As New List(Of Point)

        For i As Integer = 0 To multiPoint.Count - 1
            points.Add(Me.ReadPoint(multiPoint.GetPoint(i)))
        Next

        Return Me.GeometryFactory.CreateMultiPoint(points.ToArray)
    End Function

#End Region

#Region " ReadMultiLineString "

    ''' <summary>
    ''' Returns <see cref="MultiLineString"/> geometry converted from <see cref="MgMultiLineString"/>.
    ''' </summary>
    ''' <param name="multiLineString">A <see cref="MgMultiLineString"/> geometry.</param>
    ''' <returns>A <see cref="MultiLineString"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadMultiLineString(ByVal multiLineString As MgMultiLineString) As MultiLineString
        Dim lineStrings As New List(Of LineString)

        For i As Integer = 0 To multiLineString.Count - 1
            lineStrings.Add(Me.ReadLineString(multiLineString.GetLineString(i)))
        Next

        Return Me.GeometryFactory.CreateMultiLineString(lineStrings.ToArray)
    End Function

#End Region

#Region " ReadMultiPolygon "

    ''' <summary>
    ''' Returns <see cref="MultiPolygon"/> geometry converted from <see cref="MgMultiPolygon"/>.
    ''' </summary>
    ''' <param name="multiPolygon">A <see cref="MgMultiPolygon"/> geometry.</param>
    ''' <returns>A <see cref="MultiPolygon"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadMultiPolygon(ByVal multiPolygon As MgMultiPolygon) As MultiPolygon
        Dim polygons As New List(Of Polygon)

        For i As Integer = 0 To multiPolygon.Count - 1
            polygons.Add(Me.ReadPolygon(multiPolygon.GetPolygon(i)))
        Next

        Return Me.GeometryFactory.CreateMultiPolygon(polygons.ToArray)
    End Function

#End Region


#Region " ReadCoordinateCollection "

    ''' <summary>
    ''' Returns array of <see cref="Coordinate"/>s converted from <see cref="MgCoordinateCollection"/>.
    ''' </summary>
    ''' <param name="coordinates">A <see cref="MgCoordinateCollection"/> collection.</param>
    ''' <returns>Array of <see cref="Coordinate"/> structures.</returns>
    ''' <remarks></remarks>
    Public Function ReadCoordinateCollection(ByVal coordinates As MgCoordinateCollection) As ICoordinate()
        Dim collection As New CoordinateList
        For Each coordinate As MgCoordinate In coordinates
            collection.Add(Me.ReadCoordinate(coordinate), Me.AllowRepeatedCoordinates)
        Next
        Return collection.ToCoordinateArray
    End Function

#End Region

#Region " ReadGeometryCollection "

    ''' <summary>
    ''' Returns <see cref="GeometryCollection"/> converted from <see cref="MgGeometryCollection"/>.
    ''' </summary>
    ''' <param name="geometries">A <see cref="MgGeometryCollection"/> collection.</param>
    ''' <returns>A <see cref="GeometryCollection"/> collection.</returns>
    ''' <remarks></remarks>
    Public Function ReadGeometryCollection(ByVal geometries As MgGeometryCollection) As GeometryCollection
        Dim collection As New List(Of IGeometry)

        For Each geometry As MgGeometry In geometries
            collection.Add(Me.ReadGeometry(geometry))
        Next

        Return Me.GeometryFactory.CreateGeometryCollection(collection.ToArray)
    End Function

    ''' <summary>
    ''' Returns <see cref="GeometryCollection"/> converted from <see cref="MgMultiGeometry"/> geometry.
    ''' </summary>
    ''' <param name="multiGeometry">A <see cref="MgMultiGeometry"/> geometry.</param>
    ''' <returns>A <see cref="GeometryCollection"/> collection.</returns>
    ''' <remarks></remarks>
    Public Function ReadGeometryCollection(ByVal multiGeometry As MgMultiGeometry) As GeometryCollection
        Dim geometries As New List(Of Geometries.Geometry)

        For i As Integer = 0 To multiGeometry.Count - 1
            geometries.Add(Me.ReadGeometry(multiGeometry.GetGeometry(i)))
        Next

        Return Me.GeometryFactory.CreateGeometryCollection(geometries.ToArray)
    End Function

#End Region


#Region " ReadLineSegment "

    ''' <summary>
    ''' Returns <see cref="LineSegment"/> geometry converted from <see cref="MgLinearSegment"/> geometry.
    ''' </summary>
    ''' <param name="linearSegment">A <see cref="MgLinearSegment"/> geometry.</param>
    ''' <returns>A <see cref="LineSegment"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadLineSegment(ByVal linearSegment As MgLinearSegment) As LineSegment
        Return New LineSegment(Me.ReadCoordinate(linearSegment.StartCoordinate), Me.ReadCoordinate(linearSegment.EndCoordinate))
    End Function

    ''' <summary>
    ''' Returns <see cref="LineSegment"/> geometry given start and end <see cref="MgCoordinate"/>s.
    ''' </summary>
    ''' <param name="coordinate0">Start <see cref="MgCoordinate"/> structure.</param>
    ''' <param name="coordinate1">End <see cref="MgCoordinate"/> structure.</param>
    ''' <returns>A <see cref="LineSegment"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadLineSegment(ByVal coordinate0 As MgCoordinate, ByVal coordinate1 As MgCoordinate) As LineSegment
        Return New LineSegment(Me.ReadCoordinate(coordinate0), Me.ReadCoordinate(coordinate1))
    End Function

    ''' <summary>
    ''' Returns <see cref="LineSegment"/> geometry given start and end <see cref="MgPoint"/>s.
    ''' </summary>
    ''' <param name="point0">Start <see cref="MgPoint"/> geometry.</param>
    ''' <param name="point1">End <see cref="MgPoint"/> geometry.</param>
    ''' <returns>A <see cref="LineSegment"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadLineSegment(ByVal point0 As MgPoint, ByVal point1 As MgPoint) As LineSegment
        Return New LineSegment(Me.ReadCoordinate(point0.Coordinate), Me.ReadCoordinate(point1.Coordinate))
    End Function

#End Region

#Region " ReadEnvelope "

    ''' <summary>
    ''' Returns <see cref="Envelope"/> structure converted from <see cref="MgEnvelope"/>.
    ''' </summary>
    ''' <param name="envelope">A <see cref="MgEnvelope"/> structure.</param>
    ''' <returns>An <see cref="Envelope"/> structure.</returns>
    ''' <remarks></remarks>
    Public Function ReadEnvelope(ByVal envelope As MgEnvelope) As Envelope
        Return New Envelope(Me.ReadCoordinate(envelope.LowerLeftCoordinate), Me.ReadCoordinate(envelope.UpperRightCoordinate))
    End Function

#End Region

End Class


