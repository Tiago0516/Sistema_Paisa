using MediatR;
using SistemaPaisa.Application.DTOs;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Roles.Queries.GetRoleById;

public class GetRoleByIdHandler : IRequestHandler<GetRoleByIdQuery, RoleDto?>
{
    private readonly IRoleRepository _roleRepository;

    public GetRoleByIdHandler(IRoleRepository roleRepository) =>
        _roleRepository = roleRepository;

    public async Task<RoleDto?> Handle(
        GetRoleByIdQuery request,
        CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id);
        if (role is null) return null;

        var profile = role.ProfileRoles.FirstOrDefault()?.Profile;

        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            ProfileId = profile?.Id ?? 0,
            ProfileName = profile?.Name ?? "—",
            ClientId = role.ClientId,
            ClientName = role.Client.Name,
            IsActive = role.IsActive
        };
    }
}
