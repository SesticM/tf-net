Namespace Geometry

    Public Interface IPrecision
        Inherits IComparable(Of IPrecision)

#Region " GetMaximumSignificantDigits "

        ReadOnly Property MaximumSignificantDigits() As Integer

#End Region

#Region " GetScale "

        ReadOnly Property Scale() As Double

#End Region

#Region " GetType "

        Function Type() As PrecisionType

#End Region


#Region " Round "

        Sub Round(ByVal dp As IDirectPosition)

#End Region

    End Interface

End Namespace
