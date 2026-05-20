using Microsoft.EntityFrameworkCore;
using SistemaPaisa.Domain.Entities;
using SistemaPaisa.Infrastructure.Persistence;

namespace SistemaPaisa.Infrastructure.Repositories;

public class ActionRepository : IActionRepository
{
    private readonly AppDbContext _context;

    public ActionRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<ActionEntity>> GetAllAsync() =>
        await _context.Actions
            .AsNoTracking()
            .Include(a => a.ActionModules.Where(am => am.Estado == RelationStatuses.Active))
                .ThenInclude(am => am.Module)
            .OrderBy(a => a.Name)
            .ToListAsync();

    public async Task<ActionEntity?> GetByIdAsync(int id) =>
        await _context.Actions
            .Include(a => a.ActionModules)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<int?> GetModuleIdAsync(int actionId, CancellationToken cancellationToken = default) =>
        await _context.ActionModules
            .AsNoTracking()
            .Where(am => am.ActionId == actionId && am.Estado == RelationStatuses.Active)
            .Select(am => (int?)am.ModuleId)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<ActionEntity> AddAsync(ActionEntity action, int moduleId, int userId = 1)
    {
        _context.Actions.Add(action);
        await _context.SaveChangesAsync();

        _context.ActionModules.Add(new ActionModule
        {
            ActionId = action.Id,
            ModuleId = moduleId,
            Estado = RelationStatuses.Active,
            CreatedByUserId = userId,
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
        return action;
    }

    public async Task UpdateAsync(ActionEntity action, int moduleId, int userId = 1)
    {
        action.UpdatedAt = DateTime.UtcNow;
        _context.Actions.Update(action);

        var links = await _context.ActionModules
            .Where(am => am.ActionId == action.Id)
            .ToListAsync();

        _context.ActionModules.RemoveRange(links);
        _context.ActionModules.Add(new ActionModule
        {
            ActionId = action.Id,
            ModuleId = moduleId,
            Estado = RelationStatuses.Active,
            CreatedByUserId = userId,
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var action = await _context.Actions.FindAsync(id);
        if (action is null) return;

        var links = await _context.ActionModules.Where(am => am.ActionId == id).ToListAsync();
        _context.ActionModules.RemoveRange(links);
        _context.Actions.Remove(action);
        await _context.SaveChangesAsync();
    }
}
