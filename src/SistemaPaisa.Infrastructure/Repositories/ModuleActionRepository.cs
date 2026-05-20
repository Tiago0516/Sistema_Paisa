using Microsoft.EntityFrameworkCore;
using SistemaPaisa.Domain.Entities;
using SistemaPaisa.Infrastructure.Persistence;

namespace SistemaPaisa.Infrastructure.Repositories;

public class ModuleActionRepository : IModuleActionRepository
{
    private readonly AppDbContext _context;

    public ModuleActionRepository(AppDbContext context) => _context = context;

    public async Task<IReadOnlyList<ModuleActionInfo>> GetByModuleIdAsync(
        int moduleId,
        CancellationToken cancellationToken = default) =>
        await QueryActive()
            .Where(am => am.ModuleId == moduleId)
            .OrderBy(am => am.Action.Name)
            .Select(am => new ModuleActionInfo
            {
                Id = am.Action.Id,
                Code = am.Action.Code,
                Name = am.Action.Name,
                ModuleId = am.ModuleId,
                ModuleCode = am.Module.Code
            })
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<ModuleActionInfo>> GetByModuleCodeAsync(
        string moduleCode,
        CancellationToken cancellationToken = default) =>
        await QueryActive()
            .Where(am => am.Module.Code == moduleCode && am.Module.IsActive)
            .OrderBy(am => am.Action.Name)
            .Select(am => new ModuleActionInfo
            {
                Id = am.Action.Id,
                Code = am.Action.Code,
                Name = am.Action.Name,
                ModuleId = am.ModuleId,
                ModuleCode = am.Module.Code
            })
            .ToListAsync(cancellationToken);

    private IQueryable<ActionModule> QueryActive() =>
        _context.ActionModules
            .AsNoTracking()
            .Where(am => am.Estado == RelationStatuses.Active && am.Action.IsActive);
}
