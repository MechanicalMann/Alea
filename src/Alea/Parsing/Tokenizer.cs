using System;
using System.Collections.Generic;
using System.Text;
using Alea.Exceptions;

namespace Alea.Parsing
{
    internal class Tokenizer
    {
        private char _char;
        private Token _token;

        private int _pos = -1;

        private readonly string _input;

        internal Token CurrentToken => _token;

        internal Tokenizer(string input)
        {
            _input = input;
            NextChar();
            NextToken();
        }

        private void NextChar()
        {
            do
            {
                _char = Read();
            } while (Char.IsWhiteSpace(_char));
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

        internal IEnumerable<Token> Tokenize()
        {
            do
            {
                yield return _token;
            } while (_token.Type != TokenType.EOF);
        }

        internal Token NextToken()
        {
            _token = GetToken();
            return _token;
        }

        public Token Peek()
        {
            var pos = _pos--;
            var peek = NextToken();
            _pos = pos;
            NextChar();
            NextToken();
            return peek;
        }

        private Token GetToken()
        {
            if (_char <= 0)
                return new Token(TokenType.EOF);
            if (Char.IsDigit(_char) || (_char == '-' && Char.IsDigit(Read(_pos + 1))))
            {
                var sb = new StringBuilder();
                do
                {
                    sb.Append(_char);
                    NextChar();
                } while (Char.IsDigit(_char) || _char == '.');
                return new Token(TokenType.Constant, sb.ToString());
            }
            switch (Char.ToLowerInvariant(_char))
            {
                case '%':
                    NextChar();
                    return new Token(TokenType.Constant, "100");
                case 'h':
                    NextChar();
                    return new Token(TokenType.TakeHigh);
                case 'l':
                    NextChar();
                    return new Token(TokenType.TakeLow);
                case 'd':
                    NextChar();
                    return new Token(TokenType.Dice);
                case '(':
                    NextChar();
                    return new Token(TokenType.ParenOpen);
                case ')':
                    NextChar();
                    return new Token(TokenType.ParenClose);
                case '*':
                    NextChar();
                    return new Token(TokenType.OpMultiply);
                case '/':
                    NextChar();
                    return new Token(TokenType.OpDivide);
                case '+':
                    NextChar();
                    return new Token(TokenType.OpAdd);
                case '-':
                    NextChar();
                    return new Token(TokenType.OpSubtract);
                default:
                    throw new ParseException($"Unexpected character: {_char}");
            }
        }
    }
}
