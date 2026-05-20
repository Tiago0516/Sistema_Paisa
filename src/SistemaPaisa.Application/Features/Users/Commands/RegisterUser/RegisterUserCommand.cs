using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SistemaPaisa.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<RegisterUserResult>
{
    [Required(ErrorMessage = "First name is required")]
    [Display(Name = "First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [Display(Name = "Last name")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm password is required")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public int RoleId { get; set; }
    public int ClientId { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}
