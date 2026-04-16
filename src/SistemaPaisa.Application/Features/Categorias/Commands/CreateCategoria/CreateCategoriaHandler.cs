using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Categorias.Commands.CreateCategoria;

public class CreateCategoriaHandler : IRequestHandler<CreateCategoriaCommand, int>
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CreateCategoriaHandler(ICategoriaRepository categoriaRepository) => _categoriaRepository = categoriaRepository;

    public async Task<int> Handle(CreateCategoriaCommand request, CancellationToken cancellationToken)
    {
        var nuevaCategoria = new Categoria
        {
            Nombre = request.Nombre,
            Descripcion = request.Descripcion
        };

        var created = await _categoriaRepository.AddAsync(nuevaCategoria);
        return created.Id;
    }
}
