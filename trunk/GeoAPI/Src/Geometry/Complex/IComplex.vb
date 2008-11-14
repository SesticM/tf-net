Namespace Geometry.Complex

    Public Interface IComplex
        Inherits IList(Of IGeometry), ITransfiniteSet

#Region " GetElements "

        'See: IList(Of IGeometry)
        'Function getElements() As Collection

#End Region

#Region " GetSubComplexes "

        ReadOnly Property SubComplexes() As IComplex()

#End Region

#Region " GetSuperComplexes "

        ReadOnly Property SuperComplexes() As IComplex()

#End Region

#Region " IsMaximal "

        ReadOnly Property IsMaximal() As Boolean

#End Region

    End Interface

End Namespace