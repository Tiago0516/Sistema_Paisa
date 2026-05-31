using System.Security.Claims;
using SistemaPaisa.Application.Common.Navigation;
using Microsoft.AspNetCore.Mvc;
using SistemaPaisa.Application.Common;
using SistemaPaisa.Application.Common.Users;
using SistemaPaisa.Application.Features.Users.Commands.RegisterUser;
using SistemaPaisa.Application.Common.Permissions;
using SistemaPaisa.Web.Authorization;
using SistemaPaisa.Web.Extensions;

namespace SistemaPaisa.Web.Controllers;

[Route("users")]
[RequireModuleAccess("USERS")]
public class UsersController : Controller
{
    private readonly IUserRegistrationService _userRegistrationService;

    public UsersController(IUserRegistrationService userRegistrationService) =>
        _userRegistrationService = userRegistrationService;

    [HttpGet("register")]
    [RequireModuleAccess("USERS", PermissionCodes.Register)]
    public IActionResult Register()
    {
        var command = new RegisterUserCommand();
        if (Request.IsModalRequest())
            return PartialView("_RegisterForm", command);

        return View(command);
    }

    [HttpPost("register"), ValidateAntiForgeryToken]
    [RequireModuleAccess("USERS", PermissionCodes.Register)]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        if (!ModelState.IsValid)
        {
            if (Request.IsModalRequest())
                return PartialView("_RegisterForm", command);
            return View(command);
        }

        command.ClientId = GetClientId();
        command.CreatedBy = User.Identity?.Name ?? User.FindFirst(ClaimTypes.Email)?.Value ?? "system";

        var result = await _userRegistrationService.RegisterAsync(command);

        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "No se pudo registrar el usuario.");
            if (Request.IsModalRequest())
                return PartialView("_RegisterForm", command);
            return View(command);
        }

        if (Request.IsModalRequest())
            return Json(new { success = true });

        TempData["SuccessMessage"] = "Usuario registrado correctamente.";
        return Redirect(ModuleRoutes.GetWorkspacePath("USERS"));
    }

    private int GetClientId()
    {
        var claim = User.FindFirst(AuthClaimTypes.ClientId)?.Value;
        return int.TryParse(claim, out var id) ? id : 0;
    }
}
