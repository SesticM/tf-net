namespace Topology.CoordinateSystems
{
    using System;

    /// <summary>
    /// A named parameter value.
    /// </summary>
    public class Parameter
    {
        private string _Name;
        private double _Value;

        /// <summary>
        /// Creates an instance of a parameter
        /// </summary>
        /// <remarks>Units are always either meters or degrees.</remarks>
        /// <param name="name">Name of parameter</param>
        /// <param name="value">Value</param>
        public Parameter(string name, double value)
        {
            this._Name = name;
            this._Value = this.Value;
        }

        /// <summary>
        /// Parameter name
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
        /// Parameter value
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
    }
}

