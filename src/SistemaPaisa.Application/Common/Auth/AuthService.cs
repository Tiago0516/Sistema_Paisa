using SistemaPaisa.Application.Common.Interfaces;
using SistemaPaisa.Application.Features.Auth.Commands.Login;
using SistemaPaisa.Domain.Entities;

namespace SistemaPaisa.Application.Common.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResult> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(email.Trim());

        if (user is null)
            return LoginResult.Failed("Correo o contraseña incorrectos.");

        if (!_passwordHasher.Verify(password, user.PasswordHash))
            return LoginResult.Failed("Correo o contraseña incorrectos.");

        if (!user.Role.IsActive)
            return LoginResult.Failed("El rol asignado no está activo.");

        if (!user.Role.Client.IsActive)
            return LoginResult.Failed("El cliente asociado al rol no está activo.");

        var fullName = $"{user.FirstName} {user.LastName}".Trim();
        return LoginResult.Ok(
            user.Id,
            fullName,
            user.Email,
            user.Role.Id,
            user.Role.Name,
            user.Role.Client.Id,
            user.Role.Client.Name);
    }
}
