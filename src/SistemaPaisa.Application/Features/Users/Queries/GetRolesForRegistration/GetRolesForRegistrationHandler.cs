using MediatR;
using SistemaPaisa.Application.Features.Users.Queries.GetRolesByClient;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Users.Queries.GetRolesForRegistration;

public class GetRolesForRegistrationHandler : IRequestHandler<GetRolesForRegistrationQuery, IReadOnlyList<RoleOptionDto>>
{
    private readonly IRoleRepository _roleRepository;

    public GetRolesForRegistrationHandler(IRoleRepository roleRepository) =>
        _roleRepository = roleRepository;

    public async Task<IReadOnlyList<RoleOptionDto>> Handle(
        GetRolesForRegistrationQuery request,
        CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetAllActiveWithClientAsync();
        return roles.Select(r => new RoleOptionDto
        {
            Id = r.Id,
            Name = $"{r.Name} ({r.Client.Name})"
        }).ToList();
    }
}
