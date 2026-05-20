using MediatR;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.Users.Queries.GetAllUsers;

public record GetAllUsersQuery : IRequest<IEnumerable<UserListDto>>;
