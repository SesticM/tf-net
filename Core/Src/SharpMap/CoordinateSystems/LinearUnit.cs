namespace Topology.CoordinateSystems
{
    using Topology.Utilities;
    using System;
    using System.Text;

    /// <summary>
    /// Definition of linear units.
    /// </summary>
    public class LinearUnit : Info, ILinearUnit, IUnit, IInfo
    {
        private double _MetersPerUnit;

        /// <summary>
        /// Creates an instance of a linear unit
        /// </summary>
        /// <param name="metersPerUnit">Number of meters per <see cref="T:Topology.CoordinateSystems.LinearUnit" /></param>
        /// <param name="name">Name</param>
        /// <param name="authority">Authority name</param>
        /// <param name="authorityCode">Authority-specific identification code.</param>
        /// <param name="alias">Alias</param>
        /// <param name="abbreviation">Abbreviation</param>
        /// <param name="remarks">Provider-supplied remarks</param>
        public LinearUnit(double metersPerUnit, string name, string authority, long authorityCode, string alias, string abbreviation, string remarks) : base(name, authority, authorityCode, alias, abbreviation, remarks)
        {
            this._MetersPerUnit = metersPerUnit;
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
            if (obj is LinearUnit)
            {
                return ((obj as LinearUnit).MetersPerUnit == this.MetersPerUnit);
            }
            return false;
        }

        /// <summary>
        /// Returns Clarke's foot.
        /// </summary>
        /// <remarks>
        /// Assumes Clarke's 1865 ratio of 1 British foot = 0.3047972654 French legal metres applies to the international metre. 
        /// Used in older Australian, southern African &amp; British West Indian mapping.
        /// </remarks>
        public static ILinearUnit ClarkesFoot
        {
            get
            {
                return new LinearUnit(0.3047972654, "Clarke's foot", "EPSG", 0x232dL, "Clarke's foot", string.Empty, "Assumes Clarke's 1865 ratio of 1 British foot = 0.3047972654 French legal metres applies to the international metre. Used in older Australian, southern African & British West Indian mapping.");
            }
        }

        /// <summary>
        /// Returns the foot linear unit (1ft = 0.3048m).
        /// </summary>
        public static ILinearUnit Foot
        {
            get
            {
                return new LinearUnit(0.3048, "foot", "EPSG", 0x232aL, "ft", string.Empty, string.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the number of meters per <see cref="T:Topology.CoordinateSystems.LinearUnit" />.
        /// </summary>
        public double MetersPerUnit
        {
            get
            {
                return this._MetersPerUnit;
            }
            set
            {
                this._MetersPerUnit = value;
            }
        }

        /// <summary>
        /// Returns the meters linear unit.
        /// Also known as International metre. SI standard unit.
        /// </summary>
        public static ILinearUnit Metre
        {
            get
            {
                return new LinearUnit(1, "metre", "EPSG", 0x2329L, "m", string.Empty, "Also known as International metre. SI standard unit.");
            }
        }

        /// <summary>
        /// Returns the Nautical Mile linear unit (1NM = 1852m).
        /// </summary>
        public static ILinearUnit NauticalMile
        {
            get
            {
                return new LinearUnit(1852, "nautical mile", "EPSG", 0x2346L, "NM", string.Empty, string.Empty);
            }
        }

        /// <summary>
        /// Returns the US Survey foot linear unit (1ftUS = 0.304800609601219m).
        /// </summary>
        public static ILinearUnit USSurveyFoot
        {
            get
            {
                return new LinearUnit(0.304800609601219, "US survey foot", "EPSG", 0x232bL, "American foot", "ftUS", "Used in USA.");
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
                builder.AppendFormat(NumberFormatter.GetNfi(), "UNIT[\"{0}\", {1}", new object[] { base.Name, this.MetersPerUnit });
                if (!string.IsNullOrEmpty(base.Authority) && (base.AuthorityCode > 0L))
                {
                    builder.AppendFormat(", AUTHORITY[\"{0}\", \"{1}\"]", base.Authority, base.AuthorityCode);
                }
                builder.Append("]");
                return builder.ToString();
            }
        }

        /// <summary>
        /// Gets an XML representation of this object
        /// </summary>
        public override string XML
        {
            get
            {
                return string.Format(NumberFormatter.GetNfi(), "<CS_LinearUnit MetersPerUnit=\"{0}\">{1}</CS_LinearUnit>", new object[] { this.MetersPerUnit, base.InfoXml });
            }
        }
    }
}

