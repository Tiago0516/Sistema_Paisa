using MediatR;
using SistemaPaisa.Application.DTOs;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Productos.Queries.GetProductoById;

public class GetProductoByIdHandler : IRequestHandler<GetProductoByIdQuery, ProductoDto?>
{
    private readonly IProductoRepository _productoRepository;

    public GetProductoByIdHandler(IProductoRepository productoRepository) => _productoRepository = productoRepository;

    public async Task<ProductoDto?> Handle(GetProductoByIdQuery request, CancellationToken cancellationToken)
    {
        var p = await _productoRepository.GetByIdAsync(request.Id);
        if (p is null) return null;
        return new ProductoDto
        {
            Id = p.Id, Nombre = p.Nombre, Descripcion = p.Descripcion,
            Precio = p.Precio, Stock = p.Stock, Activo = p.Activo,
            CategoriaId = p.CategoriaId,
            CategoriaNombre = p.Categoria?.Nombre
        };
    }
}