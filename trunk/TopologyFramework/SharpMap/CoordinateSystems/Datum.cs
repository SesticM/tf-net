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
    public abstract class Datum : Info, IDatum, IInfo
    {
        private Topology.CoordinateSystems.DatumType _DatumType;

        /// <summary>
        /// Initializes a new instance of a Datum object
        /// </summary>
        /// <param name="type">Datum type</param>
        /// <param name="name">Name</param>
        /// <param name="authority">Authority name</param>
        /// <param name="code">Authority-specific identification code.</param>
        /// <param name="alias">Alias</param>
        /// <param name="abbreviation">Abbreviation</param>
        /// <param name="remarks">Provider-supplied remarks</param>
        internal Datum(Topology.CoordinateSystems.DatumType type, string name, string authority, long code, string alias, string remarks, string abbreviation) : base(name, authority, code, alias, abbreviation, remarks)
        {
            this._DatumType = this.DatumType;
        }

        /// <summary>
        /// Checks whether the values of this instance is equal to the values of another instance.
        /// Only parameters used for coordinate system are used for comparison.
        /// Name, abbreviation, authority, alias and remarks are ignored in the comparison.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if equal</returns>
        public override bool EqualParams(object obj)
        {
            if (obj is Ellipsoid)
            {
                return ((obj as Datum).DatumType == this.DatumType);
            }
            return false;
        }

        /// <summary>
        /// Gets or sets the type of the datum as an enumerated code.
        /// </summary>
        public Topology.CoordinateSystems.DatumType DatumType
        {
            get
            {
                return this._DatumType;
            }
            set
            {
                this._DatumType = value;
            }
        }
    }
}

