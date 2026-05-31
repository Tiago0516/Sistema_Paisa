using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaPaisa.Application.Common;
using SistemaPaisa.Application.Common.Auth;
using SistemaPaisa.Application.Common.ModuleActions;
using SistemaPaisa.Application.Common.Permissions;
using SistemaPaisa.Application.Common.ProfileModules;
using SistemaPaisa.Application.Common.ProfileRoles;
using SistemaPaisa.Application.Common.Users;
using SistemaPaisa.Application.Features.Auth.Commands.Login;
using SistemaPaisa.Application.Features.Users.Commands.RegisterUser;
using SistemaPaisa.Application.Features.Users.Commands.UpdateUserRole;
using SistemaPaisa.Application.Features.Users.Queries.GetRolesForRegistration;
using SistemaPaisa.Application.Common.Navigation;
using SistemaPaisa.Web.Authorization;
using SistemaPaisa.Web.Extensions;
using SistemaPaisa.Web.Services;

namespace SistemaPaisa.Web.Controllers;

public class AccountController : Controller
{
    private readonly IMediator _mediator;
    private readonly IAuthService _authService;
    private readonly IUserRegistrationService _userRegistrationService;
    private readonly IAuthSessionService _authSessionService;
    private readonly IProfileRoleService _profileRoleService;
    private readonly IProfileModuleService _profileModuleService;
    private readonly IModuleActionService _moduleActionService;
    private readonly IPermissionService _permissionService;

    public AccountController(
        IMediator mediator,
        IAuthService authService,
        IUserRegistrationService userRegistrationService,
        IAuthSessionService authSessionService,
        IProfileRoleService profileRoleService,
        IProfileModuleService profileModuleService,
        IModuleActionService moduleActionService,
        IPermissionService permissionService)
    {
        _mediator = mediator;
        _authService = authService;
        _userRegistrationService = userRegistrationService;
        _authSessionService = authSessionService;
        _profileRoleService = profileRoleService;
        _profileModuleService = profileModuleService;
        _moduleActionService = moduleActionService;
        _permissionService = permissionService;
    }

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

        var result = await _authService.LoginAsync(command.Email, command.Password);

        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "No se pudo iniciar sesión.");
            return View(command);
        }

        await _authSessionService.SignInAsync(result);
        await LoadUserAccessAsync();

        return RedirectToLocal(returnUrl);
    }

    [HttpGet, AllowAnonymous]
    [RequireModuleAccess("USERS", PermissionCodes.Register)]
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
    [RequireModuleAccess("USERS", PermissionCodes.Register)]
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

        var result = await _userRegistrationService.RegisterAsync(command);

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
            return Redirect(ModuleRoutes.GetWorkspacePath("USERS"));
        }

        TempData["SuccessMessage"] = "Cuenta creada. Ya puedes iniciar sesion.";
        return RedirectToAction(nameof(Login));
    }

    [HttpPost, ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _authSessionService.SignOutAsync();
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
    [RequireModuleAccess("USERS", PermissionCodes.Manage)]
    public async Task<IActionResult> EditRole(int id)
    {
        await LoadRolesAsync();
        ViewBag.UserId = id;
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    [RequireModuleAccess("USERS", PermissionCodes.Manage)]
    public async Task<IActionResult> EditRole(int userId, int roleId)
    {
        var updated = await _mediator.Send(new UpdateUserRoleCommand(userId, roleId));
        if (!updated) return NotFound();

        TempData["SuccessMessage"] = "Rol del usuario actualizado correctamente.";
        return Redirect(ModuleRoutes.GetWorkspacePath("USERS"));
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

    private async Task LoadUserAccessAsync()
    {
        await _profileRoleService.GetForCurrentUserAsync();

        var modules = await _profileModuleService.GetForCurrentUserAsync();
        foreach (var module in modules)
            await _moduleActionService.GetForCurrentUserByModuleCodeAsync(module.Code);

        await _permissionService.GetCurrentPermissionsAsync();
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return Redirect(ModuleRoutes.GetWorkspacePath("SYSTEM"));
    }
}
