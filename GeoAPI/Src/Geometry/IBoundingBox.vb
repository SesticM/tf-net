Namespace Geometry

    Public Interface IBoundingBox
        Inherits IEnvelope

#Region " Contains "

        Function Contains(ByVal bb As IBoundingBox) As Boolean
        Function Contains(ByVal dp As IDirectPosition) As Boolean
        Function Contains(ByVal d1 As Double, ByVal d2 As Double) As Boolean

#End Region

#Region " GetHeight "

        ReadOnly Property Height() As Double

#End Region

#Region " GetMaxX "

        ReadOnly Property MaxX() As Double

#End Region

#Region " GetMaxY "

        ReadOnly Property MaxY() As Double

#End Region

#Region " GetMinX "

        ReadOnly Property MinX() As Double

#End Region

#Region " GetMinY "

        ReadOnly Property MinY() As Double

#End Region

#Region " GetWidth "

        ReadOnly Property Width() As Double

#End Region

#Region " Include "

        Sub Include(ByVal bb As IBoundingBox)
        Sub Include(ByVal d1 As Double, ByVal d2 As Double)

#End Region

#Region " Intersects "

        Function Intersects(ByVal bb As IBoundingBox) As Boolean

#End Region

#Region " IsEmpty "

        ReadOnly Property IsEmpty() As Boolean

#End Region

#Region " SetBounds "

        WriteOnly Property Bounds() As IBoundingBox
        'Sub SetBounds(ByVal bb As BoundingBox)

#End Region

#Region " ToBounds "

        Function ToBounds(ByVal crs As ICoordinateReferenceSystem) As IBoundingBox

#End Region

    End Interface

End Namespace
