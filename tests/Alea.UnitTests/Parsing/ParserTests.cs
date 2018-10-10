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
    }
}
