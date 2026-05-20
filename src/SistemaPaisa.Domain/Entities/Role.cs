namespace SistemaPaisa.Domain.Entities;

public class Role : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ClientId { get; set; }
    public ICollection<ProfileRole> ProfileRoles { get; set; } = [];
    public Client Client { get; set; } = null!;
    public ICollection<User> Users { get; set; } = [];
}
