Imports Topology.IO
Imports Topology.Geometries
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.Geometry

''' <summary>
''' Reads AutoCAD entities and creates geometric representation of the features
''' based on JTS model using single floating precision model.
''' Curve-based entities and sub-entities are tesselated during the process.
''' Processed AutoCAD entities are supposed to be database resident (DBRO).
''' </summary>
''' <remarks>
''' To maintain the link between database-resident entity and it's JTS representation
''' you may use <see cref="Geometry.UserData"/> property to store either <c>ObjectId</c>
''' or a <c>Handle</c> of an entity. Keep in mind that <see cref="Geometry.UserData"/>
''' property may not persist during certain topology-related operations.
''' <para>
''' This library references two Autodesk libraries being part of managed ObjectARX.
''' Referenced libraries are <c>acdbmgd.dll</c> and <c>acmgd.dll</c> which may be found
''' in the root installation folder of the targeted Autodesk platform/vertical.
''' </para>
''' </remarks>
Public Class DwgReader
    Inherits GeometryReader

#Region " CTOR "

    Sub New()
        MyBase.New(Topology.Geometries.GeometryFactory.FloatingSingle)
    End Sub

    Sub New(ByVal factory As IGeometryFactory)
        MyBase.New(factory)
    End Sub

#End Region


#Region " ReadCoordinate "

    ''' <summary>
    ''' Returns 3D <see cref="Coordinate"/> converted from <see cref="Point3d"/> structure.
    ''' </summary>
    ''' <param name="point3d">A <see cref="Point3d"/> structure.</param>
    ''' <returns>A three-dimensional <see cref="Coordinate"/> representation.</returns>
    ''' <remarks></remarks>
    Public Function ReadCoordinate(ByVal point3d As Point3d) As ICoordinate
        Return New Coordinate( _
            Me.PrecisionModel.MakePrecise(point3d.X), _
            Me.PrecisionModel.MakePrecise(point3d.Y), _
            Me.PrecisionModel.MakePrecise(point3d.Z))
    End Function

    ''' <summary>
    ''' Returns 2D <see cref="Coordinate"/> converted from <see cref="Point2d"/> structure.
    ''' </summary>
    ''' <param name="point2d">A <see cref="Point2d"/> structure.</param>
    ''' <returns>A two-dimensional <see cref="Coordinate"/> representation.</returns>
    ''' <remarks></remarks>
    Public Function ReadCoordinate(ByVal point2d As Point2d) As ICoordinate
        Return New Coordinate( _
            Me.PrecisionModel.MakePrecise(point2d.X), _
            Me.PrecisionModel.MakePrecise(point2d.Y))
    End Function

#End Region

#Region " ReadPoint "

    ''' <summary>
    ''' Returns <see cref="Point"/> geometry converted from <see cref="DBPoint"/> entity.
    ''' </summary>
    ''' <param name="dbPoint">A <see cref="DBPoint"/> entity (<c>POINT</c>).</param>
    ''' <returns>A <see cref="Point"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadPoint(ByVal dbPoint As DBPoint) As IPoint
        Return Me.GeometryFactory.CreatePoint(Me.ReadCoordinate(dbPoint.Position))
    End Function

    ''' <summary>
    ''' Returns <see cref="Point"/> geometry converted from <see cref="BlockReference"/> entity.
    ''' </summary>
    ''' <param name="blockReference">A <see cref="BlockReference"/> entity (<c>INSERT</c>).</param>
    ''' <returns>A <see cref="Point"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadPoint(ByVal blockReference As BlockReference) As IPoint
        Return Me.GeometryFactory.CreatePoint(Me.ReadCoordinate(blockReference.Position))
    End Function

#End Region

#Region " ReadLineString "

    ''' <summary>
    ''' Returns <see cref="LineString"/> geometry converted from <see cref="Polyline"/> entity.
    ''' </summary>
    ''' <param name="polyline">A <see cref="Polyline"/> entity (<c>LWPOLYLINE</c>).</param>
    ''' <returns>A <see cref="LineString"/> geometry.</returns>
    ''' <remarks>
    ''' If polyline entity contains arc segments (bulges), such segments will be
    ''' tessellated using settings defined via <see cref="CurveTessellationMethod"/>.
    ''' <para>
    ''' If polyline's <c>Closed</c> flag is set to <c>True</c>, then resulting <see cref="LineString"/>'s
    ''' <see cref="LineString.IsClosed"/> property will also reflect <c>True</c>.
    ''' In case resulting coordinate sequence has <c>0..1</c> coordinates,
    ''' geometry conversion will fail returning an empty <see cref="LineString"/>.
    ''' </para>
    ''' </remarks>
    Public Function ReadLineString(ByVal polyline As Polyline) As ILineString
        Dim points As New CoordinateList
        For i As Integer = 0 To polyline.NumberOfVertices - 1
            Select Case polyline.GetSegmentType(i)
                Case SegmentType.Arc
                    points.Add( _
                        Me.GetTessellatedCurveCoordinates( _
                        polyline.GetArcSegmentAt(i)), _
                        Me.AllowRepeatedCoordinates)
                Case Else
                    points.Add( _
                        Me.ReadCoordinate( _
                        polyline.GetPoint3dAt(i)), _
                        Me.AllowRepeatedCoordinates)
            End Select
        Next

        If polyline.Closed Then
            points.Add(points(0))
        End If

        If points.Count > 1 Then
            Return Me.GeometryFactory.CreateLineString(points.ToCoordinateArray)
        Else
            Return LineString.Empty
        End If
    End Function

    ''' <summary>
    ''' Returns <see cref="LineString"/> geometry converted from <see cref="Polyline3d"/> entity.
    ''' </summary>
    ''' <param name="polyline3d">A <see cref="Polyline3d"/> entity (<c>POLYLINE</c>).</param>
    ''' <returns>A <see cref="LineString"/> geometry.</returns>
    ''' <remarks>
    ''' If polyline's <c>Closed</c> flag is set to <c>True</c>, then resulting <see cref="LineString"/>'s
    ''' <see cref="LineString.IsClosed"/> property will also reflect <c>True</c>.
    ''' In case resulting coordinate sequence has <c>0..1</c> coordinates,
    ''' geometry conversion will fail returning an empty <see cref="LineString"/>.
    ''' </remarks>
    Public Function ReadLineString(ByVal polyline3d As Polyline3d) As ILineString
        Dim points As New CoordinateList

        If polyline3d.PolyType = Poly3dType.SimplePoly Then
            Dim TR As Transaction = polyline3d.Database.TransactionManager.StartTransaction()
            Dim iterator As IEnumerator = polyline3d.GetEnumerator
            Do While iterator.MoveNext
                Dim vertex As PolylineVertex3d = CType(TR.GetObject(iterator.Current, OpenMode.ForRead), PolylineVertex3d)
                points.Add(Me.ReadCoordinate(vertex.Position), Me.AllowRepeatedCoordinates)
            Loop
            TR.Commit()
            TR.Dispose()
        Else
            Dim collection As New DBObjectCollection
            polyline3d.Explode(collection)
            For Each line As Line In collection
                points.Add(Me.ReadCoordinate(line.StartPoint), False)
                points.Add(Me.ReadCoordinate(line.EndPoint), False)
            Next
            collection.Dispose()
        End If

        If polyline3d.Closed Then
            points.Add(points(0))
        End If

        If points.Count > 1 Then
            Return Me.GeometryFactory.CreateLineString(points.ToCoordinateArray)
        Else
            Return LineString.Empty
        End If
    End Function

    ''' <summary>
    ''' Returns <see cref="LineString"/> geometry converted from <see cref="Polyline2d"/> entity.
    ''' </summary>
    ''' <param name="polyline2d">A <see cref="Polyline2d"/> ("old-style") entity.</param>
    ''' <returns>A <see cref="LineString"/> geometry.</returns>
    ''' <remarks>
    ''' If polyline's <c>Closed</c> flag is set to <c>True</c>, then resulting <see cref="LineString"/>'s
    ''' <see cref="LineString.IsClosed"/> property will also reflect <c>True</c>.
    ''' In case resulting coordinate sequence has <c>0..1</c> coordinates,
    ''' geometry conversion will fail returning an empty <see cref="LineString"/>.
    ''' </remarks>
    Public Function ReadLineString(ByVal polyline2d As Polyline2d) As ILineString
        Dim points As New CoordinateList

        If polyline2d.PolyType = Poly2dType.SimplePoly Then
            Dim TR As Transaction = polyline2d.Database.TransactionManager.StartTransaction()
            Dim iterator As IEnumerator = polyline2d.GetEnumerator
            Do While iterator.MoveNext
                Dim vertex As Vertex2d = CType(TR.GetObject(iterator.Current, OpenMode.ForRead), Vertex2d)
                points.Add(Me.ReadCoordinate(vertex.Position), Me.AllowRepeatedCoordinates)
            Loop
            TR.Commit()
            TR.Dispose()
        Else
            Dim collection As New DBObjectCollection
            polyline2d.Explode(collection)
            For Each line As Line In collection
                points.Add(Me.ReadCoordinate(line.StartPoint), False)
                points.Add(Me.ReadCoordinate(line.EndPoint), False)
            Next
            collection.Dispose()
        End If

        If polyline2d.Closed Then
            points.Add(points(0))
        End If

        If points.Count > 1 Then
            Return Me.GeometryFactory.CreateLineString(points.ToCoordinateArray)
        Else
            Return LineString.Empty
        End If
    End Function

    ''' <summary>
    ''' Returns <see cref="LineString"/> geometry converted from <see cref="Line"/> entity.
    ''' </summary>
    ''' <param name="line">A <see cref="Line"/> entity (<c>LINE</c>).</param>
    ''' <returns>A <see cref="LineString"/> geometry.</returns>
    ''' <remarks>
    ''' In case resulting coordinate sequence has 1 or 0 coordinates,
    ''' geometry conversion will fail returning an empty <see cref="LineString"/>.
    ''' </remarks>
    Public Function ReadLineString(ByVal line As Line) As ILineString
        Dim points As New CoordinateList
        points.Add(Me.ReadCoordinate(line.StartPoint))
        points.Add(Me.ReadCoordinate(line.EndPoint), Me.AllowRepeatedCoordinates)

        If points.Count > 1 Then
            Return Me.GeometryFactory.CreateLineString(points.ToCoordinateArray)
        Else
            Return LineString.Empty
        End If
    End Function

    ''' <summary>
    ''' Returns <see cref="LineString"/> geometry converted from <see cref="Mline"/> (<c>MultiLine</c>) entity.
    ''' </summary>
    ''' <param name="multiLine">A <see cref="Mline"/> entity (<c>MLINE</c>).</param>
    ''' <returns>A <see cref="LineString"/> geometry.</returns>
    ''' <remarks>
    ''' In case resulting coordinate sequence has 1 or 0 coordinates,
    ''' geometry conversion will fail returning an empty <see cref="LineString"/>.
    ''' </remarks>
    Public Function ReadLineString(ByVal multiLine As Mline) As ILineString
        Dim points As New CoordinateList

        For i As Integer = 0 To multiLine.NumberOfVertices - 1
            points.Add(Me.ReadCoordinate(multiLine.VertexAt(i)), Me.AllowRepeatedCoordinates)
        Next

        If multiLine.IsClosed Then
            points.Add(points(0))
        End If

        If points.Count > 1 Then
            Return Me.GeometryFactory.CreateLineString(points.ToCoordinateArray)
        Else
            Return LineString.Empty
        End If
    End Function

    ''' <summary>
    ''' Returns <see cref="LineString"/> geometry converted from <see cref="Arc"/> entity.
    ''' During conversion <see cref="Arc"/> is being tessellated using settings defined
    ''' via <see cref="CurveTessellationMethod"/>.
    ''' </summary>
    ''' <param name="arc">An <see cref="Arc"/> entity (<c>ARC</c>).</param>
    ''' <returns>A <see cref="LineString"/> geometry.</returns>
    ''' <remarks></remarks>
    Public Function ReadLineString(ByVal arc As Arc) As ILineString
        Dim points As New CoordinateList(Me.GetTessellatedCurveCoordinates(arc), Me.AllowRepeatedCoordinates)

        If points.Count > 1 Then
            Return Me.GeometryFactory.CreateLineString(points.ToCoordinateArray)
        Else
            Return LineString.Empty
        End If
    End Function

#End Region

#Region " ReadMultiPolygon "

    ''' <summary>
    ''' Returns <see cref="MultiPolygon"/> geometry converted from <see cref="MPolygon"/> entity.
    ''' </summary>
    ''' <param name="multiPolygon">A <see cref="MPolygon"/> entity.</param>
    ''' <returns>A <see cref="MultiPolygon"/> geometry.</returns>
    ''' <remarks>
    ''' If <see cref="MPolygon"/> entity contains arc segments (bulges), such segments will 
    ''' get tessellated using settings defined via <see cref="CurveTessellationMethod"/>.
    ''' </remarks>
    Public Function ReadMultiPolygon(ByVal multiPolygon As MPolygon) As IMultiPolygon
        Dim polygons As New List(Of Polygon)

        For i As Integer = 0 To multiPolygon.NumMPolygonLoops - 1
            If multiPolygon.GetLoopDirection(i) = LoopDirection.Exterior Then
                Dim shell As LinearRing = _
                    Me.GeometryFactory.CreateLinearRing( _
                    Me.GetMPolygonLoopCoordinates( _
                    multiPolygon, _
                    multiPolygon.GetMPolygonLoopAt(i)))

                Dim holes As New List(Of LinearRing)
                For Each j As Integer In multiPolygon.GetChildLoops(i)
                    If multiPolygon.GetLoopDirection(j) = LoopDirection.Interior Then
                        holes.Add( _
                            Me.GeometryFactory.CreateLinearRing( _
                            Me.GetMPolygonLoopCoordinates( _
                            multiPolygon, _
                            multiPolygon.GetMPolygonLoopAt(j))))
                    End If
                Next
                polygons.Add(Me.GeometryFactory.CreatePolygon(shell, holes.ToArray))
            End If
        Next

        Return Me.GeometryFactory.CreateMultiPolygon(polygons.ToArray)
    End Function

#End Region

#Region " ReadGeometry "

    ''' <summary>
    ''' Returns <see cref="Geometry"/> representation converted from <see cref="Entity"/> entity.
    ''' Throws an exception if given <see cref="Entity"/> conversion is not supported.
    ''' <para>
    ''' Supported AutoCAD entity types:
    ''' <list type="table">
    '''    <listheader>
    '''       <term>Class Name</term>
    '''       <term>DXF Code</term>
    '''    </listheader>
    '''    <item>
    '''       <term>AcDbPoint</term>
    '''       <term><c>POINT</c></term>
    '''    </item>
    '''    <item>
    '''       <term>AcDbBlockReference</term>
    '''       <term><c>INSERT</c></term>
    '''    </item>
    '''    <item>
    '''       <term>AcDbLine</term>
    '''       <term><c>LINE</c></term>
    '''    </item>
    '''    <item>
    '''       <term>AcDbArc</term>
    '''       <term><c>ARC</c></term>
    '''    </item>
    '''    <item>
    '''       <term>AcDbPolyline</term>
    '''       <term><c>LWPOLYLINE</c></term>
    '''    </item>
    '''    <item>
    '''       <term>AcDb2dPolyline</term>
    '''       <term><c>POLYLINE</c></term>
    '''    </item>
    '''    <item>
    '''       <term>AcDb3dPolyline</term>
    '''       <term><c>POLYLINE</c></term>
    '''    </item>
    '''    <item>
    '''       <term>AcDbMline</term>
    '''       <term><c>MLINE</c></term>
    '''    </item>
    '''    <item>
    '''       <term>AcDbMPolygon</term>
    '''       <term><c>MPOLYGON</c></term>
    '''    </item>
    ''' </list>
    ''' </para>
    ''' </summary>
    ''' <param name="entity">An AutoCAD <see cref="Entity"/>.</param>
    ''' <returns>A <see cref="Geometry"/> representation.</returns>
    ''' <remarks></remarks>
    Public Function ReadGeometry(ByVal entity As Entity) As IGeometry
        Select Case entity.GetRXClass.Name
            Case "AcDbPoint"
                Return Me.ReadPoint(CType(entity, DBPoint))
            Case "AcDbBlockReference"
                Return Me.ReadPoint(CType(entity, BlockReference))
            Case "AcDbLine"
                Return Me.ReadLineString(CType(entity, Line))
            Case "AcDbPolyline"
                Return Me.ReadLineString(CType(entity, Polyline))
            Case "AcDb2dPolyline"
                Return Me.ReadLineString(CType(entity, Polyline2d))
            Case "AcDb3dPolyline"
                Return Me.ReadLineString(CType(entity, Polyline3d))
            Case "AcDbArc"
                Return Me.ReadLineString(CType(entity, Arc))
            Case "AcDbMline"
                Return Me.ReadLineString(CType(entity, Mline))
            Case "AcDbMPolygon"
                'added due to possibly invalid MPolygons (returned by AutoCAD as ImpEntity)
                Dim ent As MPolygon = TryCast(entity, MPolygon)
                If ent IsNot Nothing Then
                    Return Me.ReadMultiPolygon(ent)
                Else
                    Throw New ArgumentException(String.Format("Invalid MPolygon entity. Conversion to IGeometry is not possible.", entity.GetRXClass.Name))
                    Return Nothing
                End If
            Case Else
                Throw New ArgumentException(String.Format("Conversion from {0} entity to IGeometry is not supported.", entity.GetRXClass.Name))
                Return Nothing
        End Select
    End Function

#End Region


#Region " GetTessellatedCurveCoordinates "

    Private Function GetTessellatedCurveCoordinates(ByVal curve As CircularArc3d) As ICoordinate()
        Dim points As New CoordinateList

        If curve.StartPoint <> curve.EndPoint Then
            Select Case Me.CurveTessellationMethod
                Case IO.CurveTessellation.None
                    points.Add(Me.ReadCoordinate(curve.StartPoint))
                    points.Add(Me.ReadCoordinate(curve.EndPoint))

                Case IO.CurveTessellation.Linear
                    For Each point As Point3d In curve.GetSamplePoints(CInt(Me.CurveTessellationValue))
                        points.Add(Me.ReadCoordinate(point))
                    Next

                Case IO.CurveTessellation.Scaled
                    Dim area As Double = curve.GetArea( _
                        curve.GetParameterOf(curve.StartPoint), _
                        curve.GetParameterOf(curve.EndPoint)) * _
                        Me.CurveTessellationValue

                    Dim angle As Double = Math.Acos((curve.Radius - 1.0 / (area / 2.0)) / curve.Radius)
                    Dim segments As Integer = CInt(2 * Math.PI / angle)

                    If segments < 8 Then segments = 8
                    If segments > 128 Then segments = 128

                    For Each point As Point3d In curve.GetSamplePoints(CInt(segments))
                        points.Add(Me.ReadCoordinate(point))
                    Next
            End Select
        End If

        Return points.ToCoordinateArray
    End Function

    Private Function GetTessellatedCurveCoordinates(ByVal parentEcs As Matrix3d, ByVal curve As CircularArc2d) As ICoordinate()
        Dim matrix As Matrix3d = parentEcs.Inverse
        Dim pts() As Point2d = curve.GetSamplePoints(3)

        Dim startPt As New Point3d(pts(0).X, pts(0).Y, 0)
        Dim midPt As New Point3d(pts(1).X, pts(1).Y, 0)
        Dim endPt As New Point3d(pts(2).X, pts(2).Y, 0)

        startPt.TransformBy(matrix)
        midPt.TransformBy(matrix)
        endPt.TransformBy(matrix)

        Return Me.GetTessellatedCurveCoordinates(New CircularArc3d(startPt, midPt, endPt))
    End Function

    Private Function GetTessellatedCurveCoordinates(ByVal parentEcs As Matrix3d, ByVal startPoint As Point2d, ByVal endPoint As Point2d, ByVal bulge As Double) As ICoordinate()
        Return Me.GetTessellatedCurveCoordinates( _
            parentEcs, _
            New CircularArc2d(startPoint, endPoint, bulge, False))
    End Function

    Private Function GetTessellatedCurveCoordinates(ByVal curve As Arc) As ICoordinate()
        Dim circularArc As CircularArc3d

        Try
            circularArc = New CircularArc3d( _
                curve.StartPoint, _
                curve.GetPointAtParameter((curve.EndParam - curve.StartParam) / 2), _
                curve.EndPoint)
        Catch ex As Autodesk.AutoCAD.Runtime.Exception
            circularArc = New CircularArc3d( _
                curve.StartPoint, _
                curve.GetPointAtParameter((curve.EndParam + curve.StartParam) / 2), _
                curve.EndPoint)
        End Try

        Return Me.GetTessellatedCurveCoordinates(circularArc)
    End Function

#End Region

#Region " GetMPolygonLoopCoordinates "

    Private Function GetMPolygonLoopCoordinates(ByVal multiPolygon As MPolygon, ByVal multiPolygonLoop As MPolygonLoop) As ICoordinate()
        Dim points As New CoordinateList

        For i As Integer = 0 To multiPolygonLoop.Count - 1
            Dim vert As BulgeVertex = multiPolygonLoop.Item(i)
            If vert.Bulge = 0 Then
                points.Add(Me.ReadCoordinate(vert.Vertex), Me.AllowRepeatedCoordinates)
            Else
                Dim endPoint As Point2d
                If i + 1 <= multiPolygonLoop.Count - 1 Then
                    endPoint = multiPolygonLoop.Item(i + 1).Vertex
                Else
                    endPoint = multiPolygonLoop.Item(0).Vertex
                End If

                For Each point As Coordinate In Me.GetTessellatedCurveCoordinates(multiPolygon.Ecs, vert.Vertex, endPoint, vert.Bulge)
                    points.Add(point, Me.AllowRepeatedCoordinates)
                Next
            End If
        Next

        If Not points(0).Equals2D(points(points.Count - 1)) Then
            points.Add(points(0))
        End If

        Return points.ToCoordinateArray
    End Function

#End Region

End Class
