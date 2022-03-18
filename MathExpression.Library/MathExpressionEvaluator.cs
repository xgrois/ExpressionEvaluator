using MathExpression.Library.Interfaces;

namespace MathExpression.Library;
public class MathExpressionEvaluator : IMathExpressionEvaluator
{
    // input tokens must follow postfix order
    public double Eval(IEnumerable<Token> tokens)
    {
        var stack = new Stack<double>();
        foreach (var token in tokens)
        {
            if (token.TokenType == TokenType.Integer)
            {
                // Push
                stack.Push(token.Value);
            }
            else if (Helpers.Helpers.IsOperator(token))
            {
                // Pop 2 numbers and eval
                var a = stack.Pop();
                var b = stack.Pop();
                var x = Helpers.Helpers.Eval(b, token.Value, a);
                stack.Push(x);
            }
        }
        return stack.Pop();
    }
}
