using MediatR;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.Productos.Queries.GetProductoById;

public record GetProductoByIdQuery(int Id) : IRequest<ProductoDto?>;