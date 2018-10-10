using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Alea.Parsing;

namespace Alea.UnitTests.Parsing
{
    public class TokenTests
    {
        [Theory]
        [InlineData(TokenType.EOF, false, false, 10)]
        [InlineData(TokenType.Dice, true, false, 9)]
        [InlineData(TokenType.TakeHigh, true, false, 8)]
        [InlineData(TokenType.TakeLow, true, false, 7)]
        [InlineData(TokenType.ParenOpen, false, false, 6)]
        [InlineData(TokenType.ParenClose, false, false, 5)]
        [InlineData(TokenType.OpMultiply, false, true, 4)]
        [InlineData(TokenType.OpDivide, false, true, 3)]
        [InlineData(TokenType.OpAdd, false, true, 2)]
        [InlineData(TokenType.OpSubtract, false, true, 1)]
        [InlineData(TokenType.Constant, false, false, 0)]
        internal void ShouldCalculateProperties(TokenType type, bool isDice, bool isOperator, int precedence)
        {
            var token = new Token(type);
            Assert.Equal(isDice, token.IsDice);
            Assert.Equal(isOperator, token.IsOperator);
            Assert.Equal(precedence, token.Precedence);
        }
    }
}
