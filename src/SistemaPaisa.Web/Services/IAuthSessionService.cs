using SistemaPaisa.Application.Features.Auth.Commands.Login;

namespace SistemaPaisa.Web.Services;

public interface IAuthSessionService
{
    Task SignInAsync(LoginResult loginResult, CancellationToken cancellationToken = default);

    Task SignOutAsync(CancellationToken cancellationToken = default);
}
