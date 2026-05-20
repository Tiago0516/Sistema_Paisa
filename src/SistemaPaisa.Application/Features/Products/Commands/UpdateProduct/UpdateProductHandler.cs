using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductHandler(IProductRepository productRepository) =>
        _productRepository = productRepository;

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product is null) return false;

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.IsActive = request.IsActive;
        product.CategoryId = request.CategoryId;

        await _productRepository.UpdateAsync(product);
        return true;
    }
}
