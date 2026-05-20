using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Roles.Commands.DeleteRole;

public class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand, bool>
{
    private readonly IRoleRepository _roleRepository;

    public DeleteRoleHandler(IRoleRepository roleRepository) =>
        _roleRepository = roleRepository;

    public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id);
        if (role is null) return false;

        await _roleRepository.DeleteAsync(request.Id);
        return true;
    }
}
