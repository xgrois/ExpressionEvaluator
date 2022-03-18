
var plus = new Token() { TokenType = TokenType.Sum, Value = "+" };
var minus = new Token() { TokenType = TokenType.Sub, Value = "-" };
var multi = new Token() { TokenType = TokenType.Mul, Value = "*" };
var div = new Token() { TokenType = TokenType.Div, Value = "/" };

Console.WriteLine("+ compTo - is " + plus.CompareTo(minus));
Console.WriteLine("- compTo + is " + minus.CompareTo(plus));

Console.WriteLine("+ compTo * is " + plus.CompareTo(multi));
Console.WriteLine("* compTo + is " + multi.CompareTo(plus));

Console.WriteLine("* compTo / is " + multi.CompareTo(div));
Console.WriteLine("/ compTo * is " + div.CompareTo(multi));

public class Token : IComparable<Token>
{
    public TokenType TokenType { get; set; }
    public dynamic? Value { get; set; }

    public int CompareTo(Token? y)
    {

        if ((this.TokenType == TokenType.Sum || this.TokenType == TokenType.Sub) &&
            (y!.TokenType == TokenType.Mul
            || y.TokenType == TokenType.Div
            || y.TokenType == TokenType.LeftParenthesis
            || y.TokenType == TokenType.RightParenthesis))
        {
            return 1;
        }

        if ((y!.TokenType == TokenType.Sum || y.TokenType == TokenType.Sub) &&
            (this.TokenType == TokenType.Mul
            || this.TokenType == TokenType.Div
            || this.TokenType == TokenType.LeftParenthesis
            || this.TokenType == TokenType.RightParenthesis))
        {
            return -1;
        }

        return 0;
    }

}

public enum TokenType
{
    Sum,
    Sub,
    Mul,
    Div,
    LeftParenthesis,
    RightParenthesis,
    Integer,
    EndOfExpression
}