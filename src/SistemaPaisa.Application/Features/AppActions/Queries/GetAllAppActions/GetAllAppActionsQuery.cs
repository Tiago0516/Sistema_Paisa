using MediatR;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.AppActions.Queries.GetAllAppActions;

public record GetAllAppActionsQuery : IRequest<IEnumerable<ActionDto>>;
