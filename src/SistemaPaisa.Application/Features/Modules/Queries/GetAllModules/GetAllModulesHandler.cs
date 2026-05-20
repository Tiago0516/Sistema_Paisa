using MediatR;
using SistemaPaisa.Application.DTOs;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Modules.Queries.GetAllModules;

public class GetAllModulesHandler : IRequestHandler<GetAllModulesQuery, IEnumerable<ModuleDto>>
{
    private readonly IModuleRepository _moduleRepository;

    public GetAllModulesHandler(IModuleRepository moduleRepository) =>
        _moduleRepository = moduleRepository;

    public async Task<IEnumerable<ModuleDto>> Handle(
        GetAllModulesQuery request,
        CancellationToken cancellationToken)
    {
        var modules = await _moduleRepository.GetAllAsync();
        return modules.Select(m => new ModuleDto
        {
            Id = m.Id,
            Name = m.Name,
            Description = m.Description,
            Code = m.Code,
            IsActive = m.IsActive
        });
    }
}
