namespace MathExpressionLibrary;
public enum TokenType
{
    Sum,
    Sub,
    Mul,
    Div,
    Exp,
    LeftParenthesis,
    RightParenthesis,
    Integer,
    EndOfExpression
}

// Maybe this is not the best way to do it
public class Token : IComparable<Token>
{
    public TokenType TokenType { get; set; }
    public dynamic? Value { get; set; }

    public int CompareTo(Token? y)
    {
        // +- < */^()
        if ((this.TokenType == TokenType.Sum || this.TokenType == TokenType.Sub) &&
            (y!.TokenType == TokenType.Mul
            || y.TokenType == TokenType.Div
            || y.TokenType == TokenType.Exp
            || y.TokenType == TokenType.LeftParenthesis
            || y.TokenType == TokenType.RightParenthesis))
        {
            return 1;
        }

        // */ < ^()
        if ((this.TokenType == TokenType.Mul || this.TokenType == TokenType.Div) &&
            (y!.TokenType == TokenType.Exp
            || y.TokenType == TokenType.LeftParenthesis
            || y.TokenType == TokenType.RightParenthesis))
        {
            return 1;
        }

        // */^() > +-
        if ((y!.TokenType == TokenType.Sum || y.TokenType == TokenType.Sub) &&
            (this.TokenType == TokenType.Mul
            || this.TokenType == TokenType.Div
            || this.TokenType == TokenType.Exp
            || this.TokenType == TokenType.LeftParenthesis
            || this.TokenType == TokenType.RightParenthesis))
        {
            return -1;
        }

        // ^() > */
        if ((y.TokenType == TokenType.Mul || y.TokenType == TokenType.Div) &&
            (this.TokenType == TokenType.Exp
            || this.TokenType == TokenType.LeftParenthesis
            || this.TokenType == TokenType.RightParenthesis))
        {
            return -1;
        }

        return 0;
    }
}
