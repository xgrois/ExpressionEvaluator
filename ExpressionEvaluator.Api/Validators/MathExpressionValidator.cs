using ExpressionEvaluator.Api.Contracts;
using FluentValidation;

namespace ExpressionEvaluator.Api.Validators;

/// <summary>
/// Valid expression:
/// - numbers are positive integers (0, 1, 10, 123, ...)
/// - operators are +-*/^
/// - variable names can be
///     - 1 o more letters (x, phi, alpha, ....)
///     - 1 or more letters + _ + 1 or more letters (x_a, xx_a, x_aa, ...)
///     - cannot contain digits (x1, x_1, x1x are not valid)
/// - expression can be:
///     - var or num 
///     - followed by zero or more op and (var or num)
/// My regex with unit tests can be found:
/// https://regex101.com/r/Wt3bPM/1
/// </summary>
public class MathExpressionValidator : AbstractValidator<MathExpressionRequest>
{
    public MathExpressionValidator()
    {
        RuleFor(mathExpressionRequest => mathExpressionRequest.Expression)
            .Matches(@"^(\d+|(([a-z]+)|([a-z]+[_][a-z]+)))(([+\\-\\*\\/\\^])(\d+|(([a-z]+)|([a-z]+[_][a-z]+))))*$")
            .WithMessage("Expression is not valid!");
    }
}
