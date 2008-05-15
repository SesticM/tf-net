Imports Topology.Geometries

Namespace IO

    Public MustInherit Class GeometryReaderWriter

        Private m_GeometryFactory As IGeometryFactory
        Private m_AllowRepeatedCoordinates As Boolean

#Region " CTOR "

        Sub New()
        End Sub

        Sub New(ByVal factory As IGeometryFactory)
            MyBase.New()
            m_GeometryFactory = factory
        End Sub

#End Region

#Region " GeometryFactory "

        ''' <summary>
        ''' Returns current <see cref="GeometryFactory"/> used to build geometries.
        ''' </summary>
        ''' <value></value>
        ''' <returns>Current <see cref="GeometryFactory"/> instance.</returns>
        ''' <remarks>
        ''' If there's no <see cref="GeometryFactory"/> set within class constructor,
        ''' a <c>Default</c> factory will be automatically instantiated. Otherwise,
        ''' user-supplied <see cref="GeometryFactory"/> will be used during geometry
        ''' building process.
        ''' </remarks>
        Public ReadOnly Property GeometryFactory() As IGeometryFactory
            Get
                If m_GeometryFactory Is Nothing Then
                    m_GeometryFactory = Topology.Geometries.GeometryFactory.Default
                End If
                Return m_GeometryFactory
            End Get
        End Property

#End Region

#Region " PrecisionModel "

        ''' <summary>
        ''' Returns current <see cref="PrecisionModel"/> of the coordinates within any
        ''' processed <see cref="Geometry"/>.
        ''' </summary>
        ''' <value></value>
        ''' <returns>Current <see cref="GeometryFactory.PrecisionModel"/> instance.</returns>
        ''' <remarks>
        ''' If there's no <see cref="GeometryFactory.PrecisionModel"/> set within class constructor,
        ''' returns default <see cref="GeometryFactory.PrecisionModel"/>. Default precision model is
        ''' <c>Floating</c>, meaning full double precision floating point.
        ''' </remarks>
        Public ReadOnly Property PrecisionModel() As IPrecisionModel
            Get
                Return Me.GeometryFactory.PrecisionModel
            End Get
        End Property

#End Region


#Region " AllowRepeatedCoordinates "

        ''' <summary>
        ''' Gets or sets whether processed geometries include equal sequential
        ''' (repeated) coordinates. Default value is <c>False</c>, meaning that
        ''' any repeated coordinate within given coordinate sequence will get collapsed.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AllowRepeatedCoordinates() As Boolean
            Get
                Return m_AllowRepeatedCoordinates
            End Get
            Set(ByVal value As Boolean)
                m_AllowRepeatedCoordinates = value
            End Set
        End Property

#End Region

    End Class

End Namespace
