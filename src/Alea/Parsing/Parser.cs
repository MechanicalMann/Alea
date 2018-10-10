using System;
using System.Collections.Generic;
using Alea.Exceptions;
using Alea.Expressions;

namespace Alea.Parsing
{
    /// <summary>
    /// Handles the lexing and parsing of a dice notation expression into an
    /// expression tree that can be evaluated.
    /// </summary>
    public static class Parser
    {
        private static ConstantExpression DefaultExpression => new ConstantExpression(1);

        /// <summary>
        /// Parse the given dice notation expression into an expression tree.
        /// </summary>
        /// <param name="expr">
        /// A string containing the dice notation expression to be parsed.
        /// </param>
        /// <returns>
        /// An expression tree representing the given dice notation.
        /// </returns>
        public static AleaExpression Parse(string expr)
        {
            return Parse(new Tokenizer(expr), new Random());
        }

        /// <summary>
        /// Parse the given dice notation expression into an expression tree,
        /// using the given random number generator to provide values for all
        /// dice rolls.
        /// </summary>
        /// <param name="expr">
        /// A string containing the dice notation expression to be parsed.
        /// </param>
        /// <param name="rng">
        /// A random number generator that will be used to provide values for
        /// all dice rolls in the parsed expression.
        /// </param>
        /// <returns>
        /// An expression tree representing the given dice notation.
        /// </returns>
        public static AleaExpression Parse(string expr, Random rng)
        {
            return Parse(new Tokenizer(expr), rng);
        }

        internal static AleaExpression Parse(Tokenizer tok, Random rng)
        {
            if (rng == null)
                throw new ArgumentNullException(nameof(rng));

            var operands = new Stack<AleaExpression>();
            var operators = new Stack<Token>();

            Token last = Token.EOF,
                  cur = tok.CurrentToken;

            while (cur.Type != TokenType.EOF)
            {
                if (cur.Type == TokenType.Constant)
                {
                    operands.Push(GetNode(cur));
                }
                else if (cur.Type == TokenType.ParenOpen)
                {
                    operators.Push(cur);
                }
                else if (cur.Type == TokenType.ParenClose)
                {
                    if (last.Type == TokenType.ParenOpen)
                        throw new SyntaxException("empty parentheses (must contain an expression)");
                    while (operators.Count > 0 && operators.Peek().Type != TokenType.ParenOpen)
                    {
                        operands.Push(GetNode(operators.Pop(), operands, rng));
                    }
                    // If we reached the end without finding a corresponding (
                    if (operators.Count == 0)
                        throw new SyntaxException("mismatched parenthesis (close missing an open)");
                    // Otherwise pop the corresponding ( off the stack
                    operators.Pop();
                }
                else if (cur.IsOperator || cur.IsDice)
                {
                    // The left operand of a dice expression is optional with a default of 1
                    if (cur.Type == TokenType.Dice && last.Type != TokenType.Constant)
                        operands.Push(DefaultExpression);
                    // Resolve operators with higher precedence to the left of the expression
                    while (operators.Count > 0 && operators.Peek().Precedence > cur.Precedence && operators.Peek().Type != TokenType.ParenOpen)
                    {
                        operands.Push(GetNode(operators.Pop(), operands, rng));
                    }
                    // The right operand of a take high/low expression is also optional with a default of 1
                    if ((cur.Type == TokenType.TakeHigh || cur.Type == TokenType.TakeLow) && tok.Peek().Type != TokenType.Constant)
                        operands.Push(DefaultExpression);
                    operators.Push(cur);
                }
                last = cur;
                cur = tok.NextToken();
            }
            while (operators.Count > 0)
            {
                operands.Push(GetNode(operators.Pop(), operands, rng));
            }

            // By resolving all operators and tokens, there can only be one, the AST that we've built
            if (operands.Count != 1)
                throw new SyntaxException("missing operator");

            return operands.Pop();
        }

        internal static AleaExpression GetNode(Token op, Stack<AleaExpression> operands = null, Random rng = null)
        {
            if (op.Type == TokenType.Constant)
            {
                return new ConstantExpression(op);
            }

            if (op.IsOperator)
            {
                if (operands == null || operands.Count < 2)
                    throw new SyntaxException("operand expected");
                AleaExpression right = operands.Pop(), left = operands.Pop();

                if (op.Type == TokenType.OpAdd)
                    return new AddExpression(left, right);
                if (op.Type == TokenType.OpSubtract)
                    return new SubtractExpression(left, right);
                if (op.Type == TokenType.OpMultiply)
                    return new MultiplyExpression(left, right);
                if (op.Type == TokenType.OpDivide)
                    return new DivideExpression(left, right);
            }

            if (op.Type == TokenType.TakeHigh || op.Type == TokenType.TakeLow)
            {
                if (operands == null || operands.Count < 2)
                    throw new SyntaxException("invalid number of parameters for a take high/low expression");
                var right = operands.Pop() as ConstantExpression;
                if (right == null)
                    throw new SyntaxException("the second parameter of a take high/low expression must be a number");
                var left = operands.Pop() as DiceExpression;
                if (left == null)
                    throw new SyntaxException("a take high/low expression must operate on a dice notation expression");
                return new TakeDiceExpression(op.Type == TokenType.TakeHigh, left, right);
            }

            if (op.Type == TokenType.Dice)
            {
                if (operands == null || operands.Count < 2)
                    throw new SyntaxException("invalid number of parameters for a dice notation expression");
                var right = operands.Pop() as ConstantExpression;
                if (right == null)
                    throw new SyntaxException("the second parameter of a dice notation expression must be a number");
                var left = operands.Pop() as ConstantExpression;
                if (left == null)
                    throw new SyntaxException("the first parameter of a dice notation expression must be a number");
                return new DiceExpression(left, right, rng);
            }

            throw new SyntaxException($"unknown operator: {op.Value}");
        }
    }
}
