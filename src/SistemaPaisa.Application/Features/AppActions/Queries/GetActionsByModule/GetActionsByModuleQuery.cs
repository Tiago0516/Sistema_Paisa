using MediatR;

namespace SistemaPaisa.Application.Features.AppActions.Queries.GetActionsByModule;

public record GetActionsByModuleQuery(int ModuleId) : IRequest<IEnumerable<ActionOptionDto>>;

public class ActionOptionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
