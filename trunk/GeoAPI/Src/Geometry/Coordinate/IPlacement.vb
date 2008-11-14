Namespace Geometry.Coordinate

    Public Interface IPlacement

#Region " GetInDimension "

        ReadOnly Property InDimension() As Integer

#End Region

#Region " GetOutDimension "

        ReadOnly Property OutDimension() As Integer

#End Region

#Region " Transform "

        Function Transform(ByVal darr As Double()) As Double()

#End Region

    End Interface

End Namespace
