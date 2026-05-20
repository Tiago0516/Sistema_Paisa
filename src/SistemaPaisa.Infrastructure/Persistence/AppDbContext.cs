using Microsoft.EntityFrameworkCore;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Module> Modules => Set<Module>();
    public DbSet<ActionEntity> Actions => Set<ActionEntity>();
    public DbSet<ActionModule> ActionModules => Set<ActionModule>();
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<ProfileAction> ProfileActions => Set<ProfileAction>();
    public DbSet<ProfileModule> ProfileModules => Set<ProfileModule>();
    public DbSet<ProfileRole> ProfileRoles => Set<ProfileRole>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(e =>
        {
            e.ToTable("Categories");
            e.HasKey(c => c.Id);
            e.ConfigureAuditable();
            e.Property(c => c.Name).IsRequired().HasMaxLength(120);
            e.Property(c => c.Description).HasMaxLength(250);
        });

        modelBuilder.Entity<Product>(e =>
        {
            e.ToTable("Products");
            e.HasKey(p => p.Id);
            e.ConfigureAuditable();
            e.Property(p => p.Name).IsRequired().HasMaxLength(150);
            e.Property(p => p.Description).IsRequired();
            e.Property(p => p.Price).HasColumnType("decimal(18,2)");
            e.HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Module>(e =>
        {
            e.ToTable("Modules");
            e.HasKey(m => m.Id);
            e.ConfigureAuditable();
            e.Property(m => m.Name).IsRequired().HasMaxLength(120);
            e.Property(m => m.Description).HasMaxLength(250);
            e.Property(m => m.Code).IsRequired().HasMaxLength(50);
            e.HasIndex(m => m.Code).IsUnique();
            e.Property(m => m.Icon).HasMaxLength(60);
            e.Property(m => m.ControllerName).HasMaxLength(100);
            e.Property(m => m.CreateActionName).HasMaxLength(100);
            e.Property(m => m.IsLanding).HasDefaultValue(false);
        });

        modelBuilder.Entity<ActionEntity>(e =>
        {
            e.ToTable("Actions");
            e.HasKey(a => a.Id);
            e.ConfigureAuditable();
            e.Property(a => a.Name).IsRequired().HasMaxLength(120);
            e.Property(a => a.Code).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<ActionModule>(e =>
        {
            e.ConfigureRelationAuditable("rel_Actions_Modules", "id_rel_Actions_Modules");
            e.Property(am => am.ActionId).HasColumnName("id_Actions");
            e.Property(am => am.ModuleId).HasColumnName("id_Modules");
            e.HasIndex(am => new { am.ActionId, am.ModuleId }).IsUnique();
            e.HasIndex(am => am.ModuleId);
            e.HasOne(am => am.Action)
                .WithMany(a => a.ActionModules)
                .HasForeignKey(am => am.ActionId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(am => am.Module)
                .WithMany(m => m.ActionModules)
                .HasForeignKey(am => am.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Profile>(e =>
        {
            e.ToTable("Profiles");
            e.HasKey(p => p.Id);
            e.ConfigureAuditable();
            e.Property(p => p.Name).IsRequired().HasMaxLength(120);
            e.Property(p => p.Description).HasMaxLength(250);
            e.HasOne(p => p.Module)
                .WithMany(m => m.Profiles)
                .HasForeignKey(p => p.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProfileModule>(e =>
        {
            e.ConfigureRelationAuditable("rel_Modules_Profiles", "id_rel_Modules_Profiles");
            e.Property(pm => pm.ProfileId).HasColumnName("id_Profiles");
            e.Property(pm => pm.ModuleId).HasColumnName("id_Modules");
            e.HasIndex(pm => new { pm.ProfileId, pm.ModuleId }).IsUnique();
            e.HasOne(pm => pm.Profile)
                .WithMany(p => p.ProfileModules)
                .HasForeignKey(pm => pm.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(pm => pm.Module)
                .WithMany(m => m.ProfileModules)
                .HasForeignKey(pm => pm.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProfileRole>(e =>
        {
            e.ConfigureRelationAuditable("rel_Profiles_Roles", "id_rel_Profiles_Roles");
            e.Property(pr => pr.RoleId).HasColumnName("id_Roles");
            e.Property(pr => pr.ProfileId).HasColumnName("id_Profiles");
            e.HasIndex(pr => pr.RoleId).IsUnique();
            e.HasIndex(pr => pr.ProfileId);
            e.HasOne(pr => pr.Role)
                .WithMany(r => r.ProfileRoles)
                .HasForeignKey(pr => pr.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(pr => pr.Profile)
                .WithMany(p => p.ProfileRoles)
                .HasForeignKey(pr => pr.ProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProfileAction>(e =>
        {
            e.ToTable("ProfileActions");
            e.HasKey(pa => new { pa.ProfileId, pa.ActionId });
            e.HasOne(pa => pa.Profile)
                .WithMany(p => p.ProfileActions)
                .HasForeignKey(pa => pa.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(pa => pa.Action)
                .WithMany()
                .HasForeignKey(pa => pa.ActionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Client>(e =>
        {
            e.ToTable("Clients");
            e.HasKey(c => c.Id);
            e.ConfigureAuditable();
            e.Property(c => c.Name).IsRequired().HasMaxLength(150);
            e.Property(c => c.Code).IsRequired().HasMaxLength(50);
            e.Property(c => c.Identification).HasMaxLength(50);
            e.HasIndex(c => c.Code).IsUnique();
        });

        modelBuilder.Entity<Role>(e =>
        {
            e.ToTable("Roles");
            e.HasKey(r => r.Id);
            e.ConfigureAuditable();
            e.Property(r => r.Name).IsRequired().HasMaxLength(120);
            e.Property(r => r.Description).HasMaxLength(250);
            e.HasOne(r => r.Client)
                .WithMany(c => c.Roles)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("Users");
            e.HasKey(u => u.Id);
            e.ConfigureAuditable();
            e.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
            e.Property(u => u.LastName).IsRequired().HasMaxLength(100);
            e.Property(u => u.Email).IsRequired().HasMaxLength(150);
            e.Property(u => u.PasswordHash).IsRequired().HasMaxLength(256);
            e.HasIndex(u => u.Email).IsUnique();
            e.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
