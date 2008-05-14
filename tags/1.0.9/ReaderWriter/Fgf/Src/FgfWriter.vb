Imports Topology.IO
Imports Topology.Geometries

#Region " BinaryFormat Enum "

''' <summary>
''' Definition of the binary format used by Feature Geometry Factory binary writer.
''' </summary>
''' <remarks></remarks>
Public Enum BinaryFormat

    ''' <summary>
    ''' OSGeo Feature Geometry Factory binary format.
    ''' <para>
    ''' Feature Geometry Factory format is OSGeo's extended version of the
    ''' Well Known Binary Format (WKB). In FGF, geometry types are included
    ''' that are not yet covered by any WKB specification. Only one memory
    ''' alignment type is supported, which is the same alignment type as used
    ''' by the .Net framework and Windows (little endian).
    ''' As a result, this byte flag does not need to be stored.
    ''' </para>
    ''' In FGF, the dimension flag has been added. In particular, a flag is
    ''' included for each geometry piece to indicate whether the geometry is 2D,
    ''' 3D or even 4D (storing a measure value as used by dynamic segmentation.
    ''' </summary>
    ''' <remarks></remarks>
    FGF
    ''' <summary>
    ''' Well Known Binary format.
    ''' <para>
    ''' WKB is a memory layout to store geometry used by GIS applications. This format was
    ''' created by the OpenGIS organization to allow efficient exchange of geometry data between
    ''' different components in a GIS system. Most pieces of the original specification defining
    ''' WKB format are in the document, 99-050.pdf, the OpenGIS Simple feature specification for
    ''' OLE/COM that can be found at www.opengis.org.
    ''' </para>
    ''' WKB defines a byte order of the data in every piece of geometry. This is
    ''' stored as a byte field, which as a result might change the memory alignment
    ''' from word to byte. It is defined as a 2D format only. This is insufficient
    ''' to represent 3D points, polylines and polygons.
    ''' </summary>
    ''' <remarks></remarks>
    WKB

End Enum

#End Region

''' <summary>
''' Reads features based on JTS model and creates their OSGeo Feature Data Objects (FDO) representation.
''' </summary>
''' <remarks></remarks>
Public Class FgfWriter
    Inherits GeometryWriter

    Private m_Factory As OSGeo.FDO.Geometry.FgfGeometryFactory

#Region " CTOR "

    Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="FgfWriter"/> class, using
    ''' user-supplied <see cref="OSGeo.FDO.Geometry.FgfGeometryFactory"/> for geometry processing. 
    ''' </summary>
    ''' <param name="factory">A <see cref="OSGeo.FDO.Geometry.FgfGeometryFactory"/>.</param>
    ''' <remarks></remarks>
    Sub New(ByVal factory As OSGeo.FDO.Geometry.FgfGeometryFactory)
        MyBase.New()
        m_Factory = factory
    End Sub

#End Region


#Region " Factory "

    ''' <summary>
    ''' Returns current <see cref="OSGeo.FDO.Geometry.FgfGeometryFactory"/> used to build geometries.
    ''' <para>
    ''' It's actually reserved for possible future releases of <see cref="OSGeo.FDO.Geometry.FgfGeometryFactory"/>
    ''' that might implement user-defined precision model, or incorporate common coordinate
    ''' reference system (CRS) used when building geometries.
    ''' </para>
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

#Region " GetOrdinateSequence "

    Private Function GetOrdinateSequence(ByVal geometry As Geometry) As OrdinateSequence
        Dim result As New OrdinateSequence
        Dim ordinates As New List(Of Double)

        result.Dimensionality = 1

        For Each coord As Coordinate In geometry.Coordinates
            ordinates.Add(coord.X)
            ordinates.Add(coord.Y)
            If Not Double.IsNaN(coord.Z) Then
                ordinates.Add(coord.Z)
            Else
                result.Dimensionality = 0
            End If
        Next

        result.OrdinateNumber = ordinates.Count
        result.Ordinates = ordinates.ToArray

        Return result
    End Function

#End Region


#Region " WriteDirectPosition "

    ''' <summary>
    ''' Returns <see cref="OSGeo.FDO.Geometry.IDirectPosition"/> structure converted from <see cref="Coordinate"/> structure.
    ''' If <see cref="Coordinate"/> is two-dimensional, resulting <see cref="OSGeo.FDO.Geometry.IDirectPosition"/> is
    ''' created using <c>CreatePositionXY()</c>, otherwise <c>CreatePositionXYZ()</c> is used.
    ''' </summary>
    ''' <param name="coordinate">A <see cref="Coordinate"/> structure.</param>
    ''' <returns>A <see cref="OSGeo.FDO.Geometry.IDirectPosition"/> structure.</returns>
    ''' <remarks></remarks>
    Public Function WriteDirectPosition(ByVal coordinate As ICoordinate) As OSGeo.FDO.Geometry.IDirectPosition
        If Not Double.IsNaN(coordinate.Z) Then
            Return Me.Factory.CreatePositionXYZ(coordinate.X, coordinate.Y, coordinate.Z)
        Else
            Return Me.Factory.CreatePositionXY(coordinate.X, coordinate.Y)
        End If
    End Function

    ''' <summary>
    ''' Returns byte array converted from <see cref="Coordinate"/> structure.
    ''' Resulting byte array is formatted using specified <see cref="BinaryFormat"/>.
    ''' If <see cref="Coordinate"/> is two-dimensional, resulting <see cref="OSGeo.FDO.Geometry.IDirectPosition"/>
    ''' is created using <c>CreatePositionXY()</c>, otherwise <c>CreatePositionXYZ()</c> is used.
    ''' </summary>
    ''' <param name="coordinate">A <see cref="Coordinate"/> structure.</param>
    ''' <param name="format">Byte array <see cref="BinaryFormat"/> type.</param>
    ''' <returns>Byte array formatted using specified <see cref="BinaryFormat"/>.</returns>
    ''' <remarks></remarks>
    Public Function WriteDirectPosition(ByVal coordinate As ICoordinate, ByVal format As BinaryFormat) As Byte()
        If format = BinaryFormat.FGF Then
            Return Me.Factory.GetFgf(Me.WriteDirectPosition(coordinate))
        Else
            Return Me.Factory.GetWkb(Me.WriteDirectPosition(coordinate))
        End If
    End Function

#End Region

#Region " WritePoint "

    ''' <summary>
    ''' Returns <see cref="OSGeo.FDO.Geometry.IPoint"/> geometry converted from <see cref="Point"/> geometry.
    ''' </summary>
    ''' <param name="point">A <see cref="Point"/> geometry.</param>
    ''' <returns>A <see cref="OSGeo.FDO.Geometry.IPoint"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function WritePoint(ByVal point As IPoint) As OSGeo.FDO.Geometry.IPoint
        Dim sequence As OrdinateSequence = Me.GetOrdinateSequence(point)
        Return Me.Factory.CreatePoint(sequence.Dimensionality, sequence.Ordinates)
    End Function

    ''' <summary>
    ''' Returns byte array converted from <see cref="Point"/> geometry.
    ''' Resulting byte array is formatted using specified <see cref="BinaryFormat"/>.
    ''' </summary>
    ''' <param name="point">A <see cref="Point"/> geometry.</param>
    ''' <param name="format">Byte array <see cref="BinaryFormat"/> type.</param>
    ''' <returns>Byte array formatted using specified <see cref="BinaryFormat"/>.</returns>
    ''' <remarks></remarks>
    Public Function WritePoint(ByVal point As IPoint, ByVal format As BinaryFormat) As Byte()
        If format = BinaryFormat.FGF Then
            Return Me.Factory.GetFgf(Me.WritePoint(point))
        Else
            Return Me.Factory.GetWkb(Me.WritePoint(point))
        End If
    End Function

#End Region

#Region " WriteLineString "

    ''' <summary>
    ''' Returns <see cref="OSGeo.FDO.Geometry.ILineString"/> geometry converted from <see cref="LineString"/> geometry.
    ''' </summary>
    ''' <param name="lineString">A <see cref="LineString"/> geometry.</param>
    ''' <returns>A <see cref="OSGeo.FDO.Geometry.ILineString"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function WriteLineString(ByVal lineString As ILineString) As OSGeo.FDO.Geometry.ILineString
        Dim sequence As OrdinateSequence = Me.GetOrdinateSequence(lineString)
        Return Me.Factory.CreateLineString(sequence.Dimensionality, sequence.OrdinateNumber, sequence.Ordinates)
    End Function

    ''' <summary>
    ''' Returns byte array converted from <see cref="LineString"/> geometry.
    ''' Resulting byte array is formatted using specified <see cref="BinaryFormat"/>.
    ''' </summary>
    ''' <param name="lineString">A <see cref="LineString"/> geometry.</param>
    ''' <param name="format">Byte array <see cref="BinaryFormat"/> type.</param>
    ''' <returns>Byte array formatted using specified <see cref="BinaryFormat"/>.</returns>
    ''' <remarks></remarks>
    Public Function WriteLineString(ByVal lineString As ILineString, ByVal format As BinaryFormat) As Byte()
        If format = BinaryFormat.FGF Then
            Return Me.Factory.GetFgf(Me.WriteLineString(lineString))
        Else
            Return Me.Factory.GetWkb(Me.WriteLineString(lineString))
        End If
    End Function

#End Region

#Region " WriteLinearRing "

    ''' <summary>
    ''' Returns <see cref="OSGeo.FDO.Geometry.ILinearRing"/> geometry converted from <see cref="LinearRing"/> geometry.
    ''' </summary>
    ''' <param name="linearRing">A <see cref="LinearRing"/> geometry.</param>
    ''' <returns>A <see cref="OSGeo.FDO.Geometry.ILinearRing"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function WriteLinearRing(ByVal linearRing As ILinearRing) As OSGeo.FDO.Geometry.ILinearRing
        Dim sequence As OrdinateSequence = Me.GetOrdinateSequence(linearRing)
        Return Me.Factory.CreateLinearRing(sequence.Dimensionality, sequence.OrdinateNumber, sequence.Ordinates)
    End Function

    ''' <summary>
    ''' Returns byte array converted from <see cref="LinearRing"/> geometry.
    ''' Resulting byte array is formatted using specified <see cref="BinaryFormat"/>.
    ''' </summary>
    ''' <param name="linearRing">A <see cref="LinearRing"/> geometry.</param>
    ''' <param name="format">Byte array <see cref="BinaryFormat"/> type.</param>
    ''' <returns>Byte array formatted using specified <see cref="BinaryFormat"/>.</returns>
    ''' <remarks></remarks>
    Public Function WriteLinearRing(ByVal linearRing As ILinearRing, ByVal format As BinaryFormat) As Byte()
        If format = BinaryFormat.FGF Then
            Return Me.Factory.GetFgf(Me.WriteLinearRing(linearRing))
        Else
            Return Me.Factory.GetWkb(Me.WriteLinearRing(linearRing))
        End If
    End Function

#End Region

#Region " WritePolygon "

    ''' <summary>
    ''' Returns <see cref="OSGeo.FDO.Geometry.IPolygon"/> geometry converted from <see cref="Polygon"/> geometry.
    ''' </summary>
    ''' <param name="polygon">A <see cref="Polygon"/> geometry.</param>
    ''' <returns>A <see cref="OSGeo.FDO.Geometry.IPolygon"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function WritePolygon(ByVal polygon As IPolygon) As OSGeo.FDO.Geometry.IPolygon
        Dim extRing As OSGeo.FDO.Geometry.ILinearRing = Me.WriteLinearRing(polygon.Shell)

        Dim ringCol As New OSGeo.FDO.Geometry.LinearRingCollection
        For Each hole As LinearRing In polygon.Holes
            Dim intRing As OSGeo.FDO.Geometry.ILinearRing = Me.WriteLinearRing(hole)
            ringCol.Add(intRing)
        Next

        Return Me.Factory.CreatePolygon(extRing, ringCol)
    End Function

    ''' <summary>
    ''' Returns byte array converted from <see cref="Polygon"/> geometry.
    ''' Resulting byte array is formatted using specified <see cref="BinaryFormat"/>.
    ''' </summary>
    ''' <param name="polygon">A <see cref="Polygon"/> geometry.</param>
    ''' <param name="format">Byte array <see cref="BinaryFormat"/> type.</param>
    ''' <returns>Byte array formatted using specified <see cref="BinaryFormat"/>.</returns>
    ''' <remarks></remarks>
    Public Function WritePolygon(ByVal polygon As IPolygon, ByVal format As BinaryFormat) As Byte()
        If format = BinaryFormat.FGF Then
            Return Me.Factory.GetFgf(Me.WritePolygon(polygon))
        Else
            Return Me.Factory.GetWkb(Me.WritePolygon(polygon))
        End If
    End Function

#End Region

#Region " WriteGeometry "

    ''' <summary>
    ''' Returns <see cref="OSGeo.FDO.Geometry.IGeometry"/> converted from <see cref="Geometry"/>.
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
    ''' <returns>A <see cref="OSGeo.FDO.Geometry.IGeometry"/> object.</returns>
    ''' <remarks></remarks>
    Public Function WriteGeometry(ByVal geometry As IGeometry) As OSGeo.FDO.Geometry.IGeometry
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
                Throw New ArgumentException(String.Format("Geometry conversion from {0} to IGeometry is not supported.", geometry.GeometryType))
        End Select
        Return Nothing
    End Function

    ''' <summary>
    ''' Returns byte array converted from <see cref="Geometry"/> geometry.
    ''' Resulting byte array is formatted using specified <see cref="BinaryFormat"/>.
    ''' </summary>
    ''' <param name="geometry">A <see cref="Geometry"/> geometry.</param>
    ''' <param name="format">Byte array <see cref="BinaryFormat"/> type.</param>
    ''' <returns>Byte array formatted using specified <see cref="BinaryFormat"/>.</returns>
    ''' <remarks></remarks>
    Public Function WriteGeometry(ByVal geometry As IGeometry, ByVal format As BinaryFormat) As Byte()
        If format = BinaryFormat.FGF Then
            Return Me.Factory.GetFgf(Me.WriteGeometry(geometry))
        Else
            Return Me.Factory.GetWkb(Me.WriteGeometry(geometry))
        End If
    End Function

#End Region


#Region " WriteMultiPoint "

    ''' <summary>
    ''' Returns <see cref="OSGeo.FDO.Geometry.IMultiPoint"/> geometry converted from <see cref="MultiPoint"/> geometry.
    ''' </summary>
    ''' <param name="multiPoint">A <see cref="MultiPoint"/> geometry.</param>
    ''' <returns>A <see cref="OSGeo.FDO.Geometry.IMultiPoint"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function WriteMultiPoint(ByVal multiPoint As IMultiPoint) As OSGeo.FDO.Geometry.IMultiPoint
        Dim points As New List(Of IPoint)
        For Each geometry As IGeometry In multiPoint.Geometries
            points.Add(CType(geometry, IPoint))
        Next
        Return Me.Factory.CreateMultiPoint(Me.WritePointCollection(points.ToArray))
    End Function

    ''' <summary>
    ''' Returns byte array converted from <see cref="MultiPoint"/> geometry.
    ''' Resulting byte array is formatted using specified <see cref="BinaryFormat"/>.
    ''' </summary>
    ''' <param name="multiPoint">A <see cref="MultiPoint"/> geometry.</param>
    ''' <param name="format">Byte array <see cref="BinaryFormat"/> type.</param>
    ''' <returns>Byte array formatted using specified <see cref="BinaryFormat"/>.</returns>
    ''' <remarks></remarks>
    Public Function WriteMultiPoint(ByVal multiPoint As IMultiPoint, ByVal format As BinaryFormat) As Byte()
        If format = BinaryFormat.FGF Then
            Return Me.Factory.GetFgf(Me.WriteMultiPoint(multiPoint))
        Else
            Return Me.Factory.GetWkb(Me.WriteMultiPoint(multiPoint))
        End If
    End Function

#End Region

#Region " WriteMultiLineString "

    ''' <summary>
    ''' Returns <see cref="OSGeo.FDO.Geometry.IMultiLineString"/> geometry converted from <see cref="MultiLineString"/> geometry.
    ''' </summary>
    ''' <param name="multiLineString">A <see cref="MultiLineString"/> geometry.</param>
    ''' <returns>A <see cref="OSGeo.FDO.Geometry.IMultiLineString"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function WriteMultiLineString(ByVal multiLineString As IMultiLineString) As OSGeo.FDO.Geometry.IMultiLineString
        Dim lineStrings As New List(Of ILineString)
        For Each geometry As IGeometry In multiLineString.Geometries
            lineStrings.Add(CType(geometry, ILineString))
        Next
        Return Me.Factory.CreateMultiLineString(Me.WriteLineStringCollection(lineStrings.ToArray))
    End Function

    ''' <summary>
    ''' Returns byte array converted from <see cref="MultiLineString"/> geometry.
    ''' Resulting byte array is formatted using specified <see cref="BinaryFormat"/>.
    ''' </summary>
    ''' <param name="multiLineString">A <see cref="MultiLineString"/> geometry.</param>
    ''' <param name="format">Byte array <see cref="BinaryFormat"/> type.</param>
    ''' <returns>Byte array formatted using specified <see cref="BinaryFormat"/>.</returns>
    ''' <remarks></remarks>
    Public Function WriteMultiLineString(ByVal multiLineString As IMultiLineString, ByVal format As BinaryFormat) As Byte()
        If format = BinaryFormat.FGF Then
            Return Me.Factory.GetFgf(Me.WriteMultiLineString(multiLineString))
        Else
            Return Me.Factory.GetWkb(Me.WriteMultiLineString(multiLineString))
        End If
    End Function

#End Region

#Region " WriteMultiPolygon "

    ''' <summary>
    ''' Returns <see cref="OSGeo.FDO.Geometry.IMultiPolygon"/> geometry converted from <see cref="MultiPolygon"/> geometry.
    ''' </summary>
    ''' <param name="multiPolygon">A <see cref="MultiPolygon"/> geometry.</param>
    ''' <returns>A <see cref="OSGeo.FDO.Geometry.IMultiPolygon"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function WriteMultiPolygon(ByVal multiPolygon As IMultiPolygon) As OSGeo.FDO.Geometry.IMultiPolygon
        Dim polygons As New List(Of IPolygon)
        For Each geometry As IGeometry In multiPolygon.Geometries
            polygons.Add(CType(geometry, IPolygon))
        Next
        Return Me.Factory.CreateMultiPolygon(Me.WritePolygonCollection(polygons.ToArray))
    End Function

    ''' <summary>
    ''' Returns byte array converted from <see cref="MultiPolygon"/> geometry.
    ''' Resulting byte array is formatted using specified <see cref="BinaryFormat"/>.
    ''' </summary>
    ''' <param name="multiPolygon">A <see cref="MultiPolygon"/> geometry.</param>
    ''' <param name="format">Byte array <see cref="BinaryFormat"/> type.</param>
    ''' <returns>Byte array formatted using specified <see cref="BinaryFormat"/>.</returns>
    ''' <remarks></remarks>
    Public Function WriteMultiPolygon(ByVal multiPolygon As IMultiPolygon, ByVal format As BinaryFormat) As Byte()
        If format = BinaryFormat.FGF Then
            Return Me.Factory.GetFgf(Me.WriteMultiPolygon(multiPolygon))
        Else
            Return Me.Factory.GetWkb(Me.WriteMultiPolygon(multiPolygon))
        End If
    End Function

#End Region

#Region " WriteMultiGeometry "

    ''' <summary>
    ''' Returns <see cref="OSGeo.FDO.Geometry.IMultiGeometry"/> geometry converted from <see cref="IMultiSurface"/> geometry.
    ''' </summary>
    ''' <param name="multiSurface">A <see cref="IMultiSurface"/> geometry.</param>
    ''' <returns>A <see cref="OSGeo.FDO.Geometry.IMultiGeometry"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function WriteMultiGeometry(ByVal multiSurface As IMultiSurface) As OSGeo.FDO.Geometry.IMultiGeometry
        Dim geometries As New List(Of IGeometry)
        For Each geometry As IGeometry In multiSurface.Geometries
            geometries.Add(geometry)
        Next
        Return Me.Factory.CreateMultiGeometry(Me.WriteGeometryCollection(geometries.ToArray))
    End Function

    ''' <summary>
    ''' Returns byte array converted from <see cref="IMultiSurface"/> geometry.
    ''' Resulting byte array is formatted using specified <see cref="BinaryFormat"/>.
    ''' </summary>
    ''' <param name="multiSurface">A <see cref="IMultiSurface"/> geometry.</param>
    ''' <param name="format">Byte array <see cref="BinaryFormat"/> type.</param>
    ''' <returns>Byte array formatted using specified <see cref="BinaryFormat"/>.</returns>
    ''' <remarks></remarks>
    Public Function WriteMultiGeometry(ByVal multiSurface As IMultiSurface, ByVal format As BinaryFormat) As Byte()
        If format = BinaryFormat.FGF Then
            Return Me.Factory.GetFgf(Me.WriteMultiGeometry(multiSurface))
        Else
            Return Me.Factory.GetWkb(Me.WriteMultiGeometry(multiSurface))
        End If
    End Function

#End Region


#Region " WritePointCollection "

    ''' <summary>
    ''' Returns <see cref="OSGeo.FDO.Geometry.PointCollection"/> converted from array of <see cref="Point"/>s.
    ''' </summary>
    ''' <param name="points">An array of <see cref="Point"/>s.</param>
    ''' <returns>A <see cref="OSGeo.FDO.Geometry.PointCollection"/> collection.</returns>
    ''' <remarks></remarks>
    Public Function WritePointCollection(ByVal points() As IPoint) As OSGeo.FDO.Geometry.PointCollection
        Dim collection As New OSGeo.FDO.Geometry.PointCollection
        For Each point As IPoint In points
            collection.Add(Me.WritePoint(point))
        Next
        Return collection
    End Function

#End Region

#Region " WriteLineStringCollection "

    ''' <summary>
    ''' Returns <see cref="OSGeo.FDO.Geometry.LineStringCollection"/> converted from array of <see cref="LineString"/>s.
    ''' </summary>
    ''' <param name="lineStrings">An array of <see cref="LineString"/>s.</param>
    ''' <returns>A <see cref="OSGeo.FDO.Geometry.LineStringCollection"/> collection.</returns>
    ''' <remarks></remarks>
    Public Function WriteLineStringCollection(ByVal lineStrings() As ILineString) As OSGeo.FDO.Geometry.LineStringCollection
        Dim collection As New OSGeo.FDO.Geometry.LineStringCollection
        For Each lineString As ILineString In lineStrings
            collection.Add(Me.WriteLineString(lineString))
        Next
        Return collection
    End Function

#End Region

#Region " WriteLinearRingCollection "

    ''' <summary>
    ''' Returns <see cref="OSGeo.FDO.Geometry.LinearRingCollection"/> converted from array of <see cref="LinearRing"/>s.
    ''' </summary>
    ''' <param name="linearRings">An array of <see cref="LinearRing"/>s.</param>
    ''' <returns>A <see cref="OSGeo.FDO.Geometry.LinearRingCollection"/> collection.</returns>
    ''' <remarks></remarks>
    Public Function WriteLinearRingCollection(ByVal linearRings() As ILinearRing) As OSGeo.FDO.Geometry.LinearRingCollection
        Dim collection As New OSGeo.FDO.Geometry.LinearRingCollection
        For Each linearRing As ILinearRing In linearRings
            collection.Add(Me.WriteLinearRing(linearRing))
        Next
        Return collection
    End Function

#End Region

#Region " WritePolygonCollection "

    ''' <summary>
    ''' Returns <see cref="OSGeo.FDO.Geometry.PolygonCollection"/> converted from array of <see cref="Polygon"/>s.
    ''' </summary>
    ''' <param name="polygons">An array of <see cref="Polygon"/>s.</param>
    ''' <returns>A <see cref="OSGeo.FDO.Geometry.PolygonCollection"/> collection.</returns>
    ''' <remarks></remarks>
    Public Function WritePolygonCollection(ByVal polygons() As IPolygon) As OSGeo.FDO.Geometry.PolygonCollection
        Dim collection As New OSGeo.FDO.Geometry.PolygonCollection
        For Each polygon As IPolygon In polygons
            collection.Add(Me.WritePolygon(polygon))
        Next
        Return collection
    End Function

#End Region

#Region " WriteGeometryCollection "

    ''' <summary>
    ''' Returns <see cref="OSGeo.FDO.Geometry.GeometryCollection"/> converted from <see cref="GeometryCollection"/>.
    ''' </summary>
    ''' <param name="geometries">A <see cref="GeometryCollection"/> collection.</param>
    ''' <returns>A <see cref="OSGeo.FDO.Geometry.GeometryCollection"/> collection.</returns>
    ''' <remarks></remarks>
    Public Function WriteGeometryCollection(ByVal geometries As IGeometryCollection) As OSGeo.FDO.Geometry.GeometryCollection
        Dim collection As New OSGeo.FDO.Geometry.GeometryCollection
        For Each geometry As IGeometry In geometries
            collection.Add(Me.WriteGeometry(geometry))
        Next
        Return collection
    End Function

#End Region

End Class

