Imports Org.OpenGIS.Geometry.Coordinate

Namespace Geometry

    Public Interface IPositionFactory

#Region " CreateDirectPosition "

        Function CreateDirectPosition(ByVal darr As Double()) As IDirectPosition

#End Region

#Region " CreatePointArray "

        Function CreatePointArray() As IPointArray
        Function CreatePointArray(ByVal darr As Double(), ByVal i1 As Integer, ByVal i2 As Integer) As IPointArray
        Function CreatePointArray(ByVal farr As Single(), ByVal i1 As Integer, ByVal i2 As Integer) As IPointArray

#End Region

#Region " CreatePosition "

        Function CreatePosition(ByVal p As IPosition) As IPosition

#End Region

#Region " GetCoordinateReferenceSystem "

        ReadOnly Property CoordinateReferenceSystem() As ICoordinateReferenceSystem

#End Region

#Region " GetPrecision "

        ReadOnly Property Precision() As IPrecision

#End Region

    End Interface

End Namespace
