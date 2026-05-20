namespace SistemaPaisa.Domain.Entities;

public interface IClientRepository
{
    Task<IReadOnlyList<Client>> GetAllActiveAsync();
}
