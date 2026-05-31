using SistemaPaisa.Application.Common.Interfaces;
using SistemaPaisa.Application.Common.ProfileRoles;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Common.ProfileModules;

public class ProfileModuleService : IProfileModuleService
{
    private const int AdminRoleId = 1;
    private const int InventoryRoleId = 2;
    private static readonly HashSet<string> InventoryModuleCodes = new(StringComparer.OrdinalIgnoreCase)
    {
        "CATEGORIES",
        "PRODUCTS",
        "SUPPLIERS"
    };

    private readonly ICurrentUserService _currentUser;
    private readonly IUserRepository _userRepository;
    private readonly IProfileModuleRepository _profileModuleRepository;
    private readonly IProfileRoleService _profileRoleService;
    private readonly IModuleRepository _moduleRepository;

    public ProfileModuleService(
        ICurrentUserService currentUser,
        IUserRepository userRepository,
        IProfileModuleRepository profileModuleRepository,
        IProfileRoleService profileRoleService,
        IModuleRepository moduleRepository)
    {
        _currentUser = currentUser;
        _userRepository = userRepository;
        _profileModuleRepository = profileModuleRepository;
        _profileRoleService = profileRoleService;
        _moduleRepository = moduleRepository;
    }

    public Task<IReadOnlyList<ProfileModuleInfo>> GetByProfileIdAsync(
        int profileId,
        CancellationToken cancellationToken = default) =>
        _profileModuleRepository.GetByProfileIdAsync(profileId, cancellationToken);

    public async Task<IReadOnlyList<ProfileModuleInfo>> GetForCurrentUserAsync(
        CancellationToken cancellationToken = default)
    {
        var roleId = await ResolveRoleIdAsync(cancellationToken);
        if (roleId is null)
            return [];

        if (roleId is AdminRoleId or InventoryRoleId)
            return await GetCatalogModulesAsync(roleId.Value, cancellationToken);

        var profileRole = await _profileRoleService.GetByRoleIdAsync(roleId.Value, cancellationToken);
        if (profileRole is null)
            return [];

        return await _profileModuleRepository.GetByProfileIdAsync(
            profileRole.ProfileId,
            cancellationToken);
    }

    private async Task<IReadOnlyList<ProfileModuleInfo>> GetCatalogModulesAsync(
        int roleId,
        CancellationToken cancellationToken)
    {
        var catalogModules = await _moduleRepository.GetActiveWithActionsAsync();

        if (roleId == InventoryRoleId)
        {
            catalogModules = catalogModules
                .Where(m => InventoryModuleCodes.Contains(m.Code))
                .ToList();
        }

        return catalogModules
            .Select(m => new ProfileModuleInfo
            {
                ModuleId = m.Id,
                Code = m.Code,
                Name = m.Name,
                Description = m.Description,
                Icon = m.Icon,
                ControllerName = m.ControllerName,
                CreateActionName = m.CreateActionName,
                IsLanding = m.IsLanding
            })
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
