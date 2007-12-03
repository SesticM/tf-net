namespace Topology.CoordinateSystems
{
    using System;

    /// <summary>
    /// A one-dimensional coordinate system suitable for vertical measurements.
    /// </summary>
    public interface IVerticalCoordinateSystem : ICoordinateSystem, IInfo
    {
        /// <summary>
        /// Gets the vertical datum, which indicates the measurement method
        /// </summary>
        IVerticalDatum VerticalDatum { get; set; }

        /// <summary>
        /// Gets the units used along the vertical axis.
        /// </summary>
        ILinearUnit VerticalUnit { get; set; }
    }
}

