using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.AppActions.Commands.CreateAppAction;

public class CreateAppActionHandler : IRequestHandler<CreateAppActionCommand, int>
{
    private readonly IActionRepository _actionRepository;

    public CreateAppActionHandler(IActionRepository actionRepository) =>
        _actionRepository = actionRepository;

    public async Task<int> Handle(CreateAppActionCommand request, CancellationToken cancellationToken)
    {
        var action = new ActionEntity
        {
            Name = request.Name,
            Code = request.Code
        };

        var created = await _actionRepository.AddAsync(action, request.ModuleId);
        return created.Id;
    }
}
