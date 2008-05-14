namespace Topology.CoordinateSystems
{
    using System;

    /// <summary>
    /// The IGeographicCoordinateSystem interface is a subclass of IGeodeticSpatialReference and
    /// defines the standard information stored with geographic coordinate system objects.
    /// </summary>
    public interface IGeographicCoordinateSystem : IHorizontalCoordinateSystem, ICoordinateSystem, IInfo
    {
        /// <summary>
        /// Gets details on a conversion to WGS84.
        /// </summary>
        Wgs84ConversionInfo GetWgs84ConversionInfo(int index);

        /// <summary>
        /// Gets or sets the angular units of the geographic coordinate system.
        /// </summary>
        IAngularUnit AngularUnit { get; set; }

        /// <summary>
        /// Gets the number of available conversions to WGS84 coordinates.
        /// </summary>
        int NumConversionToWGS84 { get; }

        /// <summary>
        /// Gets or sets the prime meridian of the geographic coordinate system.
        /// </summary>
        IPrimeMeridian PrimeMeridian { get; set; }
    }
}

