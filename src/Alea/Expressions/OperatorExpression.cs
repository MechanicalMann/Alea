using System;
using System.Collections.Generic;
using System.Text;

namespace Alea.Expressions
{
    public abstract class OperatorExpression : BinaryExpression
    {
        protected virtual Func<AleaExpression, AleaExpression, double> Evaluator { get; }

        public OperatorExpression(AleaExpression left, AleaExpression right, Func<AleaExpression, AleaExpression, double> evaluator)
            : base(left, right)
        {
            Evaluator = evaluator;
        }

        public override double GetValue()
        {
            return Evaluator(Left, Right);
        }
    }
}
