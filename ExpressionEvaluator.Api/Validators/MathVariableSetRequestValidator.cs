using ExpressionEvaluator.Api.Contracts;
using FluentValidation;

namespace ExpressionEvaluator.Api.Validators;

/// <summary>
/// Variable names can be
///     - 1 o more letters (x, phi, alpha, ....)
///     - 1 or more letters + _ + 1 or more letters (x_a, xx_a, x_aa, ...)
///     - cannot contain digits (x1, x_1, x1x are not valid)
/// </summary>
public class MathVariableSetRequestValidator : AbstractValidator<MathVariableSetRequest>
{
    public MathVariableSetRequestValidator()
    {
        RuleFor(mathVariableSetRequest => mathVariableSetRequest.Name)
            .NotEmpty()
            .WithMessage("Variable name is not defined")
            .Matches(@"^([a-z]+)([_][a-z]+)*$")
            .WithMessage("Variable name is not valid! Not allowed digits and/or consecutive underscores __");
    }
}
