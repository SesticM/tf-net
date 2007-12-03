namespace Topology.CoordinateSystems
{
    using System;

    /// <summary>
    /// A 2D coordinate system suitable for positions on the Earth's surface.
    /// </summary>
    public interface IHorizontalCoordinateSystem : ICoordinateSystem, IInfo
    {
        /// <summary>
        /// Returns the HorizontalDatum.
        /// </summary>
        IHorizontalDatum HorizontalDatum { get; set; }
    }
}

