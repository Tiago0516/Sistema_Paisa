using MediatR;
using SistemaPaisa.Application.DTOs;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Modules.Queries.GetModuleById;

public class GetModuleByIdHandler : IRequestHandler<GetModuleByIdQuery, ModuleDto?>
{
    private readonly IModuleRepository _moduleRepository;

    public GetModuleByIdHandler(IModuleRepository moduleRepository) =>
        _moduleRepository = moduleRepository;

    public async Task<ModuleDto?> Handle(
        GetModuleByIdQuery request,
        CancellationToken cancellationToken)
    {
        var module = await _moduleRepository.GetByIdAsync(request.Id);
        if (module is null) return null;

        return new ModuleDto
        {
            Id = module.Id,
            Name = module.Name,
            Description = module.Description,
            Code = module.Code,
            IsActive = module.IsActive
        };
    }
}
