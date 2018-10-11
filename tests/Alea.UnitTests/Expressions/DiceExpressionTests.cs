using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Alea.Exceptions;
using Alea.Expressions;

namespace Alea.UnitTests.Expressions
{
    public class DiceExpressionTests
    {
        private static ConstantExpression Constant(double value)
        {
            return new ConstantExpression(value);
        }

        private static DiceExpression Dice(double number, double sides, Random rng = null)
        {
            return new DiceExpression(Constant(number), Constant(sides), rng ?? new Random());
        }

        [Fact]
        public void ShouldRoll()
        {
            var expr = Dice(1, 6);
            Assert.InRange(expr.Evaluate(), 1, 6);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1.5)]
        [InlineData(1.000000000000001)]
        public void ShouldThrowForInvalidNumberOfDice(double number)
        {
            Assert.Throws<SemanticException>(() => Dice(number, 6));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(1.5)]
        [InlineData(1.000000000000001)]
        public void ShouldThrowForInvalidSides(double sides)
        {
            Assert.Throws<SemanticException>(() => Dice(1, sides));
        }
    }
}
