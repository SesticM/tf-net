namespace Topology.CoordinateSystems
{
    using Topology.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A 3D coordinate system, with its origin at the center of the Earth.
    /// </summary>
    public class GeocentricCoordinateSystem : CoordinateSystem, IGeocentricCoordinateSystem, ICoordinateSystem, IInfo
    {
        private IHorizontalDatum _HorizontalDatum;
        private ILinearUnit _LinearUnit;
        private IPrimeMeridian _Primemeridan;

        internal GeocentricCoordinateSystem(IHorizontalDatum datum, ILinearUnit linearUnit, IPrimeMeridian primeMeridian, List<AxisInfo> axisinfo, string name, string authority, long code, string alias, string remarks, string abbreviation) : base(name, authority, code, alias, abbreviation, remarks)
        {
            this._HorizontalDatum = datum;
            this._LinearUnit = linearUnit;
            this._Primemeridan = primeMeridian;
            if (axisinfo.Count != 3)
            {
                throw new ArgumentException("Axis info should contain three axes for geocentric coordinate systems");
            }
            base.AxisInfo = axisinfo;
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
            if (obj is GeocentricCoordinateSystem)
            {
                GeocentricCoordinateSystem system = obj as GeocentricCoordinateSystem;
                if (system.HorizontalDatum.EqualParams(this.HorizontalDatum) && system.LinearUnit.EqualParams(this.LinearUnit))
                {
                    return system.PrimeMeridian.EqualParams(this.PrimeMeridian);
                }
            }
            return false;
        }

        /// <summary>
        /// Gets units for dimension within coordinate system. Each dimension in 
        /// the coordinate system has corresponding units.
        /// </summary>
        /// <param name="dimension">Dimension</param>
        /// <returns>Unit</returns>
        public override IUnit GetUnits(int dimension)
        {
            return this._LinearUnit;
        }

        /// <summary>
        /// Returns the HorizontalDatum. The horizontal datum is used to determine where
        /// the centre of the Earth is considered to be. All coordinate points will be 
        /// measured from the centre of the Earth, and not the surface.
        /// </summary>
        public IHorizontalDatum HorizontalDatum
        {
            get
            {
                return this._HorizontalDatum;
            }
            set
            {
                this._HorizontalDatum = value;
            }
        }

        /// <summary>
        /// Gets the units used along all the axes.
        /// </summary>
        public ILinearUnit LinearUnit
        {
            get
            {
                return this._LinearUnit;
            }
            set
            {
                this._LinearUnit = value;
            }
        }

        /// <summary>
        /// Returns the PrimeMeridian.
        /// </summary>
        public IPrimeMeridian PrimeMeridian
        {
            get
            {
                return this._Primemeridan;
            }
            set
            {
                this._Primemeridan = value;
            }
        }

        /// <summary>
        /// Creates a geocentric coordinate system based on the WGS84 ellipsoid, suitable for GPS measurements
        /// </summary>
        public static IGeocentricCoordinateSystem WGS84
        {
            get
            {
                return new CoordinateSystemFactory().CreateGeocentricCoordinateSystem("WGS84 Geocentric", Topology.CoordinateSystems.HorizontalDatum.WGS84, Topology.CoordinateSystems.LinearUnit.Metre, Topology.CoordinateSystems.PrimeMeridian.Greenwich);
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
                builder.AppendFormat("GEOCCS[\"{0}\", {1}, {2}, {3}", new object[] { base.Name, this.HorizontalDatum.WKT, this.PrimeMeridian.WKT, this.LinearUnit.WKT });
                if ((((base.AxisInfo.Count != 3) || (base.AxisInfo[0].Name != "X")) || ((base.AxisInfo[0].Orientation != AxisOrientationEnum.Other) || (base.AxisInfo[1].Name != "Y"))) || (((base.AxisInfo[1].Orientation != AxisOrientationEnum.East) || (base.AxisInfo[2].Name != "Z")) || (base.AxisInfo[2].Orientation != AxisOrientationEnum.North)))
                {
                    for (int i = 0; i < base.AxisInfo.Count; i++)
                    {
                        builder.AppendFormat(", {0}", base.GetAxis(i).WKT);
                    }
                }
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
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat(NumberFormatter.GetNfi(), "<CS_CoordinateSystem Dimension=\"{0}\"><CS_GeocentricCoordinateSystem>{1}", new object[] { base.Dimension, base.InfoXml });
                foreach (AxisInfo info in base.AxisInfo)
                {
                    builder.Append(info.XML);
                }
                builder.AppendFormat("{0}{1}{2}</CS_GeocentricCoordinateSystem></CS_CoordinateSystem>", this.HorizontalDatum.XML, this.LinearUnit.XML, this.PrimeMeridian.XML);
                return builder.ToString();
            }
        }
    }
}

