namespace Topology.CoordinateSystems
{
    using System;

    /// <summary>
    /// The IPrimeMeridian interface defines the standard information stored with prime
    /// meridian objects. Any prime meridian object must implement this interface as
    /// well as the ISpatialReferenceInfo interface.
    /// </summary>
    public interface IPrimeMeridian : IInfo
    {
        /// <summary>
        /// Gets or sets the AngularUnits.
        /// </summary>
        IAngularUnit AngularUnit { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the prime meridian (relative to the Greenwich prime meridian).
        /// </summary>
        double Longitude { get; set; }
    }
}

