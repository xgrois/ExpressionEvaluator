using ExpressionEvaluator.Api.Exceptions;
using ExpressionEvaluator.Api.Services.Storages;
using System.Text.RegularExpressions;

namespace ExpressionEvaluator.Api.Services.Preprocessors;

public class MathExpressionPreprocessorService : IMathExpressionPreprocessorService
{
    private readonly IMathVariablesStorageService _mathVariablesStorageService;

    public MathExpressionPreprocessorService(IMathVariablesStorageService mathVariablesStorageService)
    {
        _mathVariablesStorageService = mathVariablesStorageService;
    }
    public async Task<string> AssignVariablesToMathExpressionAsync(string expression)
    {

        // Read all x,y,... from expression
        string variablesPattern = @"[a-zA-Z_]+";
        Regex regex = new Regex(variablesPattern);
        var matches = regex.Matches(expression);

        // Verify that all x,y,... do exist already
        // If not, the user first needs to set the variable in another endpoint
        var storedVariables = await _mathVariablesStorageService.GetAllAsync();
        foreach (Match match in matches)
        {
            var variable = await _mathVariablesStorageService.GetByNameAsync(match.Value);
            if (variable is not null)
                expression = expression.Replace(match.Value, variable.Value.ToString());

            else
                throw new VariableNotFoundException(
                    message: $"No variable with name {match.Value} was found",
                    existingVariables: storedVariables.ToArray()
                );
        }

        return expression;
    }
}
