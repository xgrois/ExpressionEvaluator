using MathExpression.Library.Exceptions;
using MathExpression.Library.Interfaces;

namespace MathExpression.Library;

public class MathExpressionTokenizer : IMathExpressionTokenizer
{
    public List<Token> Tokenize(string validMathExpression)
    {
        var tokens = new List<Token>();
        // "12+34-3"
        int L = validMathExpression.Length;

        int i = 0;


        while (i < L)
        {
            if (Char.IsDigit(validMathExpression[i]))
            {
                int numberOfDigits = 0;
                int number = 0;
                while (i < L && Char.IsDigit(validMathExpression[i]))
                {
                    int digit = (int)Char.GetNumericValue(validMathExpression, i);
                    if (numberOfDigits == 0)
                    {
                        number = digit;
                        numberOfDigits++;
                        i++;
                        continue;
                    }
                    number = number * 10 + digit;
                    numberOfDigits++;
                    i++;
                }
                tokens.Add(new Token() { TokenType = TokenType.Integer, Value = number });

            }
            else
            {

                switch (validMathExpression[i])
                {
                    case '+':
                        tokens.Add(new Token() { TokenType = TokenType.Sum, Value = "+" });
                        break;
                    case '-':
                        tokens.Add(new Token() { TokenType = TokenType.Sub, Value = "-" });
                        break;
                    case '*':
                        tokens.Add(new Token() { TokenType = TokenType.Mul, Value = "*" });
                        break;
                    case '/':
                        tokens.Add(new Token() { TokenType = TokenType.Div, Value = "/" });
                        break;
                    case '^':
                        tokens.Add(new Token() { TokenType = TokenType.Exp, Value = "^" });
                        break;
                    case '(':
                        tokens.Add(new Token() { TokenType = TokenType.LeftParenthesis, Value = "(" });
                        break;
                    case ')':
                        tokens.Add(new Token() { TokenType = TokenType.RightParenthesis, Value = ")" });
                        break;
                    case ' ':
                        break;
                    default:
                        throw new InvalidMathExpressionException($"Unknown operand {validMathExpression[i]}");
                }
                i++;
            }


        }

        //tokens.Add(new Token() { TokenType = TokenType.EndOfExpression, Value = null });

        return tokens;
    }
}
