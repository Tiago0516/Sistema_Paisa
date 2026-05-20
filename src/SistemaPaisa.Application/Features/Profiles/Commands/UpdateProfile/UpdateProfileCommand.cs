using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SistemaPaisa.Application.Features.Profiles.Commands.UpdateProfile;

public class UpdateProfileCommand : IRequest<bool>
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    [Display(Name = "Nombre")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(250)]
    [Display(Name = "Descripción")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Módulos del menú")]
    public List<int> ModuleIds { get; set; } = [];

    [Display(Name = "Activo")]
    public bool IsActive { get; set; }

    [Display(Name = "Acciones permitidas")]
    public List<int> ActionIds { get; set; } = [];
}
