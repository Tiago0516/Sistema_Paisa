using Microsoft.EntityFrameworkCore;
using SistemaPaisa.Domain.Entities;
using SistemaPaisa.Infrastructure.Persistence;

namespace SistemaPaisa.Infrastructure.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly AppDbContext _context;
    public ProductoRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Producto>> GetAllAsync() =>
        await _context.Productos
            .Include(p => p.Categoria)
            .ToListAsync();

    public async Task<Producto?> GetByIdAsync(int id) =>
        await _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Producto> AddAsync(Producto producto)
    {
        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();
        return producto;
    }

    public async Task UpdateAsync(Producto producto)
    {
        _context.Productos.Update(producto);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var p = await _context.Productos.FindAsync(id);
        if (p is not null)
        {
            _context.Productos.Remove(p);
            await _context.SaveChangesAsync();
        }
    }
}