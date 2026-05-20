using MediatR;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.AppActions.Queries.GetAppActionById;

public record GetAppActionByIdQuery(int Id) : IRequest<ActionDto?>;
