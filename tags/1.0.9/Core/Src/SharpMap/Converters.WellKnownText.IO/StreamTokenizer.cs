namespace Topology.Converters.WellKnownText.IO
{
    using Topology.Converters.WellKnownText;
    using Topology.Utilities;
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// The StreamTokenizer class takes an input stream and parses it into "tokens", allowing the tokens to be read one at a time. The parsing process is controlled by a table and a number of flags that can be set to various states. The stream tokenizer can recognize identifiers, numbers, quoted strings, and various comment style
    /// </summary>
    /// <remarks>
    /// This is a crude c# implementation of Java's <a href="http://java.sun.com/products/jdk/1.2/docs/api/java/io/StreamTokenizer.html">StreamTokenizer</a> class.
    /// </remarks>
    internal class StreamTokenizer
    {
        private int _colNumber = 1;
        private string _currentToken;
        private Topology.Converters.WellKnownText.TokenType _currentTokenType;
        private bool _ignoreWhitespace;
        private int _lineNumber = 1;
        private TextReader _reader;

        /// <summary>
        /// Initializes a new instance of the StreamTokenizer class.
        /// </summary>
        /// <param name="reader">A TextReader with some text to read.</param>
        /// <param name="ignoreWhitespace">Flag indicating whether whitespace should be ignored.</param>
        public StreamTokenizer(TextReader reader, bool ignoreWhitespace)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            this._reader = reader;
            this._ignoreWhitespace = ignoreWhitespace;
        }

        /// <summary>
        /// If the current token is a number, this field contains the value of that number. 
        /// </summary>
        /// <remarks>
        /// If the current token is a number, this field contains the value of that number. The current token is a number when the value of the ttype field is TT_NUMBER.
        /// </remarks>
        /// <exception cref="T:System.FormatException">Current token is not a number in a valid format.</exception>
        public double GetNumericValue()
        {
            string stringValue = this.GetStringValue();
            if (this.GetTokenType() != Topology.Converters.WellKnownText.TokenType.Number)
            {
                throw new Exception(string.Format(NumberFormatter.GetNfi(), "The token '{0}' is not a number at line {1} column {2}.", new object[] { stringValue, this.LineNumber, this.Column }));
            }
            return double.Parse(stringValue, NumberFormatter.GetNfi());
        }

        /// <summary>
        /// If the current token is a word token, this field contains a string giving the characters of the word token. 
        /// </summary>
        public string GetStringValue()
        {
            return this._currentToken;
        }

        /// <summary>
        /// Gets the token type of the current token.
        /// </summary>
        /// <returns></returns>
        public Topology.Converters.WellKnownText.TokenType GetTokenType()
        {
            return this._currentTokenType;
        }

        /// <summary>
        /// Determines a characters type (e.g. number, symbols, character).
        /// </summary>
        /// <param name="character">The character to determine.</param>
        /// <returns>The TokenType the character is.</returns>
        private Topology.Converters.WellKnownText.TokenType GetType(char character)
        {
            if (char.IsDigit(character))
            {
                return Topology.Converters.WellKnownText.TokenType.Number;
            }
            if (char.IsLetter(character))
            {
                return Topology.Converters.WellKnownText.TokenType.Word;
            }
            if (character == '\n')
            {
                return Topology.Converters.WellKnownText.TokenType.Eol;
            }
            if (!char.IsWhiteSpace(character) && !char.IsControl(character))
            {
                return Topology.Converters.WellKnownText.TokenType.Symbol;
            }
            return Topology.Converters.WellKnownText.TokenType.Whitespace;
        }

        /// <summary>
        /// Returns next token that is not whitespace.
        /// </summary>
        /// <returns></returns>
        private Topology.Converters.WellKnownText.TokenType NextNonWhitespaceToken()
        {
            Topology.Converters.WellKnownText.TokenType type = this.NextTokenAny();
            while ((type == Topology.Converters.WellKnownText.TokenType.Whitespace) || (type == Topology.Converters.WellKnownText.TokenType.Eol))
            {
                type = this.NextTokenAny();
            }
            return type;
        }

        /// <summary>
        /// Returns the next token.
        /// </summary>
        /// <returns>The TokenType of the next token.</returns>
        public Topology.Converters.WellKnownText.TokenType NextToken()
        {
            return this.NextToken(this._ignoreWhitespace);
        }

        /// <summary>
        /// Returns the next token.
        /// </summary>
        /// <param name="ignoreWhitespace">Determines is whitespace is ignored. True if whitespace is to be ignored.</param>
        /// <returns>The TokenType of the next token.</returns>
        public Topology.Converters.WellKnownText.TokenType NextToken(bool ignoreWhitespace)
        {
            if (ignoreWhitespace)
            {
                return this.NextNonWhitespaceToken();
            }
            return this.NextTokenAny();
        }

        private Topology.Converters.WellKnownText.TokenType NextTokenAny()
        {
            Topology.Converters.WellKnownText.TokenType eof = Topology.Converters.WellKnownText.TokenType.Eof;
            char[] chArray = new char[1];
            this._currentToken = "";
            this._currentTokenType = Topology.Converters.WellKnownText.TokenType.Eof;
            int num = this._reader.Read(chArray, 0, 1);
            bool flag = false;
            bool flag2 = false;
            byte[] bytes = null;
            ASCIIEncoding encoding = new ASCIIEncoding();
            char[] chars = null;
            while (num != 0)
            {
                bytes = new byte[] { (byte) this._reader.Peek() };
                chars = encoding.GetChars(bytes);
                char character = chArray[0];
                char ch2 = chars[0];
                this._currentTokenType = this.GetType(character);
                eof = this.GetType(ch2);
                if (flag2 && (character == '_'))
                {
                    this._currentTokenType = Topology.Converters.WellKnownText.TokenType.Word;
                }
                if (flag2 && (this._currentTokenType == Topology.Converters.WellKnownText.TokenType.Number))
                {
                    this._currentTokenType = Topology.Converters.WellKnownText.TokenType.Word;
                }
                if ((this._currentTokenType == Topology.Converters.WellKnownText.TokenType.Word) && (ch2 == '_'))
                {
                    eof = Topology.Converters.WellKnownText.TokenType.Word;
                    flag2 = true;
                }
                if ((this._currentTokenType == Topology.Converters.WellKnownText.TokenType.Word) && (eof == Topology.Converters.WellKnownText.TokenType.Number))
                {
                    eof = Topology.Converters.WellKnownText.TokenType.Word;
                    flag2 = true;
                }
                if (((character == '-') && (eof == Topology.Converters.WellKnownText.TokenType.Number)) && !flag)
                {
                    this._currentTokenType = Topology.Converters.WellKnownText.TokenType.Number;
                    eof = Topology.Converters.WellKnownText.TokenType.Number;
                }
                if ((flag && (eof == Topology.Converters.WellKnownText.TokenType.Number)) && (character == '.'))
                {
                    this._currentTokenType = Topology.Converters.WellKnownText.TokenType.Number;
                }
                if (((this._currentTokenType == Topology.Converters.WellKnownText.TokenType.Number) && (ch2 == '.')) && !flag)
                {
                    eof = Topology.Converters.WellKnownText.TokenType.Number;
                    flag = true;
                }
                this._colNumber++;
                if (this._currentTokenType == Topology.Converters.WellKnownText.TokenType.Eol)
                {
                    this._lineNumber++;
                    this._colNumber = 1;
                }
                this._currentToken = this._currentToken + character;
                if (this._currentTokenType != eof)
                {
                    num = 0;
                }
                else
                {
                    if ((this._currentTokenType == Topology.Converters.WellKnownText.TokenType.Symbol) && (character != '-'))
                    {
                        num = 0;
                        continue;
                    }
                    num = this._reader.Read(chArray, 0, 1);
                }
            }
            return this._currentTokenType;
        }

        /// <summary>
        /// The current column number of the stream being read.
        /// </summary>
        public int Column
        {
            get
            {
                return this._colNumber;
            }
        }

        /// <summary>
        /// The current line number of the stream being read.
        /// </summary>
        public int LineNumber
        {
            get
            {
                return this._lineNumber;
            }
        }
    }
}

