using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Users.Commands.UpdateUserRole;

public class UpdateUserRoleHandler : IRequestHandler<UpdateUserRoleCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public UpdateUserRoleHandler(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<bool> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user is null) return false;

        var role = await _roleRepository.GetActiveByIdAsync(request.RoleId);
        if (role is null) return false;

        await _userRepository.UpdateRoleAsync(request.UserId, request.RoleId, cancellationToken);
        return true;
    }
}
