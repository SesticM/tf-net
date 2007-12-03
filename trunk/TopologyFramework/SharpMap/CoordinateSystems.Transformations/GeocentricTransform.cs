namespace Topology.CoordinateSystems.Transformations
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <para>Latitude, Longitude and ellipsoidal height in terms of a 3-dimensional geographic system
    /// may by expressed in terms of a geocentric (earth centered) Cartesian coordinate reference system
    /// X, Y, Z with the Z axis corresponding to the earth's rotation axis positive northwards, the X
    /// axis through the intersection of the prime meridian and equator, and the Y axis through
    /// the intersection of the equator with longitude 90 degrees east. The geographic and geocentric
    /// systems are based on the same geodetic datum.</para>
    /// <para>Geocentric coordinate reference systems are conventionally taken to be defined with the X
    /// axis through the intersection of the Greenwich meridian and equator. This requires that the equivalent
    /// geographic coordinate reference systems based on a non-Greenwich prime meridian should first be
    /// transformed to their Greenwich equivalent. Geocentric coordinates X, Y and Z take their units from
    /// the units of the ellipsoid axes (a and b). As it is conventional for X, Y and Z to be in metres,
    /// if the ellipsoid axis dimensions are given in another linear unit they should first be converted
    /// to metres.</para>
    /// </remarks>
    internal class GeocentricTransform : MathTransform
    {
        /// <summary>
        /// 
        /// </summary>
        protected MathTransform _inverse;
        /// <summary>
        /// 
        /// </summary>
        protected bool _isInverse;
        /// <summary>
        /// 
        /// </summary>
        protected List<ProjectionParameter> _Parameters;
        private double ab;
        private const double AD_C = 1.0026;
        private double ba;
        private const double COS_67P5 = 0.38268343236508978;
        private double es;
        private double semiMajor;
        private double semiMinor;
        private double ses;

        /// <summary>
        /// Initializes a geocentric projection object
        /// </summary>
        /// <param name="parameters">List of parameters to initialize the projection.</param>
        internal GeocentricTransform(List<ProjectionParameter> parameters)
        {
            this._Parameters = parameters;
            this.semiMajor = this._Parameters.Find(delegate (ProjectionParameter par) {
                return par.Name.Equals("semi_major", StringComparison.OrdinalIgnoreCase);
            }).Value;
            this.semiMinor = this._Parameters.Find(delegate (ProjectionParameter par) {
                return par.Name.Equals("semi_minor", StringComparison.OrdinalIgnoreCase);
            }).Value;
            this.es = 1 - ((this.semiMinor * this.semiMinor) / (this.semiMajor * this.semiMajor));
            this.ses = (Math.Pow(this.semiMajor, 2) - Math.Pow(this.semiMinor, 2)) / Math.Pow(this.semiMinor, 2);
            this.ba = this.semiMinor / this.semiMajor;
            this.ab = this.semiMajor / this.semiMinor;
        }

        /// <summary>
        /// Initializes a geocentric projection object
        /// </summary>
        /// <param name="parameters">List of parameters to initialize the projection.</param>
        /// <param name="isInverse">Indicates whether the projection forward (meters to degrees or degrees to meters).</param>
        public GeocentricTransform(List<ProjectionParameter> parameters, bool isInverse) : this(parameters)
        {
            this._isInverse = isInverse;
        }

        /// <summary>
        /// Converts coordinates in decimal degrees to projected meters.
        /// </summary>
        /// <param name="lonlat">The point in decimal degrees.</param>
        /// <returns>Point in projected meters</returns>
        private double[] DegreesToMeters(double[] lonlat)
        {
            double d = MathTransform.Degrees2Radians(lonlat[0]);
            double a = MathTransform.Degrees2Radians(lonlat[1]);
            double num3 = (lonlat.Length < 3) ? 0 : (lonlat[2].Equals(double.NaN) ? 0 : lonlat[2]);
            double num4 = this.semiMajor / Math.Sqrt(1 - (this.es * Math.Pow(Math.Sin(a), 2)));
            double num5 = ((num4 + num3) * Math.Cos(a)) * Math.Cos(d);
            double num6 = ((num4 + num3) * Math.Cos(a)) * Math.Sin(d);
            double num7 = (((1 - this.es) * num4) + num3) * Math.Sin(a);
            return new double[] { num5, num6, num7 };
        }

        /// <summary>
        /// Returns the inverse of this conversion.
        /// </summary>
        /// <returns>IMathTransform that is the reverse of the current conversion.</returns>
        public override IMathTransform Inverse()
        {
            if (this._inverse == null)
            {
                this._inverse = new GeocentricTransform(this._Parameters, !this._isInverse);
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
        /// Converts coordinates in projected meters to decimal degrees.
        /// </summary>
        /// <param name="pnt">Point in meters</param>
        /// <returns>Transformed point in decimal degrees</returns>
        private double[] MetersToDegrees(double[] pnt)
        {
            bool flag = false;
            double num = (pnt.Length < 3) ? 0 : (pnt[2].Equals(double.NaN) ? 0 : pnt[2]);
            double rad = 0;
            double num3 = 0;
            double num4 = 0;
            if (pnt[0] != 0)
            {
                rad = Math.Atan2(pnt[1], pnt[0]);
            }
            else if (pnt[1] > 0)
            {
                rad = 1.5707963267948966;
            }
            else if (pnt[1] < 0)
            {
                rad = -1.5707963267948966;
            }
            else
            {
                flag = true;
                rad = 0;
                if (num > 0)
                {
                    num3 = 1.5707963267948966;
                }
                else if (num < 0)
                {
                    num3 = -1.5707963267948966;
                }
                else
                {
                    return new double[] { MathTransform.Radians2Degrees(rad), MathTransform.Radians2Degrees(1.5707963267948966), -this.semiMinor };
                }
            }
            double d = (pnt[0] * pnt[0]) + (pnt[1] * pnt[1]);
            double num6 = Math.Sqrt(d);
            double num7 = num * 1.0026;
            double num8 = Math.Sqrt((num7 * num7) + d);
            double x = num7 / num8;
            double num10 = num6 / num8;
            double num11 = Math.Pow(x, 3);
            double num12 = num + ((this.semiMinor * this.ses) * num11);
            double num13 = num6 - ((((this.semiMajor * this.es) * num10) * num10) * num10);
            double num14 = Math.Sqrt((num12 * num12) + (num13 * num13));
            double num15 = num12 / num14;
            double num16 = num13 / num14;
            double num17 = this.semiMajor / Math.Sqrt(1 - ((this.es * num15) * num15));
            if (num16 >= 0.38268343236508978)
            {
                num4 = (num6 / num16) - num17;
            }
            else if (num16 <= -0.38268343236508978)
            {
                num4 = (num6 / -num16) - num17;
            }
            else
            {
                num4 = (num / num15) + (num17 * (this.es - 1));
            }
            if (!flag)
            {
                num3 = Math.Atan(num15 / num16);
            }
            return new double[] { MathTransform.Radians2Degrees(rad), MathTransform.Radians2Degrees(num3), num4 };
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
                return this.DegreesToMeters(point);
            }
            return this.MetersToDegrees(point);
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
            for (int i = 0; i < points.Count; i++)
            {
                double[] point = points[i];
                list.Add(this.Transform(point));
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
                throw new NotImplementedException("The method or operation is not implemented.");
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
                throw new NotImplementedException("The method or operation is not implemented.");
            }
        }
    }
}

