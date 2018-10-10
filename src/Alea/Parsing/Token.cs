using System;
using System.Linq;

namespace Alea.Parsing
{
    /// <summary>
    /// Represents a character or characters in a dice notation string that has
    /// semantic meaning.
    /// </summary>
    internal struct Token
    {
        private static readonly TokenType[] Operators = { TokenType.OpMultiply, TokenType.OpDivide, TokenType.OpAdd, TokenType.OpSubtract };
        private static readonly TokenType[] Dice = { TokenType.Dice, TokenType.TakeHigh, TokenType.TakeLow };

        /// <summary>
        /// An end-of-file token.
        /// </summary>
        public static Token EOF = new Token(TokenType.EOF, String.Empty);

        /// <summary>
        /// The type of the token.
        /// </summary>
        internal TokenType Type { get; }

        /// <summary>
        /// The string value of the token.
        /// </summary>
        internal string Value { get; }

        /// <summary>
        /// If this token is an operator, gets the precedence of the operator.
        /// </summary>
        internal int Precedence { get; }

        /// <summary>
        /// Whether or not this token represents an arithmetic operator.
        /// </summary>
        internal bool IsOperator { get; }

        /// <summary>
        /// Whether or not this token represents the rolling of dice.
        /// </summary>
        internal bool IsDice { get; }

        /// <summary>
        /// Initialize a new token with the specified type and string value.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
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
