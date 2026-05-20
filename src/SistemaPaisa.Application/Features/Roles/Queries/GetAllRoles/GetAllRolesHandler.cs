using MediatR;
using SistemaPaisa.Application.DTOs;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Roles.Queries.GetAllRoles;

public class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleDto>>
{
    private readonly IRoleRepository _roleRepository;

    public GetAllRolesHandler(IRoleRepository roleRepository) =>
        _roleRepository = roleRepository;

    public async Task<IEnumerable<RoleDto>> Handle(
        GetAllRolesQuery request,
        CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetAllAsync();
        return roles.Select(r =>
        {
            var profile = r.ProfileRoles.FirstOrDefault()?.Profile;
            return new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                ProfileId = profile?.Id ?? 0,
                ProfileName = profile?.Name ?? "—",
                ClientId = r.ClientId,
                ClientName = r.Client.Name,
                IsActive = r.IsActive
            };
        });
    }
}
