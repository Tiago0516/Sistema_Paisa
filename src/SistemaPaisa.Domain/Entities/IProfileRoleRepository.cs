namespace SistemaPaisa.Domain.Entities;

public interface IProfileRoleRepository
{
    Task<int?> GetProfileIdByRoleIdAsync(int roleId, CancellationToken cancellationToken = default);

    Task ReplaceForRoleAsync(
        int roleId,
        int profileId,
        int userId,
        CancellationToken cancellationToken = default);
}
