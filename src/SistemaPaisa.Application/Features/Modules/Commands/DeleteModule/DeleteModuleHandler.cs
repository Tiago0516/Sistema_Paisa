using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Modules.Commands.DeleteModule;

public class DeleteModuleHandler : IRequestHandler<DeleteModuleCommand, bool>
{
    private readonly IModuleRepository _moduleRepository;

    public DeleteModuleHandler(IModuleRepository moduleRepository) =>
        _moduleRepository = moduleRepository;

    public async Task<bool> Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
    {
        var module = await _moduleRepository.GetByIdAsync(request.Id);
        if (module is null) return false;

        await _moduleRepository.DeleteAsync(request.Id);
        return true;
    }
}
