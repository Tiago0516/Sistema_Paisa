using MediatR;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.Products.Queries.GetAllProducts;

public record GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>;
