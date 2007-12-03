namespace Topology.Converters.WellKnownText
{
    using Topology.Converters.WellKnownText.IO;
    using Topology.Utilities;
    using System;
    using System.IO;

    /// <summary>
    /// Reads a stream of Well Known Text (wkt) string and returns a stream of tokens.
    /// </summary>
    internal class WktStreamTokenizer : StreamTokenizer
    {
        /// <summary>
        /// Initializes a new instance of the WktStreamTokenizer class.
        /// </summary>
        /// <remarks>The WktStreamTokenizer class ais in reading WKT streams.</remarks>
        /// <param name="reader">A TextReader that contains </param>
        public WktStreamTokenizer(TextReader reader) : base(reader, true)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
        }

        /// <summary>
        /// Reads the authority and authority code.
        /// </summary>
        /// <param name="authority">String to place the authority in.</param>
        /// <param name="authorityCode">String to place the authority code in.</param>
        public void ReadAuthority(ref string authority, ref long authorityCode)
        {
            if (base.GetStringValue() != "AUTHORITY")
            {
                this.ReadToken("AUTHORITY");
            }
            this.ReadToken("[");
            authority = this.ReadDoubleQuotedWord();
            this.ReadToken(",");
            long.TryParse(this.ReadDoubleQuotedWord(), out authorityCode);
            this.ReadToken("]");
        }

        /// <summary>
        /// Reads a string inside double quotes.
        /// </summary>
        /// <remarks>
        /// White space inside quotes is preserved.
        /// </remarks>
        /// <returns>The string inside the double quotes.</returns>
        public string ReadDoubleQuotedWord()
        {
            string str = "";
            this.ReadToken("\"");
            base.NextToken(false);
            while (base.GetStringValue() != "\"")
            {
                str = str + base.GetStringValue();
                base.NextToken(false);
            }
            return str;
        }

        /// <summary>
        /// Reads a token and checks it is what is expected.
        /// </summary>
        /// <param name="expectedToken">The expected token.</param>
        internal void ReadToken(string expectedToken)
        {
            base.NextToken();
            if (base.GetStringValue() != expectedToken)
            {
                throw new Exception(string.Format(NumberFormatter.GetNfi(), "Expecting ('{3}') but got a '{0}' at line {1} column {2}.", new object[] { base.GetStringValue(), base.LineNumber, base.Column, expectedToken }));
            }
        }
    }
}

