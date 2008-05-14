namespace Topology.CoordinateSystems
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Base interface for all coordinate systems.
    /// </summary>
    /// <remarks>
    /// <para>A coordinate system is a mathematical space, where the elements of the space
    /// are called positions. Each position is described by a list of numbers. The length 
    /// of the list corresponds to the dimension of the coordinate system. So in a 2D 
    /// coordinate system each position is described by a list containing 2 numbers.</para>
    /// <para>However, in a coordinate system, not all lists of numbers correspond to a 
    /// position - some lists may be outside the domain of the coordinate system. For 
    /// example, in a 2D Lat/Lon coordinate system, the list (91,91) does not correspond
    /// to a position.</para>
    /// <para>Some coordinate systems also have a mapping from the mathematical space into 
    /// locations in the real world. So in a Lat/Lon coordinate system, the mathematical 
    /// position (lat, long) corresponds to a location on the surface of the Earth. This 
    /// mapping from the mathematical space into real-world locations is called a Datum.</para>
    /// </remarks>
    public abstract class CoordinateSystem : Info, ICoordinateSystem, IInfo
    {
        private List<Topology.CoordinateSystems.AxisInfo> _AxisInfo;
        private double[] _DefaultEnvelope;

        /// <summary>
        /// Initializes a new instance of a coordinate system.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="authority">Authority name</param>
        /// <param name="authorityCode">Authority-specific identification code.</param>
        /// <param name="alias">Alias</param>
        /// <param name="abbreviation">Abbreviation</param>
        /// <param name="remarks">Provider-supplied remarks</param>
        internal CoordinateSystem(string name, string authority, long authorityCode, string alias, string abbreviation, string remarks) : base(name, authority, authorityCode, alias, abbreviation, remarks)
        {
        }

        /// <summary>
        /// Gets axis details for dimension within coordinate system.
        /// </summary>
        /// <param name="dimension">Dimension</param>
        /// <returns>Axis info</returns>
        public Topology.CoordinateSystems.AxisInfo GetAxis(int dimension)
        {
            if ((dimension >= this._AxisInfo.Count) || (dimension < 0))
            {
                throw new ArgumentException("AxisInfo not available for dimension " + dimension.ToString());
            }
            return this._AxisInfo[dimension];
        }

        /// <summary>
        /// Gets the units for the dimension within coordinate system. 
        /// Each dimension in the coordinate system has corresponding units.
        /// </summary>
        public abstract IUnit GetUnits(int dimension);

        internal List<Topology.CoordinateSystems.AxisInfo> AxisInfo
        {
            get
            {
                return this._AxisInfo;
            }
            set
            {
                this._AxisInfo = value;
            }
        }

        /// <summary>
        /// Gets default envelope of coordinate system.
        /// </summary>
        /// <remarks>
        /// Coordinate systems which are bounded should return the minimum bounding box of their domain. 
        /// Unbounded coordinate systems should return a box which is as large as is likely to be used. 
        /// For example, a (lon,lat) geographic coordinate system in degrees should return a box from 
        /// (-180,-90) to (180,90), and a geocentric coordinate system could return a box from (-r,-r,-r)
        /// to (+r,+r,+r) where r is the approximate radius of the Earth.
        /// </remarks>
        public double[] DefaultEnvelope
        {
            get
            {
                return this._DefaultEnvelope;
            }
            set
            {
                this._DefaultEnvelope = value;
            }
        }

        /// <summary>
        /// Dimension of the coordinate system.
        /// </summary>
        public int Dimension
        {
            get
            {
                return this._AxisInfo.Count;
            }
        }
    }
}

