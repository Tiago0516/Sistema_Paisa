using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SistemaPaisa.Application.Features.Roles.Commands.CreateRole;

public class CreateRoleCommand : IRequest<int>
{
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
}
