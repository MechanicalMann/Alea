using System;
using System.Collections.Generic;
using System.Text;
using Alea.Expressions;

namespace Alea
{
    public class Dice
    {
        public static double Roll(string diceExpression)
        {
            return ExpressionFor(diceExpression).GetValue();
        }

        public static double Roll(string diceExpression, Random rng)
        {
            return ExpressionFor(diceExpression, rng).GetValue();
        }

        public static AleaExpression ExpressionFor(string expr)
        {
            return new AleaExpressionBuilder(expr).Build();
        }

        public static AleaExpression ExpressionFor(string expr, Random rng)
        {
            return new AleaExpressionBuilder(expr).Build(rng);
        }
    }
}
