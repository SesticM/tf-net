namespace Topology.CoordinateSystems
{
    using Topology.Utilities;
    using System;

    /// <summary>
    /// A named projection parameter value.
    /// </summary>
    /// <remarks>
    /// The linear units of parameters' values match the linear units of the containing 
    /// projected coordinate system. The angular units of parameter values match the 
    /// angular units of the geographic coordinate system that the projected coordinate 
    /// system is based on. (Notice that this is different from <see cref="T:Topology.CoordinateSystems.Parameter" />,
    /// where the units are always meters and degrees.)
    /// </remarks>
    public class ProjectionParameter
    {
        private string _Name;
        private double _Value;

        /// <summary>
        /// Initializes an instance of a ProjectionParameter
        /// </summary>
        /// <param name="name">Name of parameter</param>
        /// <param name="value">Parameter value</param>
        public ProjectionParameter(string name, double value)
        {
            this._Name = name;
            this._Value = value;
        }

        /// <summary>
        /// Parameter name.
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
        /// Parameter value.
        /// The linear units of a parameters' values match the linear units of the containing 
        /// projected coordinate system. The angular units of parameter values match the 
        /// angular units of the geographic coordinate system that the projected coordinate 
        /// system is based on.
        /// </summary>
        public double Value
        {
            get
            {
                return this._Value;
            }
            set
            {
                this._Value = value;
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
                return string.Format(NumberFormatter.GetNfi(), "PARAMETER[\"{0}\", {1}]", new object[] { this.Name, this.Value });
            }
        }

        /// <summary>
        /// Gets an XML representation of this object
        /// </summary>
        public string XML
        {
            get
            {
                return string.Format(NumberFormatter.GetNfi(), "<CS_ProjectionParameter Name=\"{0}\" Value=\"{1}\"/>", new object[] { this.Name, this.Value });
            }
        }
    }
}

