using MediatR;
using SistemaPaisa.Application.DTOs;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Features.Categories.Queries.GetAllCategories;

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesHandler(ICategoryRepository categoryRepository) =>
        _categoryRepository = categoryRepository;

    public async Task<IEnumerable<CategoryDto>> Handle(
        GetAllCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            IsActive = c.IsActive
        });
    }
}
