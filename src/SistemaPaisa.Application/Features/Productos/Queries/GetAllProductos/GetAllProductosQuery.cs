using MediatR;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.Productos.Queries.GetAllProductos;

public record GetAllProductosQuery : IRequest<IEnumerable<ProductoDto>>;