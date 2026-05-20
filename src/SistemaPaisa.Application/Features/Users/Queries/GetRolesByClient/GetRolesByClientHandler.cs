using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Users.Queries.GetRolesByClient;

public class GetRolesByClientHandler : IRequestHandler<GetRolesByClientQuery, IReadOnlyList<RoleOptionDto>>
{
    private readonly IRoleRepository _roleRepository;

    public GetRolesByClientHandler(IRoleRepository roleRepository) =>
        _roleRepository = roleRepository;

    public async Task<IReadOnlyList<RoleOptionDto>> Handle(
        GetRolesByClientQuery request,
        CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetActiveByClientIdAsync(request.ClientId);
        return roles.Select(r => new RoleOptionDto { Id = r.Id, Name = r.Name }).ToList();
    }
}
