using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SistemaPaisa.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    int? CategoryId
) : IRequest<int>;
