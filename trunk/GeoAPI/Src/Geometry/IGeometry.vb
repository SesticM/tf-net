Imports Org.OpenGIS.Geometry.Complex

Namespace Geometry

    Public Interface IGeometry
        Inherits ITransfiniteSet, ICloneable

#Region " Distance "

        'Function Distance(ByVal g As IGeometry) As Double
        ReadOnly Property Distance(ByVal g As IGeometry) As Double

#End Region

#Region " GetBoundary "

        ReadOnly Property Boundary() As IBoundary

#End Region

#Region " GetBuffer "

        ReadOnly Property Buffer(ByVal d As Double) As IGeometry

#End Region

#Region " GetCentroid "

        ReadOnly Property Centroid() As IDirectPosition

#End Region

#Region " GetClosure "

        ReadOnly Property Closure() As IComplex

#End Region

#Region " GetConvexHull "

        ReadOnly Property ConvexHull() As IGeometry

#End Region

#Region " GetCoordinateDimension "

        ReadOnly Property CoordinateDimension() As Integer

#End Region

#Region " GetCoordinateReferenceSystem "

        ReadOnly Property CoordinateReferenceSystem() As ICoordinateReferenceSystem

#End Region

#Region " GetDimension "

        ReadOnly Property Dimension(ByVal dp As IDirectPosition) As Integer

#End Region

#Region " GetEnvelope "

        ReadOnly Property Envelope() As IEnvelope

#End Region

#Region " GetMaximalComplex "

        ReadOnly Property MaximalComplex() As ISet

#End Region

#Region " GetMbRegion "

        ReadOnly Property MbRegion() As IGeometry

#End Region

#Region " GetPrecision "

        ReadOnly Property Precision() As IPrecision

#End Region

#Region " GetRepresentativePoint "

        ReadOnly Property RepresentativePoint() As IDirectPosition

#End Region

#Region " IsCycle "

        ReadOnly Property IsCycle() As Boolean

#End Region

#Region " IsMutable "

        ReadOnly Property IsMutable() As Boolean

#End Region

#Region " IsSimple "

        ReadOnly Property IsSimple() As Boolean

#End Region

#Region " ToImmutable "

        Function ToImmutable() As IGeometry

#End Region

#Region " Transform "

        Function Transform(ByVal crs As ICoordinateReferenceSystem) As IGeometry
        Function Transform(ByVal crs As ICoordinateReferenceSystem, ByVal mt As IMathTransform) As IGeometry

#End Region

    End Interface

End Namespace
