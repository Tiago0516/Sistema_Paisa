using SistemaPaisa.Application.Common.Interfaces;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Common.ProfileRoles;

public class ProfileRoleService : IProfileRoleService
{
    private readonly ICurrentUserService _currentUser;
    private readonly IUserRepository _userRepository;
    private readonly IProfileRoleRepository _profileRoleRepository;

    public ProfileRoleService(
        ICurrentUserService currentUser,
        IUserRepository userRepository,
        IProfileRoleRepository profileRoleRepository)
    {
        _currentUser = currentUser;
        _userRepository = userRepository;
        _profileRoleRepository = profileRoleRepository;
    }

    public Task<ProfileRoleInfo?> GetByRoleIdAsync(
        int roleId,
        CancellationToken cancellationToken = default) =>
        _profileRoleRepository.GetByRoleIdAsync(roleId, cancellationToken);

    public async Task<ProfileRoleInfo?> GetForCurrentUserAsync(
        CancellationToken cancellationToken = default)
    {
        var roleId = await ResolveRoleIdAsync(cancellationToken);
        if (roleId is null)
            return null;

        return await GetByRoleIdAsync(roleId.Value, cancellationToken);
    }

    private async Task<int?> ResolveRoleIdAsync(CancellationToken cancellationToken)
    {
        if (_currentUser.RoleId is int roleId)
            return roleId;

        if (_currentUser.UserId is not int userId)
            return null;

        var user = await _userRepository.GetActiveWithRoleByIdAsync(userId, cancellationToken);
        return user?.RoleId;
    }
}
