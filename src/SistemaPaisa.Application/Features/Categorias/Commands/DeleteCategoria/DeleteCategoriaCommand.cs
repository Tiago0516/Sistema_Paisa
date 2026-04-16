using MediatR;

namespace SistemaPaisa.Application.Features.Categorias.Commands.DeleteCategoria;

public record DeleteCategoriaCommand(int Id) : IRequest<bool>;
