namespace Topology.CoordinateSystems.Transformations
{
    using Topology.CoordinateSystems;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Transformation for applying 
    /// </summary>
    internal class DatumTransform : MathTransform
    {
        protected IMathTransform _inverse;
        private bool _isInverse;
        private Wgs84ConversionInfo _ToWgs94;
        private double[] v;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Topology.CoordinateSystems.Transformations.DatumTransform" /> class.
        /// </summary>
        /// <param name="towgs84"></param>
        public DatumTransform(Wgs84ConversionInfo towgs84) : this(towgs84, false)
        {
        }

        private DatumTransform(Wgs84ConversionInfo towgs84, bool isInverse)
        {
            this._ToWgs94 = towgs84;
            this.v = this._ToWgs94.GetAffineTransform();
            this._isInverse = isInverse;
        }

        private double[] Apply(double[] p)
        {
            return new double[] { ((((this.v[0] * p[0]) - (this.v[3] * p[1])) + (this.v[2] * p[2])) + this.v[4]), ((((this.v[3] * p[0]) + (this.v[0] * p[1])) - (this.v[1] * p[2])) + this.v[5]), ((((-this.v[2] * p[0]) + (this.v[1] * p[1])) + (this.v[0] * p[2])) + this.v[6]) };
        }

        private double[] ApplyInverted(double[] p)
        {
            return new double[] { ((((this.v[0] * p[0]) + (this.v[3] * p[1])) - (this.v[2] * p[2])) - this.v[4]), ((((-this.v[3] * p[0]) + (this.v[0] * p[1])) + (this.v[1] * p[2])) - this.v[5]), ((((this.v[2] * p[0]) - (this.v[1] * p[1])) + (this.v[0] * p[2])) - this.v[6]) };
        }

        /// <summary>
        /// Creates the inverse transform of this object.
        /// </summary>
        /// <returns></returns>
        /// <remarks>This method may fail if the transform is not one to one. However, all cartographic projections should succeed.</remarks>
        public override IMathTransform Inverse()
        {
            if (this._inverse == null)
            {
                this._inverse = new DatumTransform(this._ToWgs94, !this._isInverse);
            }
            return this._inverse;
        }

        /// <summary>
        /// Reverses the transformation
        /// </summary>
        public override void Invert()
        {
            this._isInverse = !this._isInverse;
        }

        /// <summary>
        /// Transforms a coordinate point. The passed parameter point should not be modified.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override double[] Transform(double[] point)
        {
            if (!this._isInverse)
            {
                return this.Apply(point);
            }
            return this.ApplyInverted(point);
        }

        /// <summary>
        /// Transforms a list of coordinate point ordinal values.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
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
        /// Gets a Well-Known text representation of this object.
        /// </summary>
        /// <value></value>
        public override string WKT
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        /// <summary>
        /// Gets an XML representation of this object.
        /// </summary>
        /// <value></value>
        public override string XML
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }
    }
}

