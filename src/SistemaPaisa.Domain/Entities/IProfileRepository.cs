namespace SistemaPaisa.Domain.Entities;

public interface IProfileRepository
{
    Task<IReadOnlyList<Profile>> GetAllActiveAsync();
    Task<IEnumerable<Profile>> GetAllAsync();
    Task<Profile?> GetByIdAsync(int id);
    Task<Profile> AddAsync(Profile profile, IEnumerable<int> moduleIds, IEnumerable<int> actionIds);
    Task UpdateAsync(Profile profile, IEnumerable<int> moduleIds, IEnumerable<int> actionIds);
    Task DeleteAsync(int id);
}
