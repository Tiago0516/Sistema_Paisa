using MediatR;
using SistemaPaisa.Application.Features.Users.Queries.GetRolesByClient;

namespace SistemaPaisa.Application.Features.Users.Queries.GetRolesForRegistration;

public record GetRolesForRegistrationQuery : IRequest<IReadOnlyList<RoleOptionDto>>;
