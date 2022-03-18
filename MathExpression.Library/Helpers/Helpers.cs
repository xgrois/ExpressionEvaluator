namespace MathExpression.Library.Helpers;

public static class Helpers
{
    public static bool IsOperator(Token token)
    {
        return token.TokenType == TokenType.Sum
            || token.TokenType == TokenType.Sub
            || token.TokenType == TokenType.Mul
            || token.TokenType == TokenType.Div
            || token.TokenType == TokenType.Exp;
    }

    public static double Eval(double v1, dynamic value, double v2)
    {
        switch (value as string)
        {
            case "+":
                return v1 + v2;
            case "-":
                return v1 - v2;
            case "*":
                return v1 * v2;
            case "/":
                return v1 / v2;
            case "^":
                return Math.Pow(v1, v2);
            default:
                throw new Exception("Unknown operator. Valid operators are: +, -, *, /, ^");
        }

    }

}