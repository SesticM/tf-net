namespace Topology.CoordinateSystems
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The IGeographicTransform interface is implemented on geographic transformation
    /// objects and implements datum transformations between geographic coordinate systems.
    /// </summary>
    public interface IGeographicTransform : IInfo
    {
        /// <summary>
        /// Transforms an array of points from the source geographic coordinate system
        /// to the target geographic coordinate system.
        /// </summary>
        /// <param name="point">Points in the source geographic coordinate system</param>
        /// <returns>Points in the target geographic coordinate system</returns>
        List<double[]> Forward(List<double[]> point);
        /// <summary>
        /// Transforms an array of points from the target geographic coordinate system
        /// to the source geographic coordinate system.
        /// </summary>
        /// <param name="point">Points in the target geographic coordinate system</param>
        /// <returns>Points in the source geographic coordinate system</returns>
        List<double[]> Inverse(List<double[]> point);

        /// <summary>
        /// Returns an accessor interface to the parameters for this geographic transformation.
        /// </summary>
        IParameterInfo ParameterInfo { get; }

        /// <summary>
        /// Gets or sets source geographic coordinate system for the transformation.
        /// </summary>
        IGeographicCoordinateSystem SourceGCS { get; set; }

        /// <summary>
        /// Gets or sets the target geographic coordinate system for the transformation.
        /// </summary>
        IGeographicCoordinateSystem TargetGCS { get; set; }
    }
}

