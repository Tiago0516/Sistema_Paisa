using SistemaPaisa.Application.Features.Auth.Commands.Login;

namespace SistemaPaisa.Application.Common.Auth;

public interface IAuthService
{
    Task<LoginResult> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);
}
