namespace Topology.CoordinateSystems
{
    using Topology.Utilities;
    using System;
    using System.Text;

    /// <summary>
    /// The IEllipsoid interface defines the standard information stored with ellipsoid objects.
    /// </summary>
    public class Ellipsoid : Info, IEllipsoid, IInfo
    {
        private ILinearUnit _AxisUnit;
        private double _InverseFlattening;
        private bool _IsIvfDefinitive;
        private double _SemiMajorAxis;
        private double _SemiMinorAxis;

        /// <summary>
        /// Initializes a new instance of an Ellipsoid
        /// </summary>
        /// <param name="semiMajorAxis">Semi major axis</param>
        /// <param name="semiMinorAxis">Semi minor axis</param>
        /// <param name="inverseFlattening">Inverse flattening</param>
        /// <param name="isIvfDefinitive">Inverse Flattening is definitive for this ellipsoid (Semi-minor axis will be overridden)</param>
        /// <param name="axisUnit">Axis unit</param>
        /// <param name="name">Name</param>
        /// <param name="authority">Authority name</param>
        /// <param name="code">Authority-specific identification code.</param>
        /// <param name="alias">Alias</param>
        /// <param name="abbreviation">Abbreviation</param>
        /// <param name="remarks">Provider-supplied remarks</param>
        internal Ellipsoid(double semiMajorAxis, double semiMinorAxis, double inverseFlattening, bool isIvfDefinitive, ILinearUnit axisUnit, string name, string authority, long code, string alias, string abbreviation, string remarks) : base(name, authority, code, alias, abbreviation, remarks)
        {
            this._SemiMajorAxis = semiMajorAxis;
            this._InverseFlattening = inverseFlattening;
            this._AxisUnit = axisUnit;
            this._IsIvfDefinitive = isIvfDefinitive;
            if (isIvfDefinitive)
            {
                this._SemiMinorAxis = (1 - (1 / this._InverseFlattening)) * semiMajorAxis;
            }
            else
            {
                this._SemiMinorAxis = semiMinorAxis;
            }
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
                Ellipsoid ellipsoid = obj as Ellipsoid;
                if (((ellipsoid.InverseFlattening == this.InverseFlattening) && (ellipsoid.IsIvfDefinitive == this.IsIvfDefinitive)) && ((ellipsoid.SemiMajorAxis == this.SemiMajorAxis) && (ellipsoid.SemiMinorAxis == this.SemiMinorAxis)))
                {
                    return ellipsoid.AxisUnit.EqualParams(this.AxisUnit);
                }
            }
            return false;
        }

        /// <summary>
        /// Gets or sets the value of the axis unit.
        /// </summary>
        public ILinearUnit AxisUnit
        {
            get
            {
                return this._AxisUnit;
            }
            set
            {
                this._AxisUnit = value;
            }
        }

        /// <summary>
        /// Clarke 1866
        /// </summary>
        /// <remarks>
        /// Original definition a=20926062 and b=20855121 (British) feet. Uses Clarke's 1865 inch-metre ratio of 39.370432 to obtain metres. (Metric value then converted to US survey feet for use in the United States using 39.37 exactly giving a=20925832.16 ft US).
        /// </remarks>
        public static Ellipsoid Clarke1866
        {
            get
            {
                return new Ellipsoid(6378206.4, 6356583.8, double.PositiveInfinity, false, LinearUnit.Metre, "Clarke 1866", "EPSG", 0x1b60L, "Clarke 1866", string.Empty, "Original definition a=20926062 and b=20855121 (British) feet. Uses Clarke's 1865 inch-metre ratio of 39.370432 to obtain metres. (Metric value then converted to US survey feet for use in the United States using 39.37 exactly giving a=20925832.16 ft US).");
            }
        }

        /// <summary>
        /// Clarke 1880
        /// </summary>
        /// <remarks>
        /// Clarke gave a and b and also 1/f=293.465 (to 3 decimal places).  1/f derived from a and b = 293.4663077
        /// </remarks>
        public static Ellipsoid Clarke1880
        {
            get
            {
                return new Ellipsoid(20926202, 0, 297, true, LinearUnit.ClarkesFoot, "Clarke 1880", "EPSG", 0x1b7aL, "Clarke 1880", string.Empty, "Clarke gave a and b and also 1/f=293.465 (to 3 decimal places).  1/f derived from a and b = 293.4663077…");
            }
        }

        /// <summary>
        /// GRS 1980 / International 1979 ellipsoid
        /// </summary>
        /// <remarks>
        /// Adopted by IUGG 1979 Canberra.
        /// Inverse flattening is derived from
        /// geocentric gravitational constant GM = 3986005e8 m*m*m/s/s;
        /// dynamic form factor J2 = 108263e8 and Earth's angular velocity = 7292115e-11 rad/s.")
        /// </remarks>
        public static Ellipsoid GRS80
        {
            get
            {
                return new Ellipsoid(6378137, 0, 298.257222101, true, LinearUnit.Metre, "GRS 1980", "EPSG", 0x1b6bL, "International 1979", "", "Adopted by IUGG 1979 Canberra.  Inverse flattening is derived from geocentric gravitational constant GM = 3986005e8 m*m*m/s/s; dynamic form factor J2 = 108263e8 and Earth's angular velocity = 7292115e-11 rad/s.");
            }
        }

        /// <summary>
        /// International 1924 / Hayford 1909 ellipsoid
        /// </summary>
        /// <remarks>
        /// Described as a=6378388 m. and b=6356909m. from which 1/f derived to be 296.95926. 
        /// The figure was adopted as the International ellipsoid in 1924 but with 1/f taken as
        /// 297 exactly from which b is derived as 6356911.946m.
        /// </remarks>
        public static Ellipsoid International1924
        {
            get
            {
                return new Ellipsoid(6378388, 0, 297, true, LinearUnit.Metre, "International 1924", "EPSG", 0x1b6eL, "Hayford 1909", string.Empty, "Described as a=6378388 m. and b=6356909 m. from which 1/f derived to be 296.95926. The figure was adopted as the International ellipsoid in 1924 but with 1/f taken as 297 exactly from which b is derived as 6356911.946m.");
            }
        }

        /// <summary>
        /// Gets or sets the value of the inverse of the flattening constant of the ellipsoid.
        /// </summary>
        public double InverseFlattening
        {
            get
            {
                return this._InverseFlattening;
            }
            set
            {
                this._InverseFlattening = value;
            }
        }

        /// <summary>
        /// Tells if the Inverse Flattening is definitive for this ellipsoid. Some ellipsoids use 
        /// the IVF as the defining value, and calculate the polar radius whenever asked. Other
        /// ellipsoids use the polar radius to calculate the IVF whenever asked. This 
        /// distinction can be important to avoid floating-point rounding errors.
        /// </summary>
        public bool IsIvfDefinitive
        {
            get
            {
                return this._IsIvfDefinitive;
            }
            set
            {
                this._IsIvfDefinitive = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the semi-major axis.
        /// </summary>
        public double SemiMajorAxis
        {
            get
            {
                return this._SemiMajorAxis;
            }
            set
            {
                this._SemiMajorAxis = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the semi-minor axis.
        /// </summary>
        public double SemiMinorAxis
        {
            get
            {
                return this._SemiMinorAxis;
            }
            set
            {
                this._SemiMinorAxis = value;
            }
        }

        /// <summary>
        /// Sphere
        /// </summary>
        /// <remarks>
        /// Authalic sphere derived from GRS 1980 ellipsoid (code 7019).  (An authalic sphere is
        /// one with a surface area equal to the surface area of the ellipsoid). 1/f is infinite.
        /// </remarks>
        public static Ellipsoid Sphere
        {
            get
            {
                return new Ellipsoid(6370997, 6370997, double.PositiveInfinity, false, LinearUnit.Metre, "GRS 1980 Authalic Sphere", "EPSG", 0x1b88L, "Sphere", "", "Authalic sphere derived from GRS 1980 ellipsoid (code 7019).  (An authalic sphere is one with a surface area equal to the surface area of the ellipsoid). 1/f is infinite.");
            }
        }

        /// <summary>
        /// WGS 72 Ellipsoid
        /// </summary>
        public static Ellipsoid WGS72
        {
            get
            {
                return new Ellipsoid(6378135, 0, 298.26, true, LinearUnit.Metre, "WGS 72", "EPSG", 0x1b83L, "WGS 72", string.Empty, string.Empty);
            }
        }

        /// <summary>
        /// WGS 84 ellipsoid
        /// </summary>
        /// <remarks>
        /// Inverse flattening derived from four defining parameters 
        /// (semi-major axis;
        /// C20 = -484.16685*10e-6;
        /// earth's angular velocity w = 7292115e11 rad/sec;
        /// gravitational constant GM = 3986005e8 m*m*m/s/s).
        /// </remarks>
        public static Ellipsoid WGS84
        {
            get
            {
                return new Ellipsoid(6378137, 0, 298.257223563, true, LinearUnit.Metre, "WGS 84", "EPSG", 0x1b76L, "WGS84", "", "Inverse flattening derived from four defining parameters (semi-major axis; C20 = -484.16685*10e-6; earth's angular velocity w = 7292115e11 rad/sec; gravitational constant GM = 3986005e8 m*m*m/s/s).");
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
                builder.AppendFormat(NumberFormatter.GetNfi(), "SPHEROID[\"{0}\", {1}, {2}", new object[] { base.Name, this.SemiMajorAxis, this.InverseFlattening });
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
                return string.Format(NumberFormatter.GetNfi(), "<CS_Ellipsoid SemiMajorAxis=\"{0}\" SemiMinorAxis=\"{1}\" InverseFlattening=\"{2}\" IvfDefinitive=\"{3}\">{4}{5}</CS_Ellipsoid>", new object[] { this.SemiMajorAxis, this.SemiMinorAxis, this.InverseFlattening, this.IsIvfDefinitive ? 1 : 0, base.InfoXml, this.AxisUnit.XML });
            }
        }
    }
}

