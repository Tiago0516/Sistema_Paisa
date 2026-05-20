using MediatR;
using SistemaPaisa.Application.DTOs;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdHandler(IProductRepository productRepository) =>
        _productRepository = productRepository;

    public async Task<ProductDto?> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product is null) return null;

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            IsActive = product.IsActive,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name
        };
    }
}
