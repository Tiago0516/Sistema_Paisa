using System.ComponentModel.DataAnnotations;

namespace SistemaPaisa.Web.Api.Models;

public class UpdateSupplierRequest
{
    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(150), EmailAddress]
    public string Email { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
