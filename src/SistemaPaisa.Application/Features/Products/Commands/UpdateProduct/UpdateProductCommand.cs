using MediatR;

namespace SistemaPaisa.Application.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int Stock,
    bool IsActive,
    int? CategoryId
) : IRequest<bool>;
