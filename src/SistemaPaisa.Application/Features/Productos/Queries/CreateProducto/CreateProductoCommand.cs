using MediatR;

namespace SistemaPaisa.Application.Features.Productos.Commands.CreateProducto;

public record CreateProductoCommand(
    string Nombre,
    string Descripcion,
    decimal Precio,
    int Stock,
    int? CategoriaId
) : IRequest<int>;