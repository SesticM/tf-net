namespace Topology.CoordinateSystems.Transformations
{
    using Topology.CoordinateSystems;
    using Topology.CoordinateSystems.Projections;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Creates coordinate transformations.
    /// </summary>
    public class CoordinateTransformationFactory : ICoordinateTransformationFactory
    {
        private static IMathTransform CreateCoordinateOperation(IGeocentricCoordinateSystem geo)
        {
            List<ProjectionParameter> parameters = new List<ProjectionParameter>(2);
            parameters.Add(new ProjectionParameter("semi_major", geo.HorizontalDatum.Ellipsoid.SemiMajorAxis));
            parameters.Add(new ProjectionParameter("semi_minor", geo.HorizontalDatum.Ellipsoid.SemiMinorAxis));
            return new GeocentricTransform(parameters);
        }

        private static IMathTransform CreateCoordinateOperation(IProjection projection, IEllipsoid ellipsoid)
        {
            List<ProjectionParameter> parameters = new List<ProjectionParameter>(projection.NumParameters);
            for (int i = 0; i < projection.NumParameters; i++)
            {
                parameters.Add(projection.GetParameter(i));
            }
            parameters.Add(new ProjectionParameter("semi_major", ellipsoid.SemiMajorAxis));
            parameters.Add(new ProjectionParameter("semi_minor", ellipsoid.SemiMinorAxis));
            switch (projection.ClassName.ToLower())
            {
                case "mercator_1sp":
                case "mercator_2sp":
                    return new Mercator(parameters);

                case "transverse_mercator":
                    return new TransverseMercator(parameters);

                case "albers":
                    return new AlbersProjection(parameters);

                case "lambert_conformal_conic":
                case "lambert_conformal_conic_2sp":
                    return new LambertConformalConic2SP(parameters);
            }
            throw new NotSupportedException(string.Format("Projection {0} is not supported.", projection.ClassName));
        }

        /// <summary>
        /// Creates a transformation between two coordinate systems.
        /// </summary>
        /// <remarks>
        /// This method will examine the coordinate systems in order to construct
        /// a transformation between them. This method may fail if no path between 
        /// the coordinate systems is found, using the normal failing behavior of 
        /// the DCP (e.g. throwing an exception).</remarks>
        /// <param name="sourceCS">Source coordinate system</param>
        /// <param name="targetCS">Target coordinate system</param>
        /// <returns></returns>
        public ICoordinateTransformation CreateFromCoordinateSystems(ICoordinateSystem sourceCS, ICoordinateSystem targetCS)
        {
            if ((sourceCS is IProjectedCoordinateSystem) && (targetCS is IGeographicCoordinateSystem))
            {
                return Proj2Geog((IProjectedCoordinateSystem) sourceCS, (IGeographicCoordinateSystem) targetCS);
            }
            if ((sourceCS is IGeographicCoordinateSystem) && (targetCS is IProjectedCoordinateSystem))
            {
                return Geog2Proj((IGeographicCoordinateSystem) sourceCS, (IProjectedCoordinateSystem) targetCS);
            }
            if ((sourceCS is IGeographicCoordinateSystem) && (targetCS is IGeocentricCoordinateSystem))
            {
                return Geog2Geoc((IGeographicCoordinateSystem) sourceCS, (IGeocentricCoordinateSystem) targetCS);
            }
            if ((sourceCS is IGeocentricCoordinateSystem) && (targetCS is IGeographicCoordinateSystem))
            {
                return Geoc2Geog((IGeocentricCoordinateSystem) sourceCS, (IGeographicCoordinateSystem) targetCS);
            }
            if ((sourceCS is IProjectedCoordinateSystem) && (targetCS is IProjectedCoordinateSystem))
            {
                return Proj2Proj(sourceCS as IProjectedCoordinateSystem, targetCS as IProjectedCoordinateSystem);
            }
            if ((sourceCS is IGeocentricCoordinateSystem) && (targetCS is IGeocentricCoordinateSystem))
            {
                return CreateGeoc2Geoc((IGeocentricCoordinateSystem) sourceCS, (IGeocentricCoordinateSystem) targetCS);
            }
            if (!(sourceCS is IGeographicCoordinateSystem) || !(targetCS is IGeographicCoordinateSystem))
            {
                throw new NotSupportedException("No support for transforming between the two specified coordinate systems");
            }
            return this.CreateGeog2Geog(sourceCS as IGeographicCoordinateSystem, targetCS as IGeographicCoordinateSystem);
        }

        /// <summary>
        /// Geocentric to Geocentric transformation
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private static ICoordinateTransformation CreateGeoc2Geoc(IGeocentricCoordinateSystem source, IGeocentricCoordinateSystem target)
        {
            ConcatenatedTransform mathTransform = new ConcatenatedTransform();
            if ((source.HorizontalDatum.Wgs84Parameters != null) && !source.HorizontalDatum.Wgs84Parameters.HasZeroValuesOnly)
            {
                mathTransform.CoordinateTransformationList.Add(new CoordinateTransformation(((target.HorizontalDatum.Wgs84Parameters == null) || target.HorizontalDatum.Wgs84Parameters.HasZeroValuesOnly) ? target : GeocentricCoordinateSystem.WGS84, source, TransformType.Transformation, new DatumTransform(source.HorizontalDatum.Wgs84Parameters), "", "", -1L, "", ""));
            }
            if ((target.HorizontalDatum.Wgs84Parameters != null) && !target.HorizontalDatum.Wgs84Parameters.HasZeroValuesOnly)
            {
                mathTransform.CoordinateTransformationList.Add(new CoordinateTransformation(((source.HorizontalDatum.Wgs84Parameters == null) || source.HorizontalDatum.Wgs84Parameters.HasZeroValuesOnly) ? source : GeocentricCoordinateSystem.WGS84, target, TransformType.Transformation, new DatumTransform(target.HorizontalDatum.Wgs84Parameters).Inverse(), "", "", -1L, "", ""));
            }
            if (mathTransform.CoordinateTransformationList.Count == 1)
            {
                return new CoordinateTransformation(source, target, TransformType.ConversionAndTransformation, mathTransform.CoordinateTransformationList[0].MathTransform, "", "", -1L, "", "");
            }
            return new CoordinateTransformation(source, target, TransformType.ConversionAndTransformation, mathTransform, "", "", -1L, "", "");
        }

        /// <summary>
        /// Geographic to geographic transformation
        /// </summary>
        /// <remarks>Adds a datum shift if nessesary</remarks>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private ICoordinateTransformation CreateGeog2Geog(IGeographicCoordinateSystem source, IGeographicCoordinateSystem target)
        {
            if (source.HorizontalDatum.EqualParams(target.HorizontalDatum))
            {
                return new CoordinateTransformation(source, target, TransformType.Conversion, new Topology.CoordinateSystems.Transformations.GeographicTransform(source, target), string.Empty, string.Empty, -1L, string.Empty, string.Empty);
            }
            CoordinateTransformationFactory factory = new CoordinateTransformationFactory();
            CoordinateSystemFactory factory2 = new CoordinateSystemFactory();
            IGeocentricCoordinateSystem targetCS = factory2.CreateGeocentricCoordinateSystem(source.HorizontalDatum.Name + " Geocentric", source.HorizontalDatum, LinearUnit.Metre, source.PrimeMeridian);
            IGeocentricCoordinateSystem system2 = factory2.CreateGeocentricCoordinateSystem(target.HorizontalDatum.Name + " Geocentric", target.HorizontalDatum, LinearUnit.Metre, source.PrimeMeridian);
            ConcatenatedTransform mathTransform = new ConcatenatedTransform();
            mathTransform.CoordinateTransformationList.Add(factory.CreateFromCoordinateSystems(source, targetCS));
            mathTransform.CoordinateTransformationList.Add(factory.CreateFromCoordinateSystems(targetCS, system2));
            mathTransform.CoordinateTransformationList.Add(factory.CreateFromCoordinateSystems(system2, target));
            return new CoordinateTransformation(source, target, TransformType.Transformation, mathTransform, string.Empty, string.Empty, -1L, string.Empty, string.Empty);
        }

        private static ICoordinateTransformation Geoc2Geog(IGeocentricCoordinateSystem source, IGeographicCoordinateSystem target)
        {
            return new CoordinateTransformation(source, target, TransformType.Conversion, CreateCoordinateOperation(source).Inverse(), string.Empty, string.Empty, -1L, string.Empty, string.Empty);
        }

        private static ICoordinateTransformation Geog2Geoc(IGeographicCoordinateSystem source, IGeocentricCoordinateSystem target)
        {
            return new CoordinateTransformation(source, target, TransformType.Conversion, CreateCoordinateOperation(target), string.Empty, string.Empty, -1L, string.Empty, string.Empty);
        }

        private static ICoordinateTransformation Geog2Proj(IGeographicCoordinateSystem source, IProjectedCoordinateSystem target)
        {
            if (source.EqualParams(target.GeographicCoordinateSystem))
            {
                return new CoordinateTransformation(source, target, TransformType.Transformation, CreateCoordinateOperation(target.Projection, target.GeographicCoordinateSystem.HorizontalDatum.Ellipsoid), string.Empty, string.Empty, -1L, string.Empty, string.Empty);
            }
            ConcatenatedTransform mathTransform = new ConcatenatedTransform();
            CoordinateTransformationFactory factory = new CoordinateTransformationFactory();
            mathTransform.CoordinateTransformationList.Add(factory.CreateFromCoordinateSystems(source, target.GeographicCoordinateSystem));
            mathTransform.CoordinateTransformationList.Add(factory.CreateFromCoordinateSystems(target.GeographicCoordinateSystem, target));
            return new CoordinateTransformation(source, target, TransformType.Transformation, mathTransform, string.Empty, string.Empty, -1L, string.Empty, string.Empty);
        }

        private static ICoordinateTransformation Proj2Geog(IProjectedCoordinateSystem source, IGeographicCoordinateSystem target)
        {
            if (source.GeographicCoordinateSystem.EqualParams(target))
            {
                return new CoordinateTransformation(source, target, TransformType.Transformation, CreateCoordinateOperation(source.Projection, source.GeographicCoordinateSystem.HorizontalDatum.Ellipsoid).Inverse(), string.Empty, string.Empty, -1L, string.Empty, string.Empty);
            }
            ConcatenatedTransform mathTransform = new ConcatenatedTransform();
            CoordinateTransformationFactory factory = new CoordinateTransformationFactory();
            mathTransform.CoordinateTransformationList.Add(factory.CreateFromCoordinateSystems(source, source.GeographicCoordinateSystem));
            mathTransform.CoordinateTransformationList.Add(factory.CreateFromCoordinateSystems(source.GeographicCoordinateSystem, target));
            return new CoordinateTransformation(source, target, TransformType.Transformation, mathTransform, string.Empty, string.Empty, -1L, string.Empty, string.Empty);
        }

        private static ICoordinateTransformation Proj2Proj(IProjectedCoordinateSystem source, IProjectedCoordinateSystem target)
        {
            ConcatenatedTransform mathTransform = new ConcatenatedTransform();
            CoordinateTransformationFactory factory = new CoordinateTransformationFactory();
            mathTransform.CoordinateTransformationList.Add(factory.CreateFromCoordinateSystems(source, source.GeographicCoordinateSystem));
            mathTransform.CoordinateTransformationList.Add(factory.CreateFromCoordinateSystems(source.GeographicCoordinateSystem, target.GeographicCoordinateSystem));
            mathTransform.CoordinateTransformationList.Add(factory.CreateFromCoordinateSystems(target.GeographicCoordinateSystem, target));
            return new CoordinateTransformation(source, target, TransformType.Transformation, mathTransform, string.Empty, string.Empty, -1L, string.Empty, string.Empty);
        }
    }
}

