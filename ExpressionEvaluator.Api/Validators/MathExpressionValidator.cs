using ExpressionEvaluator.Api.Contracts;
using FluentValidation;

namespace ExpressionEvaluator.Api.Validators;

public class MathExpressionValidator : AbstractValidator<MathExpressionRequest>
{
    public MathExpressionValidator()
    {
        RuleFor(mathExpressionRequest => mathExpressionRequest.Expression)
            .Matches(@"\d+([+-]\d+)*$")
            .WithMessage("Expression is not valid!");

    }
}
