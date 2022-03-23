using ExpressionEvaluator.Api.Models;

namespace ExpressionEvaluator.Api.Exceptions;

public class VariableNotFoundException : Exception
{
    public IEnumerable<MathVariable> ExistingVariables { get; }

    public VariableNotFoundException() { }

    public VariableNotFoundException(string message)
        : base(message) { }

    public VariableNotFoundException(string message, Exception inner)
        : base(message, inner) { }

    public VariableNotFoundException(string message, IEnumerable<MathVariable> existingVariables)
        : this(message)
    {
        ExistingVariables = existingVariables;
    }
}
