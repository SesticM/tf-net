namespace Topology.CoordinateSystems.Projections
{
    using Topology.CoordinateSystems;
    using Topology.CoordinateSystems.Transformations;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Summary description for MathTransform.
    /// </summary>
    /// <remarks>
    /// <para>Universal (UTM) and Modified (MTM) Transverses Mercator projections. This
    /// is a cylindrical projection, in which the cylinder has been rotated 90°.
    /// Instead of being tangent to the equator (or to an other standard latitude),
    /// it is tangent to a central meridian. Deformation are more important as we
    /// are going futher from the central meridian. The Transverse Mercator
    /// projection is appropriate for region wich have a greater extent north-south
    /// than east-west.</para>
    /// 
    /// <para>Reference: John P. Snyder (Map Projections - A Working Manual,
    /// U.S. Geological Survey Professional Paper 1395, 1987)</para>
    /// </remarks>
    internal class TransverseMercator : MapProjection
    {
        private double central_meridian;
        private double e;
        private double e0;
        private double e1;
        private double e2;
        private double e3;
        private double es;
        private double esp;
        private double false_easting;
        private double false_northing;
        private double lat_origin;
        private double ml0;
        private double r_major;
        private double r_minor;
        private double scale_factor;

        /// <summary>
        /// Creates an instance of an TransverseMercatorProjection projection object.
        /// </summary>
        /// <param name="parameters">List of parameters to initialize the projection.</param>
        public TransverseMercator(List<ProjectionParameter> parameters) : this(parameters, false)
        {
        }

        /// <summary>
        /// Creates an instance of an TransverseMercatorProjection projection object.
        /// </summary>
        /// <param name="parameters">List of parameters to initialize the projection.</param>
        /// <param name="inverse">Flag indicating wether is a forward/projection (false) or an inverse projection (true).</param>
        /// <remarks>
        /// <list type="bullet">
        /// <listheader><term>Items</term><description>Descriptions</description></listheader>
        /// <item><term>semi_major</term><description>Semi major radius</description></item>
        /// <item><term>semi_minor</term><description>Semi minor radius</description></item>
        /// <item><term>scale_factor</term><description></description></item>
        /// <item><term>central meridian</term><description></description></item>
        /// <item><term>latitude_origin</term><description></description></item>
        /// <item><term>false_easting</term><description></description></item>
        /// <item><term>false_northing</term><description></description></item>
        /// </list>
        /// </remarks>
        public TransverseMercator(List<ProjectionParameter> parameters, bool inverse) : base(parameters, inverse)
        {
            base.Name = "Transverse_Mercator";
            base.Authority = "EPSG";
            base.AuthorityCode = 0x264fL;
            ProjectionParameter parameter = base.GetParameter("semi_major");
            ProjectionParameter parameter2 = base.GetParameter("semi_minor");
            ProjectionParameter parameter3 = base.GetParameter("scale_factor");
            ProjectionParameter parameter4 = base.GetParameter("central_meridian");
            ProjectionParameter parameter5 = base.GetParameter("latitude_of_origin");
            ProjectionParameter parameter6 = base.GetParameter("false_easting");
            ProjectionParameter parameter7 = base.GetParameter("false_northing");
            if (parameter == null)
            {
                throw new ArgumentException("Missing projection parameter 'semi_major'");
            }
            if (parameter2 == null)
            {
                throw new ArgumentException("Missing projection parameter 'semi_minor'");
            }
            if (parameter3 == null)
            {
                throw new ArgumentException("Missing projection parameter 'scale_factor'");
            }
            if (parameter4 == null)
            {
                throw new ArgumentException("Missing projection parameter 'central_meridian'");
            }
            if (parameter5 == null)
            {
                throw new ArgumentException("Missing projection parameter 'latitude_of_origin'");
            }
            if (parameter6 == null)
            {
                throw new ArgumentException("Missing projection parameter 'false_easting'");
            }
            if (parameter7 == null)
            {
                throw new ArgumentException("Missing projection parameter 'false_northing'");
            }
            this.r_major = parameter.Value;
            this.r_minor = parameter2.Value;
            this.scale_factor = parameter3.Value;
            this.central_meridian = MathTransform.Degrees2Radians(parameter4.Value);
            this.lat_origin = MathTransform.Degrees2Radians(parameter5.Value);
            this.false_easting = parameter6.Value;
            this.false_northing = parameter7.Value;
            this.es = 1 - Math.Pow(this.r_minor / this.r_major, 2);
            this.e = Math.Sqrt(this.es);
            this.e0 = MapProjection.e0fn(this.es);
            this.e1 = MapProjection.e1fn(this.es);
            this.e2 = MapProjection.e2fn(this.es);
            this.e3 = MapProjection.e3fn(this.es);
            this.ml0 = this.r_major * MapProjection.mlfn(this.e0, this.e1, this.e2, this.e3, this.lat_origin);
            this.esp = this.es / (1 - this.es);
        }

        /// <summary>
        /// Converts coordinates in decimal degrees to projected meters.
        /// </summary>
        /// <param name="lonlat">The point in decimal degrees.</param>
        /// <returns>Point in projected meters</returns>
        public override double[] DegreesToMeters(double[] lonlat)
        {
            double num4;
            double num5;
            double num = MathTransform.Degrees2Radians(lonlat[0]);
            double val = MathTransform.Degrees2Radians(lonlat[1]);
            double num3 = 0;
            num3 = MapProjection.adjust_lon(num - this.central_meridian);
            MapProjection.sincos(val, out num4, out num5);
            double x = num5 * num3;
            double num7 = Math.Pow(x, 2);
            double num8 = this.esp * Math.Pow(num5, 2);
            double num10 = Math.Tan(val);
            double num9 = Math.Pow(num10, 2);
            double d = 1 - (this.es * Math.Pow(num4, 2));
            double num12 = this.r_major / Math.Sqrt(d);
            double num13 = this.r_major * MapProjection.mlfn(this.e0, this.e1, this.e2, this.e3, val);
            return new double[] { ((((this.scale_factor * num12) * x) * (1 + ((num7 / 6) * (((1 - num9) + num8) + ((num7 / 20) * ((((5 - (18 * num9)) + Math.Pow(num9, 2)) + (72 * num8)) - (58 * this.esp))))))) + this.false_easting), ((this.scale_factor * ((num13 - this.ml0) + ((num12 * num10) * (num7 * (0.5 + ((num7 / 24) * ((((5 - num9) + (9 * num8)) + (4 * Math.Pow(num8, 2))) + ((num7 / 30) * ((((61 - (58 * num9)) + Math.Pow(num9, 2)) + (600 * num8)) - (330 * this.esp)))))))))) + this.false_northing) };
        }

        /// <summary>
        /// Returns the inverse of this projection.
        /// </summary>
        /// <returns>IMathTransform that is the reverse of the current projection.</returns>
        public override IMathTransform Inverse()
        {
            if (base._inverse == null)
            {
                base._inverse = new TransverseMercator(base._Parameters, !base._isInverse);
            }
            return base._inverse;
        }

        /// <summary>
        /// Converts coordinates in projected meters to decimal degrees.
        /// </summary>
        /// <param name="p">Point in meters</param>
        /// <returns>Transformed point in decimal degrees</returns>
        public override double[] MetersToDegrees(double[] p)
        {
            long num16 = 6L;
            double num17 = p[0] - this.false_easting;
            double x = p[1] - this.false_northing;
            double d = (this.ml0 + (x / this.scale_factor)) / this.r_major;
            double num2 = d;
            long num4 = 0L;
            while (true)
            {
                double num3 = ((((d + (this.e1 * Math.Sin(2 * num2))) - (this.e2 * Math.Sin(4 * num2))) + (this.e3 * Math.Sin(6 * num2))) / this.e0) - num2;
                num2 += num3;
                if (Math.Abs(num3) <= 1E-10)
                {
                    break;
                }
                if (num4 >= num16)
                {
                    throw new ApplicationException("Latitude failed to converge");
                }
                num4 += 1L;
            }
            if (Math.Abs(num2) < 1.5707963267948966)
            {
                double num5;
                double num6;
                MapProjection.sincos(num2, out num5, out num6);
                double num7 = Math.Tan(num2);
                double num8 = this.esp * Math.Pow(num6, 2);
                double num9 = Math.Pow(num8, 2);
                double num10 = Math.Pow(num7, 2);
                double num11 = Math.Pow(num10, 2);
                d = 1 - (this.es * Math.Pow(num5, 2));
                double num12 = this.r_major / Math.Sqrt(d);
                double num13 = (num12 * (1 - this.es)) / d;
                double num14 = num17 / (num12 * this.scale_factor);
                double num15 = Math.Pow(num14, 2);
                double rad = num2 - ((((num12 * num7) * num15) / num13) * (0.5 - ((num15 / 24) * (((((5 + (3 * num10)) + (10 * num8)) - (4 * num9)) - (9 * this.esp)) - ((num15 / 30) * (((((61 + (90 * num10)) + (298 * num8)) + (45 * num11)) - (252 * this.esp)) - (3 * num9)))))));
                double num20 = MapProjection.adjust_lon(this.central_meridian + ((num14 * (1 - ((num15 / 6) * (((1 + (2 * num10)) + num8) - ((num15 / 20) * (((((5 - (2 * num8)) + (28 * num10)) - (3 * num9)) + (8 * this.esp)) + (24 * num11))))))) / num6));
                return new double[] { MathTransform.Radians2Degrees(num20), MathTransform.Radians2Degrees(rad) };
            }
            return new double[] { MathTransform.Radians2Degrees(1.5707963267948966 * MapProjection.sign(x)), MathTransform.Radians2Degrees(this.central_meridian) };
        }
    }
}

