using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alea.Exceptions;

namespace Alea.Expressions
{
    public class DiceExpression : BinaryExpression
    {
        private readonly Random _rand;

        public int Number { get; }
        public int Sides { get; }

        public DiceExpression(ConstantExpression number, ConstantExpression sides, Random random)
            : base(number, sides)
        {
            double n = number.Value, s = sides.Value;

            if (n % 1 != 0 || n < 1)
                throw new SemanticException("Dice can only be rolled a natural number of times");
            if (s % 1 != 0 || s < 2)
                throw new SemanticException("Dice must have a natural number of sides, at least 2 or higher"); // one-sided and fractional dice make no fucking sense

            Number = (int)n;
            Sides = (int)s;
            _rand = random;
        }

        public virtual IEnumerable<int> Roll()
        {
            for (int i = 0; i < Number; i++)
            {
                yield return _rand.Next(0, Sides) + 1;
            }
        }

        public override double GetValue()
        {
            return Roll().Sum();
        }
    }
}
