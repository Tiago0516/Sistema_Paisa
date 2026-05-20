using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Common.Permissions;

public interface IPermissionService
{
    Task<RolePermissionData?> GetCurrentPermissionsAsync(CancellationToken cancellationToken = default);
    Task<bool> HasActionAsync(string moduleCode, string actionCode, CancellationToken cancellationToken = default);
    Task<bool> CanAccessModuleAsync(string moduleCode, CancellationToken cancellationToken = default);
}
