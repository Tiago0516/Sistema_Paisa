using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Modules.Commands.CreateModule;

public class CreateModuleHandler : IRequestHandler<CreateModuleCommand, int>
{
    private readonly IModuleRepository _moduleRepository;

    public CreateModuleHandler(IModuleRepository moduleRepository) =>
        _moduleRepository = moduleRepository;

    public async Task<int> Handle(CreateModuleCommand request, CancellationToken cancellationToken)
    {
        var module = new Module
        {
            Name = request.Name,
            Description = request.Description,
            Code = request.Code
        };

        var created = await _moduleRepository.AddAsync(module);
        return created.Id;
    }
}
