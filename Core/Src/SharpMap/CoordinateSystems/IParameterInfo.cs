namespace Topology.CoordinateSystems
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The IParameterInfo interface provides an interface through which clients of a
    /// Projected Coordinate System or of a Projection can set the parameters of the
    /// projection. It provides a generic interface for discovering the names and default
    /// values of parameters, and for setting and getting parameter values. Subclasses of
    /// this interface may provide projection specific parameter access methods.
    /// </summary>
    public interface IParameterInfo
    {
        /// <summary>
        /// Returns the default parameters for this projection.
        /// </summary>
        /// <returns></returns>
        Parameter[] DefaultParameters();
        /// <summary>
        /// Gets the parameter by its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Parameter GetParameterByName(string name);

        /// <summary>
        /// Gets the number of parameters expected.
        /// </summary>
        int NumParameters { get; }

        /// <summary>
        /// Gets or sets the parameters set for this projection.
        /// </summary>
        List<Parameter> Parameters { get; set; }
    }
}

