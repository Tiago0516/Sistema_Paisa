namespace SistemaPaisa.Domain.Entities;

using SistemaPaisa.Domain.Entities;

public interface IProductoRepository
{
    Task<IEnumerable<Producto>> GetAllAsync();
    Task<Producto?> GetByIdAsync(int id);
    Task<Producto> AddAsync(Producto producto);
    Task UpdateAsync(Producto producto);
    Task DeleteAsync(int id);
}