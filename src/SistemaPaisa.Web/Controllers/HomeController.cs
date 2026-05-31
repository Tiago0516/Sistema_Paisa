using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPaisa.Application.Common.Navigation;
using SistemaPaisa.Web.Models;

namespace SistemaPaisa.Web.Controllers;

public class HomeController : Controller
{
    /// <summary>
    /// Redirección legacy: /Home/Index?module=SUPPLIERS → /suppliers
    /// </summary>
    [HttpGet]
    public IActionResult Index(string? module)
    {
        if (string.IsNullOrWhiteSpace(module))
            return RedirectPermanent(ModuleRoutes.GetWorkspacePath("SYSTEM"));

        return RedirectPermanent(ModuleRoutes.GetWorkspacePath(module));
    }

    public IActionResult Privacy() => View();

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
