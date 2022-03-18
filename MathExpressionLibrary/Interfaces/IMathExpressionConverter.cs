using MathExpressionLibrary;

namespace MathExpression.Library.Interfaces;
public interface IMathExpressionConverter
{
    IEnumerable<Token> InfixToPostfix(List<Token> tokens);
}
