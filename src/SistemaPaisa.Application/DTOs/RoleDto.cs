namespace SistemaPaisa.Application.DTOs;

public class RoleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ProfileId { get; set; }
    public string ProfileName { get; set; } = string.Empty;
    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
