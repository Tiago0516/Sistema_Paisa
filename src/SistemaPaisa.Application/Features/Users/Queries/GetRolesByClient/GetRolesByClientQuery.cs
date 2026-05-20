using MediatR;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.Users.Queries.GetRolesByClient;

public record GetRolesByClientQuery(int ClientId) : IRequest<IReadOnlyList<RoleOptionDto>>;
