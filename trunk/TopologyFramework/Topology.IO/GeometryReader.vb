Imports Topology.Geometries

Namespace IO

#Region " CurveTessellation Enum "

    ''' <summary>
    ''' Defines algorithm used for curve-based geometries tesselation.
    ''' <para>
    ''' Curves need to be tessellated (broken up into lines) in order to be converted to
    ''' JTS feature representation. The degree of tessellation determines how accurate the
    ''' converted curve will be (how close it will approximate the original curve geometry)
    ''' and how much performance overhead is required to generate the representation of a curve.
    ''' </para>
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum CurveTessellation
        ''' <summary>
        ''' No tessellation, meaning that curves will get converted either into straight segments or
        ''' generic curves, depending on reader/writer curve-based geometry implementation.
        ''' <see cref="GeometryReader.CurveTessellationValue"/> parameter is ignored.
        ''' </summary>
        ''' <remarks></remarks>
        None

        ''' <summary>
        ''' Curves are tessellated by means of linear points sampling. Linear sampling
        ''' divides a curve into equal-sized segments. Number of segments depends on the
        ''' value stored in <see cref="GeometryReader.CurveTessellationValue"/>. If 
        ''' <see cref="GeometryReader.CurveTessellationValue"/> is negative or zero,
        ''' default value used for tessellation is <c>16</c>.
        ''' </summary>
        ''' <remarks></remarks>
        Linear

        ''' <summary>
        ''' Curves are tessellated by means of linear points sampling, where size of each
        ''' equal-sized segment depends on size (scale) of overall curve geometry. Smaller
        ''' the geometry, resulting number of segments is lower. Similar algorithm is 
        ''' used for on-screen curve tesselation.
        ''' Overall scale factor can be set via <see cref="GeometryReader.CurveTessellationValue"/>
        ''' property value. If <see cref="GeometryReader.CurveTessellationValue"/> is
        ''' negative or zero, default value used for tessellation is <c>1</c>.
        ''' </summary>
        ''' <remarks></remarks>
        Scaled
    End Enum

#End Region

    Public MustInherit Class GeometryReader
        Inherits GeometryReaderWriter

        Private m_CurveTessellationMethod As CurveTessellation = CurveTessellation.Linear
        Private m_CurveTessellationValue As Double = 15

#Region " CTOR "

        Sub New()
            MyBase.New()
        End Sub

        Sub New(ByVal factory As IGeometryFactory)
            MyBase.New(factory)
        End Sub

#End Region


#Region " CurveTessellationMethod "

        ''' <summary>
        ''' Method used for curve-based geometries tesselation. For more information
        ''' see <see cref="Topology.IO.CurveTessellation"/> enumerator description.
        ''' Default value is <see cref="Topology.IO.CurveTessellation.Linear"/>.
        ''' </summary>
        ''' <value>Method used for curve-based geometries tesselation.</value>
        ''' <remarks></remarks>
        Public Property CurveTessellationMethod() As Topology.IO.CurveTessellation
            Get
                Return m_CurveTessellationMethod
            End Get
            Set(ByVal value As Topology.IO.CurveTessellation)
                value = m_CurveTessellationMethod
            End Set
        End Property

#End Region

#Region " CurveTessellationValue "

        ''' <summary>
        ''' Gets or sets a parameter for curve tessellation method set by <see cref="CurveTessellationMethod"/>.
        ''' For exact parameter definition see <see cref="Topology.IO.CurveTessellation"/> enumerator description.
        ''' </summary>
        ''' <value>Curve tessellation method parameter value.</value>
        Public Property CurveTessellationValue() As Double
            Get
                Select Case Me.CurveTessellationMethod
                    Case IO.CurveTessellation.Linear
                        If m_CurveTessellationValue > 0 Then
                            Return m_CurveTessellationValue
                        Else
                            Return 16
                        End If

                    Case IO.CurveTessellation.Scaled
                        If m_CurveTessellationValue > 0 Then
                            Return m_CurveTessellationValue
                        Else
                            Return 1
                        End If

                    Case Else
                        Return 0
                End Select
            End Get
            Set(ByVal value As Double)
                value = m_CurveTessellationValue
            End Set
        End Property

#End Region

    End Class

End Namespace
