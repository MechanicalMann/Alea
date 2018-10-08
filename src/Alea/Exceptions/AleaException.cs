using System;
using System.Collections.Generic;
using System.Text;

namespace Alea.Exceptions
{
    public abstract class AleaException : Exception
    {
        protected AleaException(string message)
            : base(message)
        {
        }
    }
}
