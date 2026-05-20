using MediatR;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.Roles.Queries.GetRoleById;

public record GetRoleByIdQuery(int Id) : IRequest<RoleDto?>;
