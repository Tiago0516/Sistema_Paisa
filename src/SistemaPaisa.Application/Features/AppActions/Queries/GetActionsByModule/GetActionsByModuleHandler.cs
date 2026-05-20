using MediatR;
using SistemaPaisa.Application.Common.ModuleActions;

namespace SistemaPaisa.Application.Features.AppActions.Queries.GetActionsByModule;

public class GetActionsByModuleHandler : IRequestHandler<GetActionsByModuleQuery, IEnumerable<ActionOptionDto>>
{
    private readonly IModuleActionService _moduleActionService;

    public GetActionsByModuleHandler(IModuleActionService moduleActionService) =>
        _moduleActionService = moduleActionService;

    public async Task<IEnumerable<ActionOptionDto>> Handle(
        GetActionsByModuleQuery request,
        CancellationToken cancellationToken)
    {
        var actions = await _moduleActionService.GetByModuleIdAsync(request.ModuleId, cancellationToken);
        return actions.Select(a => new ActionOptionDto
        {
            Id = a.Id,
            Name = a.Name,
            Code = a.Code
        }).ToList();
    }
}
