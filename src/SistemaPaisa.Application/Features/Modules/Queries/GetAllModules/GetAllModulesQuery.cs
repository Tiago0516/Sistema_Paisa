using MediatR;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.Modules.Queries.GetAllModules;

public record GetAllModulesQuery : IRequest<IEnumerable<ModuleDto>>;
