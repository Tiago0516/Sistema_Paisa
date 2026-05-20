namespace SistemaPaisa.Domain.Entities;

public class Module : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    public string? Icon { get; set; }

    public string? ControllerName { get; set; }

    public string? CreateActionName { get; set; }

    public bool IsLanding { get; set; }

    public ICollection<ActionModule> ActionModules { get; set; } = [];
    public ICollection<Profile> Profiles { get; set; } = [];
    public ICollection<ProfileModule> ProfileModules { get; set; } = [];
}
