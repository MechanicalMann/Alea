using System;
using System.Collections.Generic;
using Alea.Exceptions;
using Alea.Expressions;

namespace Alea.Parsing
{
    public class Parser
    {
        private readonly Tokenizer _tokenizer;

        private ConstantExpression DefaultExpression => new ConstantExpression(1);

        public Parser(string expr)
            : this(new Tokenizer(expr))
        {
        }

        internal Parser(Tokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }

        public AleaExpression Parse()
        {
            return Parse(new Random());
        }

        public AleaExpression Parse(Random rng)
        {
            if (rng == null)
                throw new ArgumentNullException(nameof(rng));

            var operands = new Stack<AleaExpression>();
            var operators = new Stack<Token>();
            Token last = Token.EOF, cur = _tokenizer.CurrentToken;
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
                    if ((cur.Type == TokenType.TakeHigh || cur.Type == TokenType.TakeLow) && _tokenizer.Peek().Type != TokenType.Constant)
                        operands.Push(DefaultExpression);
                    operators.Push(cur);
                }
                last = cur;
                cur = _tokenizer.NextToken();
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

        private AleaExpression GetNode(Token op, Stack<AleaExpression> operands = null, Random rng = null)
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
