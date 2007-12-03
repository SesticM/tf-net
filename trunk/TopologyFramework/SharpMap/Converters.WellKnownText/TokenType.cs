namespace Topology.Converters.WellKnownText
{
    using System;

    /// <summary>
    /// Represents the type of token created by the StreamTokenizer class.
    /// </summary>
    internal enum TokenType
    {
        Word,
        Number,
        Eol,
        Eof,
        Whitespace,
        Symbol
    }
}

