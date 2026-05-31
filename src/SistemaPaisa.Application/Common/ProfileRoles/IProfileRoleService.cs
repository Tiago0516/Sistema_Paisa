using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Common.ProfileRoles;

public interface IProfileRoleService
{
    Task<ProfileRoleInfo?> GetByRoleIdAsync(
        int roleId,
        CancellationToken cancellationToken = default);

    Task<ProfileRoleInfo?> GetForCurrentUserAsync(
        CancellationToken cancellationToken = default);
}
