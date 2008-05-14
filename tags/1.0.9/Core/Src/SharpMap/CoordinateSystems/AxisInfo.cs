namespace Topology.CoordinateSystems
{
    using Topology.Utilities;
    using System;

    /// <summary>
    /// Details of axis. This is used to label axes, and indicate the orientation.
    /// </summary>
    public class AxisInfo
    {
        private string _Name;
        private AxisOrientationEnum _Orientation;

        /// <summary>
        /// Initializes a new instance of an AxisInfo.
        /// </summary>
        /// <param name="name">Name of axis</param>
        /// <param name="orientation">Axis orientation</param>
        public AxisInfo(string name, AxisOrientationEnum orientation)
        {
            this._Name = name;
            this._Orientation = orientation;
        }

        /// <summary>
        /// Human readable name for axis. Possible values are X, Y, Long, Lat or any other short string.
        /// </summary>
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
            }
        }

        /// <summary>
        /// Gets enumerated value for orientation.
        /// </summary>
        public AxisOrientationEnum Orientation
        {
            get
            {
                return this._Orientation;
            }
            set
            {
                this._Orientation = value;
            }
        }

        /// <summary>
        /// Returns the Well-known text for this object
        /// as defined in the simple features specification.
        /// </summary>
        public string WKT
        {
            get
            {
                return string.Format("AXIS[\"{0}\", {1}]", this.Name, this.Orientation.ToString().ToUpper());
            }
        }

        /// <summary>
        /// Gets an XML representation of this object
        /// </summary>
        public string XML
        {
            get
            {
                return string.Format(NumberFormatter.GetNfi(), "<CS_AxisInfo Name=\"{0}\" Orientation=\"{1}\"/>", new object[] { this.Name, this.Orientation.ToString().ToUpper() });
            }
        }
    }
}

