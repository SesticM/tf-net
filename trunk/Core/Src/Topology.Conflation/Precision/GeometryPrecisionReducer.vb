Imports Topology.Geometries
Imports Topology.Geometries.Utilities

Namespace Conflation.Precision

    ''' <summary>
    ''' Reduces the precision of a {@link Geometry} according to the supplied CoordinatePrecisionReducer.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class GeometryPrecisionReducer

        Private Shared coordPrecReducer As CoordinatePrecisionReducer
        Private geomEdit As GeometryEditor = New GeometryEditor()

        Sub New(ByVal numberPrecReducer As NumberPrecisionReducer)
            GeometryPrecisionReducer.coordPrecReducer = New CoordinatePrecisionReducer(numberPrecReducer)
        End Sub

        Public Function Reduce(ByVal geom As Geometry) As Geometry
            Return geomEdit.Edit(geom, New PrecisionReducerCoordinateOperation())
        End Function

        Private Class PrecisionReducerCoordinateOperation
            Inherits GeometryEditor.CoordinateOperation

            Public Overloads Overrides Function Edit(ByVal coordinates() As ICoordinate, ByVal geometry As IGeometry) As ICoordinate()
                For i As Integer = 0 To coordinates.Length
                    coordPrecReducer.ReducePrecision(coordinates(i))
                Next

                ' remove repeated points
                Dim noRepeatedCoordList As CoordinateList = New CoordinateList(coordinates, False)
                Dim noRepeatedCoord As Coordinate() = noRepeatedCoordList.ToCoordinateArray()

                ' Check to see if the removal of repeated points collapsed the coordinate 
                ' List to an invalid length for the type of the parent geometry.
                '* If this is the case, return the orginal coordinate list.
                '* Note that the returned geometry will still be invalid, since it
                '* has fewer unique coordinates than required. This check simply
                '* ensures that the Geometry constructors won't fail.
                '* It is not necessary to check for Point collapses, since the coordinate list can
                '* never collapse to less than one point
                If TypeOf (geometry) Is LinearRing AndAlso noRepeatedCoord.Length <= 3 Then
                    Return coordinates
                End If
                If TypeOf (geometry) Is LineString AndAlso noRepeatedCoord.Length <= 1 Then
                    Return coordinates
                End If

                Return noRepeatedCoord
            End Function

        End Class

    End Class

End Namespace
