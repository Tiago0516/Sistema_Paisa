using MediatR;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.Roles.Queries.GetAllRoles;

public record GetAllRolesQuery : IRequest<IEnumerable<RoleDto>>;
