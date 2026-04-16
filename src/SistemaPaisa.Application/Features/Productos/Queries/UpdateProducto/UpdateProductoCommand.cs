using MediatR;

namespace SistemaPaisa.Application.Features.Productos.Commands.UpdateProducto;

public record UpdateProductoCommand(
    int Id, string Nombre, string Descripcion,
    decimal Precio, int Stock, bool Activo, int? CategoriaId
) : IRequest<bool>;