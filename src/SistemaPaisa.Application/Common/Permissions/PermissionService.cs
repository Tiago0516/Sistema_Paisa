using SistemaPaisa.Application.Common.Interfaces;
using SistemaPaisa.Application.Common.ModuleActions;
using SistemaPaisa.Application.Common.ProfileModules;
using SistemaPaisa.Application.Common.ProfileRoles;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Common.Permissions;

public class PermissionService : IPermissionService
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
    private readonly IProfileRoleService _profileRoleService;
    private readonly IProfileModuleService _profileModuleService;
    private readonly IModuleActionService _moduleActionService;
    private RolePermissionData? _cached;

    public PermissionService(
        ICurrentUserService currentUser,
        IUserRepository userRepository,
        IProfileRoleService profileRoleService,
        IProfileModuleService profileModuleService,
        IModuleActionService moduleActionService)
    {
        _currentUser = currentUser;
        _userRepository = userRepository;
        _profileRoleService = profileRoleService;
        _profileModuleService = profileModuleService;
        _moduleActionService = moduleActionService;
    }

    public async Task<RolePermissionData?> GetCurrentPermissionsAsync(
        CancellationToken cancellationToken = default)
    {
        if (_cached is not null)
            return _cached;

        var roleId = await ResolveRoleIdAsync(cancellationToken);
        if (roleId is null)
            return null;

        var profileRole = await _profileRoleService.GetByRoleIdAsync(roleId.Value, cancellationToken);
        var profileModules = await _profileModuleService.GetForCurrentUserAsync(cancellationToken);

        var modules = new List<PermittedModuleData>();

        foreach (var profileModule in profileModules)
        {
            var actions = await _moduleActionService.GetForCurrentUserByModuleCodeAsync(
                profileModule.Code,
                cancellationToken);

            modules.Add(new PermittedModuleData
            {
                Id = profileModule.ModuleId,
                Code = profileModule.Code,
                Name = profileModule.Name,
                Icon = profileModule.Icon,
                ControllerName = profileModule.ControllerName,
                CreateActionName = profileModule.CreateActionName,
                IsLanding = profileModule.IsLanding,
                Actions = actions
                    .Select(a => new PermittedActionData
                    {
                        Id = a.Id,
                        Code = a.Code,
                        Name = a.Name
                    })
                    .ToList()
            });
        }

        _cached = new RolePermissionData
        {
            RoleId = roleId.Value,
            ProfileId = profileRole?.ProfileId ?? 0,
            ProfileModuleId = profileModules.FirstOrDefault()?.ModuleId ?? 0,
            Modules = modules
        };

        return _cached;
    }

    public async Task<bool> HasActionAsync(
        string moduleCode,
        string actionCode,
        CancellationToken cancellationToken = default)
    {
        var permissions = await GetCurrentPermissionsAsync(cancellationToken);
        if (permissions is null) return false;

        if (permissions.RoleId == AdminRoleId)
            return true;

        if (permissions.RoleId == InventoryRoleId)
            return InventoryModuleCodes.Contains(moduleCode);

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

        if (permissions.RoleId == AdminRoleId)
            return true;

        if (permissions.RoleId == InventoryRoleId)
            return InventoryModuleCodes.Contains(moduleCode);

        return permissions.Modules.Any(m =>
            string.Equals(m.Code, moduleCode, StringComparison.OrdinalIgnoreCase));
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
