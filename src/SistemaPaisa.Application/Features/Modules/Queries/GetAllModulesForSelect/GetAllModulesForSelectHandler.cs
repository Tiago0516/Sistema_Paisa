using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Modules.Queries.GetAllModulesForSelect;

public class GetAllModulesForSelectHandler : IRequestHandler<GetAllModulesForSelectQuery, IEnumerable<ModuleOptionDto>>
{
    private readonly IModuleRepository _moduleRepository;

    public GetAllModulesForSelectHandler(IModuleRepository moduleRepository) =>
        _moduleRepository = moduleRepository;

    public async Task<IEnumerable<ModuleOptionDto>> Handle(
        GetAllModulesForSelectQuery request,
        CancellationToken cancellationToken)
    {
        var modules = await _moduleRepository.GetAllAsync();
        return modules
            .Where(m => m.IsActive)
            .OrderBy(m => m.Name)
            .Select(m => new ModuleOptionDto { Id = m.Id, Name = m.Name });
    }
}
