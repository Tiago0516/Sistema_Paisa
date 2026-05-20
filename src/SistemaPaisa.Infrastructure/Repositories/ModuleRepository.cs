using Microsoft.EntityFrameworkCore;
using SistemaPaisa.Domain.Entities;
using SistemaPaisa.Infrastructure.Persistence;

namespace SistemaPaisa.Infrastructure.Repositories;

public class ModuleRepository : IModuleRepository
{
    private readonly AppDbContext _context;

    public ModuleRepository(AppDbContext context) => _context = context;

    public async Task<IReadOnlyList<Module>> GetActiveWithActionsAsync() =>
        await _context.Modules
            .AsNoTracking()
            .Include(m => m.ActionModules.Where(am => am.Estado == RelationStatuses.Active))
                .ThenInclude(am => am.Action)
            .Where(m => m.IsActive)
            .OrderBy(m => m.Name)
            .ToListAsync();

    public async Task<IEnumerable<Module>> GetAllAsync() =>
        await _context.Modules.OrderBy(m => m.Name).ToListAsync();

    public async Task<Module?> GetByIdAsync(int id) =>
        await _context.Modules.FindAsync(id);

    public async Task<Module> AddAsync(Module module)
    {
        _context.Modules.Add(module);
        await _context.SaveChangesAsync();
        return module;
    }

    public async Task UpdateAsync(Module module)
    {
        module.UpdatedAt = DateTime.UtcNow;
        _context.Modules.Update(module);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var module = await _context.Modules.FindAsync(id);
        if (module is not null)
        {
            _context.Modules.Remove(module);
            await _context.SaveChangesAsync();
        }
    }
}
