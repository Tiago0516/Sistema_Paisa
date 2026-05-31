using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPaisa.Application.Common.Navigation;
using SistemaPaisa.Application.Common.Permissions;
using SistemaPaisa.Web.Services;

namespace SistemaPaisa.Web.Controllers;

[Authorize]
public class ModuleWorkspaceController : Controller
{
    private readonly ModuleWorkspaceBuilder _workspaceBuilder;
    private readonly IPermissionService _permissionService;

    public ModuleWorkspaceController(
        ModuleWorkspaceBuilder workspaceBuilder,
        IPermissionService permissionService)
    {
        _workspaceBuilder = workspaceBuilder;
        _permissionService = permissionService;
    }

    [HttpGet("/")]
    [HttpGet("/home")]
    public Task<IActionResult> Home(CancellationToken cancellationToken) =>
        RenderWorkspaceAsync("SYSTEM", cancellationToken);

    [HttpGet("/{moduleSlug:moduleSlug}")]
    public async Task<IActionResult> Module(string moduleSlug, CancellationToken cancellationToken)
    {
        if (!ModuleRoutes.TryGetCode(moduleSlug, out var moduleCode) ||
            string.Equals(moduleCode, "SYSTEM", StringComparison.OrdinalIgnoreCase))
            return NotFound();

        if (!await _permissionService.CanAccessModuleAsync(moduleCode, cancellationToken))
            return RedirectToAction("AccessDenied", "Account");

        return await RenderWorkspaceAsync(moduleCode, cancellationToken);
    }

    private async Task<IActionResult> RenderWorkspaceAsync(string moduleCode, CancellationToken cancellationToken)
    {
        var model = await _workspaceBuilder.BuildAsync(moduleCode, cancellationToken);
        ViewData["Title"] = model.Grid.Title;
        ViewData["ActiveModuleCode"] = model.ActiveModuleCode;
        return View("~/Views/Home/Index.cshtml", model);
    }
}
