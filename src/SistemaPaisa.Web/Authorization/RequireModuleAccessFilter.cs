using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SistemaPaisa.Application.Common.Permissions;

namespace SistemaPaisa.Web.Authorization;

public sealed class RequireModuleAccessFilter : IAsyncAuthorizationFilter
{
    private readonly string _moduleCode;
    private readonly string? _actionCode;
    private readonly IPermissionService _permissionService;

    public RequireModuleAccessFilter(
        string moduleCode,
        string? actionCode,
        IPermissionService permissionService)
    {
        _moduleCode = moduleCode;
        _actionCode = string.IsNullOrWhiteSpace(actionCode) ? null : actionCode;
        _permissionService = permissionService;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (context.HttpContext.User.Identity?.IsAuthenticated != true)
            return;

        var allowed = _actionCode is null
            ? await _permissionService.CanAccessModuleAsync(_moduleCode, context.HttpContext.RequestAborted)
            : await _permissionService.HasActionAsync(
                _moduleCode,
                _actionCode,
                context.HttpContext.RequestAborted);

        if (allowed)
            return;

        context.Result = IsApiRequest(context)
            ? new ObjectResult(new { error = "No tiene permiso para esta operación." }) { StatusCode = StatusCodes.Status403Forbidden }
            : new RedirectToActionResult("AccessDenied", "Account", null);
    }

    private static bool IsApiRequest(AuthorizationFilterContext context) =>
        context.HttpContext.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase);
}
