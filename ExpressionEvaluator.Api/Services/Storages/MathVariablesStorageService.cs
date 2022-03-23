using ExpressionEvaluator.Api.Models;
using System.Collections.Concurrent;

namespace ExpressionEvaluator.Api.Services.Storages;

public class MathVariablesStorageService : IMathVariablesStorageService
{
    private readonly ConcurrentDictionary<string, int> _variables = new();

    public Task CreateAsync(MathVariable mathVariable)
    {
        _variables[mathVariable.Name] = mathVariable.Value;
        return Task.CompletedTask;
    }

    public Task DeleteAllAsync()
    {
        _variables.Clear();
        return Task.CompletedTask;
    }

    public Task<bool> DeleteAsync(string mathVariableName)
    {
        bool wasDeleted = _variables.TryRemove(mathVariableName, out _);
        return Task.FromResult(wasDeleted);
    }

    public Task<IEnumerable<MathVariable>> GetAllAsync()
    {
        var variables = new List<MathVariable>();

        foreach (var kvp in _variables)
            variables.Add(new MathVariable(kvp.Key, kvp.Value));

        return Task.FromResult<IEnumerable<MathVariable>>(variables);
    }

    public Task<MathVariable?> GetByNameAsync(string mathVariableName)
    {
        var found = _variables.TryGetValue(mathVariableName, out int mathVariableValue);
        if (!found)
            return Task.FromResult<MathVariable?>(null);

        return Task.FromResult<MathVariable?>(new MathVariable(mathVariableName, mathVariableValue));
    }

    public Task<bool> UpdateAsync(MathVariable mathVariable)
    {
        if (!_variables.ContainsKey(mathVariable.Name))
            return Task.FromResult(false);

        _variables[mathVariable.Name] = mathVariable.Value;
        return Task.FromResult(true);
    }
}
