namespace Topology.CoordinateSystems
{
    using System;
    using System.Text;

    /// <summary>
    /// The Info object defines the standard information
    /// stored with spatial reference objects
    /// </summary>
    public abstract class Info : IInfo
    {
        private string _Abbreviation;
        private string _Alias;
        private string _Authority;
        private long _Code;
        private string _Name;
        private string _Remarks;

        /// <summary>
        /// A base interface for metadata applicable to coordinate system objects.
        /// </summary>
        /// <remarks>
        /// <para>The metadata items ‘Abbreviation’, ‘Alias’, ‘Authority’, ‘AuthorityCode’, ‘Name’ and ‘Remarks’ 
        /// were specified in the Simple Features interfaces, so they have been kept here.</para>
        /// <para>This specification does not dictate what the contents of these items 
        /// should be. However, the following guidelines are suggested:</para>
        /// <para>When <see cref="T:Topology.CoordinateSystems.ICoordinateSystemAuthorityFactory" /> is used to create an object, the ‘Authority’
        /// and 'AuthorityCode' values should be set to the authority name of the factory object, and the authority 
        /// code supplied by the client, respectively. The other values may or may not be set. (If the authority is 
        /// EPSG, the implementer may consider using the corresponding metadata values in the EPSG tables.)</para>
        /// <para>When <see cref="T:Topology.CoordinateSystems.CoordinateSystemFactory" /> creates an object, the 'Name' should be set to the value
        /// supplied by the client. All of the other metadata items should be left empty</para>
        /// </remarks>
        /// <param name="name">Name</param>
        /// <param name="authority">Authority name</param>
        /// <param name="code">Authority-specific identification code.</param>
        /// <param name="alias">Alias</param>
        /// <param name="abbreviation">Abbreviation</param>
        /// <param name="remarks">Provider-supplied remarks</param>
        internal Info(string name, string authority, long code, string alias, string abbreviation, string remarks)
        {
            this._Name = name;
            this._Authority = authority;
            this._Code = code;
            this._Alias = alias;
            this._Abbreviation = abbreviation;
            this._Remarks = remarks;
        }

        /// <summary>
        /// Checks whether the values of this instance is equal to the values of another instance.
        /// Only parameters used for coordinate system are used for comparison.
        /// Name, abbreviation, authority, alias and remarks are ignored in the comparison.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if equal</returns>
        public abstract bool EqualParams(object obj);
        /// <summary>
        /// Returns the Well-known text for this object
        /// as defined in the simple features specification.
        /// </summary>
        public override string ToString()
        {
            return this.WKT;
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
        /// Returns an XML string of the info object
        /// </summary>
        internal string InfoXml
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("<CS_Info", new object[0]);
                if (this.AuthorityCode > 0L)
                {
                    builder.AppendFormat(" AuthorityCode=\"{0}\"", this.AuthorityCode);
                }
                if (!string.IsNullOrEmpty(this.Abbreviation))
                {
                    builder.AppendFormat(" Abbreviation=\"{0}\"", this.Abbreviation);
                }
                if (!string.IsNullOrEmpty(this.Authority))
                {
                    builder.AppendFormat(" Authority=\"{0}\"", this.Authority);
                }
                if (!string.IsNullOrEmpty(this.Name))
                {
                    builder.AppendFormat(" Name=\"{0}\"", this.Name);
                }
                builder.Append("/>");
                return builder.ToString();
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
        public abstract string WKT { get; }

        /// <summary>
        /// Gets an XML representation of this object.
        /// </summary>
        public abstract string XML { get; }
    }
}

