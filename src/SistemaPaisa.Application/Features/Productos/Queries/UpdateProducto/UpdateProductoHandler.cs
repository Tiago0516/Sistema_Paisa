using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Productos.Commands.UpdateProducto;

public class UpdateProductoHandler : IRequestHandler<UpdateProductoCommand, bool>
{
    private readonly IProductoRepository _productoRepository;

    public UpdateProductoHandler(IProductoRepository productoRepository) => _productoRepository = productoRepository;

    public async Task<bool> Handle(UpdateProductoCommand request, CancellationToken cancellationToken)
    {
        var producto = await _productoRepository.GetByIdAsync(request.Id);
        if (producto is null) return false;

        producto.Nombre = request.Nombre;
        producto.Descripcion = request.Descripcion;
        producto.Precio = request.Precio;
        producto.Stock = request.Stock;
        producto.Activo = request.Activo;
        producto.CategoriaId = request.CategoriaId;
        producto.UpdatedAt = DateTime.UtcNow;

        await _productoRepository.UpdateAsync(producto);
        return true;
    }
}