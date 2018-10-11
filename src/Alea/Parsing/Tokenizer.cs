using System;
using System.Collections.Generic;
using System.Text;
using Alea.Exceptions;

// Allow unit testing
[assembly:System.Runtime.CompilerServices.InternalsVisibleTo("Alea.UnitTests")]

namespace Alea.Parsing
{
    internal class Tokenizer
    {
        private char _char;
        private Token _token;

        private int _pos = -1;
        private int _last = Int32.MinValue;

        private readonly string _input;

        internal Token CurrentToken => _token;

        internal Tokenizer(string input)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));
            NextChar();
            NextToken();
        }

        private void NextChar(bool skipWhitespace = true)
        {
            do
            {
                _char = Read();
            } while (skipWhitespace && Char.IsWhiteSpace(_char));
        }

        private char Read()
        {
            return Read(++_pos);
        }

        private char Read(int pos)
        {
            if (pos >= _input.Length || pos < 0)
                return '\0';
            return _input[pos];
        }

        internal Token NextToken()
        {
            _token = GetToken();
            return _token;
        }

        public Token Peek()
        {
            if (_char <= 0)
                return new Token(TokenType.EOF);

            var prev = _last - 1;
            var peek = NextToken();
            _pos = prev;
            NextChar();
            NextToken();
            return peek;
        }

        private Token GetToken()
        {
            if (_char <= 0)
                return new Token(TokenType.EOF);
            
            _last = _pos;// Keep a history of the starting position of the last token read

            if (Char.IsDigit(_char) || (_char == '-' && Char.IsDigit(Read(_pos + 1))))
            {
                var sb = new StringBuilder();
                do
                {
                    sb.Append(_char);
                    NextChar(false);
                } while (Char.IsDigit(_char) || _char == '.');

                // Whitespace denotes the edge of a number
                if (Char.IsWhiteSpace(_char))
                    NextChar();

                return new Token(TokenType.Constant, sb.ToString());
            }
            switch (Char.ToLowerInvariant(_char))
            {
                case '%':
                    NextChar();
                    return new Token(TokenType.Constant, "100");
                case 'h':
                    NextChar();
                    return new Token(TokenType.TakeHigh, new string(new[] {_char}));
                case 'l':
                    NextChar();
                    return new Token(TokenType.TakeLow, new string(new[] { _char }));
                case 'd':
                    NextChar();
                    return new Token(TokenType.Dice, new string(new[] { _char }));
                case '(':
                    NextChar();
                    return new Token(TokenType.ParenOpen, "(");
                case ')':
                    NextChar();
                    return new Token(TokenType.ParenClose, ")");
                case '*':
                    NextChar();
                    return new Token(TokenType.OpMultiply, "*");
                case '/':
                    NextChar();
                    return new Token(TokenType.OpDivide, "/");
                case '+':
                    NextChar();
                    return new Token(TokenType.OpAdd, "+");
                case '-':
                    NextChar();
                    return new Token(TokenType.OpSubtract, "-");
                default:
                    throw new ParseException($"Unexpected character: {_char}");
            }
        }
    }
}
