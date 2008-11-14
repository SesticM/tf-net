Namespace MetaData.Extent

    Public Interface IGeographicBoundingBox
        Inherits IGeographicExtent

#Region " GetEastBoundLongitude "

        ReadOnly Property EastBoundLongitude() As Double

#End Region

#Region " GetNorthBoundLongitude "

        ReadOnly Property NorthBoundLongitude() As Double

#End Region

#Region " GetSouthBoundLongitude "

        ReadOnly Property SouthBoundLongitude() As Double

#End Region

#Region " GetWestBoundLongitude "

        ReadOnly Property WestBoundLongitude() As Double

#End Region

    End Interface

End Namespace