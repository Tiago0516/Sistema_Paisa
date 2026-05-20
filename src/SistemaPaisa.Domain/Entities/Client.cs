namespace SistemaPaisa.Domain.Entities;

public class Client : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Identification { get; set; }
    public ICollection<Role> Roles { get; set; } = [];
}
