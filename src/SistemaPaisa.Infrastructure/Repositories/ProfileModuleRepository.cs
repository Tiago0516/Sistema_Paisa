using Microsoft.EntityFrameworkCore;
using SistemaPaisa.Domain.Entities;
using SistemaPaisa.Infrastructure.Persistence;

namespace SistemaPaisa.Infrastructure.Repositories;

public class ProfileModuleRepository : IProfileModuleRepository
{
    private const int SystemUserId = 1;
    private readonly AppDbContext _context;

    public ProfileModuleRepository(AppDbContext context) => _context = context;

    public async Task<IReadOnlyList<ProfileModuleInfo>> GetByProfileIdAsync(
        int profileId,
        CancellationToken cancellationToken = default) =>
        await _context.ProfileModules
            .AsNoTracking()
            .Where(pm =>
                pm.ProfileId == profileId &&
                pm.Estado == RelationStatuses.Active &&
                pm.Module.IsActive)
            .OrderBy(pm => pm.Module.Name)
            .Select(pm => new ProfileModuleInfo
            {
                ModuleId = pm.ModuleId,
                Code = pm.Module.Code,
                Name = pm.Module.Name,
                Description = pm.Module.Description,
                Icon = pm.Module.Icon,
                ControllerName = pm.Module.ControllerName,
                CreateActionName = pm.Module.CreateActionName,
                IsLanding = pm.Module.IsLanding
            })
            .ToListAsync(cancellationToken);

    public async Task ReplaceForProfileAsync(
        int profileId,
        IEnumerable<int> moduleIds,
        CancellationToken cancellationToken = default)
    {
        var existing = await _context.ProfileModules
            .Where(pm => pm.ProfileId == profileId)
            .ToListAsync(cancellationToken);

        _context.ProfileModules.RemoveRange(existing);

        foreach (var moduleId in moduleIds.Distinct())
        {
            _context.ProfileModules.Add(new ProfileModule
            {
                ProfileId = profileId,
                ModuleId = moduleId,
                Estado = RelationStatuses.Active,
                CreatedByUserId = SystemUserId,
                CreatedAt = DateTime.UtcNow
            });
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
