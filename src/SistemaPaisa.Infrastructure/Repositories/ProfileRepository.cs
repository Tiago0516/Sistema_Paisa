using Microsoft.EntityFrameworkCore;
using SistemaPaisa.Domain.Entities;
using SistemaPaisa.Infrastructure.Persistence;

namespace SistemaPaisa.Infrastructure.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly AppDbContext _context;
    private readonly IProfileModuleRepository _profileModuleRepository;

    public ProfileRepository(AppDbContext context, IProfileModuleRepository profileModuleRepository)
    {
        _context = context;
        _profileModuleRepository = profileModuleRepository;
    }

    public async Task<IReadOnlyList<Profile>> GetAllActiveAsync() =>
        await _context.Profiles
            .AsNoTracking()
            .Include(p => p.Module)
            .Include(p => p.ProfileModules)
                .ThenInclude(pm => pm.Module)
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();

    public async Task<IEnumerable<Profile>> GetAllAsync() =>
        await _context.Profiles
            .AsNoTracking()
            .Include(p => p.Module)
            .Include(p => p.ProfileModules)
                .ThenInclude(pm => pm.Module)
            .Include(p => p.ProfileActions)
                .ThenInclude(pa => pa.Action)
            .OrderBy(p => p.Name)
            .ToListAsync();

    public async Task<Profile?> GetByIdAsync(int id) =>
        await _context.Profiles
            .Include(p => p.Module)
            .Include(p => p.ProfileModules)
                .ThenInclude(pm => pm.Module)
            .Include(p => p.ProfileActions)
                .ThenInclude(pa => pa.Action)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Profile> AddAsync(
        Profile profile,
        IEnumerable<int> moduleIds,
        IEnumerable<int> actionIds)
    {
        _context.Profiles.Add(profile);
        await _context.SaveChangesAsync();
        await _profileModuleRepository.ReplaceForProfileAsync(profile.Id, moduleIds);
        await ReplaceProfileActionsAsync(profile.Id, actionIds);
        return profile;
    }

    public async Task UpdateAsync(
        Profile profile,
        IEnumerable<int> moduleIds,
        IEnumerable<int> actionIds)
    {
        profile.UpdatedAt = DateTime.UtcNow;
        _context.Profiles.Update(profile);
        await _context.SaveChangesAsync();
        await _profileModuleRepository.ReplaceForProfileAsync(profile.Id, moduleIds);
        await ReplaceProfileActionsAsync(profile.Id, actionIds);
    }

    public async Task DeleteAsync(int id)
    {
        var profile = await _context.Profiles.FindAsync(id);
        if (profile is null) return;

        _context.Profiles.Remove(profile);
        await _context.SaveChangesAsync();
    }

    private async Task ReplaceProfileActionsAsync(int profileId, IEnumerable<int> actionIds)
    {
        var existing = await _context.ProfileActions
            .Where(pa => pa.ProfileId == profileId)
            .ToListAsync();

        _context.ProfileActions.RemoveRange(existing);

        foreach (var actionId in actionIds.Distinct())
        {
            _context.ProfileActions.Add(new ProfileAction
            {
                ProfileId = profileId,
                ActionId = actionId
            });
        }

        await _context.SaveChangesAsync();
    }
}
