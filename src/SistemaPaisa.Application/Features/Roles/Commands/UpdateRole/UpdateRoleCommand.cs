using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SistemaPaisa.Application.Features.Roles.Commands.UpdateRole;

public class UpdateRoleCommand : IRequest<bool>
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(250)]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Profile")]
    public int ProfileId { get; set; }

    [Display(Name = "Client")]
    public int ClientId { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; }
}
