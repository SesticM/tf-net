namespace Topology.CoordinateSystems.Transformations
{
    using Topology.CoordinateSystems;
    using System;

    /// <summary>
    /// Describes a coordinate transformation. This class only describes a 
    /// coordinate transformation, it does not actually perform the transform 
    /// operation on points. To transform points you must use a <see cref="P:Topology.CoordinateSystems.Transformations.CoordinateTransformation.MathTransform" />.
    /// </summary>
    public class CoordinateTransformation : ICoordinateTransformation
    {
        private string _AreaOfUse;
        private string _Authority;
        private long _AuthorityCode;
        private IMathTransform _MathTransform;
        private string _Name;
        private string _Remarks;
        private ICoordinateSystem _SourceCS;
        private ICoordinateSystem _TargetCS;
        private Topology.CoordinateSystems.Transformations.TransformType _TransformType;

        /// <summary>
        /// Initializes an instance of a CoordinateTransformation
        /// </summary>
        /// <param name="sourceCS">Source coordinate system</param>
        /// <param name="targetCS">Target coordinate system</param>
        /// <param name="transformType">Transformation type</param>
        /// <param name="mathTransform">Math transform</param>
        /// <param name="name">Name of transform</param>
        /// <param name="authority">Authority</param>
        /// <param name="authorityCode">Authority code</param>
        /// <param name="areaOfUse">Area of use</param>
        /// <param name="remarks">Remarks</param>
        internal CoordinateTransformation(ICoordinateSystem sourceCS, ICoordinateSystem targetCS, Topology.CoordinateSystems.Transformations.TransformType transformType, IMathTransform mathTransform, string name, string authority, long authorityCode, string areaOfUse, string remarks)
        {
            this._TargetCS = targetCS;
            this._SourceCS = sourceCS;
            this._TransformType = transformType;
            this._MathTransform = mathTransform;
            this._Name = name;
            this._Authority = authority;
            this._AuthorityCode = authorityCode;
            this._AreaOfUse = areaOfUse;
            this._Remarks = remarks;
        }

        /// <summary>
        /// Human readable description of domain in source coordinate system.
        /// </summary>
        public string AreaOfUse
        {
            get
            {
                return this._AreaOfUse;
            }
        }

        /// <summary>
        /// Authority which defined transformation and parameter values.
        /// </summary>
        /// <remarks>
        /// An Authority is an organization that maintains definitions of Authority Codes. For example the European Petroleum Survey Group (EPSG) maintains a database of coordinate systems, and other spatial referencing objects, where each object has a code number ID. For example, the EPSG code for a WGS84 Lat/Lon coordinate system is ‘4326’
        /// </remarks>
        public string Authority
        {
            get
            {
                return this._Authority;
            }
        }

        /// <summary>
        /// Code used by authority to identify transformation. An empty string is used for no code.
        /// </summary>
        /// <remarks>The AuthorityCode is a compact string defined by an Authority to reference a particular spatial reference object. For example, the European Survey Group (EPSG) authority uses 32 bit integers to reference coordinate systems, so all their code strings will consist of a few digits. The EPSG code for WGS84 Lat/Lon is ‘4326’.</remarks>
        public long AuthorityCode
        {
            get
            {
                return this._AuthorityCode;
            }
        }

        /// <summary>
        /// Gets math transform.
        /// </summary>
        public IMathTransform MathTransform
        {
            get
            {
                return this._MathTransform;
            }
        }

        /// <summary>
        /// Name of transformation.
        /// </summary>
        public string Name
        {
            get
            {
                return this._Name;
            }
        }

        /// <summary>
        /// Gets the provider-supplied remarks.
        /// </summary>
        public string Remarks
        {
            get
            {
                return this._Remarks;
            }
        }

        /// <summary>
        /// Source coordinate system.
        /// </summary>
        public ICoordinateSystem SourceCS
        {
            get
            {
                return this._SourceCS;
            }
        }

        /// <summary>
        /// Target coordinate system.
        /// </summary>
        public ICoordinateSystem TargetCS
        {
            get
            {
                return this._TargetCS;
            }
        }

        /// <summary>
        /// Semantic type of transform. For example, a datum transformation or a coordinate conversion.
        /// </summary>
        public Topology.CoordinateSystems.Transformations.TransformType TransformType
        {
            get
            {
                return this._TransformType;
            }
        }
    }
}

