using MediatR;
using SistemaPaisa.Application.DTOs;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.AppActions.Queries.GetAllAppActions;

public class GetAllAppActionsHandler : IRequestHandler<GetAllAppActionsQuery, IEnumerable<ActionDto>>
{
    private readonly IActionRepository _actionRepository;

    public GetAllAppActionsHandler(IActionRepository actionRepository) =>
        _actionRepository = actionRepository;

    public async Task<IEnumerable<ActionDto>> Handle(
        GetAllAppActionsQuery request,
        CancellationToken cancellationToken)
    {
        var actions = await _actionRepository.GetAllAsync();
        return actions.Select(a =>
        {
            var link = a.ActionModules.FirstOrDefault(am => am.Estado == RelationStatuses.Active);
            return new ActionDto
            {
                Id = a.Id,
                Name = a.Name,
                Code = a.Code,
                ModuleId = link?.ModuleId ?? 0,
                ModuleName = link?.Module.Name ?? "—",
                IsActive = a.IsActive
            };
        });
    }
}
