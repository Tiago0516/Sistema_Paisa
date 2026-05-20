using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SistemaPaisa.Application.Features.AppActions.Commands.CreateAppAction;

public class CreateAppActionCommand : IRequest<int>
{
    [Required, MaxLength(120)]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    [Display(Name = "Code")]
    public string Code { get; set; } = string.Empty;

    [Display(Name = "Module")]
    public int ModuleId { get; set; }
}
