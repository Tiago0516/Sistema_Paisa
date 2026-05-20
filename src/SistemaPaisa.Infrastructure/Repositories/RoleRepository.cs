using Microsoft.EntityFrameworkCore;
using SistemaPaisa.Domain.Entities;
using SistemaPaisa.Infrastructure.Persistence;

namespace SistemaPaisa.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;
    private readonly IProfileRoleRepository _profileRoleRepository;

    public RoleRepository(AppDbContext context, IProfileRoleRepository profileRoleRepository)
    {
        _context = context;
        _profileRoleRepository = profileRoleRepository;
    }

    public async Task<IReadOnlyList<Role>> GetActiveByClientIdAsync(int clientId) =>
        await _context.Roles
            .AsNoTracking()
            .Where(r => r.IsActive && r.ClientId == clientId)
            .OrderBy(r => r.Name)
            .ToListAsync();

    public async Task<Role?> GetActiveByIdAsync(int roleId) =>
        await _context.Roles
            .AsNoTracking()
            .Include(r => r.Client)
            .FirstOrDefaultAsync(r => r.Id == roleId && r.IsActive);

    public async Task<IReadOnlyList<Role>> GetAllActiveWithClientAsync() =>
        await _context.Roles
            .AsNoTracking()
            .Include(r => r.Client)
            .Where(r => r.IsActive && r.Client.IsActive)
            .OrderBy(r => r.Client.Name)
            .ThenBy(r => r.Name)
            .ToListAsync();

    public async Task<Role?> GetDefaultForRegistrationAsync() =>
        await _context.Roles
            .AsNoTracking()
            .Where(r => r.IsActive && r.Name == "Store Operator")
            .FirstOrDefaultAsync()
        ?? await _context.Roles
            .AsNoTracking()
            .Where(r => r.IsActive)
            .OrderBy(r => r.Id)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<Role>> GetAllAsync() =>
        await _context.Roles
            .AsNoTracking()
            .Include(r => r.Client)
            .Include(r => r.ProfileRoles.Where(pr => pr.Estado == RelationStatuses.Active))
                .ThenInclude(pr => pr.Profile)
            .OrderBy(r => r.Name)
            .ToListAsync();

    public async Task<Role?> GetByIdAsync(int id) =>
        await _context.Roles
            .Include(r => r.Client)
            .Include(r => r.ProfileRoles.Where(pr => pr.Estado == RelationStatuses.Active))
                .ThenInclude(pr => pr.Profile)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<Role> AddAsync(Role role, int profileId, int userId = 1)
    {
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
        await _profileRoleRepository.ReplaceForRoleAsync(role.Id, profileId, userId);
        return role;
    }

    public async Task UpdateAsync(Role role, int profileId, int userId = 1)
    {
        role.UpdatedAt = DateTime.UtcNow;
        _context.Roles.Update(role);
        await _context.SaveChangesAsync();
        await _profileRoleRepository.ReplaceForRoleAsync(role.Id, profileId, userId);
    }

    public async Task DeleteAsync(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role is null) return;

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
    }
}
