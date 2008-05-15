namespace Topology.CoordinateSystems
{
    using System;

    /// <summary>
    /// A 3D coordinate system, with its origin at the center of the Earth.
    /// </summary>
    public interface IGeocentricCoordinateSystem : ICoordinateSystem, IInfo
    {
        /// <summary>
        /// Returns the HorizontalDatum. The horizontal datum is used to determine where
        /// the centre of the Earth is considered to be. All coordinate points will be 
        /// measured from the centre of the Earth, and not the surface.
        /// </summary>
        IHorizontalDatum HorizontalDatum { get; set; }

        /// <summary>
        /// Gets the units used along all the axes.
        /// </summary>
        ILinearUnit LinearUnit { get; set; }

        /// <summary>
        /// Returns the PrimeMeridian.
        /// </summary>
        IPrimeMeridian PrimeMeridian { get; set; }
    }
}

