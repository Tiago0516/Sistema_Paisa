namespace SistemaPaisa.Domain.Entities;

public interface IProfileModuleRepository
{
    Task<IReadOnlyList<ProfileModuleInfo>> GetByProfileIdAsync(
        int profileId,
        CancellationToken cancellationToken = default);

    Task ReplaceForProfileAsync(
        int profileId,
        IEnumerable<int> moduleIds,
        CancellationToken cancellationToken = default);
}

public class ProfileModuleInfo
{
    public int ModuleId { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string? Icon { get; init; }
    public string? ControllerName { get; init; }
    public string? CreateActionName { get; init; }
    public bool IsLanding { get; init; }
}
