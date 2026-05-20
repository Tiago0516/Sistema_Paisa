using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SistemaPaisa.Application.Features.Modules.Commands.CreateModule;

public class CreateModuleCommand : IRequest<int>
{
    [Required, MaxLength(120)]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(250)]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    [Display(Name = "Code")]
    public string Code { get; set; } = string.Empty;
}
