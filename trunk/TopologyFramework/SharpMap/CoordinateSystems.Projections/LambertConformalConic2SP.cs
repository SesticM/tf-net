namespace Topology.CoordinateSystems.Projections
{
    using Topology.CoordinateSystems;
    using Topology.CoordinateSystems.Transformations;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implemetns the Lambert Conformal Conic 2SP Projection.
    /// </summary>
    /// <remarks>
    /// <para>The Lambert Conformal Conic projection is a standard projection for presenting maps
    /// of land areas whose East-West extent is large compared with their North-South extent.
    /// This projection is "conformal" in the sense that lines of latitude and longitude, 
    /// which are perpendicular to one another on the earth's surface, are also perpendicular
    /// to one another in the projected domain.</para>
    /// </remarks>
    internal class LambertConformalConic2SP : MapProjection
    {
        private double _falseEasting;
        private double _falseNorthing;
        private double center_lat;
        private double center_lon;
        private double e;
        private double es;
        private double f0;
        private double ns;
        private double rh;

        /// <summary>
        /// Creates an instance of an LambertConformalConic2SPProjection projection object.
        /// </summary>
        /// <remarks>
        /// <para>The parameters this projection expects are listed below.</para>
        /// <list type="table">
        /// <listheader><term>Items</term><description>Descriptions</description></listheader>
        /// <item><term>latitude_of_false_origin</term><description>The latitude of the point which is not the natural origin and at which grid coordinate values false easting and false northing are defined.</description></item>
        /// <item><term>longitude_of_false_origin</term><description>The longitude of the point which is not the natural origin and at which grid coordinate values false easting and false northing are defined.</description></item>
        /// <item><term>latitude_of_1st_standard_parallel</term><description>For a conic projection with two standard parallels, this is the latitude of intersection of the cone with the ellipsoid that is nearest the pole.  Scale is true along this parallel.</description></item>
        /// <item><term>latitude_of_2nd_standard_parallel</term><description>For a conic projection with two standard parallels, this is the latitude of intersection of the cone with the ellipsoid that is furthest from the pole.  Scale is true along this parallel.</description></item>
        /// <item><term>easting_at_false_origin</term><description>The easting value assigned to the false origin.</description></item>
        /// <item><term>northing_at_false_origin</term><description>The northing value assigned to the false origin.</description></item>
        /// </list>
        /// </remarks>
        /// <param name="parameters">List of parameters to initialize the projection.</param>
        public LambertConformalConic2SP(List<ProjectionParameter> parameters) : this(parameters, false)
        {
        }

        /// <summary>
        /// Creates an instance of an Albers projection object.
        /// </summary>
        /// <remarks>
        /// <para>The parameters this projection expects are listed below.</para>
        /// <list type="table">
        /// <listheader><term>Parameter</term><description>Description</description></listheader>
        /// <item><term>latitude_of_origin</term><description>The latitude of the point which is not the natural origin and at which grid coordinate values false easting and false northing are defined.</description></item>
        /// <item><term>central_meridian</term><description>The longitude of the point which is not the natural origin and at which grid coordinate values false easting and false northing are defined.</description></item>
        /// <item><term>standard_parallel_1</term><description>For a conic projection with two standard parallels, this is the latitude of intersection of the cone with the ellipsoid that is nearest the pole.  Scale is true along this parallel.</description></item>
        /// <item><term>standard_parallel_2</term><description>For a conic projection with two standard parallels, this is the latitude of intersection of the cone with the ellipsoid that is furthest from the pole.  Scale is true along this parallel.</description></item>
        /// <item><term>false_easting</term><description>The easting value assigned to the false origin.</description></item>
        /// <item><term>false_northing</term><description>The northing value assigned to the false origin.</description></item>
        /// </list>
        /// </remarks>
        /// <param name="parameters">List of parameters to initialize the projection.</param>
        /// <param name="isInverse">Indicates whether the projection forward (meters to degrees or degrees to meters).</param>
        public LambertConformalConic2SP(List<ProjectionParameter> parameters, bool isInverse) : base(parameters, isInverse)
        {
            double num5;
            double num6;
            base.Name = "Lambert_Conformal_Conic_2SP";
            base.Authority = "EPSG";
            base.AuthorityCode = 0x264aL;
            ProjectionParameter parameter = base.GetParameter("latitude_of_origin");
            ProjectionParameter parameter2 = base.GetParameter("central_meridian");
            ProjectionParameter parameter3 = base.GetParameter("standard_parallel_1");
            ProjectionParameter parameter4 = base.GetParameter("standard_parallel_2");
            ProjectionParameter parameter5 = base.GetParameter("false_easting");
            ProjectionParameter parameter6 = base.GetParameter("false_northing");
            if (parameter == null)
            {
                throw new ArgumentException("Missing projection parameter 'latitude_of_origin'");
            }
            if (parameter2 == null)
            {
                throw new ArgumentException("Missing projection parameter 'central_meridian'");
            }
            if (parameter3 == null)
            {
                throw new ArgumentException("Missing projection parameter 'standard_parallel_1'");
            }
            if (parameter4 == null)
            {
                throw new ArgumentException("Missing projection parameter 'standard_parallel_2'");
            }
            if (parameter5 == null)
            {
                throw new ArgumentException("Missing projection parameter 'false_easting'");
            }
            if (parameter6 == null)
            {
                throw new ArgumentException("Missing projection parameter 'false_northing'");
            }
            double num = MathTransform.Degrees2Radians(parameter.Value);
            double num2 = MathTransform.Degrees2Radians(parameter2.Value);
            double val = MathTransform.Degrees2Radians(parameter3.Value);
            double num4 = MathTransform.Degrees2Radians(parameter4.Value);
            this._falseEasting = parameter5.Value;
            this._falseNorthing = parameter6.Value;
            if (Math.Abs((double) (val + num4)) < 1E-10)
            {
                throw new ArgumentException("Equal latitudes for St. Parallels on opposite sides of equator.");
            }
            this.es = 1 - Math.Pow(base._semiMinor / base._semiMajor, 2);
            this.e = Math.Sqrt(this.es);
            this.center_lon = num2;
            this.center_lat = num;
            MapProjection.sincos(val, out num5, out num6);
            double num7 = num5;
            double num8 = MapProjection.msfnz(this.e, num5, num6);
            double x = MapProjection.tsfnz(this.e, val, num5);
            MapProjection.sincos(num4, out num5, out num6);
            double num9 = MapProjection.msfnz(this.e, num5, num6);
            double num12 = MapProjection.tsfnz(this.e, num4, num5);
            num5 = Math.Sin(this.center_lat);
            double num10 = MapProjection.tsfnz(this.e, this.center_lat, num5);
            if (Math.Abs((double) (val - num4)) > 1E-10)
            {
                this.ns = Math.Log(num8 / num9) / Math.Log(x / num12);
            }
            else
            {
                this.ns = num7;
            }
            this.f0 = num8 / (this.ns * Math.Pow(x, this.ns));
            this.rh = (base._semiMajor * this.f0) * Math.Pow(num10, this.ns);
        }

        /// <summary>
        /// Converts coordinates in decimal degrees to projected meters.
        /// </summary>
        /// <param name="lonlat">The point in decimal degrees.</param>
        /// <returns>Point in projected meters</returns>
        public override double[] DegreesToMeters(double[] lonlat)
        {
            double num4;
            double num = MathTransform.Degrees2Radians(lonlat[0]);
            double num2 = MathTransform.Degrees2Radians(lonlat[1]);
            if (Math.Abs((double) (Math.Abs(num2) - 1.5707963267948966)) > 1E-10)
            {
                double sinphi = Math.Sin(num2);
                double x = MapProjection.tsfnz(this.e, num2, sinphi);
                num4 = (base._semiMajor * this.f0) * Math.Pow(x, this.ns);
            }
            else
            {
                double num3 = num2 * this.ns;
                if (num3 <= 0)
                {
                    throw new ApplicationException();
                }
                num4 = 0;
            }
            double a = this.ns * MapProjection.adjust_lon(num - this.center_lon);
            return new double[] { ((num4 * Math.Sin(a)) + this._falseEasting), ((this.rh - (num4 * Math.Cos(a))) + this._falseNorthing) };
        }

        /// <summary>
        /// Returns the inverse of this projection.
        /// </summary>
        /// <returns>IMathTransform that is the reverse of the current projection.</returns>
        public override IMathTransform Inverse()
        {
            if (base._inverse == null)
            {
                base._inverse = new LambertConformalConic2SP(base._Parameters, !base._isInverse);
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
            double num3;
            double num4;
            double naN = double.NaN;
            double rad = double.NaN;
            long flag = 0L;
            double num8 = p[0] - this._falseEasting;
            double num9 = (this.rh - p[1]) + this._falseNorthing;
            if (this.ns > 0)
            {
                num3 = Math.Sqrt((num8 * num8) + (num9 * num9));
                num4 = 1;
            }
            else
            {
                num3 = -Math.Sqrt((num8 * num8) + (num9 * num9));
                num4 = -1;
            }
            double num6 = 0;
            if (num3 != 0)
            {
                num6 = Math.Atan2(num4 * num8, num4 * num9);
            }
            if ((num3 != 0) || (this.ns > 0))
            {
                num4 = 1 / this.ns;
                double ts = Math.Pow(num3 / (base._semiMajor * this.f0), num4);
                rad = MapProjection.phi2z(this.e, ts, out flag);
                if (flag != 0L)
                {
                    throw new ApplicationException();
                }
            }
            else
            {
                rad = -1.5707963267948966;
            }
            naN = MapProjection.adjust_lon((num6 / this.ns) + this.center_lon);
            return new double[] { MathTransform.Radians2Degrees(naN), MathTransform.Radians2Degrees(rad) };
        }
    }
}

