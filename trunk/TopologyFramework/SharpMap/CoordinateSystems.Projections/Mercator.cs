namespace Topology.CoordinateSystems.Projections
{
    using Topology.CoordinateSystems;
    using Topology.CoordinateSystems.Transformations;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implements the Mercator projection.
    /// </summary>
    /// <remarks>
    /// <para>This map projection introduced in 1569 by Gerardus Mercator. It is often described as a cylindrical projection,
    /// but it must be derived mathematically. The meridians are equally spaced, parallel vertical lines, and the
    /// parallels of latitude are parallel, horizontal straight lines, spaced farther and farther apart as their distance
    /// from the Equator increases. This projection is widely used for navigation charts, because any straight line
    /// on a Mercator-projection map is a line of constant true bearing that enables a navigator to plot a straight-line
    /// course. It is less practical for world maps because the scale is distorted; areas farther away from the equator
    /// appear disproportionately large. On a Mercator projection, for example, the landmass of Greenland appears to be
    /// greater than that of the continent of South America; in actual area, Greenland is smaller than the Arabian Peninsula.
    /// </para>
    /// </remarks>
    internal class Mercator : MapProjection
    {
        private double _falseEasting;
        private double _falseNorthing;
        private double e;
        private double e2;
        private double k0;
        private double lat_origin;
        private double lon_center;

        /// <summary>
        /// Initializes the MercatorProjection object with the specified parameters to project points. 
        /// </summary>
        /// <param name="parameters">ParameterList with the required parameters.</param>
        /// <remarks>
        /// </remarks>
        public Mercator(List<ProjectionParameter> parameters) : this(parameters, false)
        {
        }

        /// <summary>
        /// Initializes the MercatorProjection object with the specified parameters.
        /// </summary>
        /// <param name="parameters">List of parameters to initialize the projection.</param>
        /// <param name="isInverse">Indicates whether the projection forward (meters to degrees or degrees to meters).</param>
        /// <remarks>
        /// <para>The parameters this projection expects are listed below.</para>
        /// <list type="table">
        /// <listheader><term>Items</term><description>Descriptions</description></listheader>
        /// <item><term>central_meridian</term><description>The longitude of the point from which the values of both the geographical coordinates on the ellipsoid and the grid coordinates on the projection are deemed to increment or decrement for computational purposes. Alternatively it may be considered as the longitude of the point which in the absence of application of false coordinates has grid coordinates of (0,0).</description></item>
        /// <item><term>latitude_of_origin</term><description>The latitude of the point from which the values of both the geographical coordinates on the ellipsoid and the grid coordinates on the projection are deemed to increment or decrement for computational purposes. Alternatively it may be considered as the latitude of the point which in the absence of application of false coordinates has grid coordinates of (0,0).</description></item>
        /// <item><term>scale_factor</term><description>The factor by which the map grid is reduced or enlarged during the projection process, defined by its value at the natural origin.</description></item>
        /// <item><term>false_easting</term><description>Since the natural origin may be at or near the centre of the projection and under normal coordinate circumstances would thus give rise to negative coordinates over parts of the mapped area, this origin is usually given false coordinates which are large enough to avoid this inconvenience. The False Easting, FE, is the easting value assigned to the abscissa (east).</description></item>
        /// <item><term>false_northing</term><description>Since the natural origin may be at or near the centre of the projection and under normal coordinate circumstances would thus give rise to negative coordinates over parts of the mapped area, this origin is usually given false coordinates which are large enough to avoid this inconvenience. The False Northing, FN, is the northing value assigned to the ordinate.</description></item>
        /// </list>
        /// </remarks>
        public Mercator(List<ProjectionParameter> parameters, bool isInverse) : base(parameters, isInverse)
        {
            base.Authority = "EPSG";
            ProjectionParameter parameter = base.GetParameter("central_meridian");
            ProjectionParameter parameter2 = base.GetParameter("latitude_of_origin");
            ProjectionParameter parameter3 = base.GetParameter("scale_factor");
            ProjectionParameter parameter4 = base.GetParameter("false_easting");
            ProjectionParameter parameter5 = base.GetParameter("false_northing");
            if (parameter == null)
            {
                throw new ArgumentException("Missing projection parameter 'central_meridian'");
            }
            if (parameter2 == null)
            {
                throw new ArgumentException("Missing projection parameter 'latitude_of_origin'");
            }
            if (parameter4 == null)
            {
                throw new ArgumentException("Missing projection parameter 'false_easting'");
            }
            if (parameter5 == null)
            {
                throw new ArgumentException("Missing projection parameter 'false_northing'");
            }
            this.lon_center = MathTransform.Degrees2Radians(parameter.Value);
            this.lat_origin = MathTransform.Degrees2Radians(parameter2.Value);
            this._falseEasting = parameter4.Value;
            this._falseNorthing = parameter5.Value;
            double num = base._semiMinor / base._semiMajor;
            this.e2 = 1 - (num * num);
            this.e = Math.Sqrt(this.e2);
            if (parameter3 == null)
            {
                this.k0 = Math.Cos(this.lat_origin) / Math.Sqrt(1 - ((this.e2 * Math.Sin(this.lat_origin)) * Math.Sin(this.lat_origin)));
                base.AuthorityCode = 0x264dL;
                base.Name = "Mercator_2SP";
            }
            else
            {
                this.k0 = parameter3.Value;
                base.Name = "Mercator_1SP";
            }
            base.Authority = "EPSG";
        }

        /// <summary>
        /// Converts coordinates in decimal degrees to projected meters.
        /// </summary>
        /// <remarks>
        /// <para>The parameters this projection expects are listed below.</para>
        /// <list type="table">
        /// <listheader><term>Items</term><description>Descriptions</description></listheader>
        /// <item><term>longitude_of_natural_origin</term><description>The longitude of the point from which the values of both the geographical coordinates on the ellipsoid and the grid coordinates on the projection are deemed to increment or decrement for computational purposes. Alternatively it may be considered as the longitude of the point which in the absence of application of false coordinates has grid coordinates of (0,0).  Sometimes known as ""central meridian""."</description></item>
        /// <item><term>latitude_of_natural_origin</term><description>The latitude of the point from which the values of both the geographical coordinates on the ellipsoid and the grid coordinates on the projection are deemed to increment or decrement for computational purposes. Alternatively it may be considered as the latitude of the point which in the absence of application of false coordinates has grid coordinates of (0,0).</description></item>
        /// <item><term>scale_factor_at_natural_origin</term><description>The factor by which the map grid is reduced or enlarged during the projection process, defined by its value at the natural origin.</description></item>
        /// <item><term>false_easting</term><description>Since the natural origin may be at or near the centre of the projection and under normal coordinate circumstances would thus give rise to negative coordinates over parts of the mapped area, this origin is usually given false coordinates which are large enough to avoid this inconvenience. The False Easting, FE, is the easting value assigned to the abscissa (east).</description></item>
        /// <item><term>false_northing</term><description>Since the natural origin may be at or near the centre of the projection and under normal coordinate circumstances would thus give rise to negative coordinates over parts of the mapped area, this origin is usually given false coordinates which are large enough to avoid this inconvenience. The False Northing, FN, is the northing value assigned to the ordinate .</description></item>
        /// </list>
        /// </remarks>
        /// <param name="lonlat">The point in decimal degrees.</param>
        /// <returns>Point in projected meters</returns>
        public override double[] DegreesToMeters(double[] lonlat)
        {
            if (double.IsNaN(lonlat[0]) || double.IsNaN(lonlat[1]))
            {
                return new double[] { double.NaN, double.NaN };
            }
            double num = MathTransform.Degrees2Radians(lonlat[0]);
            double num2 = MathTransform.Degrees2Radians(lonlat[1]);
            if (Math.Abs((double) (Math.Abs(num2) - 1.5707963267948966)) <= 1E-10)
            {
                throw new ApplicationException("Transformation cannot be computed at the poles.");
            }
            double num3 = this.e * Math.Sin(num2);
            double num4 = this._falseEasting + ((base._semiMajor * this.k0) * (num - this.lon_center));
            double num5 = this._falseNorthing + ((base._semiMajor * this.k0) * Math.Log(Math.Tan(0.78539816339744828 + (num2 * 0.5)) * Math.Pow((1 - num3) / (1 + num3), this.e * 0.5)));
            return new double[] { num4, num5 };
        }

        /// <summary>
        /// Returns the inverse of this projection.
        /// </summary>
        /// <returns>IMathTransform that is the reverse of the current projection.</returns>
        public override IMathTransform Inverse()
        {
            if (base._inverse == null)
            {
                base._inverse = new Mercator(base._Parameters, !base._isInverse);
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
            double naN = double.NaN;
            double rad = double.NaN;
            double num3 = p[0] - this._falseEasting;
            double num4 = p[1] - this._falseNorthing;
            double d = Math.Exp(-num4 / (base._semiMajor * this.k0));
            double num6 = 1.5707963267948966 - (2 * Math.Atan(d));
            double num7 = Math.Pow(this.e, 4);
            double num8 = Math.Pow(this.e, 6);
            double num9 = Math.Pow(this.e, 8);
            rad = (((num6 + (((((this.e2 * 0.5) + ((5 * num7) / 24)) + (num8 / 12)) + ((13 * num9) / 360)) * Math.Sin(2 * num6))) + (((((7 * num7) / 48) + ((29 * num8) / 240)) + ((811 * num9) / 11520)) * Math.Sin(4 * num6))) + ((((7 * num8) / 120) + ((81 * num9) / 1120)) * Math.Sin(6 * num6))) + (((4279 * num9) / 161280) * Math.Sin(8 * num6));
            naN = (num3 / (base._semiMajor * this.k0)) + this.lon_center;
            return new double[] { MathTransform.Radians2Degrees(naN), MathTransform.Radians2Degrees(rad) };
        }
    }
}

