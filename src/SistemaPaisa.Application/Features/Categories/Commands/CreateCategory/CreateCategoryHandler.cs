using MediatR;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, int>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryHandler(ICategoryRepository categoryRepository) =>
        _categoryRepository = categoryRepository;

    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Name = request.Name,
            Description = request.Description
        };

        var created = await _categoryRepository.AddAsync(category);
        return created.Id;
    }
}
