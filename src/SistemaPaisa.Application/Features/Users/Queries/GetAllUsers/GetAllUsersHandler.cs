using MediatR;
using SistemaPaisa.Application.DTOs;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserListDto>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersHandler(IUserRepository userRepository) =>
        _userRepository = userRepository;

    public async Task<IEnumerable<UserListDto>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllWithRoleAsync();
        return users.Select(u => new UserListDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            RoleId = u.RoleId,
            RoleName = u.Role.Name,
            ClientName = u.Role.Client.Name,
            IsActive = u.IsActive
        });
    }
}
