using System;
using System.Collections.Generic;
using System.Text;

namespace Alea.Exceptions
{
    public class SemanticException : AleaException
    {
        /// <inheritdoc />
        public SemanticException(string message) : base(message)
        {
        }
    }
}
