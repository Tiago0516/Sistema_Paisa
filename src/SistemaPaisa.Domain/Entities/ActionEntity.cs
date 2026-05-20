namespace SistemaPaisa.Domain.Entities;

public class ActionEntity : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public ICollection<ActionModule> ActionModules { get; set; } = [];
}
