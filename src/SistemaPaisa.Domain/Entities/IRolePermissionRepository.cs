namespace SistemaPaisa.Domain.Entities;

public interface IRolePermissionRepository
{
    Task<RolePermissionData?> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken = default);
}

public class RolePermissionData
{
    public int RoleId { get; init; }
    public int ProfileId { get; init; }
    public int ProfileModuleId { get; init; }
    public IReadOnlyList<PermittedModuleData> Modules { get; init; } = [];
}

public class PermittedModuleData
{
    public int Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Icon { get; init; }
    public string? ControllerName { get; init; }
    public string? CreateActionName { get; init; }
    public bool IsLanding { get; init; }
    public IReadOnlyList<PermittedActionData> Actions { get; init; } = [];
}

public class PermittedActionData
{
    public int Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
}
