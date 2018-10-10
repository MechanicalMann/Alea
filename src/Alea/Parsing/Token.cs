using System;
using System.Linq;

namespace Alea.Parsing
{
    internal struct Token
    {
        private static readonly TokenType[] Operators = { TokenType.OpMultiply, TokenType.OpDivide, TokenType.OpAdd, TokenType.OpSubtract };
        private static readonly TokenType[] Dice = { TokenType.Dice, TokenType.TakeHigh, TokenType.TakeLow };

        public static Token EOF = new Token(TokenType.EOF, String.Empty);

        internal TokenType Type { get; }
        internal string Value { get; }

        internal int Precedence { get; }

        internal bool IsOperator { get; }
        internal bool IsDice { get; }

        public Token(TokenType type, string value = "")
        {
            Type = type;
            Value = value;
            Precedence = 10 - (int)Type;
            IsOperator = Operators.Contains(Type); // I am very lazy
            IsDice = Dice.Contains(Type);
        }
    }
}
