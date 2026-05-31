using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SistemaPaisa.Application.Features.Suppliers.Commands.CreateSupplier;

public class CreateSupplierCommand : IRequest<int>
{
    [Required, MaxLength(150)]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(150), EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;
}
