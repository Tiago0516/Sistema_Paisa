using Microsoft.AspNetCore.Routing;
using SistemaPaisa.Application.Common.Navigation;

namespace SistemaPaisa.Web.Routing;

public sealed class ModuleSlugRouteConstraint : IRouteConstraint
{
    public bool Match(
        HttpContext? httpContext,
        IRouter? route,
        string routeKey,
        RouteValueDictionary values,
        RouteDirection routeDirection)
    {
        if (!values.TryGetValue(routeKey, out var raw) || raw is not string slug)
            return false;

        return ModuleRoutes.TryGetCode(slug, out _);
    }
}
