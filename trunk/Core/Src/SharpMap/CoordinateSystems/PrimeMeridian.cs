namespace Topology.CoordinateSystems
{
    using Topology.Utilities;
    using System;
    using System.Text;

    /// <summary>
    /// A meridian used to take longitude measurements from.
    /// </summary>
    public class PrimeMeridian : Info, IPrimeMeridian, IInfo
    {
        private IAngularUnit _AngularUnit;
        private double _Longitude;

        /// <summary>
        /// Initializes a new instance of a prime meridian
        /// </summary>
        /// <param name="longitude">Longitude of prime meridian</param>
        /// <param name="angularUnit">Angular unit</param>
        /// <param name="name">Name</param>
        /// <param name="authority">Authority name</param>
        /// <param name="authorityCode">Authority-specific identification code.</param>
        /// <param name="alias">Alias</param>
        /// <param name="abbreviation">Abbreviation</param>
        /// <param name="remarks">Provider-supplied remarks</param>
        internal PrimeMeridian(double longitude, IAngularUnit angularUnit, string name, string authority, long authorityCode, string alias, string abbreviation, string remarks) : base(name, authority, authorityCode, alias, abbreviation, remarks)
        {
            this._Longitude = longitude;
            this._AngularUnit = angularUnit;
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
            if (obj is PrimeMeridian)
            {
                PrimeMeridian meridian = obj as PrimeMeridian;
                if (meridian.AngularUnit.EqualParams(this.AngularUnit))
                {
                    return (meridian.Longitude == this.Longitude);
                }
            }
            return false;
        }

        /// <summary>
        /// Gets or sets the AngularUnits.
        /// </summary>
        public IAngularUnit AngularUnit
        {
            get
            {
                return this._AngularUnit;
            }
            set
            {
                this._AngularUnit = value;
            }
        }

        /// <summary>
        /// Athens prime meridian.
        /// Used in Greece for older mapping based on Hatt projection.
        /// </summary>
        public static PrimeMeridian Athens
        {
            get
            {
                return new PrimeMeridian(23.4258815, Topology.CoordinateSystems.AngularUnit.Degrees, "Athens", "EPSG", 0x22d0L, string.Empty, string.Empty, "Used in Greece for older mapping based on Hatt projection.");
            }
        }

        /// <summary>
        /// Bern prime meridian.
        /// 1895 value. Newer value of 7 deg 26 min 22.335 sec E determined in 1938.
        /// </summary>
        public static PrimeMeridian Bern
        {
            get
            {
                return new PrimeMeridian(7.26225, Topology.CoordinateSystems.AngularUnit.Degrees, "Bern", "EPSG", 0x22cbL, string.Empty, string.Empty, "1895 value. Newer value of 7 deg 26 min 22.335 sec E determined in 1938.");
            }
        }

        /// <summary>
        /// Bogota prime meridian
        /// </summary>
        public static PrimeMeridian Bogota
        {
            get
            {
                return new PrimeMeridian(-74.04513, Topology.CoordinateSystems.AngularUnit.Degrees, "Bogota", "EPSG", 0x22c8L, string.Empty, string.Empty, string.Empty);
            }
        }

        /// <summary>
        /// Brussels prime meridian
        /// </summary>
        public static PrimeMeridian Brussels
        {
            get
            {
                return new PrimeMeridian(4.220471, Topology.CoordinateSystems.AngularUnit.Degrees, "Brussels", "EPSG", 0x22ceL, string.Empty, string.Empty, string.Empty);
            }
        }

        /// <summary>
        /// Ferro prime meridian.
        /// Used in Austria and former Czechoslovakia.
        /// </summary>
        public static PrimeMeridian Ferro
        {
            get
            {
                return new PrimeMeridian(-17.4, Topology.CoordinateSystems.AngularUnit.Degrees, "Ferro", "EPSG", 0x22cdL, string.Empty, string.Empty, "Used in Austria and former Czechoslovakia.");
            }
        }

        /// <summary>
        /// Greenwich prime meridian
        /// </summary>
        public static PrimeMeridian Greenwich
        {
            get
            {
                return new PrimeMeridian(0, Topology.CoordinateSystems.AngularUnit.Degrees, "Greenwich", "EPSG", 0x22c5L, string.Empty, string.Empty, string.Empty);
            }
        }

        /// <summary>
        /// Jakarta prime meridian
        /// </summary>
        public static PrimeMeridian Jakarta
        {
            get
            {
                return new PrimeMeridian(106.482779, Topology.CoordinateSystems.AngularUnit.Degrees, "Jakarta", "EPSG", 0x22ccL, string.Empty, string.Empty, string.Empty);
            }
        }

        /// <summary>
        /// Lisbon prime meridian
        /// </summary>
        public static PrimeMeridian Lisbon
        {
            get
            {
                return new PrimeMeridian(-9.0754862, Topology.CoordinateSystems.AngularUnit.Degrees, "Lisbon", "EPSG", 0x22c6L, string.Empty, string.Empty, string.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the longitude of the prime meridian (relative to the Greenwich prime meridian).
        /// </summary>
        public double Longitude
        {
            get
            {
                return this._Longitude;
            }
            set
            {
                this._Longitude = value;
            }
        }

        /// <summary>
        /// Madrid prime meridian
        /// </summary>
        public static PrimeMeridian Madrid
        {
            get
            {
                return new PrimeMeridian(-3.411658, Topology.CoordinateSystems.AngularUnit.Degrees, "Madrid", "EPSG", 0x22c9L, string.Empty, string.Empty, string.Empty);
            }
        }

        /// <summary>
        /// Oslo prime meridian.
        /// Formerly known as Kristiania or Christiania.
        /// </summary>
        public static PrimeMeridian Oslo
        {
            get
            {
                return new PrimeMeridian(10.43225, Topology.CoordinateSystems.AngularUnit.Degrees, "Oslo", "EPSG", 0x22d1L, string.Empty, string.Empty, "Formerly known as Kristiania or Christiania.");
            }
        }

        /// <summary>
        /// Paris prime meridian.
        /// Value adopted by IGN (Paris) in 1936. Equivalent to 2 deg 20min 14.025sec. Preferred by EPSG to earlier value of 2deg 20min 13.95sec (2.596898 grads) used by RGS London.
        /// </summary>
        public static PrimeMeridian Paris
        {
            get
            {
                return new PrimeMeridian(2.5969213, Topology.CoordinateSystems.AngularUnit.Degrees, "Paris", "EPSG", 0x22c7L, string.Empty, string.Empty, "Value adopted by IGN (Paris) in 1936. Equivalent to 2 deg 20min 14.025sec. Preferred by EPSG to earlier value of 2deg 20min 13.95sec (2.596898 grads) used by RGS London.");
            }
        }

        /// <summary>
        /// Rome prime meridian
        /// </summary>
        public static PrimeMeridian Rome
        {
            get
            {
                return new PrimeMeridian(12.27084, Topology.CoordinateSystems.AngularUnit.Degrees, "Rome", "EPSG", 0x22caL, string.Empty, string.Empty, string.Empty);
            }
        }

        /// <summary>
        /// Stockholm prime meridian
        /// </summary>
        public static PrimeMeridian Stockholm
        {
            get
            {
                return new PrimeMeridian(18.03298, Topology.CoordinateSystems.AngularUnit.Degrees, "Stockholm", "EPSG", 0x22cfL, string.Empty, string.Empty, string.Empty);
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
                builder.AppendFormat(NumberFormatter.GetNfi(), "PRIMEM[\"{0}\", {1}", new object[] { base.Name, this.Longitude });
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
                return string.Format(NumberFormatter.GetNfi(), "<CS_PrimeMeridian Longitude=\"{0}\" >{1}{2}</CS_PrimeMeridian>", new object[] { this.Longitude, base.InfoXml, this.AngularUnit.XML });
            }
        }
    }
}

