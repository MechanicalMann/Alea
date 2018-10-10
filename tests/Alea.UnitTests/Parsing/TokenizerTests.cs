using System;
using System.Collections.Generic;
using System.Text;
using Alea.Exceptions;
using Xunit;
using Alea.Parsing;

namespace Alea.UnitTests.Parsing
{
    public class TokenizerTests
    {
        [Fact]
        public void ShouldHandleEmptyString()
        {
            var tokenizer = new Tokenizer("");
            Assert.Equal(TokenType.EOF, tokenizer.CurrentToken.Type);
        }

        [Fact]
        public void ShouldThrowForNullInput()
        {
            Assert.Throws<ArgumentNullException>(() => new Tokenizer(null));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1.25)]
        [InlineData(-234987234.238472398472093847)]
        public void ShouldTokenizeNumber(double number)
        {
            var s = number.ToString();
            var token = new Tokenizer(s).CurrentToken;
            Assert.Equal(TokenType.Constant, token.Type);
            Assert.Equal(s, token.Value);
        }

        [Fact]
        public void ShouldTokenizePercentile()
        {
            var token = new Tokenizer("%").CurrentToken;
            Assert.Equal(TokenType.Constant, token.Type);
            Assert.Equal("100", token.Value);
        }

        [Theory]
        [InlineData("d", TokenType.Dice)]
        [InlineData("D", TokenType.Dice)]
        [InlineData("h", TokenType.TakeHigh)]
        [InlineData("H", TokenType.TakeHigh)]
        [InlineData("l", TokenType.TakeLow)]
        [InlineData("L", TokenType.TakeLow)]
        [InlineData("(", TokenType.ParenOpen)]
        [InlineData(")", TokenType.ParenClose)]
        [InlineData("*", TokenType.OpMultiply)]
        [InlineData("/", TokenType.OpDivide)]
        [InlineData("+", TokenType.OpAdd)]
        [InlineData("-", TokenType.OpSubtract)]
        internal void ShouldTokenizeDice(string input, TokenType expectedType)
        {
            var token = new Tokenizer(input).CurrentToken;
            Assert.Equal(expectedType, token.Type);
        }

        [Fact]
        public void ShouldThrowForInvalidCharacter()
        {
            Assert.Throws<ParseException>(() => new Tokenizer("Q"));
        }

        [Fact]
        public void ShouldSkipWhitespace()
        {
            var token = new Tokenizer("                 +").CurrentToken;
            Assert.Equal(TokenType.OpAdd, token.Type);
        }

        [Theory]
        [InlineData("+-")]
        [InlineData("+      -")]
        public void ShouldPeek(string input)
        {
            var tokenizer = new Tokenizer(input);
            Assert.Equal(TokenType.OpAdd, tokenizer.CurrentToken.Type);

            var peek = tokenizer.Peek();
            Assert.Equal(TokenType.OpSubtract, peek.Type);
            Assert.Equal(TokenType.OpAdd, tokenizer.CurrentToken.Type);
        }

        [Fact]
        public void ShouldPeekAtEOF()
        {
            var tokenizer = new Tokenizer("+");
            Assert.Equal(TokenType.EOF, tokenizer.Peek().Type);
        }
    }
}
