using MediatR;

namespace SistemaPaisa.Application.Features.Productos.Commands.DeleteProducto;

public record DeleteProductoCommand(int Id) : IRequest<bool>;