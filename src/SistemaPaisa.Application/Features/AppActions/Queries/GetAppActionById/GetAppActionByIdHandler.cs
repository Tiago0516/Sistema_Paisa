using MediatR;
using SistemaPaisa.Application.DTOs;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.AppActions.Queries.GetAppActionById;

public class GetAppActionByIdHandler : IRequestHandler<GetAppActionByIdQuery, ActionDto?>
{
    private readonly IActionRepository _actionRepository;

    public GetAppActionByIdHandler(IActionRepository actionRepository) =>
        _actionRepository = actionRepository;

    public async Task<ActionDto?> Handle(
        GetAppActionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var action = await _actionRepository.GetByIdAsync(request.Id);
        if (action is null) return null;

        var link = action.ActionModules.FirstOrDefault(am => am.Estado == RelationStatuses.Active);
        var moduleId = link?.ModuleId ?? await _actionRepository.GetModuleIdAsync(action.Id, cancellationToken) ?? 0;

        return new ActionDto
        {
            Id = action.Id,
            Name = action.Name,
            Code = action.Code,
            ModuleId = moduleId,
            ModuleName = link?.Module.Name ?? "—",
            IsActive = action.IsActive
        };
    }
}
