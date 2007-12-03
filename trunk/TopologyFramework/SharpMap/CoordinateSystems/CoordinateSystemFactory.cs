namespace Topology.CoordinateSystems
{
    using Topology.Converters.WellKnownText;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Builds up complex objects from simpler objects or values.
    /// </summary>
    /// <remarks>
    /// <para>ICoordinateSystemFactory allows applications to make coordinate systems that 
    /// cannot be created by a <see cref="T:Topology.CoordinateSystems.ICoordinateSystemAuthorityFactory" />. This factory is very 
    /// flexible, whereas the authority factory is easier to use.</para>
    /// <para>So <see cref="T:Topology.CoordinateSystems.ICoordinateSystemAuthorityFactory" />can be used to make 'standard' coordinate 
    /// systems, and <see cref="T:Topology.CoordinateSystems.CoordinateSystemFactory" /> can be used to make 'special' 
    /// coordinate systems.</para>
    /// <para>For example, the EPSG authority has codes for USA state plane coordinate systems 
    /// using the NAD83 datum, but these coordinate systems always use meters. EPSG does not 
    /// have codes for NAD83 state plane coordinate systems that use feet units. This factory
    /// lets an application create such a hybrid coordinate system.</para>
    /// </remarks>
    public class CoordinateSystemFactory : ICoordinateSystemFactory
    {
        /// <summary>
        /// Creates a <see cref="T:Topology.CoordinateSystems.ICompoundCoordinateSystem" /> [NOT IMPLEMENTED].
        /// </summary>
        /// <param name="name">Name of compound coordinate system.</param>
        /// <param name="head">Head coordinate system</param>
        /// <param name="tail">Tail coordinate system</param>
        /// <returns>Compound coordinate system</returns>
        public ICompoundCoordinateSystem CreateCompoundCoordinateSystem(string name, ICoordinateSystem head, ICoordinateSystem tail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates an <see cref="T:Topology.CoordinateSystems.Ellipsoid" /> from radius values.
        /// </summary>
        /// <seealso cref="M:Topology.CoordinateSystems.CoordinateSystemFactory.CreateFlattenedSphere(System.String,System.Double,System.Double,Topology.CoordinateSystems.ILinearUnit)" />
        /// <param name="name">Name of ellipsoid</param>
        /// <param name="semiMajorAxis"></param>
        /// <param name="semiMinorAxis"></param>
        /// <param name="linearUnit"></param>
        /// <returns>Ellipsoid</returns>
        public IEllipsoid CreateEllipsoid(string name, double semiMajorAxis, double semiMinorAxis, ILinearUnit linearUnit)
        {
            return new Ellipsoid(semiMajorAxis, semiMinorAxis, 1, false, linearUnit, name, string.Empty, -1L, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// Creates a <see cref="T:Topology.CoordinateSystems.IFittedCoordinateSystem" />.
        /// </summary>
        /// <remarks>The units of the axes in the fitted coordinate system will be 
        /// inferred from the units of the base coordinate system. If the affine map
        /// performs a rotation, then any mixed axes must have identical units. For
        /// example, a (lat_deg,lon_deg,height_feet) system can be rotated in the 
        /// (lat,lon) plane, since both affected axes are in degrees. But you 
        /// should not rotate this coordinate system in any other plane.</remarks>
        /// <param name="name">Name of coordinate system</param>
        /// <param name="baseCoordinateSystem">Base coordinate system</param>
        /// <param name="toBaseWkt"></param>
        /// <param name="arAxes"></param>
        /// <returns>Fitted coordinate system</returns>
        public IFittedCoordinateSystem CreateFittedCoordinateSystem(string name, ICoordinateSystem baseCoordinateSystem, string toBaseWkt, List<AxisInfo> arAxes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates an <see cref="T:Topology.CoordinateSystems.Ellipsoid" /> from an major radius, and inverse flattening.
        /// </summary>
        /// <seealso cref="M:Topology.CoordinateSystems.CoordinateSystemFactory.CreateEllipsoid(System.String,System.Double,System.Double,Topology.CoordinateSystems.ILinearUnit)" />
        /// <param name="name">Name of ellipsoid</param>
        /// <param name="semiMajorAxis">Semi major-axis</param>
        /// <param name="inverseFlattening">Inverse flattening</param>
        /// <param name="linearUnit">Linear unit</param>
        /// <returns>Ellipsoid</returns>
        public IEllipsoid CreateFlattenedSphere(string name, double semiMajorAxis, double inverseFlattening, ILinearUnit linearUnit)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Invalid name");
            }
            return new Ellipsoid(semiMajorAxis, -1, inverseFlattening, true, linearUnit, name, string.Empty, -1L, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// Creates a spatial reference object given its Well-known text representation.
        /// The output object may be either a <see cref="T:Topology.CoordinateSystems.IGeographicCoordinateSystem" /> or
        /// a <see cref="T:Topology.CoordinateSystems.IProjectedCoordinateSystem" />.
        /// </summary>
        /// <param name="WKT">The Well-known text representation for the spatial reference</param>
        /// <returns>The resulting spatial reference object</returns>
        public ICoordinateSystem CreateFromWkt(string WKT)
        {
            return (CoordinateSystemWktReader.Parse(WKT) as ICoordinateSystem);
        }

        /// <summary>
        /// Creates a coordinate system object from an XML string.
        /// </summary>
        /// <param name="xml">XML representation for the spatial reference</param>
        /// <returns>The resulting spatial reference object</returns>
        public ICoordinateSystem CreateFromXml(string xml)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Creates a <see cref="M:Topology.CoordinateSystems.CoordinateSystemFactory.CreateGeocentricCoordinateSystem(System.String,Topology.CoordinateSystems.IHorizontalDatum,Topology.CoordinateSystems.ILinearUnit,Topology.CoordinateSystems.IPrimeMeridian)" /> from a <see cref="T:Topology.CoordinateSystems.IHorizontalDatum">datum</see>, 
        /// <see cref="T:Topology.CoordinateSystems.ILinearUnit">linear unit</see> and <see cref="T:Topology.CoordinateSystems.IPrimeMeridian" />.
        /// </summary>
        /// <param name="name">Name of geocentric coordinate system</param>
        /// <param name="datum">Horizontal datum</param>
        /// <param name="linearUnit">Linear unit</param>
        /// <param name="primeMeridian">Prime meridian</param>
        /// <returns>Geocentric Coordinate System</returns>
        public IGeocentricCoordinateSystem CreateGeocentricCoordinateSystem(string name, IHorizontalDatum datum, ILinearUnit linearUnit, IPrimeMeridian primeMeridian)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Invalid name");
            }
            List<AxisInfo> axisinfo = new List<AxisInfo>(3);
            axisinfo.Add(new AxisInfo("X", AxisOrientationEnum.Other));
            axisinfo.Add(new AxisInfo("Y", AxisOrientationEnum.Other));
            axisinfo.Add(new AxisInfo("Z", AxisOrientationEnum.Other));
            return new GeocentricCoordinateSystem(datum, linearUnit, primeMeridian, axisinfo, name, string.Empty, -1L, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// Creates a <see cref="T:Topology.CoordinateSystems.GeographicCoordinateSystem" />, which could be Lat/Lon or Lon/Lat.
        /// </summary>
        /// <param name="name">Name of geographical coordinate system</param>
        /// <param name="angularUnit">Angular units</param>
        /// <param name="datum">Horizontal datum</param>
        /// <param name="primeMeridian">Prime meridian</param>
        /// <param name="axis0">First axis</param>
        /// <param name="axis1">Second axis</param>
        /// <returns>Geographic coordinate system</returns>
        public IGeographicCoordinateSystem CreateGeographicCoordinateSystem(string name, IAngularUnit angularUnit, IHorizontalDatum datum, IPrimeMeridian primeMeridian, AxisInfo axis0, AxisInfo axis1)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Invalid name");
            }
            List<AxisInfo> axisInfo = new List<AxisInfo>(2);
            axisInfo.Add(axis0);
            axisInfo.Add(axis1);
            return new GeographicCoordinateSystem(angularUnit, datum, primeMeridian, axisInfo, name, string.Empty, -1L, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// Creates <see cref="T:Topology.CoordinateSystems.HorizontalDatum" /> from ellipsoid and Bursa-World parameters.
        /// </summary>
        /// <remarks>
        /// Since this method contains a set of Bursa-Wolf parameters, the created 
        /// datum will always have a relationship to WGS84. If you wish to create a
        /// horizontal datum that has no relationship with WGS84, then you can 
        /// either specify a <see cref="T:Topology.CoordinateSystems.DatumType">horizontalDatumType</see> of <see cref="F:Topology.CoordinateSystems.DatumType.HD_Other" />, or create it via WKT.
        /// </remarks>
        /// <param name="name">Name of ellipsoid</param>
        /// <param name="datumType">Type of datum</param>
        /// <param name="ellipsoid">Ellipsoid</param>
        /// <param name="toWgs84">Wgs84 conversion parameters</param>
        /// <returns>Horizontal datum</returns>
        public IHorizontalDatum CreateHorizontalDatum(string name, DatumType datumType, IEllipsoid ellipsoid, Wgs84ConversionInfo toWgs84)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Invalid name");
            }
            if (ellipsoid == null)
            {
                throw new ArgumentException("Ellipsoid was null");
            }
            return new HorizontalDatum(ellipsoid, toWgs84, datumType, name, string.Empty, -1L, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// Creates a <see cref="T:Topology.CoordinateSystems.ILocalCoordinateSystem">local coordinate system</see>.
        /// </summary>
        /// <remarks>
        /// The dimension of the local coordinate system is determined by the size of 
        /// the axis array. All the axes will have the same units. If you want to make 
        /// a coordinate system with mixed units, then you can make a compound 
        /// coordinate system from different local coordinate systems.
        /// </remarks>
        /// <param name="name">Name of local coordinate system</param>
        /// <param name="datum">Local datum</param>
        /// <param name="unit">Units</param>
        /// <param name="axes">Axis info</param>
        /// <returns>Local coordinate system</returns>
        public ILocalCoordinateSystem CreateLocalCoordinateSystem(string name, ILocalDatum datum, IUnit unit, List<AxisInfo> axes)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Creates a <see cref="T:Topology.CoordinateSystems.ILocalDatum" />.
        /// </summary>
        /// <param name="name">Name of datum</param>
        /// <param name="datumType">Datum type</param>
        /// <returns></returns>
        public ILocalDatum CreateLocalDatum(string name, DatumType datumType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a <see cref="T:Topology.CoordinateSystems.PrimeMeridian" />, relative to Greenwich.
        /// </summary>
        /// <param name="name">Name of prime meridian</param>
        /// <param name="angularUnit">Angular unit</param>
        /// <param name="longitude">Longitude</param>
        /// <returns>Prime meridian</returns>
        public IPrimeMeridian CreatePrimeMeridian(string name, IAngularUnit angularUnit, double longitude)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Invalid name");
            }
            return new PrimeMeridian(longitude, angularUnit, name, string.Empty, -1L, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// Creates a <see cref="T:Topology.CoordinateSystems.ProjectedCoordinateSystem" /> using a projection object.
        /// </summary>
        /// <param name="name">Name of projected coordinate system</param>
        /// <param name="gcs">Geographic coordinate system</param>
        /// <param name="projection">Projection</param>
        /// <param name="linearUnit">Linear unit</param>
        /// <param name="axis0">Primary axis</param>
        /// <param name="axis1">Secondary axis</param>
        /// <returns>Projected coordinate system</returns>
        public IProjectedCoordinateSystem CreateProjectedCoordinateSystem(string name, IGeographicCoordinateSystem gcs, IProjection projection, ILinearUnit linearUnit, AxisInfo axis0, AxisInfo axis1)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Invalid name");
            }
            if (gcs == null)
            {
                throw new ArgumentException("Geographic coordinate system was null");
            }
            if (projection == null)
            {
                throw new ArgumentException("Projection was null");
            }
            if (linearUnit == null)
            {
                throw new ArgumentException("Linear unit was null");
            }
            List<AxisInfo> axisInfo = new List<AxisInfo>(2);
            axisInfo.Add(axis0);
            axisInfo.Add(axis1);
            return new ProjectedCoordinateSystem(null, gcs, linearUnit, projection, axisInfo, name, string.Empty, -1L, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// Creates a <see cref="T:Topology.CoordinateSystems.Projection" />.
        /// </summary>
        /// <param name="name">Name of projection</param>
        /// <param name="wktProjectionClass">Projection class</param>
        /// <param name="parameters">Projection parameters</param>
        /// <returns>Projection</returns>
        public IProjection CreateProjection(string name, string wktProjectionClass, List<ProjectionParameter> parameters)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Invalid name");
            }
            if ((parameters == null) || (parameters.Count == 0))
            {
                throw new ArgumentException("Invalid projection parameters");
            }
            return new Projection(wktProjectionClass, parameters, name, string.Empty, -1L, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// Creates a <see cref="T:Topology.CoordinateSystems.IVerticalCoordinateSystem" /> from a <see cref="T:Topology.CoordinateSystems.IVerticalDatum">datum</see> and <see cref="T:Topology.CoordinateSystems.LinearUnit">linear units</see>.
        /// </summary>
        /// <param name="name">Name of vertical coordinate system</param>
        /// <param name="datum">Vertical datum</param>
        /// <param name="verticalUnit">Unit</param>
        /// <param name="axis">Axis info</param>
        /// <returns>Vertical coordinate system</returns>
        public IVerticalCoordinateSystem CreateVerticalCoordinateSystem(string name, IVerticalDatum datum, ILinearUnit verticalUnit, AxisInfo axis)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a <see cref="T:Topology.CoordinateSystems.IVerticalDatum" /> from an enumerated type value.
        /// </summary>
        /// <param name="name">Name of datum</param>
        /// <param name="datumType">Type of datum</param>
        /// <returns>Vertical datum</returns>
        public IVerticalDatum CreateVerticalDatum(string name, DatumType datumType)
        {
            throw new NotImplementedException();
        }
    }
}

