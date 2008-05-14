namespace Topology.CoordinateSystems.Projections
{
    using Topology.CoordinateSystems;
    using Topology.CoordinateSystems.Transformations;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implements the Albers projection.
    /// </summary>
    /// <remarks>
    /// <para>Implements the Albers projection. The Albers projection is most commonly
    /// used to project the United States of America. It gives the northern
    /// border with Canada a curved appearance.</para>
    /// 
    /// <para>The <a href="http://www.geog.mcgill.ca/courses/geo201/mapproj/naaeana.gif">Albers Equal Area</a>
    /// projection has the property that the area bounded
    /// by any pair of parallels and meridians is exactly reproduced between the 
    /// image of those parallels and meridians in the projected domain, that is,
    /// the projection preserves the correct area of the earth though distorts
    /// direction, distance and shape somewhat.</para>
    /// </remarks>
    internal class AlbersProjection : MapProjection
    {
        private double _falseEasting;
        private double _falseNorthing;
        private double C;
        private double e;
        private double e_sq;
        private double lon_center;
        private double n;
        private double ro0;

        /// <summary>
        /// Creates an instance of an Albers projection object.
        /// </summary>
        /// <param name="parameters">List of parameters to initialize the projection.</param>
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
        public AlbersProjection(List<ProjectionParameter> parameters) : this(parameters, false)
        {
        }

        /// <summary>
        /// Creates an instance of an Albers projection object.
        /// </summary>
        /// <remarks>
        /// <para>The parameters this projection expects are listed below.</para>
        /// <list type="table">
        /// <listheader><term>Items</term><description>Descriptions</description></listheader>
        /// <item><term>latitude_of_center</term><description>The latitude of the point which is not the natural origin and at which grid coordinate values false easting and false northing are defined.</description></item>
        /// <item><term>longitude_of_center</term><description>The longitude of the point which is not the natural origin and at which grid coordinate values false easting and false northing are defined.</description></item>
        /// <item><term>standard_parallel_1</term><description>For a conic projection with two standard parallels, this is the latitude of intersection of the cone with the ellipsoid that is nearest the pole.  Scale is true along this parallel.</description></item>
        /// <item><term>standard_parallel_2</term><description>For a conic projection with two standard parallels, this is the latitude of intersection of the cone with the ellipsoid that is furthest from the pole.  Scale is true along this parallel.</description></item>
        /// <item><term>false_easting</term><description>The easting value assigned to the false origin.</description></item>
        /// <item><term>false_northing</term><description>The northing value assigned to the false origin.</description></item>
        /// </list>
        /// </remarks>
        /// <param name="parameters">List of parameters to initialize the projection.</param>
        /// <param name="isInverse">Indicates whether the projection forward (meters to degrees or degrees to meters).</param>
        public AlbersProjection(List<ProjectionParameter> parameters, bool isInverse) : base(parameters, isInverse)
        {
            base.Name = "Albers_Conic_Equal_Area";
            ProjectionParameter parameter = base.GetParameter("longitude_of_center");
            ProjectionParameter parameter2 = base.GetParameter("latitude_of_center");
            ProjectionParameter parameter3 = base.GetParameter("standard_parallel_1");
            ProjectionParameter parameter4 = base.GetParameter("standard_parallel_2");
            ProjectionParameter parameter5 = base.GetParameter("false_easting");
            ProjectionParameter parameter6 = base.GetParameter("false_northing");
            if (parameter == null)
            {
                throw new ArgumentException("Missing projection parameter 'longitude_of_center'");
            }
            if (parameter2 == null)
            {
                throw new ArgumentException("Missing projection parameter 'latitude_of_center'");
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
            this.lon_center = MathTransform.Degrees2Radians(parameter.Value);
            double lat = MathTransform.Degrees2Radians(parameter2.Value);
            double num2 = MathTransform.Degrees2Radians(parameter3.Value);
            double num3 = MathTransform.Degrees2Radians(parameter4.Value);
            this._falseEasting = MathTransform.Degrees2Radians(parameter5.Value);
            this._falseNorthing = MathTransform.Degrees2Radians(parameter6.Value);
            if (Math.Abs((double) (num2 + num3)) < double.Epsilon)
            {
                throw new ApplicationException("Equal latitudes for standard parallels on opposite sides of Equator.");
            }
            this.e_sq = 1 - Math.Pow(base._semiMinor / base._semiMajor, 2);
            this.e = Math.Sqrt(this.e_sq);
            double num4 = this.alpha(num2);
            double num5 = this.alpha(num3);
            double x = Math.Cos(num2) / Math.Sqrt(1 - (this.e_sq * Math.Pow(Math.Sin(num2), 2)));
            double num7 = Math.Cos(num3) / Math.Sqrt(1 - (this.e_sq * Math.Pow(Math.Sin(num3), 2)));
            this.n = (Math.Pow(x, 2) - Math.Pow(num7, 2)) / (num5 - num4);
            this.C = Math.Pow(x, 2) + (this.n * num4);
            this.ro0 = this.Ro(this.alpha(lat));
        }

        private double alpha(double lat)
        {
            double x = Math.Sin(lat);
            double num2 = Math.Pow(x, 2);
            return ((1 - this.e_sq) * ((x / (1 - (this.e_sq * num2))) - ((1 / (2 * this.e)) * Math.Log((1 - (this.e * x)) / (1 + (this.e * x))))));
        }

        /// <summary>
        /// Converts coordinates in decimal degrees to projected meters.
        /// </summary>
        /// <param name="lonlat">The point in decimal degrees.</param>
        /// <returns>Point in projected meters</returns>
        public override double[] DegreesToMeters(double[] lonlat)
        {
            double num = MathTransform.Degrees2Radians(lonlat[0]);
            double lat = MathTransform.Degrees2Radians(lonlat[1]);
            double a = this.alpha(lat);
            double num4 = this.Ro(a);
            double num5 = this.n * (num - this.lon_center);
            return new double[] { (this._falseEasting + (num4 * Math.Sin(num5))), ((this._falseNorthing + this.ro0) - (num4 * Math.Cos(num5))) };
        }

        /// <summary>
        /// Returns the inverse of this projection.
        /// </summary>
        /// <returns>IMathTransform that is the reverse of the current projection.</returns>
        public override IMathTransform Inverse()
        {
            if (base._inverse == null)
            {
                base._inverse = new AlbersProjection(base._Parameters, !base._isInverse);
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
            double num = Math.Atan((p[0] - this._falseEasting) / (this.ro0 - (p[1] - this._falseNorthing)));
            double x = Math.Sqrt(Math.Pow(p[0] - this._falseEasting, 2) + Math.Pow(this.ro0 - (p[1] - this._falseNorthing), 2));
            double num3 = (this.C - ((Math.Pow(x, 2) * Math.Pow(this.n, 2)) / Math.Pow(base._semiMajor, 2))) / this.n;
            Math.Sin(num3 / (1 - (((1 - this.e_sq) / (2 * this.e)) * Math.Log((1 - this.e) / (1 + this.e)))));
            double a = Math.Asin(num3 * 0.5);
            double maxValue = double.MaxValue;
            int num6 = 0;
            while (Math.Abs((double) (a - maxValue)) > 1E-06)
            {
                maxValue = a;
                double num7 = Math.Sin(a);
                double num8 = this.e_sq * Math.Pow(num7, 2);
                a += (Math.Pow(1 - num8, 2) / (2 * Math.Cos(a))) * (((num3 / (1 - this.e_sq)) - (num7 / (1 - num8))) + ((1 / (2 * this.e)) * Math.Log((1 - (this.e * num7)) / (1 + (this.e * num7)))));
                num6++;
                if (num6 > 0x19)
                {
                    throw new ApplicationException("Transformation failed to converge in Albers backwards transformation");
                }
            }
            double rad = this.lon_center + (num / this.n);
            return new double[] { MathTransform.Radians2Degrees(rad), MathTransform.Radians2Degrees(a) };
        }

        private double Ro(double a)
        {
            return ((base._semiMajor * Math.Sqrt(this.C - (this.n * a))) / this.n);
        }
    }
}

