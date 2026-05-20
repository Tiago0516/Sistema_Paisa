using MediatR;

namespace SistemaPaisa.Application.Features.Users.Commands.UpdateUserRole;

public record UpdateUserRoleCommand(int UserId, int RoleId) : IRequest<bool>;
