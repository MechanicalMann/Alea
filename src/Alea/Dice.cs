using System;
using System.Collections.Generic;
using System.Text;
using Alea.Expressions;
using Alea.Parsing;

namespace Alea
{
    /// <summary>
    /// A static class that provides access to dice notation evaluation and
    /// expression parsing functions.
    /// </summary>
    public static class Dice
    {
        /// <summary>
        /// Evaluate the given dice notation expression.
        /// </summary>
        /// <param name="dice">
        /// A string containing a dice notation expression.
        /// </param>
        /// <returns>
        /// The value of the expression after all dice have been rolled.
        /// </returns>
        public static double Roll(string dice)
        {
            return ExpressionFor(dice).Evaluate();
        }

        /// <summary>
        /// Evaluate the given dice notation expression, using the provided
        /// random number generator to provide random values for dice rolls.
        /// </summary>
        /// <param name="dice">
        /// A string containing a dice notation expression.
        /// </param>
        /// <param name="rng">
        /// A random number generator that will be used in all dice rolls.
        /// </param>
        /// <returns>
        /// The value of the expression after all dice have been rolled.
        /// </returns>
        public static double Roll(string dice, Random rng)
        {
            return ExpressionFor(dice, rng).Evaluate();
        }

        /// <summary>
        /// Parse the given dice notation expression into an expression tree.
        /// </summary>
        /// <param name="expr">
        /// A string containing a dice notation expression.
        /// </param>
        /// <returns>
        /// The parsed expression tree representing the input dice notation.
        /// </returns>
        public static AleaExpression ExpressionFor(string expr)
        {
            return Parser.Parse(expr);
        }

        /// <summary>
        /// Parse the given dice notation expression into an expression tree,
        /// using the provided random number generator to provide random values
        /// for all dice rolls.
        /// </summary>
        /// <param name="expr">
        /// A string containing a dice notation expression.
        /// </param>
        /// <param name="rng">
        /// A random number generator that will be used in all dice rolls.
        /// </param>
        /// <returns>
        /// The parsed expression tree representing the input dice notation.
        /// </returns>
        public static AleaExpression ExpressionFor(string expr, Random rng)
        {
            return Parser.Parse(expr, rng);
        }
    }
}
