using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.AppActions.Commands.DeleteAppAction;

public class DeleteAppActionHandler : IRequestHandler<DeleteAppActionCommand, bool>
{
    private readonly IActionRepository _actionRepository;

    public DeleteAppActionHandler(IActionRepository actionRepository) =>
        _actionRepository = actionRepository;

    public async Task<bool> Handle(DeleteAppActionCommand request, CancellationToken cancellationToken)
    {
        var action = await _actionRepository.GetByIdAsync(request.Id);
        if (action is null) return false;

        await _actionRepository.DeleteAsync(request.Id);
        return true;
    }
}
