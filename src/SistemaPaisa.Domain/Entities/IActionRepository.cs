namespace SistemaPaisa.Domain.Entities;

public interface IActionRepository
{
    Task<IEnumerable<ActionEntity>> GetAllAsync();
    Task<ActionEntity?> GetByIdAsync(int id);
    Task<int?> GetModuleIdAsync(int actionId, CancellationToken cancellationToken = default);
    Task<ActionEntity> AddAsync(ActionEntity action, int moduleId, int userId = 1);
    Task UpdateAsync(ActionEntity action, int moduleId, int userId = 1);
    Task DeleteAsync(int id);
}
