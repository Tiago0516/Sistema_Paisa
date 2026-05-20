using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SistemaPaisa.Application.Common;
using SistemaPaisa.Application.Features.Users.Commands.RegisterUser;
using SistemaPaisa.Web.Extensions;

namespace SistemaPaisa.Web.Controllers;

public class UsersController : Controller
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator) => _mediator = mediator;

    public IActionResult Register()
    {
        var command = new RegisterUserCommand();
        if (Request.IsModalRequest())
            return PartialView("_RegisterForm", command);

        return View(command);
    }

    [HttpPost, ValidateAntiForgeryToken]
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

        var result = await _mediator.Send(command);

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
        return RedirectToAction("Index", "Home", new { module = "USERS" });
    }

    private int GetClientId()
    {
        var claim = User.FindFirst(AuthClaimTypes.ClientId)?.Value;
        return int.TryParse(claim, out var id) ? id : 0;
    }
}
