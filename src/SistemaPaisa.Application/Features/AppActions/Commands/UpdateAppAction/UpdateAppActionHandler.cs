using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.AppActions.Commands.UpdateAppAction;

public class UpdateAppActionHandler : IRequestHandler<UpdateAppActionCommand, bool>
{
    private readonly IActionRepository _actionRepository;

    public UpdateAppActionHandler(IActionRepository actionRepository) =>
        _actionRepository = actionRepository;

    public async Task<bool> Handle(UpdateAppActionCommand request, CancellationToken cancellationToken)
    {
        var action = await _actionRepository.GetByIdAsync(request.Id);
        if (action is null) return false;

        action.Name = request.Name;
        action.Code = request.Code;
        action.IsActive = request.IsActive;

        await _actionRepository.UpdateAsync(action, request.ModuleId);
        return true;
    }
}
