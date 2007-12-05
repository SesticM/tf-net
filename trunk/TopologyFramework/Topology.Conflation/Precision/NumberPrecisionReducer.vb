Imports Topology.Geometries

Namespace Conflation.Precision

    ''' <summary>
    ''' Reduces the precision of a number by rounding it off after scaling by a given scale factor.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NumberPrecisionReducer

        Private scaleFactor As Double = 0.0
        Private multiplyByScaleFactor As Boolean = True

        Sub New()
        End Sub

        ''' <summary>
        ''' A negative value for scaleFactor indicates that the precision reduction 
        ''' will eliminate significant digits to the left of the decimal point.
        ''' (I.e. the scale factor will be divided rather than multiplied).
        ''' A zero value for scaleFactor will result in no precision reduction being 
        ''' performed. A scale factor is normally an integer value.
        ''' </summary>
        ''' <param name="scaleFactor"></param>
        ''' <remarks></remarks>
        Sub New(ByVal scaleFactor As Double)
            setScaleFactor(scaleFactor)
        End Sub

        ''' <summary>
        ''' Computes the scale factor for a given number of decimal places.
        ''' A negative value for decimalPlaces indicates the scale factor
        ''' should be divided rather than multiplied. The negative sign
        ''' is carried through to the computed scale factor.
        ''' </summary>
        ''' <param name="decimalPlaces"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ScaleFactorForDecimalPlaces(ByVal decimalPlaces As Integer) As Double
            Dim power As Integer = Math.Abs(decimalPlaces)
            Dim sign As Integer = IIf(decimalPlaces >= 0, 1, -1)
            Dim scaleFactor As Double = 1.0

            For i As Integer = 1 To power
                scaleFactor *= 10.0
            Next

            Return scaleFactor * sign
        End Function

        Public Sub SetScaleFactor(ByVal scaleFactor As Double)
            Me.scaleFactor = Math.Abs(scaleFactor)
            multiplyByScaleFactor = scaleFactor >= 0
        End Sub

        Public Function ReducePrecision(ByVal d As Double) As Double
            ' sanity check
            If (scaleFactor = 0.0) Then Return d

            If (multiplyByScaleFactor) Then
                Dim scaled As Double = d + scaleFactor
                Return Math.Floor(scaled + 0.5) / scaleFactor
            Else
                Dim scaled As Double = d / scaleFactor
                Return Math.Floor(scaled + 0.5) * scaleFactor
            End If
        End Function

    End Class

End Namespace
