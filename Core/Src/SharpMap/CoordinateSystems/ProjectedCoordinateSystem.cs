namespace Topology.CoordinateSystems
{
    using Topology.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A 2D cartographic coordinate system.
    /// </summary>
    public class ProjectedCoordinateSystem : HorizontalCoordinateSystem, IProjectedCoordinateSystem, IHorizontalCoordinateSystem, ICoordinateSystem, IInfo
    {
        private IGeographicCoordinateSystem _GeographicCoordinateSystem;
        private ILinearUnit _LinearUnit;
        private IProjection _Projection;

        /// <summary>
        /// Initializes a new instance of a projected coordinate system
        /// </summary>
        /// <param name="datum">Horizontal datum</param>
        /// <param name="geographicCoordinateSystem">Geographic coordinate system</param>
        /// <param name="linearUnit">Linear unit</param>
        /// <param name="projection">Projection</param>
        /// <param name="axisInfo">Axis info</param>
        /// <param name="name">Name</param>
        /// <param name="authority">Authority name</param>
        /// <param name="code">Authority-specific identification code.</param>
        /// <param name="alias">Alias</param>
        /// <param name="abbreviation">Abbreviation</param>
        /// <param name="remarks">Provider-supplied remarks</param>
        internal ProjectedCoordinateSystem(IHorizontalDatum datum, IGeographicCoordinateSystem geographicCoordinateSystem, ILinearUnit linearUnit, IProjection projection, List<AxisInfo> axisInfo, string name, string authority, long code, string alias, string remarks, string abbreviation) : base(datum, axisInfo, name, authority, code, alias, abbreviation, remarks)
        {
            this._GeographicCoordinateSystem = geographicCoordinateSystem;
            this._LinearUnit = linearUnit;
            this._Projection = projection;
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
            if (obj is ProjectedCoordinateSystem)
            {
                ProjectedCoordinateSystem system = obj as ProjectedCoordinateSystem;
                if (system.Dimension != base.Dimension)
                {
                    return false;
                }
                for (int i = 0; i < system.Dimension; i++)
                {
                    if (system.GetAxis(i).Orientation != base.GetAxis(i).Orientation)
                    {
                        return false;
                    }
                    if (!system.GetUnits(i).EqualParams(this.GetUnits(i)))
                    {
                        return false;
                    }
                }
                if ((system.GeographicCoordinateSystem.EqualParams(this.GeographicCoordinateSystem) && system.HorizontalDatum.EqualParams(base.HorizontalDatum)) && system.LinearUnit.EqualParams(this.LinearUnit))
                {
                    return system.Projection.EqualParams(this.Projection);
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
        /// Gets or sets the GeographicCoordinateSystem.
        /// </summary>
        public IGeographicCoordinateSystem GeographicCoordinateSystem
        {
            get
            {
                return this._GeographicCoordinateSystem;
            }
            set
            {
                this._GeographicCoordinateSystem = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="P:Topology.CoordinateSystems.ProjectedCoordinateSystem.LinearUnit">LinearUnits</see>. The linear unit must be the same as the <see cref="T:Topology.CoordinateSystems.CoordinateSystem" /> units.
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
        /// Gets or sets the projection
        /// </summary>
        public IProjection Projection
        {
            get
            {
                return this._Projection;
            }
            set
            {
                this._Projection = value;
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
                builder.AppendFormat("PROJCS[\"{0}\", {1}, {2}", base.Name, this.GeographicCoordinateSystem.WKT, this.Projection.WKT);
                for (int i = 0; i < this.Projection.NumParameters; i++)
                {
                    builder.AppendFormat(NumberFormatter.GetNfi(), ", {0}", new object[] { this.Projection.GetParameter(i).WKT });
                }
                builder.AppendFormat(", {0}", this.LinearUnit.WKT);
                if (((base.AxisInfo.Count != 2) || (base.AxisInfo[0].Name != "X")) || (((base.AxisInfo[0].Orientation != AxisOrientationEnum.East) || (base.AxisInfo[1].Name != "Y")) || (base.AxisInfo[1].Orientation != AxisOrientationEnum.North)))
                {
                    for (int j = 0; j < base.AxisInfo.Count; j++)
                    {
                        builder.AppendFormat(", {0}", base.GetAxis(j).WKT);
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
        /// Gets an XML representation of this object.
        /// </summary>
        public override string XML
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat(NumberFormatter.GetNfi(), "<CS_CoordinateSystem Dimension=\"{0}\"><CS_ProjectedCoordinateSystem>{1}", new object[] { base.Dimension, base.InfoXml });
                foreach (AxisInfo info in base.AxisInfo)
                {
                    builder.Append(info.XML);
                }
                builder.AppendFormat("{0}{1}{2}</CS_ProjectedCoordinateSystem></CS_CoordinateSystem>", this.GeographicCoordinateSystem.XML, this.LinearUnit.XML, this.Projection.XML);
                return builder.ToString();
            }
        }
    }
}

