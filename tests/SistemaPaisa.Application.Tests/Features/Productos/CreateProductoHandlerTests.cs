using FluentAssertions;
using Moq;
using SistemaPaisa.Application.Features.Productos.Commands.CreateProducto;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Tests.Features.Productos;

[TestClass]
public class CreateProductoHandlerTests
{
    private readonly Mock<IProductoRepository> _repoMock = new();

    [TestMethod]
    public async Task Handle_DebeCrearProducto_YRetornarId()
    {

        var command = new CreateProductoCommand("Tocineta", "Tocineta JJ", 16000, 50, 1);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Producto>()))
            .ReturnsAsync((Producto p) => { p.Id = 1; return p; });

        var handler = new CreateProductoHandler(_repoMock.Object);

        var id = await handler.Handle(command, CancellationToken.None);

        id.Should().Be(1);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Producto>()), Times.Once);
    }
}