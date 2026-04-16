using MediatR;
using SistemaPaisa.Application.DTOs;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Productos.Queries.GetAllProductos;

public class GetAllProductosHandler : IRequestHandler<GetAllProductosQuery, IEnumerable<ProductoDto>>
{
    private readonly IProductoRepository _productoRepository;

    public GetAllProductosHandler(IProductoRepository productoRepository) => _productoRepository = productoRepository;

    public async Task<IEnumerable<ProductoDto>> Handle(GetAllProductosQuery request, CancellationToken cancellationToken)
    {
        var productos = await _productoRepository.GetAllAsync();
        return productos.Select(p => new ProductoDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            Precio = p.Precio,
            Stock = p.Stock,
            Activo = p.Activo,
            CategoriaId = p.CategoriaId,
            CategoriaNombre = p.Categoria?.Nombre
        });
    }
}