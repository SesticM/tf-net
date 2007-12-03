namespace Topology.CoordinateSystems.Transformations
{
    using Topology.CoordinateSystems;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The GeographicTransform class is implemented on geographic transformation objects and
    /// implements datum transformations between geographic coordinate systems.
    /// </summary>
    public class GeographicTransform : MathTransform
    {
        private IGeographicCoordinateSystem _SourceGCS;
        private IGeographicCoordinateSystem _TargetGCS;

        internal GeographicTransform(IGeographicCoordinateSystem sourceGCS, IGeographicCoordinateSystem targetGCS)
        {
            this._SourceGCS = sourceGCS;
            this._TargetGCS = targetGCS;
        }

        /// <summary>
        /// Creates the inverse transform of this object.
        /// </summary>
        /// <remarks>This method may fail if the transform is not one to one. However, all cartographic projections should succeed.</remarks>
        /// <returns></returns>
        public override IMathTransform Inverse()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Reverses the transformation
        /// </summary>
        public override void Invert()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Transforms a coordinate point. The passed parameter point should not be modified.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override double[] Transform(double[] point)
        {
            double[] numArray = (double[]) point.Clone();
            numArray[0] /= this.SourceGCS.AngularUnit.RadiansPerUnit;
            numArray[0] -= this.SourceGCS.PrimeMeridian.Longitude / this.SourceGCS.PrimeMeridian.AngularUnit.RadiansPerUnit;
            numArray[0] += this.TargetGCS.PrimeMeridian.Longitude / this.TargetGCS.PrimeMeridian.AngularUnit.RadiansPerUnit;
            numArray[0] *= this.SourceGCS.AngularUnit.RadiansPerUnit;
            return numArray;
        }

        /// <summary>
        /// Transforms a list of coordinate point ordinal values.
        /// </summary>
        /// <remarks>
        /// This method is provided for efficiently transforming many points. The supplied array 
        /// of ordinal values will contain packed ordinal values. For example, if the source 
        /// dimension is 3, then the ordinals will be packed in this order (x0,y0,z0,x1,y1,z1 ...).
        /// The size of the passed array must be an integer multiple of DimSource. The returned 
        /// ordinal values are packed in a similar way. In some DCPs. the ordinals may be 
        /// transformed in-place, and the returned array may be the same as the passed array.
        /// So any client code should not attempt to reuse the passed ordinal values (although
        /// they can certainly reuse the passed array). If there is any problem then the server
        /// implementation will throw an exception. If this happens then the client should not
        /// make any assumptions about the state of the ordinal values.
        /// </remarks>
        /// <param name="points"></param>
        /// <returns></returns>
        public override List<double[]> TransformList(List<double[]> points)
        {
            List<double[]> list = new List<double[]>(points.Count);
            foreach (double[] numArray in points)
            {
                list.Add(this.Transform(numArray));
            }
            return list;
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
        /// as defined in the simple features specification. [NOT IMPLEMENTED].
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

