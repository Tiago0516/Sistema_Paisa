using MediatR;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.Products.Queries.GetProductById;

public record GetProductByIdQuery(int Id) : IRequest<ProductDto?>;
