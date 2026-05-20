using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SistemaPaisa.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand : IRequest<bool>
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(250)]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Active")]
    public bool IsActive { get; set; }

    public UpdateCategoryCommand() { }

    public UpdateCategoryCommand(int id, string name, string description, bool isActive)
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = isActive;
    }
}
