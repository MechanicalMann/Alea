using System;
using System.Collections.Generic;
using System.Text;

namespace Alea.Expressions
{
    /// <summary>
    /// Represents a binary operator expression.
    /// </summary>
    public abstract class BinaryExpression : AleaExpression
    {
        public virtual AleaExpression Left { get; }
        public virtual AleaExpression Right { get; }

        protected BinaryExpression(AleaExpression left, AleaExpression right)
        {
            Left = left;
            Right = right;
        }
    }
}
