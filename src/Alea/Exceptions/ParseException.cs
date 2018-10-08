using System;
using System.Collections.Generic;
using System.Text;

namespace Alea.Exceptions
{
    /// <summary>
    /// Represents an unrecoverable error when parsing an expression.
    /// </summary>
    public class ParseException : AleaException
    {
        public ParseException(string message) : base($"Parser error: {message}")
        {
        }
    }
}
