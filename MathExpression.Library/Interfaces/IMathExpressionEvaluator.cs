namespace MathExpression.Library.Interfaces;
public interface IMathExpressionEvaluator
{
    double Eval(IEnumerable<Token> tokens);
}
