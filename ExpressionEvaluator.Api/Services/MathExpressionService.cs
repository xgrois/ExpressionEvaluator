using MathExpression.Library;
using MathExpression.Library.Exceptions;
using MathExpression.Library.Interfaces;

namespace ExpressionEvaluator.Api.Services;

public class MathExpressionService : IMathExpressionService
{
    private readonly IMathExpressionTokenizer _tokenizer;
    private readonly IMathExpressionConverter _converter;
    private readonly IMathExpressionEvaluator _evaluator;

    public MathExpressionService(IMathExpressionTokenizer tokenizer, IMathExpressionConverter converter, IMathExpressionEvaluator evaluator)
    {
        _tokenizer = tokenizer;
        _converter = converter;
        _evaluator = evaluator;
    }

    public double Eval(string expression)
    {

        // Tokenize (humans write infix notation)
        var tokens = new List<Token>();
        try
        {
            tokens = _tokenizer.Tokenize(expression);
        }
        catch (InvalidMathExpressionException)
        {
            throw;
        }


        // Sort tokens as postfix
        var tokensAsPostfix = _converter.InfixToPostfix(tokens);

        // Eval
        var result = _evaluator.Eval(tokensAsPostfix);

        return result;
    }

}
