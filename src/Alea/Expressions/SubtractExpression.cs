using System;
using System.Collections.Generic;
using System.Text;

namespace Alea.Expressions
{
    public class SubtractExpression : OperatorExpression
    {
        public SubtractExpression(AleaExpression left, AleaExpression right)
            : base(left, right, (a, b) => a.Evaluate() - b.Evaluate())
        {
        }
    }
}
