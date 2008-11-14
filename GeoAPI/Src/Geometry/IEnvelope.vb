Namespace Geometry

    Public Interface IEnvelope

#Region " GetCenter "

        ReadOnly Property Center(ByVal i As Integer) As Double

#End Region

#Region " GetCoordinateReferenceSystem "

        ReadOnly Property CoordinateReferenceSystem() As ICoordinateReferenceSystem

#End Region

#Region " GetDimension "

        ReadOnly Property Dimension() As Integer

#End Region

#Region " GetLength "

        ReadOnly Property Length(ByVal i As Integer) As Double

#End Region

#Region " GetLowerCorner "

        ReadOnly Property LowerCorner() As IDirectPosition

#End Region

#Region " GetMaximum "

        ReadOnly Property Maximum(ByVal i As Integer) As Double

#End Region

#Region " GetMinimum "

        ReadOnly Property Minimum(ByVal i As Integer) As Double

#End Region

#Region " GetUpperCorner "

        ReadOnly Property UpperCorner() As IDirectPosition

#End Region

    End Interface

End Namespace
