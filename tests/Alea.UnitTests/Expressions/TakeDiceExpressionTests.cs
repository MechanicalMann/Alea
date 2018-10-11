using System;
using System.Collections.Generic;
using System.Text;
using Alea.Exceptions;
using Xunit;
using Alea.Expressions;

namespace Alea.UnitTests.Expressions
{
    public class TakeDiceExpressionTests
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
        public void ShouldTakeHigh()
        {
            var rng = new Random(1);
            var expr = new TakeDiceExpression(true, Dice(2, 6, rng), Constant(1));
            Assert.Equal(2, expr.Evaluate());
        }

        [Fact]
        public void ShouldTakeLow()
        {
            var rng = new Random(1);
            var expr = new TakeDiceExpression(false, Dice(2, 6, rng), Constant(1));
            Assert.Equal(1, expr.Evaluate());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1.5)]
        [InlineData(1.000000000000001)]
        public void ShouldThrowForInvalidRollsTaken(double take)
        {
            var dice = Dice(2, 6);
            Assert.Throws<SemanticException>(() => new TakeDiceExpression(true, dice, Constant(take)));
        }

        [Fact]
        public void ShouldThrowWhenTakenIsHigherThanNumberOfDice()
        {
            var dice = Dice(2, 6);
            Assert.Throws<SemanticException>(() => new TakeDiceExpression(true, dice, Constant(127)));
        }
    }
}
