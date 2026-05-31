using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SistemaPaisa.Application.Features.Suppliers.Commands.UpdateSupplier;

public class UpdateSupplierCommand : IRequest<bool>
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    [Display(Name = "Nombre")]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(150), EmailAddress]
    [Display(Name = "Correo")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Activo")]
    public bool IsActive { get; set; }

    public UpdateSupplierCommand() { }

    public UpdateSupplierCommand(int id, string name, string email, bool isActive)
    {
        Id = id;
        Name = name;
        Email = email;
        IsActive = isActive;
    }
}
