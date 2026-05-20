using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Infrastructure.Persistence;

internal static class RelationEntityConfiguration
{
    public static void ConfigureRelationAuditable<T>(
        this EntityTypeBuilder<T> builder,
        string tableName,
        string idColumnName) where T : RelationAuditableEntity
    {
        builder.ToTable(tableName);
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName(idColumnName);
        builder.Property(e => e.Estado)
            .HasColumnName("estado")
            .HasMaxLength(50)
            .IsUnicode(false)
            .IsRequired()
            .HasDefaultValue(RelationStatuses.Active)
            .ValueGeneratedOnAdd();
        builder.Property(e => e.CreatedByUserId).HasColumnName("id_usuario_creador");
        builder.Property(e => e.CreatedAt)
            .HasColumnName("fecha_creacion")
            .HasDefaultValueSql("GETDATE()")
            .ValueGeneratedOnAdd();
        builder.Property(e => e.UpdatedByUserId).HasColumnName("id_usuario_modifica");
        builder.Property(e => e.UpdatedAt).HasColumnName("fecha_modificacion");
    }
}
