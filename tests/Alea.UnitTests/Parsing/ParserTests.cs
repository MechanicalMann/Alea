using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Alea.Exceptions;
using Alea.Expressions;
using Alea.Parsing;

namespace Alea.UnitTests.Parsing
{
    public class ParserTests
    {
        private static Token Constant(double value)
        {
            return new Token(TokenType.Constant, value.ToString());
        }

        private static ConstantExpression ConstantNode(double value)
        {
            return new ConstantExpression(value);
        }

        #region GetNode

        [Fact]
        public void ShouldGetConstant()
        {
            var node = Parser.GetNode(Constant(1));
            Assert.IsAssignableFrom<ConstantExpression>(node);
        }

        [Theory]
        [InlineData(TokenType.OpAdd, typeof(AddExpression))]
        [InlineData(TokenType.OpSubtract, typeof(SubtractExpression))]
        [InlineData(TokenType.OpMultiply, typeof(MultiplyExpression))]
        [InlineData(TokenType.OpDivide, typeof(DivideExpression))]
        internal void ShouldGetOperator(TokenType type, Type nodeType)
        {
            var operands = new Stack<AleaExpression>(new[] { ConstantNode(1), ConstantNode(2) });
            var node = Parser.GetNode(new Token(type), operands);
            Assert.IsAssignableFrom(nodeType, node);
        }

        [Fact]
        public void ShouldThrowForTooFewOperatorOperands()
        {
            var token = new Token(TokenType.OpAdd);
            var operands = new Stack<AleaExpression>();
            Assert.Throws<SyntaxException>(() => Parser.GetNode(token, operands));
            operands.Push(ConstantNode(1));
            Assert.Throws<SyntaxException>(() => Parser.GetNode(token, operands));
        }

        [Theory]
        [InlineData(TokenType.TakeHigh)]
        internal void ShouldGetTakeDiceExpression(TokenType type)
        {
            var operands = new Stack<AleaExpression>(new AleaExpression[] { new DiceExpression(ConstantNode(1), ConstantNode(2), new Random()), ConstantNode(1) });
            var node = Parser.GetNode(new Token(type), operands);
            Assert.IsAssignableFrom<TakeDiceExpression>(node);
        }

        [Fact]
        public void ShouldThrowForTooFewTakeDiceOperands()
        {
            var token = new Token(TokenType.TakeHigh);
            var operands = new Stack<AleaExpression>();
            Assert.Throws<SyntaxException>(() => Parser.GetNode(token, operands));
            operands.Push(ConstantNode(1));
            Assert.Throws<SyntaxException>(() => Parser.GetNode(token, operands));
        }

        [Fact]
        public void ShouldThrowForTakeDiceInvalidConstParam()
        {
            var token = new Token(TokenType.TakeHigh);
            var operands = new Stack<AleaExpression>(new AleaExpression[] { null, null });
            Assert.Throws<SyntaxException>(() => Parser.GetNode(token, operands));
        }

        [Fact]
        public void ShouldThrowForTakeDiceInvalidDiceParam()
        {
            var token = new Token(TokenType.TakeHigh);
            var operands = new Stack<AleaExpression>(new AleaExpression[] { null, ConstantNode(1) });
            Assert.Throws<SyntaxException>(() => Parser.GetNode(token, operands));
        }

        [Fact]
        public void ShouldGetDiceExpression()
        {
            var operands = new Stack<AleaExpression>(new[] { ConstantNode(1), ConstantNode(2) });
            var node = Parser.GetNode(new Token(TokenType.Dice), operands);
            Assert.IsAssignableFrom<DiceExpression>(node);
        }

        [Fact]
        public void ShouldThrowForTooFewDiceOperands()
        {
            var token = new Token(TokenType.Dice);
            var operands = new Stack<AleaExpression>();
            Assert.Throws<SyntaxException>(() => Parser.GetNode(token, operands));
            operands.Push(ConstantNode(1));
            Assert.Throws<SyntaxException>(() => Parser.GetNode(token, operands));
        }

        [Fact]
        public void ShouldThrowForDiceInvalidSecondParam()
        {
            var token = new Token(TokenType.Dice);
            var operands = new Stack<AleaExpression>(new AleaExpression[] { null, null });
            Assert.Throws<SyntaxException>(() => Parser.GetNode(token, operands));
        }

        [Fact]
        public void ShouldThrowForDiceInvalidFirstParam()
        {
            var token = new Token(TokenType.Dice);
            var operands = new Stack<AleaExpression>(new AleaExpression[] { null, ConstantNode(1) });
            Assert.Throws<SyntaxException>(() => Parser.GetNode(token, operands));
        }

        [Fact]
        public void ShouldThrowForInvalidToken()
        {
            var token = new Token((TokenType)256, "invalid");
            Assert.Throws<SyntaxException>(() => Parser.GetNode(token));
        }

        #endregion

        #region Parse

        [Fact]
        public void ShouldParseSimpleExpression()
        {
            var expr = Parser.Parse("1");
            Assert.IsAssignableFrom<ConstantExpression>(expr);
        }

        [Fact]
        public void ShouldParseSimpleParentheses()
        {
            var expr = Parser.Parse("(1)");
            Assert.IsAssignableFrom<ConstantExpression>(expr);
        }

        [Fact]
        public void ShouldParseDiceExpression()
        {
            var expr = Parser.Parse("1d20");
            Assert.IsAssignableFrom<DiceExpression>(expr);
        }

        [Theory]
        [InlineData("1 + 1")]
        [InlineData("1 - 1")]
        [InlineData("1 * 1")]
        [InlineData("1 / 1")]
        public void ShouldParseBasicOperators(string s)
        {
            var expr = Parser.Parse(s);
            Assert.IsAssignableFrom<OperatorExpression>(expr);
        }

        [Theory]
        [InlineData("6d6H4", true, 4)]
        [InlineData("4d10L2", false, 2)]
        public void ShouldParseTakeDiceExpressions(string s, bool takeHigh, int taken)
        {
            var expr = Parser.Parse(s);
            Assert.IsAssignableFrom<TakeDiceExpression>(expr);

            var tde = (TakeDiceExpression)expr;
            Assert.Equal(takeHigh, tde.TakeHigh);
            Assert.Equal(taken, tde.Take);
        }

        [Fact]
        public void ShouldAddDefaultParamToDice()
        {
            var expr = Parser.Parse("d20");
            Assert.IsAssignableFrom<DiceExpression>(expr);

            var de = (DiceExpression)expr;
            Assert.Equal(1, de.Number);
            Assert.Equal(20, de.Sides);
        }

        [Fact]
        public void ShouldAddDefaultParamToTakeDice()
        {
            var expr = Parser.Parse("2d20H");
            Assert.IsAssignableFrom<TakeDiceExpression>(expr);

            var tde = (TakeDiceExpression)expr;
            Assert.Equal(1, tde.Take);
        }

        [Fact]
        public void ShouldParseComplexParentheses()
        {
            var expr = Parser.Parse("(((1+2)+3+(4+5))+6+(7+8)+9)+10");
            Assert.IsAssignableFrom<AddExpression>(expr);

            var val = expr.Evaluate();
            Assert.Equal(55, val);
        }

        [Theory]
        [InlineData("3 * 3 + 3", 12)]
        [InlineData("3 + 3 * 3", 12)]
        [InlineData("3 / 3 - 3", -2)]
        [InlineData("3 - 3 / 3",  2)]
        [InlineData("3 + 6 * (5 + 4) / 3 - 7",  14)]
        public void ShouldFollowOperatorPrecedence(string e, double result)
        {
            var expr = Parser.Parse(e);
            Assert.Equal(result, expr.Evaluate());
        }

        [Fact]
        public void ShouldThrowForNullRNG()
        {
            Assert.Throws<ArgumentNullException>(() => Parser.Parse("1", null));
        }

        [Fact]
        public void ShouldThrowForEmptyParentheses()
        {
            Assert.Throws<SyntaxException>(() => Parser.Parse("()"));
        }

        [Fact]
        public void ShouldThrowForMismatchedCloseParen()
        {
            Assert.Throws<SyntaxException>(() => Parser.Parse(")"));
        }

        [Fact]
        public void ShouldThrowForMismatchedOpenParen()
        {
            Assert.Throws<SyntaxException>(() => Parser.Parse("("));
        }

        [Fact]
        public void ShouldThrowForUnbalancedExpression()
        {
            Assert.Throws<SyntaxException>(() => Parser.Parse("1 2"));
        }

        #endregion
    }
}
