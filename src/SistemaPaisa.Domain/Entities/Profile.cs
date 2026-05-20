namespace SistemaPaisa.Domain.Entities;

public class Profile : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ModuleId { get; set; }
    public Module Module { get; set; } = null!;
    public ICollection<ProfileModule> ProfileModules { get; set; } = [];
    public ICollection<ProfileAction> ProfileActions { get; set; } = [];
    public ICollection<ProfileRole> ProfileRoles { get; set; } = [];
}
