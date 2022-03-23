namespace ExpressionEvaluator.Api.Services.Preprocessors;

public interface IMathExpressionPreprocessorService
{
    public Task<string> AssignVariablesToMathExpressionAsync(string inputExpression);
}
