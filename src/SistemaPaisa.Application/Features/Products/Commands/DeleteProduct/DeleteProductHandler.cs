using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductHandler(IProductRepository productRepository) =>
        _productRepository = productRepository;

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken) =>
        await _productRepository.DeleteAsync(request.Id);
}
