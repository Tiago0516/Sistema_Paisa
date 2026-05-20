namespace SistemaPaisa.Application.DTOs;

public class ProfileDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ModuleId { get; set; }
    public string ModuleName { get; set; } = string.Empty;
    public IReadOnlyList<int> ModuleIds { get; set; } = [];
    public string ModulesSummary { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public IReadOnlyList<int> ActionIds { get; set; } = [];
    public string ActionsSummary { get; set; } = string.Empty;
}
