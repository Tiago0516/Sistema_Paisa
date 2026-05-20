using System.Security.Claims;
using SistemaPaisa.Application.Common;
using SistemaPaisa.Application.Common.Interfaces;

namespace SistemaPaisa.Web.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor) =>
        _httpContextAccessor = httpContextAccessor;

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

    public int? UserId => GetIntClaim(ClaimTypes.NameIdentifier);

    public int? RoleId => GetIntClaim(AuthClaimTypes.RoleId);

    public int? ClientId => GetIntClaim(AuthClaimTypes.ClientId);

    private int? GetIntClaim(string claimType)
    {
        var value = _httpContextAccessor.HttpContext?.User.FindFirst(claimType)?.Value;
        return int.TryParse(value, out var id) ? id : null;
    }
}
