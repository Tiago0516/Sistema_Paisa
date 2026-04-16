using MediatR;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.Categorias.Queries.GetAllCategorias;

public record GetAllCategoriasQuery : IRequest<IEnumerable<CategoriaDto>>;
