using MediatR;

namespace SistemaPaisa.Application.Features.Roles.Commands.DeleteRole;

public record DeleteRoleCommand(int Id) : IRequest<bool>;
