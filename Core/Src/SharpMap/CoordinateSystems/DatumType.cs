namespace Topology.CoordinateSystems
{
    using System;

    /// <summary>
    /// A vertical datum of geoid model derived heights, also called GPS-derived heights.
    /// These heights are approximations of orthometric heights (H), constructed from the
    /// ellipsoidal heights (h) by the use of the given geoid undulation model (N) through
    /// the equation: H=h-N.
    /// </summary>
    public enum DatumType
    {
        /// <summary>
        /// These datums, such as ED50, NAD27 and NAD83, have been designed to support
        /// horizontal positions on the ellipsoid as opposed to positions in 3-D space. These datums were designed mainly to support a horizontal component of a position in a domain of limited extent, such as a country, a region or a continent.
        /// </summary>
        HD_Classic = 0x3e9,
        /// <summary>
        /// A geocentric datum is a "satellite age" modern geodetic datum mainly of global
        /// extent, such as WGS84 (used in GPS), PZ90 (used in GLONASS) and ITRF. These
        /// datums were designed to support both a horizontal component of position and 
        /// a vertical component of position (through ellipsoidal heights). The regional
        /// realizations of ITRF, such as ETRF, are also included in this category.
        /// </summary>
        HD_Geocentric = 0x3ea,
        /// <summary>
        /// Highest possible value for horizontal datum types.
        /// </summary>
        HD_Max = 0x7cf,
        /// <summary>
        /// Lowest possible value for horizontal datum types
        /// </summary>
        HD_Min = 0x3e8,
        /// <summary>
        /// Unspecified horizontal datum type. Horizontal datums with this type should never
        /// supply a conversion to WGS84 using Bursa Wolf parameters.
        /// </summary>
        HD_Other = 0x3e8,
        /// <summary>
        /// Highest possible value for local datum types.
        /// </summary>
        LD_Max = 0x7fff,
        /// <summary>
        /// Lowest possible value for local datum types.
        /// </summary>
        LD_Min = 0x2710,
        /// <summary>
        /// The vertical datum of altitudes or heights in the atmosphere. These are
        /// approximations of orthometric heights obtained with the help of a barometer or
        /// a barometric altimeter. These values are usually expressed in one of the
        /// following units: meters, feet, millibars (used to measure pressure levels), or
        /// theta value (units used to measure geopotential height).
        /// </summary>
        VD_AltitudeBarometric = 0x7d3,
        /// <summary>
        /// This attribute is used to support the set of datums generated for hydrographic
        /// engineering projects where depth measurements below sea level are needed. It is
        /// often called a hydrographic or a marine datum. Depths are measured in the 
        /// direction perpendicular (approximately) to the actual equipotential surfaces of
        /// the earth's gravity field, using such procedures as echo-sounding.
        /// </summary>
        VD_Depth = 0x7d6,
        /// <summary>
        /// A vertical datum for ellipsoidal heights that are measured along the normal to
        /// the ellipsoid used in the definition of horizontal datum.
        /// </summary>
        VD_Ellipsoidal = 0x7d2,
        /// <summary>
        /// A vertical datum of geoid model derived heights, also called GPS-derived heights.
        /// These heights are approximations of orthometric heights (H), constructed from the
        /// ellipsoidal heights (h) by the use of the given geoid undulation model (N) 
        /// through the equation: H=h-N.
        /// </summary>
        VD_GeoidModelDerived = 0x7d5,
        /// <summary>
        /// Highest possible value for vertical datum types.
        /// </summary>
        VD_Max = 0xbb7,
        /// <summary>
        /// Lowest possible value for vertical datum types.
        /// </summary>
        VD_Min = 0x7d0,
        /// <summary>
        /// A normal height system.
        /// </summary>
        VD_Normal = 0x7d4,
        /// <summary>
        /// A vertical datum for orthometric heights that are measured along the plumb line.
        /// </summary>
        VD_Orthometric = 0x7d1,
        /// <summary>
        /// Unspecified vertical datum type.
        /// </summary>
        VD_Other = 0x7d0
    }
}

