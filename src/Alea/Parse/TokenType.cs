using System;
using System.Collections.Generic;
using System.Text;

namespace Alea.Parse
{
    /// <summary>
    /// Represents the type of a token emitted by the tokenizer.
    /// </summary>
    /// <remarks>
    /// Token types are listed here in reverse order of precedence.
    /// </remarks>
    internal enum TokenType
    {
        EOF = 0,

        // A single dice notation expression
        Dice = 1,

        // Complex modifiers
        TakeHigh = 2,
        TakeLow = 3,

        // Grouping
        ParenOpen = 4,
        ParenClose = 5,

        // Operators
        OpMultiply = 6,
        OpDivide = 7,
        OpAdd = 8,
        OpSubtract = 9,

        // A numeric constant
        Constant = 10,
    }
}
