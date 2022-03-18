namespace MathExpression.Library.Exceptions;
public class InvalidMathExpressionException : Exception
{
    public InvalidMathExpressionException() { }
    public InvalidMathExpressionException(string message) : base(message) { }
    public InvalidMathExpressionException(string message, Exception innerException) : base(message, innerException) { }
}
