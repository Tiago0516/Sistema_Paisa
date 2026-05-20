using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaPaisa.Application.Common;
using SistemaPaisa.Application.Features.Auth.Commands.Login;
using SistemaPaisa.Application.Features.Users.Commands.RegisterUser;
using SistemaPaisa.Application.Features.Users.Commands.UpdateUserRole;
using SistemaPaisa.Application.Features.Users.Queries.GetRolesForRegistration;
using SistemaPaisa.Web.Extensions;

namespace SistemaPaisa.Web.Controllers;

public class AccountController : Controller
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator) => _mediator = mediator;

    [HttpGet, AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToLocal(returnUrl);

        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginCommand());
    }

    [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
    public async Task<IActionResult> Login(LoginCommand command, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(command);

        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Sign in failed.");
            return View(command);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, result.UserId.ToString()),
            new(ClaimTypes.Name, result.FullName),
            new(ClaimTypes.Email, result.Email),
            new(ClaimTypes.Role, result.RoleName),
            new(AuthClaimTypes.RoleId, result.RoleId.ToString()),
            new(AuthClaimTypes.RoleName, result.RoleName),
            new(AuthClaimTypes.ClientId, result.ClientId.ToString()),
            new(AuthClaimTypes.ClientName, result.ClientName)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            });

        return RedirectToLocal(returnUrl);
    }

    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> Register()
    {
        var command = new RegisterUserCommand();
        if (User.Identity?.IsAuthenticated == true)
            await LoadRolesAsync();

        if (Request.IsModalRequest())
            return PartialView("_RegisterForm", command);

        return View(command);
    }

    [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        if (!ModelState.IsValid)
        {
            if (User.Identity?.IsAuthenticated == true)
                await LoadRolesAsync(command.RoleId);

            if (Request.IsModalRequest())
                return PartialView("_RegisterForm", command);
            return View(command);
        }

        if (User.Identity?.IsAuthenticated == true)
        {
            command.ClientId = GetClientId();
            command.CreatedBy = User.Identity?.Name ?? "admin";
        }
        else
        {
            command.ClientId = 0;
            command.CreatedBy = "self-registration";
        }

        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "No se pudo crear la cuenta.");
            if (Request.IsModalRequest())
                return PartialView("_RegisterForm", command);
            return View(command);
        }

        if (Request.IsModalRequest())
            return Json(new { success = true });

        if (User.Identity?.IsAuthenticated == true)
        {
            TempData["SuccessMessage"] = "Usuario registrado correctamente.";
            return RedirectToAction("Index", "Home", new { module = "USERS" });
        }

        TempData["SuccessMessage"] = "Cuenta creada. Ya puedes iniciar sesion.";
        return RedirectToAction(nameof(Login));
    }

    [HttpPost, ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    [HttpGet, AllowAnonymous]
    public IActionResult AccessDenied() => View();

    private int GetClientId()
    {
        var claim = User.FindFirst(AuthClaimTypes.ClientId)?.Value;
        return int.TryParse(claim, out var id) ? id : 0;
    }

    [HttpGet]
    public async Task<IActionResult> EditRole(int id)
    {
        await LoadRolesAsync();
        ViewBag.UserId = id;
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRole(int userId, int roleId)
    {
        var updated = await _mediator.Send(new UpdateUserRoleCommand(userId, roleId));
        if (!updated) return NotFound();

        TempData["SuccessMessage"] = "Rol del usuario actualizado correctamente.";
        return RedirectToAction("Index", "Home", new { module = "USERS" });
    }

    private async Task LoadRolesAsync(int? selectedRoleId = null)
    {
        var roles = await _mediator.Send(new GetRolesForRegistrationQuery());
        ViewBag.Roles = roles.Select(r => new SelectListItem
        {
            Value = r.Id.ToString(),
            Text = r.Name,
            Selected = selectedRoleId.HasValue && selectedRoleId.Value == r.Id
        }).ToList();
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }
}
