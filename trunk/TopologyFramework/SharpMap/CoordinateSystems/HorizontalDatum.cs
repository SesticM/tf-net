namespace Topology.CoordinateSystems
{
    using Topology.Utilities;
    using System;
    using System.Text;

    /// <summary>
    /// Horizontal datum defining the standard datum information.
    /// </summary>
    public class HorizontalDatum : Datum, IHorizontalDatum, IDatum, IInfo
    {
        private IEllipsoid _Ellipsoid;
        private Wgs84ConversionInfo _Wgs84ConversionInfo;

        /// <summary>
        /// Initializes a new instance of a horizontal datum
        /// </summary>
        /// <param name="ellipsoid">Ellipsoid</param>
        /// <param name="toWgs84">Parameters for a Bursa Wolf transformation into WGS84</param>
        /// <param name="type">Datum type</param>
        /// <param name="name">Name</param>
        /// <param name="authority">Authority name</param>
        /// <param name="code">Authority-specific identification code.</param>
        /// <param name="alias">Alias</param>
        /// <param name="abbreviation">Abbreviation</param>
        /// <param name="remarks">Provider-supplied remarks</param>
        internal HorizontalDatum(IEllipsoid ellipsoid, Wgs84ConversionInfo toWgs84, DatumType type, string name, string authority, long code, string alias, string remarks, string abbreviation) : base(type, name, authority, code, alias, remarks, abbreviation)
        {
            this._Ellipsoid = ellipsoid;
            this._Wgs84ConversionInfo = toWgs84;
        }

        /// <summary>
        /// Checks whether the values of this instance is equal to the values of another instance.
        /// Only parameters used for coordinate system are used for comparison.
        /// Name, abbreviation, authority, alias and remarks are ignored in the comparison.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if equal</returns>
        public override bool EqualParams(object obj)
        {
            if (obj is HorizontalDatum)
            {
                HorizontalDatum datum = obj as HorizontalDatum;
                if ((datum.Wgs84Parameters == null) && (this.Wgs84Parameters != null))
                {
                    return false;
                }
                if ((datum.Wgs84Parameters != null) && !datum.Wgs84Parameters.Equals(this.Wgs84Parameters))
                {
                    return false;
                }
                if (datum.Ellipsoid == this.Ellipsoid)
                {
                    return (base.DatumType == datum.DatumType);
                }
            }
            return false;
        }

        /// <summary>
        /// European Datum 1950
        /// </summary>
        /// <remarks>
        /// <para>Area of use:
        /// Europe - west - Denmark; Faroe Islands; France offshore; Israel offshore; Italy including San 
        /// Marino and Vatican City State; Ireland offshore; Netherlands offshore; Germany; Greece (offshore);
        /// North Sea; Norway; Spain; Svalbard; Turkey; United Kingdom UKCS offshore. Egypt - Western Desert.
        /// </para>
        /// <para>Origin description: Fundamental point: Potsdam (Helmert Tower). 
        /// Latitude: 52 deg 22 min 51.4456 sec N; Longitude: 13 deg  3 min 58.9283 sec E (of Greenwich).</para>
        /// </remarks>
        public static HorizontalDatum ED50
        {
            get
            {
                return new HorizontalDatum(Topology.CoordinateSystems.Ellipsoid.International1924, new Wgs84ConversionInfo(-87, -98, -121, 0, 0, 0, 0), DatumType.HD_Geocentric, "European Datum 1950", "EPSG", 0x1856L, "ED50", string.Empty, string.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the ellipsoid of the datum
        /// </summary>
        public IEllipsoid Ellipsoid
        {
            get
            {
                return this._Ellipsoid;
            }
            set
            {
                this._Ellipsoid = value;
            }
        }

        /// <summary>
        /// European Terrestrial Reference System 1989
        /// </summary>
        /// <remarks>
        /// <para>Area of use: 
        /// Europe: Albania; Andorra; Austria; Belgium; Bosnia and Herzegovina; Bulgaria; Croatia; 
        /// Cyprus; Czech Republic; Denmark; Estonia; Finland; Faroe Islands; France; Germany; Greece; 
        /// Hungary; Ireland; Italy; Latvia; Liechtenstein; Lithuania; Luxembourg; Malta; Netherlands; 
        /// Norway; Poland; Portugal; Romania; San Marino; Serbia and Montenegro; Slovakia; Slovenia; 
        /// Spain; Svalbard; Sweden; Switzerland; United Kingdom (UK) including Channel Islands and 
        /// Isle of Man; Vatican City State.</para>
        /// <para>Origin description: Fixed to the stable part of the Eurasian continental 
        /// plate and consistent with ITRS at the epoch 1989.0.</para>
        /// </remarks>
        public static HorizontalDatum ETRF89
        {
            get
            {
                HorizontalDatum datum = new HorizontalDatum(Topology.CoordinateSystems.Ellipsoid.GRS80, null, DatumType.HD_Geocentric, "European Terrestrial Reference System 1989", "EPSG", 0x1872L, "ETRF89", "The distinction in usage between ETRF89 and ETRS89 is confused: although in principle conceptually different in practice both are used for the realisation.", string.Empty);
                datum.Wgs84Parameters = new Wgs84ConversionInfo();
                return datum;
            }
        }

        /// <summary>
        /// World Geodetic System 1972
        /// </summary>
        /// <remarks>
        /// <para>Used by GPS before 1987. For Transit satellite positioning see also WGS 72BE. Datum code 6323 reserved for southern hemisphere ProjCS's.</para>
        /// <para>Area of use: World</para>
        /// <para>Origin description: Developed from a worldwide distribution of terrestrial and
        /// geodetic satellite observations and defined through a set of station coordinates.</para>
        /// </remarks>
        public static HorizontalDatum WGS72
        {
            get
            {
                HorizontalDatum datum = new HorizontalDatum(Topology.CoordinateSystems.Ellipsoid.WGS72, null, DatumType.HD_Geocentric, "World Geodetic System 1972", "EPSG", 0x18b2L, string.Empty, "Used by GPS before 1987. For Transit satellite positioning see also WGS 72BE. Datum code 6323 reserved for southern hemisphere ProjCS's.", string.Empty);
                datum.Wgs84Parameters = new Wgs84ConversionInfo(0, 0, 4.5, 0, 0, 0.554, 0.219);
                return datum;
            }
        }

        /// <summary>
        /// EPSG's WGS 84 datum has been the then current realisation. No distinction is made between the original WGS 84 
        /// frame, WGS 84 (G730), WGS 84 (G873) and WGS 84 (G1150). Since 1997, WGS 84 has been maintained within 10cm of 
        /// the then current ITRF.
        /// </summary>
        /// <remarks>
        /// <para>Area of use: World</para>
        /// <para>Origin description: Defined through a consistent set of station coordinates. These have changed with time: by 0.7m 
        /// on 29/6/1994 [WGS 84 (G730)], a further 0.2m on 29/1/1997 [WGS 84 (G873)] and a further 0.06m on 
        /// 20/1/2002 [WGS 84 (G1150)].</para>
        /// </remarks>
        public static HorizontalDatum WGS84
        {
            get
            {
                return new HorizontalDatum(Topology.CoordinateSystems.Ellipsoid.WGS84, null, DatumType.HD_Geocentric, "World Geodetic System 1984", "EPSG", 0x18b6L, string.Empty, "EPSG's WGS 84 datum has been the then current realisation. No distinction is made between the original WGS 84 frame, WGS 84 (G730), WGS 84 (G873) and WGS 84 (G1150). Since 1997, WGS 84 has been maintained within 10cm of the then current ITRF.", string.Empty);
            }
        }

        /// <summary>
        /// Gets preferred parameters for a Bursa Wolf transformation into WGS84
        /// </summary>
        public Wgs84ConversionInfo Wgs84Parameters
        {
            get
            {
                return this._Wgs84ConversionInfo;
            }
            set
            {
                this._Wgs84ConversionInfo = value;
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
                builder.AppendFormat("DATUM[\"{0}\", {1}", base.Name, this._Ellipsoid.WKT);
                if (this._Wgs84ConversionInfo != null)
                {
                    builder.AppendFormat(", {0}", this._Wgs84ConversionInfo.WKT);
                }
                if (!string.IsNullOrEmpty(base.Authority) && (base.AuthorityCode > 0L))
                {
                    builder.AppendFormat(", AUTHORITY[\"{0}\", \"{1}\"]", base.Authority, base.AuthorityCode);
                }
                builder.Append("]");
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
                return string.Format(NumberFormatter.GetNfi(), "<CS_HorizontalDatum DatumType=\"{0}\">{1}{2}{3}</CS_HorizontalDatum>", new object[] { (int) base.DatumType, base.InfoXml, this.Ellipsoid.XML, (this.Wgs84Parameters == null) ? string.Empty : this.Wgs84Parameters.XML });
            }
        }
    }
}

