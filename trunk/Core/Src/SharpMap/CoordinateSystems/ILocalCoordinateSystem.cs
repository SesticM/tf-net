namespace Topology.CoordinateSystems
{
    using System;

    /// <summary>
    /// A local coordinate system, with uncertain relationship to the world.
    /// </summary>
    /// <remarks>In general, a local coordinate system cannot be related to other coordinate 
    /// systems. However, if two objects supporting this interface have the same dimension, 
    /// axes, units and datum then client code is permitted to assume that the two coordinate
    /// systems are identical. This allows several datasets from a common source (e.g. a CAD
    /// system) to be overlaid. In addition, some implementations of the Coordinate 
    /// Transformation (CT) package may have a mechanism for correlating local datums. (E.g. 
    /// from a database of transformations, which is created and maintained from real-world 
    /// measurements.)
    /// </remarks>
    public interface ILocalCoordinateSystem : ICoordinateSystem, IInfo
    {
        /// <summary>
        /// Gets or sets the local datum
        /// </summary>
        ILocalDatum LocalDatum { get; set; }
    }
}

