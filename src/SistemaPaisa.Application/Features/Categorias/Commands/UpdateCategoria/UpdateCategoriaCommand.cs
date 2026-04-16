using MediatR;

namespace SistemaPaisa.Application.Features.Categorias.Commands.UpdateCategoria;

public record UpdateCategoriaCommand(
    int Id,
    string Nombre,
    string Descripcion,
    bool Activa
) : IRequest<bool>;
