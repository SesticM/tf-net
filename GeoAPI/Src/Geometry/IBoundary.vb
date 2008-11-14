Imports Org.OpenGIS.Geometry.Complex

Namespace Geometry

    Public Interface IBoundary
        Inherits IComplex, IGeometry, ITransfiniteSet

#Region " IsCycle "

        ReadOnly Property IsCycle() As Boolean

#End Region

    End Interface

End Namespace