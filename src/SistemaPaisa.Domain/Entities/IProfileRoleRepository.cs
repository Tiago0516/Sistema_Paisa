namespace SistemaPaisa.Domain.Entities;

public interface IProfileRoleRepository
{
    Task<ProfileRoleInfo?> GetByRoleIdAsync(
        int roleId,
        CancellationToken cancellationToken = default);

    Task<int?> GetProfileIdByRoleIdAsync(int roleId, CancellationToken cancellationToken = default);

    Task ReplaceForRoleAsync(
        int roleId,
        int profileId,
        int userId,
        CancellationToken cancellationToken = default);
}

public class ProfileRoleInfo
{
    public int RelationId { get; init; }
    public int RoleId { get; init; }
    public string RoleName { get; init; } = string.Empty;
    public int ProfileId { get; init; }
    public string ProfileName { get; init; } = string.Empty;
}
