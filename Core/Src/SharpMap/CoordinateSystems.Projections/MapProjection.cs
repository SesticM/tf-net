namespace Topology.CoordinateSystems.Projections
{
    using Topology.CoordinateSystems;
    using Topology.CoordinateSystems.Transformations;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// Projections inherit from this abstract class to get access to useful mathematical functions.
    /// </summary>
    internal abstract class MapProjection : MathTransform, IProjection, IInfo
    {
        private string _Abbreviation;
        private string _Alias;
        private string _Authority;
        private long _Code;
        protected double _e;
        protected double _es;
        protected MathTransform _inverse;
        protected bool _isInverse;
        protected bool _isSpherical;
        private string _Name;
        protected List<ProjectionParameter> _Parameters;
        private string _Remarks;
        protected double _semiMajor;
        protected double _semiMinor;
        /// <summary>
        /// DBLLONG
        /// </summary>
        protected const double DBLLONG = 4.61168601E+18;
        /// <summary>
        /// EPSLN
        /// </summary>
        protected const double EPSLN = 1E-10;
        /// <summary>
        /// Half of PI
        /// </summary>
        protected const double HALF_PI = 1.5707963267948966;
        /// <summary>
        /// MAX_VAL
        /// </summary>
        protected const double MAX_VAL = 4;
        /// <summary>
        /// PI
        /// </summary>
        protected const double PI = 3.1415926535897931;
        /// <summary>
        /// prjMAXLONG
        /// </summary>
        protected const double prjMAXLONG = 2147483647;
        /// <summary>
        /// S2R
        /// </summary>
        protected const double S2R = 4.848136811095359E-06;
        /// <summary>
        /// PI * 2
        /// </summary>
        protected const double TWO_PI = 6.2831853071795862;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        protected MapProjection(List<ProjectionParameter> parameters)
        {
            this._Parameters = parameters;
            ProjectionParameter parameter = this.GetParameter("semi_major");
            ProjectionParameter parameter2 = this.GetParameter("semi_minor");
            if (parameter == null)
            {
                throw new ArgumentException("Missing projection parameter 'semi_major'");
            }
            if (parameter2 == null)
            {
                throw new ArgumentException("Missing projection parameter 'semi_minor'");
            }
            this._semiMajor = parameter.Value;
            this._semiMinor = parameter2.Value;
            this._isSpherical = this._semiMajor == this._semiMinor;
            this._es = 1 - ((this._semiMinor * this._semiMinor) / (this._semiMajor * this._semiMajor));
            this._e = Math.Sqrt(this._es);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="isInverse"></param>
        protected MapProjection(List<ProjectionParameter> parameters, bool isInverse) : this(parameters)
        {
            this._isInverse = isInverse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        protected static double adjust_lon(double x)
        {
            long num = 0L;
            do
            {
                if (Math.Abs(x) <= 3.1415926535897931)
                {
                    return x;
                }
                if (((long) Math.Abs((double) (x / 3.1415926535897931))) < 2L)
                {
                    x -= sign(x) * 6.2831853071795862;
                }
                else if (((long) Math.Abs((double) (x / 6.2831853071795862))) < 2147483647)
                {
                    x -= ((long) (x / 6.2831853071795862)) * 6.2831853071795862;
                }
                else if (((long) Math.Abs((double) (x / 13493037698.238833))) < 2147483647)
                {
                    x -= ((long) (x / 13493037698.238833)) * 13493037698.238833;
                }
                else if (((long) Math.Abs((double) (x / 2.8976077779357651E+19))) < 2147483647)
                {
                    x -= ((long) (x / 2.8976077779357651E+19)) * 2.8976077779357651E+19;
                }
                else
                {
                    x -= sign(x) * 6.2831853071795862;
                }
                num += 1L;
            }
            while (num <= 4);
            return x;
        }

        /// <summary>
        /// Function to eliminate roundoff errors in asin
        /// </summary>
        protected static double asinz(double con)
        {
            if (Math.Abs(con) > 1)
            {
                if (con > 1)
                {
                    con = 1;
                }
                else
                {
                    con = -1;
                }
            }
            return Math.Asin(con);
        }

        /// <summary>
        /// Function to calculate UTM zone number--NOTE Longitude entered in DEGREES!!!
        /// </summary>
        protected static long calc_utm_zone(double lon)
        {
            return (long) (((lon + 180) / 6) + 1);
        }

        /// <summary>
        /// Returns the cube of a number.
        /// </summary>
        /// <param name="x"> </param>
        protected static double CUBE(double x)
        {
            return Math.Pow(x, 3);
        }

        public abstract double[] DegreesToMeters(double[] lonlat);
        /// <summary>
        /// Functions to compute the constants e0, e1, e2, and e3 which are used
        /// in a series for calculating the distance along a meridian.  The
        /// input x represents the eccentricity squared.
        /// </summary>
        protected static double e0fn(double x)
        {
            return (1 - ((0.25 * x) * (1 + ((x / 16) * (3 + (1.25 * x))))));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        protected static double e1fn(double x)
        {
            return ((0.375 * x) * (1 + ((0.25 * x) * (1 + (0.46875 * x)))));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        protected static double e2fn(double x)
        {
            return (((0.05859375 * x) * x) * (1 + (0.75 * x)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        protected static double e3fn(double x)
        {
            return (((x * x) * x) * 0.011393229166666666);
        }

        /// <summary>
        /// Function to compute the constant e4 from the input of the eccentricity
        /// of the spheroid, x.  This constant is used in the Polar Stereographic
        /// projection.
        /// </summary>
        protected static double e4fn(double x)
        {
            double num = 1 + x;
            double num2 = 1 - x;
            return Math.Sqrt(Math.Pow(num, num) * Math.Pow(num2, num2));
        }

        /// <summary>
        /// Checks whether the values of this instance is equal to the values of another instance.
        /// Only parameters used for coordinate system are used for comparison.
        /// Name, abbreviation, authority, alias and remarks are ignored in the comparison.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if equal</returns>
        public bool EqualParams(object obj)
        {
            if (!(obj is MapProjection))
            {
                return false;
            }
            MapProjection proj = obj as MapProjection;
            if (proj.NumParameters != this.NumParameters)
            {
                return false;
            }
            Predicate<ProjectionParameter> match = null;
            for (int i = 0; i < this._Parameters.Count; i++)
            {
                if (match == null)
                {
                    match = delegate (ProjectionParameter par) {
                        return par.Name.Equals(proj.GetParameter(i).Name, StringComparison.OrdinalIgnoreCase);
                    };
                }
                ProjectionParameter parameter = this._Parameters.Find(match);
                if (parameter == null)
                {
                    return false;
                }
                if (parameter.Value != proj.GetParameter(i).Value)
                {
                    return false;
                }
            }
            if (this.IsInverse != proj.IsInverse)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        public ProjectionParameter GetParameter(int Index)
        {
            return this._Parameters[Index];
        }

        /// <summary>
        /// Gets an named parameter of the projection.
        /// </summary>
        /// <remarks>The parameter name is case insensitive</remarks>
        /// <param name="name">Name of parameter</param>
        /// <returns>parameter or null if not found</returns>
        public ProjectionParameter GetParameter(string name)
        {
            return this._Parameters.Find(delegate (ProjectionParameter par) {
                return par.Name.Equals(name, StringComparison.OrdinalIgnoreCase);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        protected static double GMAX(ref double A, ref double B)
        {
            return Math.Max(A, B);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        protected static double GMIN(ref double A, ref double B)
        {
            if (A >= B)
            {
                return B;
            }
            return A;
        }

        /// <summary>
        /// IMOD
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        protected static double IMOD(double A, double B)
        {
            return (A - ((A / B) * B));
        }

        /// <summary>
        /// Reverses the transformation
        /// </summary>
        public override void Invert()
        {
            this._isInverse = !this._isInverse;
        }

        /// <summary>
        /// Converts a latitude value in degrees to radians.
        /// </summary>
        /// <param name="y">The value in degrees to to radians.</param>
        /// <param name="edge">If true, -90 and +90 are valid, otherwise they are considered out of range.</param>
        /// <returns></returns>
        protected static double LatitudeToRadians(double y, bool edge)
        {
            if (!(edge ? ((y >= -90) && (y <= 90)) : ((y > -90) && (y < 90))))
            {
                throw new ArgumentOutOfRangeException("x", y, " not a valid latitude in degrees.");
            }
            return MathTransform.Degrees2Radians(y);
        }

        /// <summary>
        /// Converts a longitude value in degrees to radians.
        /// </summary>
        /// <param name="x">The value in degrees to convert to radians.</param>
        /// <param name="edge">If true, -180 and +180 are valid, otherwise they are considered out of range.</param>
        /// <returns></returns>
        protected static double LongitudeToRadians(double x, bool edge)
        {
            if (!(edge ? ((x >= -180) && (x <= 180)) : ((x > -180) && (x < 180))))
            {
                throw new ArgumentOutOfRangeException("x", x, " not a valid longitude in degrees.");
            }
            return MathTransform.Degrees2Radians(x);
        }

        public abstract double[] MetersToDegrees(double[] p);
        /// <summary>
        /// Function computes the value of M which is the distance along a meridian
        /// from the Equator to latitude phi.
        /// </summary>
        protected static double mlfn(double e0, double e1, double e2, double e3, double phi)
        {
            return ((((e0 * phi) - (e1 * Math.Sin(2 * phi))) + (e2 * Math.Sin(4 * phi))) - (e3 * Math.Sin(6 * phi)));
        }

        /// <summary>
        /// Function to compute the constant small m which is the radius of
        /// a parallel of latitude, phi, divided by the semimajor axis.
        /// </summary>
        protected static double msfnz(double eccent, double sinphi, double cosphi)
        {
            double num = eccent * sinphi;
            return (cosphi / Math.Sqrt(1 - (num * num)));
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="eccent"></param>
        /// <param name="qs"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        protected static double phi1z(double eccent, double qs, out long flag)
        {
            flag = 0L;
            double val = asinz(0.5 * qs);
            if (eccent < 1E-10)
            {
                return val;
            }
            double num = eccent * eccent;
            for (long i = 1L; i <= 0x19L; i += 1L)
            {
                double num5;
                double num6;
                sincos(val, out num5, out num6);
                double num3 = eccent * num5;
                double num4 = 1 - (num3 * num3);
                double num2 = (((0.5 * num4) * num4) / num6) * (((qs / (1 - num)) - (num5 / num4)) + ((0.5 / eccent) * Math.Log((1 - num3) / (1 + num3))));
                val += num2;
                if (Math.Abs(num2) <= 1E-07)
                {
                    return val;
                }
            }
            throw new ApplicationException("Convergence error.");
        }

        /// <summary>
        /// Function to compute the latitude angle, phi2, for the inverse of the
        /// Lambert Conformal Conic and Polar Stereographic projections.
        /// </summary>
        /// <param name="eccent">Spheroid eccentricity</param>
        /// <param name="ts">Constant value t</param>
        /// <param name="flag">Error flag number</param>
        protected static double phi2z(double eccent, double ts, out long flag)
        {
            flag = 0L;
            double y = 0.5 * eccent;
            double a = 1.5707963267948966 - (2 * Math.Atan(ts));
            for (long i = 0L; i <= 15L; i += 1L)
            {
                double num3 = Math.Sin(a);
                double num = eccent * num3;
                double num2 = (1.5707963267948966 - (2 * Math.Atan(ts * Math.Pow((1 - num) / (1 + num), y)))) - a;
                a += num2;
                if (Math.Abs(num2) <= 1E-10)
                {
                    return a;
                }
            }
            throw new ApplicationException("Convergence error - phi2z-conv");
        }

        /// <summary>
        /// Function to compute constant small q which is the radius of a 
        /// parallel of latitude, phi, divided by the semimajor axis. 
        /// </summary>
        protected static double qsfnz(double eccent, double sinphi, double cosphi)
        {
            if (eccent > 1E-07)
            {
                double num = eccent * sinphi;
                return ((1 - (eccent * eccent)) * ((sinphi / (1 - (num * num))) - ((0.5 / eccent) * Math.Log((1 - num) / (1 + num)))));
            }
            return (2 * sinphi);
        }

        /// <summary>
        /// Returns the quad of a number.
        /// </summary>
        /// <param name="x"> </param>
        protected static double QUAD(double x)
        {
            return Math.Pow(x, 4);
        }

        /// <summary>
        /// Function to return the sign of an argument
        /// </summary>
        protected static double sign(double x)
        {
            if (x < 0)
            {
                return -1;
            }
            return 1;
        }

        /// <summary>
        /// Function to calculate the sine and cosine in one call.  Some computer
        /// systems have implemented this function, resulting in a faster implementation
        /// than calling each function separately.  It is provided here for those
        /// computer systems which don`t implement this function
        /// </summary>
        protected static void sincos(double val, out double sin_val, out double cos_val)
        {
            sin_val = Math.Sin(val);
            cos_val = Math.Cos(val);
        }

        /// <summary>
        /// Transforms the specified cp.
        /// </summary>
        /// <param name="cp">The cp.</param>
        /// <returns></returns>
        public override double[] Transform(double[] cp)
        {
            if (!this._isInverse)
            {
                return this.DegreesToMeters(cp);
            }
            return this.MetersToDegrees(cp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ord"></param>
        /// <returns></returns>
        public override List<double[]> TransformList(List<double[]> ord)
        {
            List<double[]> list = new List<double[]>(ord.Count);
            for (int i = 0; i < ord.Count; i++)
            {
                double[] point = ord[i];
                list.Add(this.Transform(point));
            }
            return list;
        }

        /// <summary>
        /// Function to compute the constant small t for use in the forward
        /// computations in the Lambert Conformal Conic and the Polar
        /// Stereographic projections.
        /// </summary>
        protected static double tsfnz(double eccent, double phi, double sinphi)
        {
            double num = eccent * sinphi;
            double y = 0.5 * eccent;
            num = Math.Pow((1 - num) / (1 + num), y);
            return (Math.Tan(0.5 * (1.5707963267948966 - phi)) / num);
        }

        /// <summary>
        /// Gets or sets the abbreviation of the object.
        /// </summary>
        public string Abbreviation
        {
            get
            {
                return this._Abbreviation;
            }
            set
            {
                this._Abbreviation = value;
            }
        }

        /// <summary>
        /// Gets or sets the alias of the object.
        /// </summary>
        public string Alias
        {
            get
            {
                return this._Alias;
            }
            set
            {
                this._Alias = value;
            }
        }

        /// <summary>
        /// Gets or sets the authority name for this object, e.g., "EPSG",
        /// is this is a standard object with an authority specific
        /// identity code. Returns "CUSTOM" if this is a custom object.
        /// </summary>
        public string Authority
        {
            get
            {
                return this._Authority;
            }
            set
            {
                this._Authority = value;
            }
        }

        /// <summary>
        /// Gets or sets the authority specific identification code of the object
        /// </summary>
        public long AuthorityCode
        {
            get
            {
                return this._Code;
            }
            set
            {
                this._Code = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ClassName
        {
            get
            {
                return this.ClassName;
            }
        }

        /// <summary>
        /// Returns true if this projection is inverted.
        /// Most map projections define forward projection as "from geographic to projection", and backwards
        /// as "from projection to geographic". If this projection is inverted, this will be the other way around.
        /// </summary>
        internal bool IsInverse
        {
            get
            {
                return this._isInverse;
            }
        }

        /// <summary>
        /// Gets or sets the name of the object.
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
        /// 
        /// </summary>
        public int NumParameters
        {
            get
            {
                return this._Parameters.Count;
            }
        }

        /// <summary>
        /// Gets or sets the provider-supplied remarks for the object.
        /// </summary>
        public string Remarks
        {
            get
            {
                return this._Remarks;
            }
            set
            {
                this._Remarks = value;
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
                StringBuilder builder = new StringBuilder();
                if (this._isInverse)
                {
                    builder.Append("INVERSE_MT[");
                }
                builder.AppendFormat("PARAM_MT[\"{0}\"", this.Name);
                for (int i = 0; i < this.NumParameters; i++)
                {
                    builder.AppendFormat(", {0}", this.GetParameter(i).WKT);
                }
                builder.Append("]");
                if (this._isInverse)
                {
                    builder.Append("]");
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Gets an XML representation of this object
        /// </summary>
        public override string XML
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("<CT_MathTransform>");
                if (this._isInverse)
                {
                    builder.AppendFormat("<CT_InverseTransform Name=\"{0}\">", this.ClassName);
                }
                else
                {
                    builder.AppendFormat("<CT_ParameterizedMathTransform Name=\"{0}\">", this.ClassName);
                }
                for (int i = 0; i < this.NumParameters; i++)
                {
                    builder.AppendFormat(this.GetParameter(i).XML, new object[0]);
                }
                if (this._isInverse)
                {
                    builder.Append("</CT_InverseTransform>");
                }
                else
                {
                    builder.Append("</CT_ParameterizedMathTransform>");
                }
                builder.Append("</CT_MathTransform>");
                return builder.ToString();
            }
        }
    }
}

