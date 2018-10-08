using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alea.Exceptions;

namespace Alea.Expressions
{
    public class TakeDiceExpression : BinaryExpression
    {
        public DiceExpression Dice => Left as DiceExpression;
        public bool TakeHigh { get; }
        public int Take { get; }

        public TakeDiceExpression(bool takeHigh, DiceExpression dice, ConstantExpression take)
            : base(dice, take)
        {
            var t = take.Value;
            if (t % 1 != 0 || t < 1)
                throw new SemanticException("The number of rolls taken must be a natural number");
            if (t > dice.Number)
                throw new SemanticException("The number of rolls taken cannot be greater than the total number of rolls");
            Take = (int)t;
            TakeHigh = takeHigh;
        }

        public override double GetValue()
        {
            var r = Dice.Roll();
            if (TakeHigh)
                r = r.OrderByDescending(x => x);
            else
                r = r.OrderBy(x => x);
            return r.Take(Take).Sum();
        }
    }
}
