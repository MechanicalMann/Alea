using System;
using System.Collections.Generic;
using System.Text;
using Alea.Parse;

namespace Alea.Expressions
{
    public class ConstantExpression : AleaExpression
    {
        public double Value { get; }

        internal ConstantExpression(Token token)
        {
            Value = Double.Parse(token.Value, System.Globalization.NumberStyles.Number);
        }

        public ConstantExpression(double value)
        {
            Value = value;
        }

        public override double GetValue()
        {
            return Value;
        }
    }
}
