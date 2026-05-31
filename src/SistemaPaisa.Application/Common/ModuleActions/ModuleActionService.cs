using SistemaPaisa.Application.Common.Interfaces;
using SistemaPaisa.Application.Common.ProfileModules;
using SistemaPaisa.Application.Common.ProfileRoles;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Common.ModuleActions;

public class ModuleActionService : IModuleActionService
{
    private const int AdminRoleId = 1;
    private const int InventoryRoleId = 2;

    private readonly ICurrentUserService _currentUser;
    private readonly IUserRepository _userRepository;
    private readonly IModuleActionRepository _moduleActionRepository;
    private readonly IProfileModuleService _profileModuleService;
    private readonly IProfileRoleService _profileRoleService;
    private readonly IProfileRepository _profileRepository;

    public ModuleActionService(
        ICurrentUserService currentUser,
        IUserRepository userRepository,
        IModuleActionRepository moduleActionRepository,
        IProfileModuleService profileModuleService,
        IProfileRoleService profileRoleService,
        IProfileRepository profileRepository)
    {
        _currentUser = currentUser;
        _userRepository = userRepository;
        _moduleActionRepository = moduleActionRepository;
        _profileModuleService = profileModuleService;
        _profileRoleService = profileRoleService;
        _profileRepository = profileRepository;
    }

    public Task<IReadOnlyList<ModuleActionInfo>> GetByModuleIdAsync(
        int moduleId,
        CancellationToken cancellationToken = default) =>
        _moduleActionRepository.GetByModuleIdAsync(moduleId, cancellationToken);

    public Task<IReadOnlyList<ModuleActionInfo>> GetByModuleCodeAsync(
        string moduleCode,
        CancellationToken cancellationToken = default) =>
        _moduleActionRepository.GetByModuleCodeAsync(moduleCode, cancellationToken);

    public async Task<IReadOnlyList<ModuleActionInfo>> GetForCurrentUserByModuleCodeAsync(
        string moduleCode,
        CancellationToken cancellationToken = default)
    {
        var userModules = await _profileModuleService.GetForCurrentUserAsync(cancellationToken);
        var module = userModules.FirstOrDefault(m =>
            string.Equals(m.Code, moduleCode, StringComparison.OrdinalIgnoreCase));

        if (module is null)
            return [];

        var catalogActions = await _moduleActionRepository.GetByModuleIdAsync(
            module.ModuleId,
            cancellationToken);

        var roleId = await ResolveRoleIdAsync(cancellationToken);
        if (roleId is AdminRoleId or InventoryRoleId)
            return catalogActions;

        var profileRole = await _profileRoleService.GetForCurrentUserAsync(cancellationToken);
        if (profileRole is null)
            return [];

        var profile = await _profileRepository.GetByIdAsync(profileRole.ProfileId);
        if (profile is null)
            return [];

        var permittedActionIds = profile.ProfileActions
            .Where(pa => pa.Action.IsActive)
            .Select(pa => pa.ActionId)
            .ToHashSet();

        if (permittedActionIds.Count == 0)
            return catalogActions;

        return catalogActions
            .Where(a => permittedActionIds.Contains(a.Id))
            .ToList();
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
