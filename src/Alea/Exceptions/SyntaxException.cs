using System;
using System.Collections.Generic;
using System.Text;

namespace Alea.Exceptions
{
    public class SyntaxException : AleaException
    {
        /// <inheritdoc />
        public SyntaxException(string message) : base($"Syntax error: {message}")
        {
        }
    }
}
