using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using SistemaPaisa.Application.Common;
using SistemaPaisa.Application.Features.Auth.Commands.Login;

namespace SistemaPaisa.Web.Services;

public class AuthSessionService : IAuthSessionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthSessionService(IHttpContextAccessor httpContextAccessor) =>
        _httpContextAccessor = httpContextAccessor;

    public async Task SignInAsync(
        LoginResult loginResult,
        CancellationToken cancellationToken = default)
    {
        var httpContext = _httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("No hay contexto HTTP disponible.");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, loginResult.UserId.ToString()),
            new(ClaimTypes.Name, loginResult.FullName),
            new(ClaimTypes.Email, loginResult.Email),
            new(ClaimTypes.Role, loginResult.RoleName),
            new(AuthClaimTypes.RoleId, loginResult.RoleId.ToString()),
            new(AuthClaimTypes.RoleName, loginResult.RoleName),
            new(AuthClaimTypes.ClientId, loginResult.ClientId.ToString()),
            new(AuthClaimTypes.ClientName, loginResult.ClientName)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            });
    }

    public Task SignOutAsync(CancellationToken cancellationToken = default)
    {
        var httpContext = _httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("No hay contexto HTTP disponible.");

        return httpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
