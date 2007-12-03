namespace Topology.CoordinateSystems
{
    using Topology.Utilities;
    using System;
    using System.Text;

    /// <summary>
    /// Class for defining units
    /// </summary>
    public class Unit : Info, IUnit, IInfo
    {
        private double _ConversionFactor;

        /// <summary>
        /// Initializes a new unit
        /// </summary>
        /// <param name="name">Name of unit</param>
        /// <param name="conversionFactor">Conversion factor to base unit</param>
        internal Unit(string name, double conversionFactor) : this(conversionFactor, name, string.Empty, -1L, string.Empty, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new unit
        /// </summary>
        /// <param name="conversionFactor">Conversion factor to base unit</param>
        /// <param name="name">Name of unit</param>
        /// <param name="authority">Authority name</param>
        /// <param name="authorityCode">Authority-specific identification code.</param>
        /// <param name="alias">Alias</param>
        /// <param name="abbreviation">Abbreviation</param>
        /// <param name="remarks">Provider-supplied remarks</param>
        internal Unit(double conversionFactor, string name, string authority, long authorityCode, string alias, string abbreviation, string remarks) : base(name, authority, authorityCode, alias, abbreviation, remarks)
        {
            this._ConversionFactor = conversionFactor;
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
            if (obj is Unit)
            {
                return ((obj as Unit).ConversionFactor == this.ConversionFactor);
            }
            return false;
        }

        /// <summary>
        /// Gets or sets the number of units per base-unit.
        /// </summary>
        public double ConversionFactor
        {
            get
            {
                return this._ConversionFactor;
            }
            set
            {
                this._ConversionFactor = value;
            }
        }

        /// <summary>
        /// Returns the Well-known text for this object
        /// as defined in the simple features specification.
        /// </summary>
        public override string WKT
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat(NumberFormatter.GetNfi(), "UNIT[\"{0}\", {1}", new object[] { base.Name, this._ConversionFactor });
                if (!string.IsNullOrEmpty(base.Authority) && (base.AuthorityCode > 0L))
                {
                    builder.AppendFormat(", AUTHORITY[\"{0}\", \"{1}\"]", base.Authority, base.AuthorityCode);
                }
                builder.Append("]");
                return builder.ToString();
            }
        }

        /// <summary>
        /// Gets an XML representation of this object [NOT IMPLEMENTED].
        /// </summary>
        public override string XML
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}

