using Microsoft.AspNetCore.Mvc;

namespace SistemaPaisa.Web.Authorization;

/// <summary>
/// Valida acceso al módulo (y opcionalmente a una acción) usando IPermissionService.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class RequireModuleAccessAttribute : TypeFilterAttribute
{
    public RequireModuleAccessAttribute(string moduleCode, string? actionCode = null)
        : base(typeof(RequireModuleAccessFilter))
    {
        Arguments = [moduleCode, actionCode ?? string.Empty];
    }
}
