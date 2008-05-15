namespace Topology.CoordinateSystems
{
    using Topology.Utilities;
    using System;
    using System.Text;

    /// <summary>
    /// Definition of angular units.
    /// </summary>
    public class AngularUnit : Info, IAngularUnit, IUnit, IInfo
    {
        private double _RadiansPerUnit;

        /// <summary>
        /// Initializes a new instance of a angular unit
        /// </summary>
        /// <param name="radiansPerUnit">Radians per unit</param>
        public AngularUnit(double radiansPerUnit) : this(radiansPerUnit, string.Empty, string.Empty, -1L, string.Empty, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of a angular unit
        /// </summary>
        /// <param name="radiansPerUnit">Radians per unit</param>
        /// <param name="name">Name</param>
        /// <param name="authority">Authority name</param>
        /// <param name="authorityCode">Authority-specific identification code.</param>
        /// <param name="alias">Alias</param>
        /// <param name="abbreviation">Abbreviation</param>
        /// <param name="remarks">Provider-supplied remarks</param>
        internal AngularUnit(double radiansPerUnit, string name, string authority, long authorityCode, string alias, string abbreviation, string remarks) : base(name, authority, authorityCode, alias, abbreviation, remarks)
        {
            this._RadiansPerUnit = radiansPerUnit;
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
            if (obj is AngularUnit)
            {
                return ((obj as AngularUnit).RadiansPerUnit == this.RadiansPerUnit);
            }
            return false;
        }

        /// <summary>
        /// The angular degrees are PI/180 = 0.017453292519943295769236907684886 radians
        /// </summary>
        public static AngularUnit Degrees
        {
            get
            {
                return new AngularUnit(0.017453292519943295, "degree", "EPSG", 0x238eL, "deg", string.Empty, "=pi/180 radians");
            }
        }

        /// <summary>
        /// Pi / 200 = 0.015707963267948966192313216916398 radians
        /// </summary>
        public static AngularUnit Gon
        {
            get
            {
                return new AngularUnit(0.015707963267948967, "gon", "EPSG", 0x2392L, "g", string.Empty, "=pi/200 radians.");
            }
        }

        /// <summary>
        /// Pi / 200 = 0.015707963267948966192313216916398 radians
        /// </summary>
        public static AngularUnit Grad
        {
            get
            {
                return new AngularUnit(0.015707963267948967, "grad", "EPSG", 0x2391L, "gr", string.Empty, "=pi/200 radians.");
            }
        }

        /// <summary>
        /// SI standard unit
        /// </summary>
        public static AngularUnit Radian
        {
            get
            {
                return new AngularUnit(1, "radian", "EPSG", 0x238dL, "rad", string.Empty, "SI standard unit.");
            }
        }

        /// <summary>
        /// Gets or sets the number of radians per <see cref="T:Topology.CoordinateSystems.AngularUnit" />.
        /// </summary>
        public double RadiansPerUnit
        {
            get
            {
                return this._RadiansPerUnit;
            }
            set
            {
                this._RadiansPerUnit = value;
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
                builder.AppendFormat(NumberFormatter.GetNfi(), "UNIT[\"{0}\", {1}", new object[] { base.Name, this.RadiansPerUnit });
                if (!string.IsNullOrEmpty(base.Authority) && (base.AuthorityCode > 0L))
                {
                    builder.AppendFormat(", AUTHORITY[\"{0}\", \"{1}\"]", base.Authority, base.AuthorityCode);
                }
                builder.Append("]");
                return builder.ToString();
            }
        }

        /// <summary>
        /// Gets an XML representation of this object.
        /// </summary>
        public override string XML
        {
            get
            {
                return string.Format(NumberFormatter.GetNfi(), "<CS_AngularUnit RadiansPerUnit=\"{0}\">{1}</CS_AngularUnit>", new object[] { this.RadiansPerUnit, base.InfoXml });
            }
        }
    }
}

