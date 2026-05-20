using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Clients.Queries.GetAllClients;

public class GetAllClientsHandler : IRequestHandler<GetAllClientsQuery, IEnumerable<ClientOptionDto>>
{
    private readonly IClientRepository _clientRepository;

    public GetAllClientsHandler(IClientRepository clientRepository) =>
        _clientRepository = clientRepository;

    public async Task<IEnumerable<ClientOptionDto>> Handle(
        GetAllClientsQuery request,
        CancellationToken cancellationToken)
    {
        var clients = await _clientRepository.GetAllActiveAsync();
        return clients.Select(c => new ClientOptionDto { Id = c.Id, Name = c.Name });
    }
}
