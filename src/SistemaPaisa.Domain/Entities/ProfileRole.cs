namespace SistemaPaisa.Domain.Entities;

public class ProfileRole : RelationAuditableEntity
{
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public int ProfileId { get; set; }
    public Profile Profile { get; set; } = null!;
}
