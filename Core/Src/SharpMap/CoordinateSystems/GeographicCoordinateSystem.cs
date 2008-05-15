namespace Topology.CoordinateSystems
{
    using Topology.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A coordinate system based on latitude and longitude. 
    /// </summary>
    /// <remarks>
    /// Some geographic coordinate systems are Lat/Lon, and some are Lon/Lat. 
    /// You can find out which this is by examining the axes. You should also 
    /// check the angular units, since not all geographic coordinate systems 
    /// use degrees.
    /// </remarks>
    public class GeographicCoordinateSystem : HorizontalCoordinateSystem, IGeographicCoordinateSystem, IHorizontalCoordinateSystem, ICoordinateSystem, IInfo
    {
        private IAngularUnit _AngularUnit;
        private IPrimeMeridian _PrimeMeridian;
        private List<Wgs84ConversionInfo> _WGS84ConversionInfo;

        /// <summary>
        /// Creates an instance of a Geographic Coordinate System
        /// </summary>
        /// <param name="angularUnit">Angular units</param>
        /// <param name="horizontalDatum">Horizontal datum</param>
        /// <param name="primeMeridian">Prime meridian</param>
        /// <param name="axisInfo">Axis info</param>
        /// <param name="name">Name</param>
        /// <param name="authority">Authority name</param>
        /// <param name="authorityCode">Authority-specific identification code.</param>
        /// <param name="alias">Alias</param>
        /// <param name="abbreviation">Abbreviation</param>
        /// <param name="remarks">Provider-supplied remarks</param>
        internal GeographicCoordinateSystem(IAngularUnit angularUnit, IHorizontalDatum horizontalDatum, IPrimeMeridian primeMeridian, List<AxisInfo> axisInfo, string name, string authority, long authorityCode, string alias, string abbreviation, string remarks) : base(horizontalDatum, axisInfo, name, authority, authorityCode, alias, abbreviation, remarks)
        {
            this._AngularUnit = angularUnit;
            this._PrimeMeridian = primeMeridian;
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
            if (obj is GeographicCoordinateSystem)
            {
                GeographicCoordinateSystem system = obj as GeographicCoordinateSystem;
                if (system.Dimension != base.Dimension)
                {
                    return false;
                }
                if ((this.WGS84ConversionInfo != null) && (system.WGS84ConversionInfo == null))
                {
                    return false;
                }
                if ((this.WGS84ConversionInfo == null) && (system.WGS84ConversionInfo != null))
                {
                    return false;
                }
                if ((this.WGS84ConversionInfo != null) && (system.WGS84ConversionInfo != null))
                {
                    if (this.WGS84ConversionInfo.Count != system.WGS84ConversionInfo.Count)
                    {
                        return false;
                    }
                    for (int i = 0; i < this.WGS84ConversionInfo.Count; i++)
                    {
                        if (!system.WGS84ConversionInfo[i].Equals(this.WGS84ConversionInfo[i]))
                        {
                            return false;
                        }
                    }
                }
                if (base.AxisInfo.Count == system.AxisInfo.Count)
                {
                    for (int j = 0; j < system.AxisInfo.Count; j++)
                    {
                        if (system.AxisInfo[j].Orientation != base.AxisInfo[j].Orientation)
                        {
                            return false;
                        }
                    }
                    if (system.AngularUnit.EqualParams(this.AngularUnit) && system.HorizontalDatum.EqualParams(base.HorizontalDatum))
                    {
                        return system.PrimeMeridian.EqualParams(this.PrimeMeridian);
                    }
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
            return this._AngularUnit;
        }

        /// <summary>
        /// Gets details on a conversion to WGS84.
        /// </summary>
        public Wgs84ConversionInfo GetWgs84ConversionInfo(int index)
        {
            return this._WGS84ConversionInfo[index];
        }

        /// <summary>
        /// Gets or sets the angular units of the geographic coordinate system.
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
        /// Gets the number of available conversions to WGS84 coordinates.
        /// </summary>
        public int NumConversionToWGS84
        {
            get
            {
                return this._WGS84ConversionInfo.Count;
            }
        }

        /// <summary>
        /// Gets or sets the prime meridian of the geographic coordinate system.
        /// </summary>
        public IPrimeMeridian PrimeMeridian
        {
            get
            {
                return this._PrimeMeridian;
            }
            set
            {
                this._PrimeMeridian = value;
            }
        }

        /// <summary>
        /// Creates a decimal degrees geographic coordinate system based on the WGS84 ellipsoid, suitable for GPS measurements
        /// </summary>
        public static GeographicCoordinateSystem WGS84
        {
            get
            {
                List<AxisInfo> axisInfo = new List<AxisInfo>(2);
                axisInfo.Add(new AxisInfo("Lon", AxisOrientationEnum.East));
                axisInfo.Add(new AxisInfo("Lat", AxisOrientationEnum.North));
                return new GeographicCoordinateSystem(Topology.CoordinateSystems.AngularUnit.Degrees, Topology.CoordinateSystems.HorizontalDatum.WGS84, Topology.CoordinateSystems.PrimeMeridian.Greenwich, axisInfo, "WGS 84", "EPSG", 0x10e6L, string.Empty, string.Empty, string.Empty);
            }
        }

        internal List<Wgs84ConversionInfo> WGS84ConversionInfo
        {
            get
            {
                return this._WGS84ConversionInfo;
            }
            set
            {
                this._WGS84ConversionInfo = value;
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
                builder.AppendFormat("GEOGCS[\"{0}\", {1}, {2}, {3}", new object[] { base.Name, base.HorizontalDatum.WKT, this.PrimeMeridian.WKT, this.AngularUnit.WKT });
                if (((base.AxisInfo.Count != 2) || (base.AxisInfo[0].Name != "Lon")) || (((base.AxisInfo[0].Orientation != AxisOrientationEnum.East) || (base.AxisInfo[1].Name != "Lat")) || (base.AxisInfo[1].Orientation != AxisOrientationEnum.North)))
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
                builder.AppendFormat(NumberFormatter.GetNfi(), "<CS_CoordinateSystem Dimension=\"{0}\"><CS_GeographicCoordinateSystem>{1}", new object[] { base.Dimension, base.InfoXml });
                foreach (AxisInfo info in base.AxisInfo)
                {
                    builder.Append(info.XML);
                }
                builder.AppendFormat("{0}{1}{2}</CS_GeographicCoordinateSystem></CS_CoordinateSystem>", base.HorizontalDatum.XML, this.AngularUnit.XML, this.PrimeMeridian.XML);
                return builder.ToString();
            }
        }
    }
}

