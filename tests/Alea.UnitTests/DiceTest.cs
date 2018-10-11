using System;
using Xunit;
using Alea;
using Alea.Expressions;

namespace Alea.UnitTests
{
    public class DiceTest
    {
        [Fact]
        public void ShouldRollADie()
        {
            var result = Dice.Roll("1d6");
            Assert.InRange(result, 1, 6);
        }

        [Fact]
        public void ShouldRollADieWithRNG()
        {
            var rng = new Random(1);
            var result = Dice.Roll("1d6", rng);
            Assert.Equal(2, result);
        }

        [Fact]
        public void ShouldGetExpression()
        {
            var expr = Dice.ExpressionFor("1d6");
            Assert.IsAssignableFrom<DiceExpression>(expr);
        }

        [Fact]
        public void ShouldGetExpressionWithRNG()
        {
            var expr = Dice.ExpressionFor("1d6", new Random(1));
            Assert.IsAssignableFrom<DiceExpression>(expr);
            var result = expr.Evaluate();
            Assert.Equal(2, result);
        }
    }
}
