using ExpressionEvaluator.Api.Models;

namespace ExpressionEvaluator.Api.Services.Storages;

public interface IMathVariablesStorageService
{
    public Task<IEnumerable<MathVariable>> GetAllAsync();
    public Task<MathVariable?> GetByNameAsync(string mathVariableName);
    public Task CreateAsync(MathVariable mathVariable);
    public Task<bool> UpdateAsync(MathVariable mathVariable);
    public Task<bool> DeleteAsync(string mathVariableName);

}
