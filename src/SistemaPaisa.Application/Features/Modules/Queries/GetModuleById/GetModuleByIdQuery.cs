using MediatR;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.Modules.Queries.GetModuleById;

public record GetModuleByIdQuery(int Id) : IRequest<ModuleDto?>;
