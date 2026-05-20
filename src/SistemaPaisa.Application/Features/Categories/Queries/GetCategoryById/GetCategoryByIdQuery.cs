using MediatR;
using SistemaPaisa.Application.DTOs;

namespace SistemaPaisa.Application.Features.Categories.Queries.GetCategoryById;

public record GetCategoryByIdQuery(int Id) : IRequest<CategoryDto?>;
