using Microsoft.EntityFrameworkCore;
using SistemaPaisa.Domain.Entities;
using SistemaPaisa.Infrastructure.Persistence;

namespace SistemaPaisa.Infrastructure.Data;
internal static class ProfilePermissionBootstrap
{
    private const int SystemUserId = 1;

    public static async Task SeedAsync(AppDbContext context)
    {
        var rolesWithoutProfile = await context.Roles
            .Where(r => r.IsActive && !context.ProfileRoles.Any(pr =>
                pr.RoleId == r.Id && pr.Estado == RelationStatuses.Active))
            .ToListAsync();

        if (rolesWithoutProfile.Count == 0)
            return;

        var defaultProfile = await context.Profiles
            .Where(p => p.IsActive)
            .OrderBy(p => p.Id)
            .FirstOrDefaultAsync();

        if (defaultProfile is null)
            return;

        foreach (var role in rolesWithoutProfile)
        {
            context.ProfileRoles.Add(new ProfileRole
            {
                RoleId = role.Id,
                ProfileId = defaultProfile.Id,
                Estado = RelationStatuses.Active,
                CreatedByUserId = SystemUserId,
                CreatedAt = DateTime.UtcNow
            });
        }

        await context.SaveChangesAsync();
    }
}
