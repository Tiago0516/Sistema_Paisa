using MediatR;

namespace SistemaPaisa.Application.Features.Clients.Queries.GetAllClients;

public record GetAllClientsQuery : IRequest<IEnumerable<ClientOptionDto>>;

public class ClientOptionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
