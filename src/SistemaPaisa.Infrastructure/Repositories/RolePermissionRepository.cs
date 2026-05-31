using Microsoft.EntityFrameworkCore;
using SistemaPaisa.Domain.Entities;
using SistemaPaisa.Infrastructure.Persistence;

namespace SistemaPaisa.Infrastructure.Repositories;

public class RolePermissionRepository : IRolePermissionRepository
{
    private const int AdminRoleId = 1;
    private const int InventoryRoleId = 2;
    private static readonly HashSet<string> InventoryModuleCodes = new(StringComparer.OrdinalIgnoreCase)
    {
        "CATEGORIES",
        "PRODUCTS"
    };

    private readonly AppDbContext _context;
    private readonly IModuleActionRepository _moduleActionRepository;
    private readonly IProfileModuleRepository _profileModuleRepository;
    private readonly IProfileRoleRepository _profileRoleRepository;

    public RolePermissionRepository(
        AppDbContext context,
        IModuleActionRepository moduleActionRepository,
        IProfileModuleRepository profileModuleRepository,
        IProfileRoleRepository profileRoleRepository)
    {
        _context = context;
        _moduleActionRepository = moduleActionRepository;
        _profileModuleRepository = profileModuleRepository;
        _profileRoleRepository = profileRoleRepository;
    }

    public async Task<RolePermissionData?> GetByRoleIdAsync(
        int roleId,
        CancellationToken cancellationToken = default)
    {
        var role = await _context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == roleId && r.IsActive, cancellationToken);

        if (role is null)
            return null;

        var profileId = await _profileRoleRepository.GetProfileIdByRoleIdAsync(roleId, cancellationToken);

        if (role.Id == AdminRoleId)
            return await GetCatalogPermissionsAsync(role.Id, profileId, null, cancellationToken);

        if (role.Id == InventoryRoleId)
            return await GetCatalogPermissionsAsync(role.Id, profileId, InventoryModuleCodes, cancellationToken);

        if (profileId is null)
            return null;

        var profile = await _context.Profiles
            .AsNoTracking()
            .Include(p => p.ProfileActions)
                .ThenInclude(pa => pa.Action)
            .FirstOrDefaultAsync(p => p.Id == profileId && p.IsActive, cancellationToken);

        if (profile is null)
            return null;

        var profileModules = await _profileModuleRepository.GetByProfileIdAsync(
            profile.Id,
            cancellationToken);

        var permittedActionIds = profile.ProfileActions.Count > 0
            ? profile.ProfileActions
                .Where(pa => pa.Action.IsActive)
                .Select(pa => pa.ActionId)
                .ToHashSet()
            : null;

        var modules = new List<PermittedModuleData>();

        foreach (var profileModule in profileModules)
        {
            var catalogActions = await _moduleActionRepository.GetByModuleIdAsync(
                profileModule.ModuleId,
                cancellationToken);

            var actions = ResolveModuleActions(catalogActions, permittedActionIds);

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
            });
        }

        return new RolePermissionData
        {
            RoleId = role.Id,
            ProfileId = profile.Id,
            ProfileModuleId = profileModules.FirstOrDefault()?.ModuleId ?? profile.ModuleId,
            Modules = modules
        };
    }

    private async Task<RolePermissionData> GetCatalogPermissionsAsync(
        int roleId,
        int? profileId,
        IReadOnlySet<string>? allowedModuleCodes,
        CancellationToken cancellationToken)
    {
        var catalogModules = await _context.Modules
            .AsNoTracking()
            .Where(m => m.IsActive)
            .OrderBy(m => m.Name)
            .ToListAsync(cancellationToken);

        if (allowedModuleCodes is not null)
        {
            catalogModules = catalogModules
                .Where(m => allowedModuleCodes.Contains(m.Code))
                .ToList();
        }

        var modules = new List<PermittedModuleData>();

        foreach (var module in catalogModules)
        {
            var catalogActions = await _moduleActionRepository.GetByModuleIdAsync(
                module.Id,
                cancellationToken);

            modules.Add(new PermittedModuleData
            {
                Id = module.Id,
                Code = module.Code,
                Name = module.Name,
                Icon = module.Icon,
                ControllerName = module.ControllerName,
                CreateActionName = module.CreateActionName,
                IsLanding = module.IsLanding,
                Actions = ResolveModuleActions(catalogActions, null)
            });
        }

        return new RolePermissionData
        {
            RoleId = roleId,
            ProfileId = profileId ?? 0,
            ProfileModuleId = modules.FirstOrDefault()?.Id ?? 0,
            Modules = modules
        };
    }

    private static List<PermittedActionData> ResolveModuleActions(
        IReadOnlyList<ModuleActionInfo> catalogActions,
        HashSet<int>? permittedActionIds)
    {
        IEnumerable<ModuleActionInfo> source = catalogActions;

        if (permittedActionIds is { Count: > 0 })
            source = catalogActions.Where(a => permittedActionIds.Contains(a.Id));

        return source
            .Select(a => new PermittedActionData
            {
                Id = a.Id,
                Code = a.Code,
                Name = a.Name
            })
            .ToList();
    }
}