using SistemaPaisa.Application.Common.Interfaces;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Common.Permissions;

public class PermissionService : IPermissionService
{
    private readonly ICurrentUserService _currentUser;
    private readonly IUserRepository _userRepository;
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private RolePermissionData? _cached;

    public PermissionService(
        ICurrentUserService currentUser,
        IUserRepository userRepository,
        IRolePermissionRepository rolePermissionRepository)
    {
        _currentUser = currentUser;
        _userRepository = userRepository;
        _rolePermissionRepository = rolePermissionRepository;
    }

    public async Task<RolePermissionData?> GetCurrentPermissionsAsync(
        CancellationToken cancellationToken = default)
    {
        if (_cached is not null)
            return _cached;

        if (_currentUser.UserId is not int userId)
            return null;

        var user = await _userRepository.GetActiveWithRoleByIdAsync(userId, cancellationToken);
        if (user is null)
            return null;

        _cached = await _rolePermissionRepository.GetByRoleIdAsync(user.RoleId, cancellationToken);
        return _cached;
    }

    public async Task<bool> HasActionAsync(
        string moduleCode,
        string actionCode,
        CancellationToken cancellationToken = default)
    {
        var permissions = await GetCurrentPermissionsAsync(cancellationToken);
        if (permissions is null) return false;

        var module = permissions.Modules.FirstOrDefault(m =>
            string.Equals(m.Code, moduleCode, StringComparison.OrdinalIgnoreCase));

        return module?.Actions.Any(a =>
            string.Equals(a.Code, actionCode, StringComparison.OrdinalIgnoreCase)) == true;
    }

    public async Task<bool> CanAccessModuleAsync(
        string moduleCode,
        CancellationToken cancellationToken = default)
    {
        var permissions = await GetCurrentPermissionsAsync(cancellationToken);
        if (permissions is null) return false;

        return permissions.Modules.Any(m =>
            string.Equals(m.Code, moduleCode, StringComparison.OrdinalIgnoreCase));
    }
}
