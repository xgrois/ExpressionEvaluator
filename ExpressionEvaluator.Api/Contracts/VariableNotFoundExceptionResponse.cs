using ExpressionEvaluator.Api.Models;

namespace ExpressionEvaluator.Api.Contracts;

public record VariableNotFoundExceptionResponse(string Message, IEnumerable<MathVariable> Variables);
