using Microsoft.EntityFrameworkCore;
using SistemaPaisa.Domain.Entities;
using SistemaPaisa.Infrastructure.Persistence;

namespace SistemaPaisa.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) => _context = context;

    public async Task<User?> GetByEmailAsync(string email) =>
        await _context.Users
            .Include(u => u.Role)
                .ThenInclude(r => r.Client)
            .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);

    public async Task<bool> EmailExistsAsync(string email) =>
        await _context.Users.AnyAsync(u => u.Email == email);

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<IEnumerable<User>> GetAllWithRoleAsync() =>
        await _context.Users
            .Include(u => u.Role)
                .ThenInclude(r => r.Client)
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .ToListAsync();

    public async Task<User?> GetByIdAsync(int id) =>
        await _context.Users
            .Include(u => u.Role)
                .ThenInclude(r => r.Client)
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> GetActiveWithRoleByIdAsync(
        int id,
        CancellationToken cancellationToken = default) =>
        await _context.Users
            .AsNoTracking()
            .Include(u => u.Role)
            .FirstOrDefaultAsync(
                u => u.Id == id && u.IsActive && u.Role.IsActive,
                cancellationToken);

    public async Task UpdateRoleAsync(int userId, int roleId, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FindAsync([userId], cancellationToken);
        if (user is null) return;

        user.RoleId = roleId;
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
