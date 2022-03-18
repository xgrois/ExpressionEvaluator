using MathExpression.Library.Interfaces;

namespace MathExpression.Library;
public class MathExpressionConverter : IMathExpressionConverter
{
    public IEnumerable<Token> InfixToPostfix(List<Token> tokens)
    {
        var postfix = new List<Token>();
        var stack = new Stack<Token>();

        stack.Push(new Token() { TokenType = TokenType.LeftParenthesis, Value = "(" });
        tokens.Add(new Token() { TokenType = TokenType.RightParenthesis, Value = ")" });

        foreach (var token in tokens)
        {
            if (token.TokenType == TokenType.Integer)
            {
                postfix.Add(token);
            }
            else if (Helpers.Helpers.IsOperator(token))
            {
                // repeatedly pop from stack and add to postfix (same or higher precedence)
                // then push currrent token to stack
                while (Helpers.Helpers.IsOperator(stack.Peek()) && token.CompareTo(stack.Peek()) >= 0)
                {
                    postfix.Add(stack.Pop());
                }
                stack.Push(token);
            }
            else if (token.TokenType == TokenType.LeftParenthesis)
            {
                stack.Push(token);
            }
            else if (token.TokenType == TokenType.RightParenthesis)
            {
                // repeatedly pop from stack and add to postfix (until a LP is found)
                // remove LP from stack
                while (stack.Peek().TokenType != TokenType.LeftParenthesis)
                {
                    postfix.Add(stack.Pop());
                }
                stack.Pop();
            }
        }

        return postfix;
    }
}
