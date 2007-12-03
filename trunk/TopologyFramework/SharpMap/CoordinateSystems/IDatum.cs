namespace Topology.CoordinateSystems
{
    using System;

    /// <summary>
    /// A set of quantities from which other quantities are calculated.
    /// </summary>
    /// <remarks>
    /// For the OGC abstract model, it can be defined as a set of real points on the earth 
    /// that have coordinates. EG. A datum can be thought of as a set of parameters 
    /// defining completely the origin and orientation of a coordinate system with respect 
    /// to the earth. A textual description and/or a set of parameters describing the 
    /// relationship of a coordinate system to some predefined physical locations (such 
    /// as center of mass) and physical directions (such as axis of spin). The definition 
    /// of the datum may also include the temporal behavior (such as the rate of change of
    /// the orientation of the coordinate axes).
    /// </remarks>
    public interface IDatum : IInfo
    {
        /// <summary>
        /// Gets or sets the type of the datum as an enumerated code.
        /// </summary>
        Topology.CoordinateSystems.DatumType DatumType { get; set; }
    }
}

