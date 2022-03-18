using MathExpressionLibrary;

namespace MathExpression.Library.Interfaces;

public interface IMathExpressionTokenizer
{
    public List<Token> Tokenize(string expression);
}