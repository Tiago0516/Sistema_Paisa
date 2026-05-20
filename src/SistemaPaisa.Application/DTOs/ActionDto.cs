namespace SistemaPaisa.Application.DTOs;

public class ActionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int ModuleId { get; set; }
    public string ModuleName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
