using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Categorias.Commands.DeleteCategoria;

public class DeleteCategoriaHandler : IRequestHandler<DeleteCategoriaCommand, bool>
{
    private readonly ICategoriaRepository _categoriaRepository;

    public DeleteCategoriaHandler(ICategoriaRepository categoriaRepository) => _categoriaRepository = categoriaRepository;

    public async Task<bool> Handle(DeleteCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoria = await _categoriaRepository.GetByIdAsync(request.Id);
        if (categoria is null) return false;
        await _categoriaRepository.DeleteAsync(categoria.Id);
        return true;
    }
}
