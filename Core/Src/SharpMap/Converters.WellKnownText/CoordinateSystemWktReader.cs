namespace Topology.Converters.WellKnownText
{
    using Topology.CoordinateSystems;
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Creates an object based on the supplied Well Known Text (WKT).
    /// </summary>
    public class CoordinateSystemWktReader
    {
        /// <summary>
        /// Reads and parses a WKT-formatted projection string.
        /// </summary>
        /// <param name="wkt">String containing WKT.</param>
        /// <returns>Object representation of the WKT.</returns>
        /// <exception cref="T:System.ArgumentException">If a token is not recognised.</exception>
        public static IInfo Parse(string wkt)
        {
            IInfo info = null;
            StringReader reader = new StringReader(wkt);
            WktStreamTokenizer tokenizer = new WktStreamTokenizer(reader);
            tokenizer.NextToken();
            string stringValue = tokenizer.GetStringValue();
            switch (stringValue)
            {
                case "UNIT":
                    info = ReadUnit(tokenizer);
                    break;

                case "SPHEROID":
                    info = ReadEllipsoid(tokenizer);
                    break;

                case "DATUM":
                    info = ReadHorizontalDatum(tokenizer);
                    break;

                case "PRIMEM":
                    info = ReadPrimeMeridian(tokenizer);
                    break;

                case "VERT_CS":
                case "GEOGCS":
                case "PROJCS":
                case "COMPD_CS":
                case "GEOCCS":
                case "FITTED_CS":
                case "LOCAL_CS":
                    info = ReadCoordinateSystem(wkt, tokenizer);
                    break;

                default:
                    throw new ArgumentException(string.Format("'{0'} is not recongnized.", stringValue));
            }
            reader.Close();
            return info;
        }

        /// <summary>
        /// Returns a <see cref="T:Topology.CoordinateSystems.AngularUnit" /> given a piece of WKT.
        /// </summary>
        /// <param name="tokenizer">WktStreamTokenizer that has the WKT.</param>
        /// <returns>An object that implements the IUnit interface.</returns>
        private static IAngularUnit ReadAngularUnit(WktStreamTokenizer tokenizer)
        {
            tokenizer.ReadToken("[");
            string name = tokenizer.ReadDoubleQuotedWord();
            tokenizer.ReadToken(",");
            tokenizer.NextToken();
            double numericValue = tokenizer.GetNumericValue();
            string authority = string.Empty;
            long authorityCode = -1L;
            tokenizer.NextToken();
            if (tokenizer.GetStringValue() == ",")
            {
                tokenizer.ReadAuthority(ref authority, ref authorityCode);
                tokenizer.ReadToken("]");
            }
            return new AngularUnit(numericValue, name, authority, authorityCode, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// Returns a <see cref="T:Topology.CoordinateSystems.AxisInfo" /> given a piece of WKT.
        /// </summary>
        /// <param name="tokenizer">WktStreamTokenizer that has the WKT.</param>
        /// <returns>An AxisInfo object.</returns>
        private static AxisInfo ReadAxis(WktStreamTokenizer tokenizer)
        {
            if (tokenizer.GetStringValue() != "AXIS")
            {
                tokenizer.ReadToken("AXIS");
            }
            tokenizer.ReadToken("[");
            string name = tokenizer.ReadDoubleQuotedWord();
            tokenizer.ReadToken(",");
            tokenizer.NextToken();
            string stringValue = tokenizer.GetStringValue();
            tokenizer.ReadToken("]");
            switch (stringValue.ToUpper())
            {
                case "DOWN":
                    return new AxisInfo(name, AxisOrientationEnum.Down);

                case "EAST":
                    return new AxisInfo(name, AxisOrientationEnum.East);

                case "NORTH":
                    return new AxisInfo(name, AxisOrientationEnum.North);

                case "OTHER":
                    return new AxisInfo(name, AxisOrientationEnum.Other);

                case "SOUTH":
                    return new AxisInfo(name, AxisOrientationEnum.South);

                case "UP":
                    return new AxisInfo(name, AxisOrientationEnum.Up);

                case "WEST":
                    return new AxisInfo(name, AxisOrientationEnum.West);
            }
            throw new ArgumentException("Invalid axis name '" + stringValue + "' in WKT");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coordinateSystem"></param>
        /// <param name="tokenizer"></param>
        /// <returns></returns>
        private static ICoordinateSystem ReadCoordinateSystem(string coordinateSystem, WktStreamTokenizer tokenizer)
        {
            switch (tokenizer.GetStringValue())
            {
                case "GEOGCS":
                    return ReadGeographicCoordinateSystem(tokenizer);

                case "PROJCS":
                    return ReadProjectedCoordinateSystem(tokenizer);

                case "COMPD_CS":
                case "VERT_CS":
                case "GEOCCS":
                case "FITTED_CS":
                case "LOCAL_CS":
                    throw new NotSupportedException(string.Format("{0} coordinate system is not supported.", coordinateSystem));
            }
            throw new InvalidOperationException(string.Format("{0} coordinate system is not recognized.", coordinateSystem));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenizer"></param>
        /// <returns></returns>
        private static IEllipsoid ReadEllipsoid(WktStreamTokenizer tokenizer)
        {
            tokenizer.ReadToken("[");
            string name = tokenizer.ReadDoubleQuotedWord();
            tokenizer.ReadToken(",");
            tokenizer.NextToken();
            double numericValue = tokenizer.GetNumericValue();
            tokenizer.ReadToken(",");
            tokenizer.NextToken();
            double inverseFlattening = tokenizer.GetNumericValue();
            tokenizer.NextToken();
            string authority = string.Empty;
            long authorityCode = -1L;
            if (tokenizer.GetStringValue() == ",")
            {
                tokenizer.ReadAuthority(ref authority, ref authorityCode);
                tokenizer.ReadToken("]");
            }
            return new Ellipsoid(numericValue, 0, inverseFlattening, true, LinearUnit.Metre, name, authority, authorityCode, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenizer"></param>
        /// <returns></returns>
        private static IGeographicCoordinateSystem ReadGeographicCoordinateSystem(WktStreamTokenizer tokenizer)
        {
            tokenizer.ReadToken("[");
            string name = tokenizer.ReadDoubleQuotedWord();
            tokenizer.ReadToken(",");
            tokenizer.ReadToken("DATUM");
            IHorizontalDatum horizontalDatum = ReadHorizontalDatum(tokenizer);
            tokenizer.ReadToken(",");
            tokenizer.ReadToken("PRIMEM");
            IPrimeMeridian primeMeridian = ReadPrimeMeridian(tokenizer);
            tokenizer.ReadToken(",");
            tokenizer.ReadToken("UNIT");
            IAngularUnit angularUnit = ReadAngularUnit(tokenizer);
            string authority = string.Empty;
            long authorityCode = -1L;
            tokenizer.NextToken();
            List<AxisInfo> axisInfo = new List<AxisInfo>(2);
            if (tokenizer.GetStringValue() == ",")
            {
                tokenizer.NextToken();
                while (tokenizer.GetStringValue() == "AXIS")
                {
                    axisInfo.Add(ReadAxis(tokenizer));
                    tokenizer.NextToken();
                }
                if (tokenizer.GetStringValue() == "AUTHORITY")
                {
                    tokenizer.ReadAuthority(ref authority, ref authorityCode);
                    tokenizer.ReadToken("]");
                }
            }
            if (axisInfo.Count == 0)
            {
                axisInfo.Add(new AxisInfo("Lon", AxisOrientationEnum.East));
                axisInfo.Add(new AxisInfo("Lat", AxisOrientationEnum.North));
            }
            return new GeographicCoordinateSystem(angularUnit, horizontalDatum, primeMeridian, axisInfo, name, authority, authorityCode, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenizer"></param>
        /// <returns></returns>
        private static IHorizontalDatum ReadHorizontalDatum(WktStreamTokenizer tokenizer)
        {
            Wgs84ConversionInfo info = null;
            string authority = string.Empty;
            long authorityCode = -1L;
            tokenizer.ReadToken("[");
            string name = tokenizer.ReadDoubleQuotedWord();
            tokenizer.ReadToken(",");
            tokenizer.ReadToken("SPHEROID");
            IEllipsoid ellipsoid = ReadEllipsoid(tokenizer);
            tokenizer.NextToken();
            while (tokenizer.GetStringValue() == ",")
            {
                tokenizer.NextToken();
                if (tokenizer.GetStringValue() == "TOWGS84")
                {
                    info = ReadWGS84ConversionInfo(tokenizer);
                    tokenizer.NextToken();
                }
                else if (tokenizer.GetStringValue() == "AUTHORITY")
                {
                    tokenizer.ReadAuthority(ref authority, ref authorityCode);
                    tokenizer.ReadToken("]");
                }
            }
            return new HorizontalDatum(ellipsoid, info, DatumType.HD_Geocentric, name, authority, authorityCode, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// Returns a <see cref="T:Topology.CoordinateSystems.LinearUnit" /> given a piece of WKT.
        /// </summary>
        /// <param name="tokenizer">WktStreamTokenizer that has the WKT.</param>
        /// <returns>An object that implements the IUnit interface.</returns>
        private static ILinearUnit ReadLinearUnit(WktStreamTokenizer tokenizer)
        {
            tokenizer.ReadToken("[");
            string name = tokenizer.ReadDoubleQuotedWord();
            tokenizer.ReadToken(",");
            tokenizer.NextToken();
            double numericValue = tokenizer.GetNumericValue();
            string authority = string.Empty;
            long authorityCode = -1L;
            tokenizer.NextToken();
            if (tokenizer.GetStringValue() == ",")
            {
                tokenizer.ReadAuthority(ref authority, ref authorityCode);
                tokenizer.ReadToken("]");
            }
            return new LinearUnit(numericValue, name, authority, authorityCode, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenizer"></param>
        /// <returns></returns>
        private static IPrimeMeridian ReadPrimeMeridian(WktStreamTokenizer tokenizer)
        {
            tokenizer.ReadToken("[");
            string name = tokenizer.ReadDoubleQuotedWord();
            tokenizer.ReadToken(",");
            tokenizer.NextToken();
            double numericValue = tokenizer.GetNumericValue();
            tokenizer.NextToken();
            string authority = string.Empty;
            long authorityCode = -1L;
            if (tokenizer.GetStringValue() == ",")
            {
                tokenizer.ReadAuthority(ref authority, ref authorityCode);
                tokenizer.ReadToken("]");
            }
            return new PrimeMeridian(numericValue, AngularUnit.Degrees, name, authority, authorityCode, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenizer"></param>
        /// <returns></returns>
        private static IProjectedCoordinateSystem ReadProjectedCoordinateSystem(WktStreamTokenizer tokenizer)
        {
            tokenizer.ReadToken("[");
            string name = tokenizer.ReadDoubleQuotedWord();
            tokenizer.ReadToken(",");
            tokenizer.ReadToken("GEOGCS");
            IGeographicCoordinateSystem geographicCoordinateSystem = ReadGeographicCoordinateSystem(tokenizer);
            tokenizer.ReadToken(",");
            IProjection projection = ReadProjection(tokenizer);
            IUnit unit = ReadLinearUnit(tokenizer);
            string authority = string.Empty;
            long authorityCode = -1L;
            tokenizer.NextToken();
            List<AxisInfo> axisInfo = new List<AxisInfo>(2);
            if (tokenizer.GetStringValue() == ",")
            {
                tokenizer.NextToken();
                while (tokenizer.GetStringValue() == "AXIS")
                {
                    axisInfo.Add(ReadAxis(tokenizer));
                    tokenizer.NextToken();
                }
                if (tokenizer.GetStringValue() == "AUTHORITY")
                {
                    tokenizer.ReadAuthority(ref authority, ref authorityCode);
                    tokenizer.ReadToken("]");
                }
            }
            if (axisInfo.Count == 0)
            {
                axisInfo.Add(new AxisInfo("X", AxisOrientationEnum.East));
                axisInfo.Add(new AxisInfo("Y", AxisOrientationEnum.North));
            }
            return new ProjectedCoordinateSystem(geographicCoordinateSystem.HorizontalDatum, geographicCoordinateSystem, unit as LinearUnit, projection, axisInfo, name, authority, authorityCode, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenizer"></param>
        /// <returns></returns>
        private static IProjection ReadProjection(WktStreamTokenizer tokenizer)
        {
            tokenizer.ReadToken("PROJECTION");
            tokenizer.ReadToken("[");
            string className = tokenizer.ReadDoubleQuotedWord();
            tokenizer.ReadToken("]");
            tokenizer.ReadToken(",");
            tokenizer.ReadToken("PARAMETER");
            List<ProjectionParameter> parameters = new List<ProjectionParameter>();
            while (tokenizer.GetStringValue() == "PARAMETER")
            {
                tokenizer.ReadToken("[");
                string name = tokenizer.ReadDoubleQuotedWord();
                tokenizer.ReadToken(",");
                tokenizer.NextToken();
                double numericValue = tokenizer.GetNumericValue();
                tokenizer.ReadToken("]");
                tokenizer.ReadToken(",");
                parameters.Add(new ProjectionParameter(name, numericValue));
                tokenizer.NextToken();
            }
            string authority = string.Empty;
            return new Projection(className, parameters, className, authority, -1L, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// Returns a IUnit given a piece of WKT.
        /// </summary>
        /// <param name="tokenizer">WktStreamTokenizer that has the WKT.</param>
        /// <returns>An object that implements the IUnit interface.</returns>
        private static IUnit ReadUnit(WktStreamTokenizer tokenizer)
        {
            tokenizer.ReadToken("[");
            string name = tokenizer.ReadDoubleQuotedWord();
            tokenizer.ReadToken(",");
            tokenizer.NextToken();
            double numericValue = tokenizer.GetNumericValue();
            string authority = string.Empty;
            long authorityCode = -1L;
            tokenizer.NextToken();
            if (tokenizer.GetStringValue() == ",")
            {
                tokenizer.ReadAuthority(ref authority, ref authorityCode);
                tokenizer.ReadToken("]");
            }
            return new Unit(numericValue, name, authority, authorityCode, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// Reads either 3, 6 or 7 parameter Bursa-Wolf values from TOWGS84 token
        /// </summary>
        /// <param name="tokenizer"></param>
        /// <returns></returns>
        private static Wgs84ConversionInfo ReadWGS84ConversionInfo(WktStreamTokenizer tokenizer)
        {
            tokenizer.ReadToken("[");
            Wgs84ConversionInfo info = new Wgs84ConversionInfo();
            tokenizer.NextToken();
            info.Dx = tokenizer.GetNumericValue();
            tokenizer.ReadToken(",");
            tokenizer.NextToken();
            info.Dy = tokenizer.GetNumericValue();
            tokenizer.ReadToken(",");
            tokenizer.NextToken();
            info.Dz = tokenizer.GetNumericValue();
            tokenizer.NextToken();
            if (tokenizer.GetStringValue() == ",")
            {
                tokenizer.NextToken();
                info.Ex = tokenizer.GetNumericValue();
                tokenizer.ReadToken(",");
                tokenizer.NextToken();
                info.Ey = tokenizer.GetNumericValue();
                tokenizer.ReadToken(",");
                tokenizer.NextToken();
                info.Ez = tokenizer.GetNumericValue();
                tokenizer.NextToken();
                if (tokenizer.GetStringValue() == ",")
                {
                    tokenizer.NextToken();
                    info.Ppm = tokenizer.GetNumericValue();
                }
            }
            if (tokenizer.GetStringValue() != "]")
            {
                tokenizer.ReadToken("]");
            }
            return info;
        }
    }
}

