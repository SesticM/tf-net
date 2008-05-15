namespace Topology.CoordinateSystems
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The GeographicTransform class is implemented on geographic transformation objects and
    /// implements datum transformations between geographic coordinate systems.
    /// </summary>
    public class GeographicTransform : Info, IGeographicTransform, IInfo
    {
        private IGeographicCoordinateSystem _SourceGCS;
        private IGeographicCoordinateSystem _TargetGCS;

        internal GeographicTransform(string name, string authority, long code, string alias, string remarks, string abbreviation, IGeographicCoordinateSystem sourceGCS, IGeographicCoordinateSystem targetGCS) : base(name, authority, code, alias, abbreviation, remarks)
        {
            this._SourceGCS = sourceGCS;
            this._TargetGCS = targetGCS;
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
            if (obj is GeographicTransform)
            {
                GeographicTransform transform = obj as GeographicTransform;
                if (transform.SourceGCS.EqualParams(this.SourceGCS))
                {
                    return transform.TargetGCS.EqualParams(this.TargetGCS);
                }
            }
            return false;
        }

        /// <summary>
        /// Transforms an array of points from the source geographic coordinate
        /// system to the target geographic coordinate system.
        /// </summary>
        /// <param name="points">On input points in the source geographic coordinate system</param>
        /// <returns>Output points in the target geographic coordinate system</returns>
        public List<double[]> Forward(List<double[]> points)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Transforms an array of points from the target geographic coordinate
        /// system to the source geographic coordinate system.
        /// </summary>
        /// <param name="points">Input points in the target geographic coordinate system,</param>
        /// <returns>Output points in the source geographic coordinate system</returns>
        public List<double[]> Inverse(List<double[]> points)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an accessor interface to the parameters for this geographic transformation.
        /// </summary>
        public IParameterInfo ParameterInfo
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets or sets the source geographic coordinate system for the transformation.
        /// </summary>
        public IGeographicCoordinateSystem SourceGCS
        {
            get
            {
                return this._SourceGCS;
            }
            set
            {
                this._SourceGCS = value;
            }
        }

        /// <summary>
        /// Gets or sets the target geographic coordinate system for the transformation.
        /// </summary>
        public IGeographicCoordinateSystem TargetGCS
        {
            get
            {
                return this._TargetGCS;
            }
            set
            {
                this._TargetGCS = value;
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
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets an XML representation of this object [NOT IMPLEMENTED].
        /// </summary>
        public override string XML
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}

