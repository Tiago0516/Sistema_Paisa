namespace SistemaPaisa.Domain.Entities;

public interface IModuleRepository
{
    Task<IReadOnlyList<Module>> GetActiveWithActionsAsync();
    Task<IEnumerable<Module>> GetAllAsync();
    Task<Module?> GetByIdAsync(int id);
    Task<Module> AddAsync(Module module);
    Task UpdateAsync(Module module);
    Task DeleteAsync(int id);
}
