using FluentAssertions;
using Moq;
using SistemaPaisa.Application.Features.Productos.Queries.GetAllProductos;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Tests.Features.Productos;

[TestClass]
public class GetAllProductosHandlerTests
{
    private readonly Mock<IProductoRepository> _repoMock = new();

    [TestMethod]
    public async Task Handle_DebeRetornarListaDeProductos()
    {
        var productos = new List<Producto>
        {
            new() { Id = 1, Nombre = "Tocineta JJ", Precio = 16000, Stock = 20 },
            new() { Id = 2, Nombre = "Tocineta Don toño", Precio = 12500, Stock = 20 }
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(productos);

        var handler = new GetAllProductosHandler(_repoMock.Object);

        var result = await handler.Handle(new GetAllProductosQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
        result.First().Nombre.Should().Be("Tocineta JJ");
    }

    [TestMethod]
    public async Task Handle_DebeRetornarListaVacia_CuandoNoHayProductos()
    {
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Producto>());
        var handler = new GetAllProductosHandler(_repoMock.Object);

        var result = await handler.Handle(new GetAllProductosQuery(), CancellationToken.None);

        result.Should().BeEmpty();
    }
}