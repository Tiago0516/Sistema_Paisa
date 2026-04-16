using MediatR;
using SistemaPaisa.Application.DTOs;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Categorias.Queries.GetAllCategorias;

public class GetAllCategoriasHandler : IRequestHandler<GetAllCategoriasQuery, IEnumerable<CategoriaDto>>
{
    private readonly ICategoriaRepository _categoriaRepository;

    public GetAllCategoriasHandler(ICategoriaRepository categoriaRepository) => _categoriaRepository = categoriaRepository;

    public async Task<IEnumerable<CategoriaDto>> Handle(GetAllCategoriasQuery request, CancellationToken cancellationToken)
    {
        var categorias = await _categoriaRepository.GetAllAsync();
        return categorias.Select(c => new CategoriaDto
        {
            Id = c.Id,
            Nombre = c.Nombre,
            Descripcion = c.Descripcion,
            Activa = c.Activa
        });
    }
}
