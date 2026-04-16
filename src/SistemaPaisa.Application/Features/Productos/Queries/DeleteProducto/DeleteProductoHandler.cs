using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Productos.Commands.DeleteProducto;

public class DeleteProductoHandler : IRequestHandler<DeleteProductoCommand, bool>
{
    private readonly IProductoRepository _productoRepository;

    public DeleteProductoHandler(IProductoRepository productoRepository) => _productoRepository = productoRepository;

    public async Task<bool> Handle(DeleteProductoCommand request, CancellationToken cancellationToken)
    {
        var producto = await _productoRepository.GetByIdAsync(request.Id);
        if (producto is null) return false;
        await _productoRepository.DeleteAsync(producto.Id);
        return true;
    }
}