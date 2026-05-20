using FluentAssertions;
using Moq;
using SistemaPaisa.Application.Features.Products.Commands.CreateProduct;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Tests.Features.Products;

[TestClass]
public class CreateProductHandlerTests
{
    private readonly Mock<IProductRepository> _repoMock = new();

    [TestMethod]
    public async Task Handle_ShouldCreateProduct_AndReturnId()
    {
        var command = new CreateProductCommand("Bacon", "JJ Bacon", 16000, 50, 1);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Product>()))
            .ReturnsAsync((Product p) => { p.Id = 1; return p; });

        var handler = new CreateProductHandler(_repoMock.Object);

        var id = await handler.Handle(command, CancellationToken.None);

        id.Should().Be(1);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
    }
}
