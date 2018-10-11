using System;
using System.Collections.Generic;
using System.Text;
using Alea.Expressions;
using Xunit;

namespace Alea.UnitTests.Expressions
{
    public class OperatorExpressionTests
    {
        private static ConstantExpression Constant(double value)
        {
            return new ConstantExpression(value);
        }

        [Fact]
        public void ShouldAdd()
        {
            var expr = new AddExpression(Constant(1), Constant(2));
            Assert.Equal(3, expr.Evaluate());
        }

        [Fact]
        public void ShouldSubtract()
        {
            var expr = new SubtractExpression(Constant(3), Constant(2));
            Assert.Equal(1, expr.Evaluate());
        }

        [Fact]
        public void ShouldMultiply()
        {
            var expr = new MultiplyExpression(Constant(2), Constant(2));
            Assert.Equal(4, expr.Evaluate());
        }

        [Fact]
        public void ShouldDivide()
        {
            var expr = new DivideExpression(Constant(4), Constant(2));
            Assert.Equal(2, expr.Evaluate());
        }
    }
}
