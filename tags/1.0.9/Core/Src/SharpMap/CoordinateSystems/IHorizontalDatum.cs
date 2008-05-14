namespace Topology.CoordinateSystems
{
    using System;

    /// <summary>
    /// Procedure used to measure positions on the surface of the Earth.
    /// </summary>
    public interface IHorizontalDatum : IDatum, IInfo
    {
        /// <summary>
        /// Gets or sets the ellipsoid of the datum.
        /// </summary>
        IEllipsoid Ellipsoid { get; set; }

        /// <summary>
        /// Gets preferred parameters for a Bursa Wolf transformation into WGS84. The 7 returned values 
        /// correspond to (dx,dy,dz) in meters, (ex,ey,ez) in arc-seconds, and scaling in parts-per-million.
        /// </summary>
        Wgs84ConversionInfo Wgs84Parameters { get; set; }
    }
}

