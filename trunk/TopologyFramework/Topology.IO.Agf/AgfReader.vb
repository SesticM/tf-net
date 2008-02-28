Imports Topology.IO
Imports Topology.Geometries

''' <summary>
''' Reads Autodesk Feature Data Objects (FDO) geometries and creates geometric representation
''' of the features based on JTS model. Curve-based geometries are currently not supported.
''' </summary>
''' <remarks></remarks>
Public Class AgfReader
    Inherits GeometryReader

#Region " CTOR "

    Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="AgfReader"/> class, using
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
    ''' Returns <see cref="Coordinate"/> structure converted from <see cref="Autodesk.Gis.Geometry.IDirectPosition"/>.
    ''' Supported dimensionalities are <c>XY</c> and <c>XYZ</c> only.
    ''' </summary>
    ''' <param name="directPosition">A <see cref="Autodesk.Gis.Geometry.IDirectPosition"/> structure.</param>
    ''' <returns>A <see cref="Coordinate"/> structure.</returns>
    ''' <remarks></remarks>
    Public Function ReadCoordinate(ByVal directPosition As Autodesk.Gis.Geometry.IDirectPosition) As ICoordinate
        If directPosition.Dimensionality = 0 Then
            Return New Coordinate(directPosition.X, directPosition.Y)
        Else
            Return New Coordinate(directPosition.X, directPosition.Y, directPosition.Z)
        End If
    End Function

#End Region

#Region " ReadPoint "

    ''' <summary>
    ''' Returns <see cref="Point"/> geometry converted from <see cref="Autodesk.Gis.Geometry.IPoint"/>.
    ''' </summary>
    ''' <param name="point">A <see cref="Autodesk.Gis.Geometry.IPoint"/> geometry.</param>
    ''' <returns>A <see cref="Point"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadPoint(ByVal point As Autodesk.Gis.Geometry.IPoint) As IPoint
        Dim coordinate As ICoordinate = Me.ReadCoordinate(point.Position)
        Return Me.GeometryFactory.CreatePoint(coordinate)
    End Function

#End Region

#Region " ReadLineString "

    ''' <summary>
    ''' Returns <see cref="LineString"/> geometry converted from <see cref="Autodesk.Gis.Geometry.ILineString"/>.
    ''' </summary>
    ''' <param name="lineString">A <see cref="Autodesk.Gis.Geometry.ILineString"/> geometry.</param>
    ''' <returns>A <see cref="LineString"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadLineString(ByVal lineString As Autodesk.Gis.Geometry.ILineString) As ILineString
        Dim coordinates As New CoordinateList

        For Each pos As Autodesk.Gis.Geometry.IDirectPosition In lineString.Positions
            coordinates.Add(Me.ReadCoordinate(pos), Me.AllowRepeatedCoordinates)
        Next

        Return Me.GeometryFactory.CreateLineString(coordinates.ToCoordinateArray)
    End Function

#End Region

#Region " ReadLinearRing "

    ''' <summary>
    ''' Returns <see cref="LinearRing"/> geometry converted from <see cref="Autodesk.Gis.Geometry.ILinearRing"/>.
    ''' </summary>
    ''' <param name="linearRing">A <see cref="Autodesk.Gis.Geometry.ILinearRing"/> geometry.</param>
    ''' <returns>A <see cref="LinearRing"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadLinearRing(ByVal linearRing As Autodesk.Gis.Geometry.ILinearRing) As ILinearRing
        Dim coordinates As New CoordinateList

        For Each pos As Autodesk.Gis.Geometry.IDirectPosition In linearRing.Positions
            coordinates.Add(Me.ReadCoordinate(pos), Me.AllowRepeatedCoordinates)
        Next

        Return Me.GeometryFactory.CreateLinearRing(coordinates.ToCoordinateArray)
    End Function

#End Region

#Region " ReadPolygon "

    ''' <summary>
    ''' Returns <see cref="Polygon"/> geometry converted from <see cref="Autodesk.Gis.Geometry.IPolygon"/>.
    ''' </summary>
    ''' <param name="polygon">A <see cref="Autodesk.Gis.Geometry.IPolygon"/> geometry.</param>
    ''' <returns>A <see cref="Polygon"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadPolygon(ByVal polygon As Autodesk.Gis.Geometry.IPolygon) As IPolygon
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
    ''' Returns <see cref="Geometry"/> object converted from <see cref="Autodesk.Gis.Geometry.IGeometry"/>.
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
    ''' <param name="geometry">A <see cref="Autodesk.Gis.Geometry.IGeometry"/> object.</param>
    ''' <returns>A <see cref="Geometry"/> object.</returns>
    ''' <remarks></remarks>
    Public Function ReadGeometry(ByVal geometry As Autodesk.Gis.Geometry.IGeometry) As IGeometry
        Select Case geometry.DerivedType
            Case 1      'Point
                Return Me.ReadPoint(CType(geometry, Autodesk.Gis.Geometry.IPoint))
            Case 2      'LineString
                Return Me.ReadLineString(CType(geometry, Autodesk.Gis.Geometry.ILineString))
            Case 3      'Polygon
                Return Me.ReadPolygon(CType(geometry, Autodesk.Gis.Geometry.IPolygon))
            Case 4      'MultiPoint
                Return Me.ReadMultiPoint(CType(geometry, Autodesk.Gis.Geometry.IMultiPoint))
            Case 5      'MultiLineString
                Return Me.ReadMultiLineString(CType(geometry, Autodesk.Gis.Geometry.IMultiLineString))
            Case 6      'MultiPolygon
                Return Me.ReadMultiPolygon(CType(geometry, Autodesk.Gis.Geometry.IMultiPolygon))
            Case Else
                Throw New ArgumentException(String.Format("Reading of geometry type {0} is not supported.", geometry.Text))
        End Select

        Return Nothing
    End Function

    '''' <summary>
    '''' Returns <see cref="Geometry"/> object given <see cref="MgFeatureReader"/> and a
    '''' name of the feature class geometry property.
    '''' </summary>
    '''' <param name="featureReader">A <see cref="MgFeatureReader"/> object.</param>
    '''' <param name="geometryPropertyName">Name of the feature class geometry property (i.e. "Geometry").</param>
    '''' <returns>A <see cref="Geometry"/> object.</returns>
    '''' <remarks></remarks>
    'Public Function ReadGeometry(ByRef featureReader As Autodesk.Gis.Common.Io., ByVal geometryPropertyName As String) As IGeometry
    '    Dim reader As MgByteReader = featureReader.GetGeometry(geometryPropertyName)
    '    Dim agfReader As MgAgfReaderWriter = New MgAgfReaderWriter
    '    Dim geometry As Autodesk.Gis.Geometry.IGeometry = agfReader.Read(reader)
    '    Return Me.ReadGeometry(geometry)
    'End Function

#End Region


#Region " ReadMultiPoint "

    ''' <summary>
    ''' Returns <see cref="MultiPoint"/> geometry converted from <see cref="Autodesk.Gis.Geometry.IMultiPoint"/>.
    ''' </summary>
    ''' <param name="multiPoint">A <see cref="Autodesk.Gis.Geometry.IMultiPoint"/> geometry.</param>
    ''' <returns>A <see cref="MultiPoint"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadMultiPoint(ByVal multiPoint As Autodesk.Gis.Geometry.IMultiPoint) As IMultiPoint
        Dim points As New List(Of Point)

        For i As Integer = 0 To multiPoint.Count - 1
            points.Add(Me.ReadPoint(multiPoint.Item(i)))
        Next

        Return Me.GeometryFactory.CreateMultiPoint(points.ToArray)
    End Function

#End Region

#Region " ReadMultiLineString "

    ''' <summary>
    ''' Returns <see cref="MultiLineString"/> geometry converted from <see cref="Autodesk.Gis.Geometry.IMultiLineString"/>.
    ''' </summary>
    ''' <param name="multiLineString">A <see cref="Autodesk.Gis.Geometry.IMultiLineString"/> geometry.</param>
    ''' <returns>A <see cref="MultiLineString"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadMultiLineString(ByVal multiLineString As Autodesk.Gis.Geometry.IMultiLineString) As MultiLineString
        Dim lineStrings As New List(Of LineString)

        For i As Integer = 0 To multiLineString.Count - 1
            lineStrings.Add(Me.ReadLineString(multiLineString.Item(i)))
        Next

        Return Me.GeometryFactory.CreateMultiLineString(lineStrings.ToArray)
    End Function

#End Region

#Region " ReadMultiPolygon "

    ''' <summary>
    ''' Returns <see cref="MultiPolygon"/> geometry converted from <see cref="Autodesk.Gis.Geometry.IMultiPolygon"/>.
    ''' </summary>
    ''' <param name="multiPolygon">A <see cref="Autodesk.Gis.Geometry.IMultiPolygon"/> geometry.</param>
    ''' <returns>A <see cref="MultiPolygon"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadMultiPolygon(ByVal multiPolygon As Autodesk.Gis.Geometry.IMultiPolygon) As MultiPolygon
        Dim polygons As New List(Of Polygon)

        For i As Integer = 0 To multiPolygon.Count - 1
            polygons.Add(Me.ReadPolygon(multiPolygon.Item(i)))
        Next

        Return Me.GeometryFactory.CreateMultiPolygon(polygons.ToArray)
    End Function

#End Region


#Region " ReadCoordinateCollection "

    ''' <summary>
    ''' Returns array of <see cref="Coordinate"/>s converted from <see cref="Autodesk.Gis.Geometry.DirectPositionCollection"/>.
    ''' </summary>
    ''' <param name="directPositions">A <see cref="Autodesk.Gis.Geometry.DirectPositionCollection"/> collection.</param>
    ''' <returns>Array of <see cref="Coordinate"/> structures.</returns>
    ''' <remarks></remarks>
    Public Function ReadCoordinateCollection(ByVal directPositions As Autodesk.Gis.Geometry.DirectPositionCollection) As ICoordinate()
        Dim collection As New CoordinateList
        For Each pos As Autodesk.Gis.Geometry.IDirectPosition In directPositions
            collection.Add(Me.ReadCoordinate(pos), Me.AllowRepeatedCoordinates)
        Next
        Return collection.ToCoordinateArray
    End Function

#End Region

#Region " ReadGeometryCollection "

    ''' <summary>
    ''' Returns <see cref="GeometryCollection"/> converted from <see cref="Autodesk.Gis.Geometry.GeometryCollection"/>.
    ''' </summary>
    ''' <param name="geometries">A <see cref="Autodesk.Gis.Geometry.GeometryCollection"/> collection.</param>
    ''' <returns>A <see cref="GeometryCollection"/> collection.</returns>
    ''' <remarks></remarks>
    Public Function ReadGeometryCollection(ByVal geometries As Autodesk.Gis.Geometry.GeometryCollection) As GeometryCollection
        Dim collection As New List(Of IGeometry)

        For Each geometry As Autodesk.Gis.Geometry.IGeometry In geometries
            collection.Add(Me.ReadGeometry(geometry))
        Next

        Return Me.GeometryFactory.CreateGeometryCollection(collection.ToArray)
    End Function

    ''' <summary>
    ''' Returns <see cref="GeometryCollection"/> converted from <see cref="Autodesk.Gis.Geometry.IMultiGeometry"/> geometry.
    ''' </summary>
    ''' <param name="multiGeometry">A <see cref="Autodesk.Gis.Geometry.IMultiGeometry"/> geometry.</param>
    ''' <returns>A <see cref="GeometryCollection"/> collection.</returns>
    ''' <remarks></remarks>
    Public Function ReadGeometryCollection(ByVal multiGeometry As Autodesk.Gis.Geometry.IMultiGeometry) As GeometryCollection
        Dim geometries As New List(Of Geometries.Geometry)

        For i As Integer = 0 To multiGeometry.Count - 1
            geometries.Add(Me.ReadGeometry(multiGeometry.Item(i)))
        Next

        Return Me.GeometryFactory.CreateGeometryCollection(geometries.ToArray)
    End Function

#End Region


#Region " ReadLineSegment "

    ''' <summary>
    ''' Returns <see cref="LineSegment"/> geometry converted from <see cref="Autodesk.Gis.Geometry.ILineStringSegment"/> geometry.
    ''' </summary>
    ''' <param name="lineStringSegment">A <see cref="Autodesk.Gis.Geometry.ILineStringSegment"/> geometry.</param>
    ''' <returns>A <see cref="LineSegment"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadLineSegment(ByVal lineStringSegment As Autodesk.Gis.Geometry.ILineStringSegment) As LineSegment
        Return New LineSegment(Me.ReadCoordinate(lineStringSegment.StartPosition), Me.ReadCoordinate(lineStringSegment.EndPosition))
    End Function

#End Region

#Region " ReadEnvelope "

    ''' <summary>
    ''' Returns <see cref="Envelope"/> structure converted from <see cref="Autodesk.Gis.Geometry.IEnvelope"/>.
    ''' </summary>
    ''' <param name="envelope">A <see cref="Autodesk.Gis.Geometry.IEnvelope"/> structure.</param>
    ''' <returns>An <see cref="Envelope"/> structure.</returns>
    ''' <remarks></remarks>
    Public Function ReadEnvelope(ByVal envelope As Autodesk.Gis.Geometry.IEnvelope) As IEnvelope
        Return New Envelope( _
            New Coordinate(envelope.MinX, envelope.MinY, envelope.MinZ), _
            New Coordinate(envelope.MaxX, envelope.MaxY, envelope.MaxZ))
    End Function

#End Region

    'Public Function Read(ByVal circularArcSegment As Autodesk.Gis.Geometry.ICircularArcSegment) As LineString
    '    Dim m_Geometry As Autodesk.Gis.Geometry.IGeometry = Autodesk.Gis.Spatial.SpatialUtility.TesselateCurve(circularArcSegment)
    '    Select Case m_Geometry.DerivedType
    '        Case Autodesk.Gis.Common.GeometryType.GeometryType_LineString
    '            Return Me.Read(CType(m_Geometry, Autodesk.Gis.Geometry.ILineString))
    '        Case Else
    '            Return LineString.Empty
    '    End Select
    'End Function

End Class


