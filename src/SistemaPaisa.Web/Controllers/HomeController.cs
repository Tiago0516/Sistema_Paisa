using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPaisa.Web.Models;
using SistemaPaisa.Web.Services;

namespace SistemaPaisa.Web.Controllers;

public class HomeController : Controller
{
    private readonly ModuleWorkspaceBuilder _workspaceBuilder;

    public HomeController(ModuleWorkspaceBuilder workspaceBuilder) =>
        _workspaceBuilder = workspaceBuilder;

    public async Task<IActionResult> Index(string? module, CancellationToken cancellationToken)
    {
        var model = await _workspaceBuilder.BuildAsync(module, cancellationToken);
        ViewData["Title"] = model.Grid.Title;
        ViewData["ActiveModuleCode"] = model.ActiveModuleCode;
        return View(model);
    }

    public IActionResult Privacy() => View();

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
