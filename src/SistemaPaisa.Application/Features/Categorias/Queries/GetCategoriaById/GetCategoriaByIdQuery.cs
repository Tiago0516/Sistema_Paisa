using MediatR;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.Categorias.Queries.GetCategoriaById;

public record GetCategoriaByIdQuery(int Id) : IRequest<CategoriaDto?>;
