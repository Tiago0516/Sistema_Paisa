using MediatR;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.Categories.Queries.GetAllCategories;

public record GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>;
