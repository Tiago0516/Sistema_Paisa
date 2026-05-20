namespace SistemaPaisa.Domain.Entities;

public class ProfileModule : RelationAuditableEntity
{
    public int ProfileId { get; set; }
    public Profile Profile { get; set; } = null!;
    public int ModuleId { get; set; }
    public Module Module { get; set; } = null!;
}
