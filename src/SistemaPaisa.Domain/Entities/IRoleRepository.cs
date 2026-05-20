namespace SistemaPaisa.Domain.Entities;

public interface IRoleRepository
{
    Task<IReadOnlyList<Role>> GetActiveByClientIdAsync(int clientId);
    Task<Role?> GetActiveByIdAsync(int roleId);
    Task<IReadOnlyList<Role>> GetAllActiveWithClientAsync();
    Task<Role?> GetDefaultForRegistrationAsync();
    Task<IEnumerable<Role>> GetAllAsync();
    Task<Role?> GetByIdAsync(int id);
    Task<Role> AddAsync(Role role, int profileId, int userId = 1);
    Task UpdateAsync(Role role, int profileId, int userId = 1);
    Task DeleteAsync(int id);
}
