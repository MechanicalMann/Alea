using System;
using System.Collections.Generic;
using System.Text;

namespace Alea.Expressions
{
    public class MultiplyExpression : OperatorExpression
    {
        public MultiplyExpression(AleaExpression left, AleaExpression right)
            : base(left, right, (a, b) => a.GetValue() * b.GetValue())
        {
        }
    }
}
