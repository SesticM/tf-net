namespace Topology.CoordinateSystems
{
    using System;

    /// <summary>
    /// The IProjection interface defines the standard information stored with projection
    /// objects. A projection object implements a coordinate transformation from a geographic
    /// coordinate system to a projected coordinate system, given the ellipsoid for the
    /// geographic coordinate system. It is expected that each coordinate transformation of
    /// interest, e.g., Transverse Mercator, Lambert, will be implemented as a COM class of
    /// coType Projection, supporting the IProjection interface.
    /// </summary>
    public interface IProjection : IInfo
    {
        /// <summary>
        /// Gets an indexed parameter of the projection.
        /// </summary>
        /// <param name="n">Index of parameter</param>
        /// <returns>n'th parameter</returns>
        ProjectionParameter GetParameter(int n);
        /// <summary>
        /// Gets an named parameter of the projection.
        /// </summary>
        /// <remarks>The parameter name is case insensitive</remarks>
        /// <param name="name">Name of parameter</param>
        /// <returns>parameter or null if not found</returns>
        ProjectionParameter GetParameter(string name);

        /// <summary>
        /// Gets the projection classification name (e.g. 'Transverse_Mercator').
        /// </summary>
        string ClassName { get; }

        /// <summary>
        /// Gets number of parameters of the projection.
        /// </summary>
        int NumParameters { get; }
    }
}

