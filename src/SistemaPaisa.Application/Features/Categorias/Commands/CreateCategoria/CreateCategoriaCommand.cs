using MediatR;

namespace SistemaPaisa.Application.Features.Categorias.Commands.CreateCategoria;

public record CreateCategoriaCommand(
    string Nombre,
    string Descripcion
) : IRequest<int>;
