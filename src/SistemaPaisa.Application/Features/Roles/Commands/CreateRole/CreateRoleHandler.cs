using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Roles.Commands.CreateRole;

public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, int>
{
    private readonly IRoleRepository _roleRepository;

    public CreateRoleHandler(IRoleRepository roleRepository) =>
        _roleRepository = roleRepository;

    public async Task<int> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new Role
        {
            Name = request.Name,
            Description = request.Description,
            ClientId = request.ClientId
        };

        var created = await _roleRepository.AddAsync(role, request.ProfileId);
        return created.Id;
    }
}
