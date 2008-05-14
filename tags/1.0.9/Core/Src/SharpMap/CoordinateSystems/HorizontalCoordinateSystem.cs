namespace Topology.CoordinateSystems
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A 2D coordinate system suitable for positions on the Earth's surface.
    /// </summary>
    public abstract class HorizontalCoordinateSystem : CoordinateSystem, IHorizontalCoordinateSystem, ICoordinateSystem, IInfo
    {
        private IHorizontalDatum _HorizontalDatum;

        /// <summary>
        /// Creates an instance of HorizontalCoordinateSystem
        /// </summary>
        /// <param name="datum">Horizontal datum</param>
        /// <param name="axisInfo">Axis information</param>
        /// <param name="name">Name</param>
        /// <param name="authority">Authority name</param>
        /// <param name="code">Authority-specific identification code.</param>
        /// <param name="alias">Alias</param>
        /// <param name="abbreviation">Abbreviation</param>
        /// <param name="remarks">Provider-supplied remarks</param>
        internal HorizontalCoordinateSystem(IHorizontalDatum datum, List<AxisInfo> axisInfo, string name, string authority, long code, string alias, string remarks, string abbreviation) : base(name, authority, code, alias, abbreviation, remarks)
        {
            this._HorizontalDatum = datum;
            if (axisInfo.Count != 2)
            {
                throw new ArgumentException("Axis info should contain two axes for horizontal coordinate systems");
            }
            base.AxisInfo = axisInfo;
        }

        /// <summary>
        /// Gets or sets the HorizontalDatum.
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
    }
}

