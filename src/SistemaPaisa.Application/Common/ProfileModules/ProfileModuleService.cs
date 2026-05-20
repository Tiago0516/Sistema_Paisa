using SistemaPaisa.Application.Common.Permissions;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Common.ProfileModules;

public class ProfileModuleService : IProfileModuleService
{
    private readonly IProfileModuleRepository _profileModuleRepository;
    private readonly IPermissionService _permissionService;

    public ProfileModuleService(
        IProfileModuleRepository profileModuleRepository,
        IPermissionService permissionService)
    {
        _profileModuleRepository = profileModuleRepository;
        _permissionService = permissionService;
    }

    public Task<IReadOnlyList<ProfileModuleInfo>> GetByProfileIdAsync(
        int profileId,
        CancellationToken cancellationToken = default) =>
        _profileModuleRepository.GetByProfileIdAsync(profileId, cancellationToken);

    public async Task<IReadOnlyList<ProfileModuleInfo>> GetForCurrentUserAsync(
        CancellationToken cancellationToken = default)
    {
        var permissions = await _permissionService.GetCurrentPermissionsAsync(cancellationToken);
        if (permissions is null)
            return [];

        return permissions.Modules
            .Select(m => new ProfileModuleInfo
            {
                ModuleId = m.Id,
                Code = m.Code,
                Name = m.Name,
                Description = string.Empty,
                Icon = m.Icon,
                ControllerName = m.ControllerName,
                CreateActionName = m.CreateActionName,
                IsLanding = m.IsLanding
            })
            .ToList();
    }
}
