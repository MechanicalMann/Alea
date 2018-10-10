using System;
using System.Collections.Generic;
using System.Text;
using Alea.Expressions;
using Alea.Parsing;

namespace Alea
{
    public class Dice
    {
        public static double Roll(string diceExpression)
        {
            return ExpressionFor(diceExpression).Evaluate();
        }

        public static double Roll(string diceExpression, Random rng)
        {
            return ExpressionFor(diceExpression, rng).Evaluate();
        }

        public static AleaExpression ExpressionFor(string expr)
        {
            return new Parser(expr).Parse();
        }

        public static AleaExpression ExpressionFor(string expr, Random rng)
        {
            return new Parser(expr).Parse(rng);
        }
    }
}
