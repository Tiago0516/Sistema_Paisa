using Microsoft.EntityFrameworkCore;
using SistemaPaisa.Domain.Entities;
using SistemaPaisa.Infrastructure.Persistence;

namespace SistemaPaisa.Infrastructure.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Categoria>> GetAllAsync() =>
        await _context.Categorias.ToListAsync();

    public async Task<Categoria?> GetByIdAsync(int id) =>
        await _context.Categorias.FindAsync(id);

    public async Task<Categoria> AddAsync(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        return categoria;
    }

    public async Task UpdateAsync(Categoria categoria)
    {
        _context.Categorias.Update(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var c = await _context.Categorias.FindAsync(id);
        if (c is not null)
        {
            _context.Categorias.Remove(c);
            await _context.SaveChangesAsync();
        }
    }
}
