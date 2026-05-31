using Microsoft.AspNetCore.Mvc;
using SistemaPaisa.Application.Common.Navigation;

namespace SistemaPaisa.Web.Extensions;

public static class ControllerNavigationExtensions
{
    public static IActionResult RedirectToModuleWorkspace(this Controller controller, string moduleCode) =>
        controller.Redirect(ModuleRoutes.GetWorkspacePath(moduleCode));
}
