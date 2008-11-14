Namespace Geometry.Coordinate

    Public Interface IPointArray
        Inherits IList(Of IDirectPosition)

#Region " GetCoordinateReferenceSystem "

        ReadOnly Property CoordinateReferenceSystem() As ICoordinateReferenceSystem

#End Region

#Region " GetDimension "

        ReadOnly Property Dimension() As Integer

#End Region

    End Interface

End Namespace
