Imports Org.OpenGIS.Geometry.Coordinate

Namespace Geometry

    Public Interface IDirectPosition
        Inherits IPosition, ICloneable, IEquatable(Of IDirectPosition)

#Region " GetCoordinateReferenceSystem "

        ReadOnly Property GetCoordinateReferenceSystem() As ICoordinateReferenceSystem

#End Region

#Region " GetCoordinates "

        ReadOnly Property Coordinates() As Double()

#End Region

#Region " GetDimension "

        ReadOnly Property Dimension() As Integer

#End Region

#Region " GetOrdinate, SetOrdinate "

        Property Ordinate(ByVal i As Integer) As Double

#End Region

    End Interface

End Namespace

