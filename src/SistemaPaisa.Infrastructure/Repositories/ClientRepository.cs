using Microsoft.EntityFrameworkCore;
using SistemaPaisa.Domain.Entities;
using SistemaPaisa.Infrastructure.Persistence;

namespace SistemaPaisa.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;

    public ClientRepository(AppDbContext context) => _context = context;

    public async Task<IReadOnlyList<Client>> GetAllActiveAsync() =>
        await _context.Clients
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();
}
