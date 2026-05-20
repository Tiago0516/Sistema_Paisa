using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryHandler(ICategoryRepository categoryRepository) =>
        _categoryRepository = categoryRepository;

    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken) =>
        await _categoryRepository.DeleteAsync(request.Id);
}
