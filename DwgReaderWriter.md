## Introduction ##

A reader/writer for geometric entities exposed by Autodesk AutoCAD and it's verticals.

**DWG Reader** reads AutoCAD entities and creates geometric representation of the features based on JTS model using single floating precision model. Curve-based entities and sub-entities are tesselated during the process using one of available tesselation methods. Processed AutoCAD entities are supposed to be database resident (DBRO).

**DWG Writer** reads features based on JTS model and creates their AutoCAD representation using single floating precision model. Created AutoCAD entities are not database resident, it's up to you to commit them to the existing drawing `Database`.

## Components ##

A `Topology.IO.Dwg.dll` library file available for download [here](http://code.google.com/p/tf-net/downloads/list). Library exposes `DwgReader` and `DwgWriter` classes residing within `Topology.IO.Dwg` namespace.


## Prerequisites ##

References two Autodesk libraries being part of managed ObjectARX. Referenced libraries are `acdbmgd.dll` and `acmgd.dll` which may be found in the root installation folder of the targeted Autodesk platform/vertical. This reader/writer is supposed to work on following versions of basic Autodesk platform: AutoCAD 2005, AutoCAD 2006, AutoCAD 2007 and AutoCAD 2008, including vertical products in the same versions.

Since TF.NET is based on .NET Framework 2.0 specification, you need to have it installed on your system along Autodesk platform. AutoCAD versions 2005 and 2006, including their verticals, do come with .NET 1.1, although they will work even if .NET 2.0 is installed. .NET Framework 2.0 is available for download [here](http://www.microsoft.com/downloads/details.aspx?familyid=0856eacb-4362-4b0d-8edd-aab15c5e04f5&displaylang=en).

Also references TF.NET core library available for download [here](http://code.google.com/p/tf-net/downloads/list).


## Limitations ##

Not all ObjectARX types can be read from or written to.


## Examples ##

Example of simple conversion between JTS and AutoCAD geometries:
  1. Build this example into DLL
  1. Load created DLL into AutoCAD using `NETLOAD` command
  1. Draw either lightweigh or 3D polyline (with or without arc segments)
  1. Run the example using `WRITEPOLYLINE` command and select a polyline
  1. Resulting polyline of chosen type is drawn in red, having it's arc segments tesselated

Source code:

```
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

End Class
```

Example of creating a buffer around AutoCAD polyline:
  1. Build this example into DLL
  1. Load created DLL into AutoCAD using `NETLOAD` command
  1. Draw any type of polyline
  1. Run the example using `CREATEBUFFER` command
  1. Select a polyline (open or closed one)
  1. Enter buffer offset (double value)
  1. Resulting buffer representation is drawn in red, using polyline entities

Buffer around polyline:

![http://www.geoinova.com/examples/buffer_1.jpg](http://www.geoinova.com/examples/buffer_1.jpg)

Buffer around closed polyline:

![http://www.geoinova.com/examples/buffer_2.jpg](http://www.geoinova.com/examples/buffer_2.jpg)

Source code:

```
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
```

Example of merging AutoCAD lines or polylines:
  1. Build this example into DLL
  1. Load created DLL into AutoCAD using `NETLOAD` command
  1. Draw a network of connected lines or polylines
  1. Run the example using `MERGELINEWORK` command
  1. Select lines/polylines for merging
  1. Resulting merged linework is drawn in red, using polyline entities

Before merging - note that linework consists of Line entities only:

![http://www.geoinova.com/examples/merge_before.jpg](http://www.geoinova.com/examples/merge_before.jpg)

After merging - resulting polylines:

![http://www.geoinova.com/examples/merge_after.jpg](http://www.geoinova.com/examples/merge_after.jpg)

Source code:

```
    <CommandMethod("MERGELINEWORK")> _
    Public Shared Sub MergeLinework()
        Dim ed As Editor = Application.DocumentManager.MdiActiveDocument.Editor

        Dim values() As TypedValue = {New TypedValue(DxfCode.Start, "*LINE")}
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
```