namespace SistemaPaisa.Domain.Entities;

public abstract class RelationAuditableEntity
{
    public int Id { get; set; }
    public string Estado { get; set; } = RelationStatuses.Active;
    public int CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int? UpdatedByUserId { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public bool IsActive => Estado == RelationStatuses.Active;
}

public static class RelationStatuses
{
    public const string Active = "ACTIVO";
    public const string Inactive = "INACTIVO";
}
