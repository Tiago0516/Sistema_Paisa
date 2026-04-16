using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Categorias.Commands.UpdateCategoria;

public class UpdateCategoriaHandler : IRequestHandler<UpdateCategoriaCommand, bool>
{
    private readonly ICategoriaRepository _categoriaRepository;

    public UpdateCategoriaHandler(ICategoriaRepository categoriaRepository) => _categoriaRepository = categoriaRepository;

    public async Task<bool> Handle(UpdateCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoria = await _categoriaRepository.GetByIdAsync(request.Id);
        if (categoria is null) return false;

        categoria.Nombre = request.Nombre;
        categoria.Descripcion = request.Descripcion;
        categoria.Activa = request.Activa;
        categoria.UpdatedAt = DateTime.UtcNow;

        await _categoriaRepository.UpdateAsync(categoria);
        return true;
    }
}
