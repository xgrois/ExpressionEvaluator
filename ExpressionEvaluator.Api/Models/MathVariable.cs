namespace ExpressionEvaluator.Api.Models;

/*
public class MathVariable
{
    public string Name { get; set; } = default!;
    public int Value { get; set; }
}
*/

public record MathVariable(string Name, int Value);