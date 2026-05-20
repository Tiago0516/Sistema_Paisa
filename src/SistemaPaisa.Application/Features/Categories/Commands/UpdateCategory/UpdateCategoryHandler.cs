using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, bool>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryHandler(ICategoryRepository categoryRepository) =>
        _categoryRepository = categoryRepository;

    public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id);
        if (category is null) return false;

        category.Name = request.Name;
        category.Description = request.Description;
        category.IsActive = request.IsActive;

        await _categoryRepository.UpdateAsync(category);
        return true;
    }
}
