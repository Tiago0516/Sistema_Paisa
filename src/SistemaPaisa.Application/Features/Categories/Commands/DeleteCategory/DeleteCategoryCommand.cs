using MediatR;

namespace SistemaPaisa.Application.Features.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(int Id) : IRequest;
