using MediatR;

namespace SistemaPaisa.Application.Features.Products.Commands.DeleteProduct;

public record DeleteProductCommand(int Id) : IRequest;
