namespace Topology.CoordinateSystems
{
    using System;

    /// <summary>
    /// The IAngularUnit interface defines methods on angular units.
    /// </summary>
    public interface IAngularUnit : IUnit, IInfo
    {
        /// <summary>
        /// Gets or sets the number of radians per angular unit.
        /// </summary>
        double RadiansPerUnit { get; set; }
    }
}

