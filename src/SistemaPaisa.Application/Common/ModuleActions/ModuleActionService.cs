using SistemaPaisa.Application.Common.Interfaces;
using SistemaPaisa.Application.Common.Permissions;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Common.ModuleActions;

public class ModuleActionService : IModuleActionService
{
    private readonly IModuleActionRepository _moduleActionRepository;
    private readonly IPermissionService _permissionService;

    public ModuleActionService(
        IModuleActionRepository moduleActionRepository,
        IPermissionService permissionService)
    {
        _moduleActionRepository = moduleActionRepository;
        _permissionService = permissionService;
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
        var permissions = await _permissionService.GetCurrentPermissionsAsync(cancellationToken);
        if (permissions is null)
            return [];

        var module = permissions.Modules.FirstOrDefault(m =>
            string.Equals(m.Code, moduleCode, StringComparison.OrdinalIgnoreCase));

        if (module is null)
            return [];

        return module.Actions
            .Select(a => new ModuleActionInfo
            {
                Id = a.Id,
                Code = a.Code,
                Name = a.Name,
                ModuleId = module.Id,
                ModuleCode = module.Code
            })
            .ToList();
    }
}
