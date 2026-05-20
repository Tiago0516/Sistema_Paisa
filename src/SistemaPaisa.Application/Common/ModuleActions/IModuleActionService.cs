using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Common.ModuleActions;

public interface IModuleActionService
{
    Task<IReadOnlyList<ModuleActionInfo>> GetByModuleIdAsync(
        int moduleId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ModuleActionInfo>> GetByModuleCodeAsync(
        string moduleCode,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ModuleActionInfo>> GetForCurrentUserByModuleCodeAsync(
        string moduleCode,
        CancellationToken cancellationToken = default);
}
