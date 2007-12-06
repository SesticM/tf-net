Imports Topology.Geometries
Imports Topology.Utilities
Imports Topology.Precision

Namespace Conflation.Geometries

    ''' <summary>
    ''' Improves the robustness of buffer computation by using small
    ''' perturbations of the buffer distance. Also used enhanced precision.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class BufferDistancePerturber

        Private distance As Double
        Private maximumPerturbation As Double

        Sub New(ByVal distance As Double, ByVal maximumPerturbation As Double)
            Me.distance = distance
            Me.maximumPerturbation = maximumPerturbation
        End Sub

        Public Shared Function SafeBuffer(ByVal geom As Geometry, ByVal distance As Double) As Geometry
            Dim buffer As Geometry = Nothing

            Try
                'buffer = EnhancedPrecisionOp.Buffer(geom, distance)
                buffer = geom.Buffer(distance)
            Catch ex As AssertionFailedException
                ' eat the exception
            End Try

            Return buffer
        End Function

        ''' <summary>
        ''' Attempts to compute a buffer using small perturbations of the buffer distance
        ''' if necessary.  If this routine is unable to perform the buffer computation correctly
        ''' the orginal buffer exception will be propagated.
        ''' </summary>
        ''' <param name="geom">The Geometry to compute the buffer for.</param>
        ''' <returns>The buffer of the input Geometry.</returns>
        ''' <remarks></remarks>
        Public Function Buffer(ByVal geom As Geometry) As Geometry
            Dim sBuffer As Geometry = SafeBuffer(geom, distance)

            If (isBufferComputedCorrectly(geom, sBuffer)) Then
                Return sBuffer
            Else
                Debug.Assert("Buffer robustness error found.")
            End If
            sBuffer = SafeBuffer(geom, distance + maximumPerturbation)

            If (isBufferComputedCorrectly(geom, sBuffer)) Then
                Return sBuffer
            End If

            Return geom.Buffer(distance - maximumPerturbation)
        End Function

        ''' <summary>
        ''' Check various assertions about the geometry and the buffer to try to determine
        ''' whether the JTS buffer function failed to compute the buffer correctly.
        ''' These are heuristics only - this may not catch all errors.
        ''' </summary>
        ''' <param name="geom">The geometry.</param>
        ''' <param name="buffer">The buffer computed by JTS.</param>
        ''' <returns><c>True</c> if the buffer seems to be correct.</returns>
        ''' <remarks></remarks>
        Private Function IsBufferComputedCorrectly(ByVal geom As Geometry, ByVal buffer As Geometry) As Boolean
            If buffer Is Nothing Then
                Return False
            End If

            ' sometimes buffer() computes empty geometries
            If Not geom.IsEmpty AndAlso buffer.IsEmpty Then
                Return False
            End If

            ' sometimes buffer() computes a very small geometry as the buffer
            If Not buffer.EnvelopeInternal.Contains(geom.EnvelopeInternal) Then
                Return False
            End If

            Return True
        End Function

    End Class

End Namespace


