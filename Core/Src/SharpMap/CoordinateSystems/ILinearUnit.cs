namespace Topology.CoordinateSystems
{
    using System;

    /// <summary>
    /// The ILinearUnit interface defines methods on linear units.
    /// </summary>
    public interface ILinearUnit : IUnit, IInfo
    {
        /// <summary>
        /// Gets or sets the number of meters per <see cref="T:Topology.CoordinateSystems.ILinearUnit" />.
        /// </summary>
        double MetersPerUnit { get; set; }
    }
}

