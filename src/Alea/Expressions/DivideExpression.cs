using System;
using System.Collections.Generic;
using System.Text;

namespace Alea.Expressions
{
    public class DivideExpression : OperatorExpression
    {
        public DivideExpression(AleaExpression left, AleaExpression right)
            : base(left, right, (a, b) => a.Evaluate() / b.Evaluate())
        {
        }
    }
}
