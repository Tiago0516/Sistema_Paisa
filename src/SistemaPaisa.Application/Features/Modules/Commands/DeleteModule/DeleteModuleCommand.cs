using MediatR;

namespace SistemaPaisa.Application.Features.Modules.Commands.DeleteModule;

public record DeleteModuleCommand(int Id) : IRequest<bool>;
