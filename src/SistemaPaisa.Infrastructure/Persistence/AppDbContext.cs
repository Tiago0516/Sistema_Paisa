using Microsoft.EntityFrameworkCore;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Categoria> Categorias => Set<Categoria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
            e.Property(p => p.Precio).HasColumnType("decimal(18,2)");
            e.HasOne(p => p.Categoria)
                .WithMany()
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Categoria>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Nombre).IsRequired().HasMaxLength(120);
            e.Property(c => c.Descripcion).HasMaxLength(250);
        });
    }
}