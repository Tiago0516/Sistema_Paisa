namespace SistemaPaisa.Domain.Entities;

public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> GetAllAsync();
    Task<Categoria?> GetByIdAsync(int id);
    Task<Categoria> AddAsync(Categoria categoria);
    Task UpdateAsync(Categoria categoria);
    Task DeleteAsync(int id);
}
