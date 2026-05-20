namespace SistemaPaisa.Domain.Entities;

public class ActionModule : RelationAuditableEntity
{
    public int ActionId { get; set; }
    public ActionEntity Action { get; set; } = null!;
    public int ModuleId { get; set; }
    public Module Module { get; set; } = null!;
}
