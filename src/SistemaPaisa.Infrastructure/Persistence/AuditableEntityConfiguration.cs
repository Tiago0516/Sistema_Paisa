using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Infrastructure.Persistence;

internal static class AuditableEntityConfiguration
{
    public static void ConfigureAuditable<T>(this EntityTypeBuilder<T> builder)
        where T : AuditableEntity
    {
        builder.Property(e => e.CreatedBy).HasMaxLength(100);
        builder.Property(e => e.UpdatedBy).HasMaxLength(100);
    }
}
