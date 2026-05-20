using FluentAssertions;
using Moq;
using SistemaPaisa.Application.Features.Products.Queries.GetAllProducts;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Tests.Features.Products;

[TestClass]
public class GetAllProductsHandlerTests
{
    [TestMethod]
    public async Task Handle_ShouldReturnAllProducts()
    {
        var products = new List<Product>
        {
            new() { Id = 1, Name = "A", Description = "D", Price = 10, Stock = 1 },
            new() { Id = 2, Name = "B", Description = "D", Price = 20, Stock = 2 }
        };

        var repoMock = new Mock<IProductRepository>();
        repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

        var handler = new GetAllProductsHandler(repoMock.Object);
        var result = (await handler.Handle(new GetAllProductsQuery(), CancellationToken.None)).ToList();

        result.Should().HaveCount(2);
        result[0].Name.Should().Be("A");
    }
}
