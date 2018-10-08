using System;
using System.Collections.Generic;
using System.Text;

namespace Alea.Expressions
{
    /// <summary>
    /// Represents an expression in dice notation.
    /// </summary>
    public abstract class AleaExpression
    {
        /// <summary>
        /// Evaluate the dice notation expression.
        /// </summary>
        /// <returns>
        /// The final numeric value of the dice notation expression.
        /// </returns>
        /// <remarks>
        /// This does not preserve the rolls of any dice that are part of the
        /// expression. All dice will be rolled again on every call.
        /// </remarks>
        public abstract double GetValue();
    }
}
