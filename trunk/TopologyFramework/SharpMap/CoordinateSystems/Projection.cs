namespace Topology.CoordinateSystems
{
    using Topology.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// The Projection class defines the standard information stored with a projection
    /// objects. A projection object implements a coordinate transformation from a geographic
    /// coordinate system to a projected coordinate system, given the ellipsoid for the
    /// geographic coordinate system. It is expected that each coordinate transformation of
    /// interest, e.g., Transverse Mercator, Lambert, will be implemented as a class of
    /// type Projection, supporting the IProjection interface.
    /// </summary>
    public class Projection : Info, IProjection, IInfo
    {
        private string _ClassName;
        private List<ProjectionParameter> _Parameters;

        internal Projection(string className, List<ProjectionParameter> parameters, string name, string authority, long code, string alias, string remarks, string abbreviation) : base(name, authority, code, alias, abbreviation, remarks)
        {
            this._Parameters = parameters;
            this._ClassName = className;
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
            if (!(obj is Projection))
            {
                return false;
            }
            Projection proj = obj as Projection;
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
            return true;
        }

        /// <summary>
        /// Gets an indexed parameter of the projection.
        /// </summary>
        /// <param name="n">Index of parameter</param>
        /// <returns>n'th parameter</returns>
        public ProjectionParameter GetParameter(int n)
        {
            return this._Parameters[n];
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
        /// Gets the projection classification name (e.g. "Transverse_Mercator").
        /// </summary>
        public string ClassName
        {
            get
            {
                return this._ClassName;
            }
        }

        /// <summary>
        /// Gets the number of parameters of the projection.
        /// </summary>
        public int NumParameters
        {
            get
            {
                return this._Parameters.Count;
            }
        }

        /// <summary>
        /// Gets or sets the parameters of the projection
        /// </summary>
        internal List<ProjectionParameter> Parameters
        {
            get
            {
                return this._Parameters;
            }
            set
            {
                this._Parameters = value;
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
                builder.AppendFormat("PROJECTION[\"{0}\"", base.Name);
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
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat(NumberFormatter.GetNfi(), "<CS_Projection Classname=\"{0}\">{1}", new object[] { this.ClassName, base.InfoXml });
                foreach (ProjectionParameter parameter in this.Parameters)
                {
                    builder.Append(parameter.XML);
                }
                builder.Append("</CS_Projection>");
                return builder.ToString();
            }
        }
    }
}

