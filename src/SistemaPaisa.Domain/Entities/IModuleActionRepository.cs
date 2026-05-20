namespace SistemaPaisa.Domain.Entities;

public interface IModuleActionRepository
{
    Task<IReadOnlyList<ModuleActionInfo>> GetByModuleIdAsync(
        int moduleId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ModuleActionInfo>> GetByModuleCodeAsync(
        string moduleCode,
        CancellationToken cancellationToken = default);
}

public class ModuleActionInfo
{
    public int Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public int ModuleId { get; init; }
    public string ModuleCode { get; init; } = string.Empty;
}
