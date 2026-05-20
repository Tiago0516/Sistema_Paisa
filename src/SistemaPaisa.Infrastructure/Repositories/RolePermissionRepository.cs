using Microsoft.EntityFrameworkCore;
using SistemaPaisa.Domain.Entities;
using SistemaPaisa.Infrastructure.Persistence;

namespace SistemaPaisa.Infrastructure.Repositories;

public class RolePermissionRepository : IRolePermissionRepository
{
    private readonly AppDbContext _context;
    private readonly IModuleActionRepository _moduleActionRepository;
    private readonly IProfileModuleRepository _profileModuleRepository;
    private readonly IProfileRoleRepository _profileRoleRepository;

    public RolePermissionRepository(
        AppDbContext context,
        IModuleActionRepository moduleActionRepository,
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
        if (profileId is null)
        if (profile is null)
            return null;
            .FirstOrDefaultAsync(p => p.Id == profileId && p.IsActive, cancellationToken);
        var profileModules = await _profileModuleRepository.GetByProfileIdAsync(
            profile.Id,
            cancellationToken);

        if (profileModules.Count == 0 && profile.ModuleId > 0)
        {
            var fallback = await _context.Modules
                .AsNoTracking()
                .Where(m => m.Id == profile.ModuleId && m.IsActive)
                .Select(m => new ProfileModuleInfo
                {
                    ModuleId = m.Id,
                    Code = m.Code,
                    Name = m.Name,
                    Description = m.Description
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (fallback is not null)
                profileModules = [fallback];
        }

        var permittedActions = ResolvePermittedActions(profile);

        if (profile is null)
            return null;

        var profileModules = await _profileModuleRepository.GetByProfileIdAsync(
            profile.Id,
            cancellationToken);

            var actions = catalogActions
                .Where(a => IsActionPermitted(permittedActions, a.Id))
                .Select(a => new PermittedActionData
                {
                    Id = a.Id,
                    Code = a.Code,
                    Name = a.Name
                })
                .ToList();

            if (actions.Count == 0)
                continue;
            ? profile.ProfileActions
                .Where(pa => pa.Action.IsActive)
                .Select(pa => pa.ActionId)
                .ToHashSet()
            : null;

                Actions = actions
            });
        }

            var catalogActions = await _moduleActionRepository.GetByModuleIdAsync(
                profileModule.ModuleId,
                cancellationToken);

            ProfileModuleId = profile.ModuleId,

            modules.Add(new PermittedModuleData
            {
                Id = profileModule.ModuleId,
    private static List<ActionEntity> ResolvePermittedActions(Profile profile)
                ControllerName = profileModule.ControllerName,
        if (profile.ProfileActions.Count > 0)
        {
            return profile.ProfileActions
                .Select(pa => pa.Action)
                .Where(a => a.IsActive)
                .ToList();
        }

        return [];
    }

    private static bool IsActionPermitted(IReadOnlyList<ActionEntity> permittedActions, int actionId) =>
        permittedActions.Count == 0 || permittedActions.Any(a => a.Id == actionId);
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
