using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SistemaPaisa.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest<int>
{
    [Required, MaxLength(120)]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(250)]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;
}
