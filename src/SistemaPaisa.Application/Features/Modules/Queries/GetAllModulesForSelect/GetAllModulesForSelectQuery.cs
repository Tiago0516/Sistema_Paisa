using MediatR;

namespace SistemaPaisa.Application.Features.Modules.Queries.GetAllModulesForSelect;

public record GetAllModulesForSelectQuery : IRequest<IEnumerable<ModuleOptionDto>>;

public class ModuleOptionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
