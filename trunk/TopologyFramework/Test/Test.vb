Imports Topology.Geometries
Imports Topology.IO.Dwg

Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.EditorInput

Public Class Test

    <CommandMethod("WRITEPOLYLINE")> _
    Public Shared Sub WritePolyline()
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        Dim selOptions As PromptSelectionOptions = New PromptSelectionOptions
        selOptions.MessageForAdding = "Select object:"
        selOptions.AllowDuplicates = False
        selOptions.SingleOnly = True

        Dim result As PromptSelectionResult = ed.GetSelection(selOptions)
        If result.Status = PromptStatus.OK Then
            Dim selSet As SelectionSet = result.Value
            Dim objId As ObjectId = selSet.GetObjectIds(0)
            Dim db As Database = HostApplicationServices.WorkingDatabase
            Dim tr As Transaction = db.TransactionManager.StartTransaction()
            Dim inEnt As Entity = tr.GetObject(objId, OpenMode.ForRead)

            Dim reader As New DwgReader
            Dim writer As New DwgWriter
            Dim lineString As LineString = lineString.Empty
            Dim outEnt As Entity = Nothing
            Try
                Select Case inEnt.GetRXClass.Name
                    Case "AcDbPolyline", "AcDb3dPolyline", "AcDb2dPolyline"
                        Dim kwdOptions As New PromptKeywordOptions(vbLf + "Write object as: ")
                        kwdOptions.AllowNone = True
                        kwdOptions.Keywords.Add("AcDbPolyline")
                        kwdOptions.Keywords.Add("AcDb2dPolyline")
                        kwdOptions.Keywords.Add("AcDb3dPolyline")
                        Dim kwdResult As PromptResult = ed.GetKeywords(kwdOptions)

                        If kwdResult.Status = PromptStatus.OK Then
                            Select Case inEnt.GetRXClass.Name
                                Case "AcDbPolyline"
                                    lineString = reader.ReadLineString(CType(inEnt, Polyline))
                                Case "AcDb3dPolyline"
                                    lineString = reader.ReadLineString(CType(inEnt, Polyline3d))
                                Case "AcDb2dPolyline"
                                    lineString = reader.ReadLineString(CType(inEnt, Polyline2d))
                            End Select

                            Select Case kwdResult.StringResult
                                Case "AcDbPolyline"
                                    outEnt = writer.WritePolyline(lineString)
                                Case "AcDb2dPolyline"
                                    outEnt = writer.WritePolyline2d(lineString)
                                Case "AcDb3dPolyline"
                                    outEnt = writer.WritePolyline3d(lineString)
                            End Select
                        End If
                        outEnt.ColorIndex = 1
                End Select

                Dim bt As BlockTable = tr.GetObject(db.BlockTableId, OpenMode.ForRead)
                Dim btr As BlockTableRecord = tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite)
                btr.AppendEntity(outEnt)
                tr.AddNewlyCreatedDBObject(outEnt, True)
                tr.Commit()
            Finally
                tr.Dispose()
            End Try
        End If
    End Sub

    <CommandMethod("CREATEBUFFER")> _
    Public Shared Sub CreateBuffer()
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor
        Dim selOptions As PromptSelectionOptions = New PromptSelectionOptions
        selOptions.MessageForAdding = "Select object:"
        selOptions.AllowDuplicates = False
        selOptions.SingleOnly = True

        Dim result As PromptSelectionResult = ed.GetSelection(selOptions)
        If result.Status = PromptStatus.OK Then
            Dim selSet As SelectionSet = result.Value
            Dim objId As ObjectId = selSet.GetObjectIds(0)
            Dim db As Database = HostApplicationServices.WorkingDatabase
            Dim tr As Transaction = db.TransactionManager.StartTransaction()
            Dim inEnt As Entity = tr.GetObject(objId, OpenMode.ForRead)

            Dim reader As New DwgReader
            Dim writer As New DwgWriter
            Dim lineString As LineString = lineString.Empty
            Dim outEnts() As Polyline = Nothing
            Try
                Select Case inEnt.GetRXClass.Name
                    Case "AcDbPolyline"
                        lineString = reader.ReadLineString(CType(inEnt, Polyline))
                    Case "AcDb3dPolyline"
                        lineString = reader.ReadLineString(CType(inEnt, Polyline3d))
                    Case "AcDb2dPolyline"
                        lineString = reader.ReadLineString(CType(inEnt, Polyline2d))
                End Select

                Dim dblOptions As New PromptDoubleOptions("Enter buffer offset: ")
                dblOptions.AllowNegative = False
                dblOptions.AllowNone = False
                dblOptions.AllowZero = False
                Dim dblOptionsResult As PromptDoubleResult = ed.GetDouble(dblOptions)

                If dblOptionsResult.Status = PromptStatus.OK Then
                    Dim offset As Double = dblOptionsResult.Value

                    Dim buffer As Geometry = lineString.Buffer(offset)
                    If buffer.GeometryType = "Polygon" Then
                        outEnts = writer.WritePolyline(CType(buffer, Polygon))

                    End If
                End If

                Dim bt As BlockTable = tr.GetObject(db.BlockTableId, OpenMode.ForRead)
                Dim btr As BlockTableRecord = tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite)

                If outEnts IsNot Nothing Then
                    For Each outEnt As Entity In outEnts
                        outEnt.ColorIndex = 1
                        btr.AppendEntity(outEnt)
                        tr.AddNewlyCreatedDBObject(outEnt, True)
                    Next
                End If

                tr.Commit()
            Finally
                tr.Dispose()
            End Try
        End If
    End Sub


    <CommandMethod("MERGELINEWORK")> _
    Public Shared Sub MergeLinework()
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor

        Dim values() As TypedValue = {New TypedValue(DxfCode.Start, "*LINE,ARC")}
        Dim filter As New SelectionFilter(values)

        Dim selOptions As PromptSelectionOptions = New PromptSelectionOptions
        selOptions.MessageForAdding = "Select objects:"
        selOptions.AllowDuplicates = False
        selOptions.SingleOnly = False

        Dim result As PromptSelectionResult = ed.GetSelection(selOptions, filter)
        If result.Status = PromptStatus.OK Then
            Dim db As Database = HostApplicationServices.WorkingDatabase
            Dim tr As Transaction = db.TransactionManager.StartTransaction()
            Dim reader As New DwgReader
            Dim writer As New DwgWriter

            Dim selSet As SelectionSet = result.Value
            Dim merger As New Topology.Operation.Linemerge.LineMerger
            For Each objId As ObjectId In selSet.GetObjectIds
                Dim ent As Entity = tr.GetObject(objId, OpenMode.ForRead)
                Dim geometry As Geometry = reader.ReadGeometry(ent)
                merger.Add(geometry)
            Next

            Dim bt As BlockTable = tr.GetObject(db.BlockTableId, OpenMode.ForRead)
            Dim btr As BlockTableRecord = tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite)

            For Each lineString As LineString In merger.MergedLineStrings
                Dim outEnt As Entity = writer.WritePolyline(lineString)
                outEnt.ColorIndex = 1
                btr.AppendEntity(outEnt)
                tr.AddNewlyCreatedDBObject(outEnt, True)
            Next

            tr.Commit()
            tr.Dispose()
        End If
    End Sub

End Class
