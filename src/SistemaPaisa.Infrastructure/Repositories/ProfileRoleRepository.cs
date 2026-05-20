using Microsoft.EntityFrameworkCore;
using SistemaPaisa.Domain.Entities;
using SistemaPaisa.Infrastructure.Persistence;

namespace SistemaPaisa.Infrastructure.Repositories;

public class ProfileRoleRepository : IProfileRoleRepository
{
    private readonly AppDbContext _context;

    public ProfileRoleRepository(AppDbContext context) => _context = context;

    public async Task<int?> GetProfileIdByRoleIdAsync(
        int roleId,
        CancellationToken cancellationToken = default) =>
        await _context.ProfileRoles
            .AsNoTracking()
            .Where(pr => pr.RoleId == roleId && pr.Estado == RelationStatuses.Active)
            .Select(pr => (int?)pr.ProfileId)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task ReplaceForRoleAsync(
        int roleId,
        int profileId,
        int userId,
        CancellationToken cancellationToken = default)
    {
        var existing = await _context.ProfileRoles
            .Where(pr => pr.RoleId == roleId)
            .ToListAsync(cancellationToken);

        _context.ProfileRoles.RemoveRange(existing);

        _context.ProfileRoles.Add(new ProfileRole
        {
            RoleId = roleId,
            ProfileId = profileId,
            Estado = RelationStatuses.Active,
            CreatedByUserId = userId,
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync(cancellationToken);
    }
}
