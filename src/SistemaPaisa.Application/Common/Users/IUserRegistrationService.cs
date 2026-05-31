using SistemaPaisa.Application.Features.Users.Commands.RegisterUser;

namespace SistemaPaisa.Application.Common.Users;

public interface IUserRegistrationService
{
    Task<RegisterUserResult> RegisterAsync(
        RegisterUserCommand command,
        CancellationToken cancellationToken = default);
}
