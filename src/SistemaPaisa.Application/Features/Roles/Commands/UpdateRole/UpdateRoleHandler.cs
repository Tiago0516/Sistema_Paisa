using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Roles.Commands.UpdateRole;

public class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, bool>
{
    private readonly IRoleRepository _roleRepository;

    public UpdateRoleHandler(IRoleRepository roleRepository) =>
        _roleRepository = roleRepository;

    public async Task<bool> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id);
        if (role is null) return false;

        role.Name = request.Name;
        role.Description = request.Description;
        role.ClientId = request.ClientId;
        role.IsActive = request.IsActive;

        await _roleRepository.UpdateAsync(role, request.ProfileId);
        return true;
    }
}
