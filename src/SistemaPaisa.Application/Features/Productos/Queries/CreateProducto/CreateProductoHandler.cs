using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Productos.Commands.CreateProducto;

public class CreateProductoHandler : IRequestHandler<CreateProductoCommand, int>
{
    private readonly IProductoRepository _productoRepository;

    public CreateProductoHandler(IProductoRepository productoRepository) => _productoRepository = productoRepository;

    public async Task<int> Handle(CreateProductoCommand request, CancellationToken cancellationToken)
    {
        var nuevoProducto = new Producto
        {
            Nombre = request.Nombre,
            Descripcion = request.Descripcion,
            Precio = request.Precio,
            Stock = request.Stock,
            CategoriaId = request.CategoriaId
        };

        var created = await _productoRepository.AddAsync(nuevoProducto);
        return created.Id;
    }
}