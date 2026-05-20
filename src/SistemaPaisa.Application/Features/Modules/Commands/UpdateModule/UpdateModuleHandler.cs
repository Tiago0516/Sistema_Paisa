using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Modules.Commands.UpdateModule;

public class UpdateModuleHandler : IRequestHandler<UpdateModuleCommand, bool>
{
    private readonly IModuleRepository _moduleRepository;

    public UpdateModuleHandler(IModuleRepository moduleRepository) =>
        _moduleRepository = moduleRepository;

    public async Task<bool> Handle(UpdateModuleCommand request, CancellationToken cancellationToken)
    {
        var module = await _moduleRepository.GetByIdAsync(request.Id);
        if (module is null) return false;

        module.Name = request.Name;
        module.Description = request.Description;
        module.Code = request.Code;
        module.IsActive = request.IsActive;

        await _moduleRepository.UpdateAsync(module);
        return true;
    }
}
