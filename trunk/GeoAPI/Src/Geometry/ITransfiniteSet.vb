Namespace Geometry

    Public Interface ITransfiniteSet
        Inherits IEquatable(Of ITransfiniteSet)

#Region " Contains "

        Function Contains(ByVal dp As IDirectPosition) As Boolean
        Function Contains(ByVal ts As ITransfiniteSet) As Boolean

#End Region

#Region " Difference "

        Function Difference(ByVal ts As ITransfiniteSet) As ITransfiniteSet

#End Region

#Region " Intersection "

        Function Intersection(ByVal ts As ITransfiniteSet) As ITransfiniteSet

#End Region

#Region " Intersects "

        Function Intersects(ByVal ts As ITransfiniteSet) As Boolean

#End Region

#Region " SymmetricDifference "

        Function SymmetricDifference(ByVal ts As ITransfiniteSet) As ITransfiniteSet

#End Region

#Region " Union "

        Function Union(ByVal ts As ITransfiniteSet) As ITransfiniteSet

#End Region

    End Interface

End Namespace
