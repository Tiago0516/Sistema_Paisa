using MediatR;

namespace SistemaPaisa.Application.Features.AppActions.Commands.DeleteAppAction;

public record DeleteAppActionCommand(int Id) : IRequest<bool>;
