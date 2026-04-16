using MediatR;
using SistemaPaisa.Application.DTOs;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Categorias.Queries.GetCategoriaById;

public class GetCategoriaByIdHandler : IRequestHandler<GetCategoriaByIdQuery, CategoriaDto?>
{
    private readonly ICategoriaRepository _categoriaRepository;

    public GetCategoriaByIdHandler(ICategoriaRepository categoriaRepository) => _categoriaRepository = categoriaRepository;

    public async Task<CategoriaDto?> Handle(GetCategoriaByIdQuery request, CancellationToken cancellationToken)
    {
        var c = await _categoriaRepository.GetByIdAsync(request.Id);
        if (c is null) return null;
        return new CategoriaDto
        {
            Id = c.Id,
            Nombre = c.Nombre,
            Descripcion = c.Descripcion,
            Activa = c.Activa
        };
    }
}
