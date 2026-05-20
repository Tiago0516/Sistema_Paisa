namespace SistemaPaisa.Domain.Entities;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task<User> AddAsync(User user);
    Task<IEnumerable<User>> GetAllWithRoleAsync();
    Task<User?> GetByIdAsync(int id);

    Task<User?> GetActiveWithRoleByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task UpdateRoleAsync(int userId, int roleId, CancellationToken cancellationToken = default);
}
